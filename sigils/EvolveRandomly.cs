using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using APIPlugin;
using BepInEx.Bootstrap;
using DiskCardGame;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private NewAbility AddEvolveRandomly()
		{
			// setup ability
			const string rulebookName = "Evolve Randomly";
			const string rulebookDescription =
				"A card bearing this sigil will grow into a random form after 1 turn on the board.";
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription);

			Texture2D tex = SigilUtils.LoadTextureFromResource(Resources.ability_randomEvolve);

			var abIds = SigilUtils.GetAbilityId(info.rulebookName);
			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(EvolveRandomly), tex, abIds);
			EvolveRandomly.ability = newAbility.ability;

			return newAbility;
		}

		private void InitializeEvolveRandomlyCardListConfig()
		{
			int numOfCards = GetNumCardsForRandomEvolve();

			for (var i = 0; i < numOfCards; i++)
			{
				string cardWithHyphenAndCommas = GetCardForRandomEvolve(i);
				Log.LogDebug($"Card #[{i + 1}] has [{cardWithHyphenAndCommas}]");

				string[] split = cardWithHyphenAndCommas.Split('-');

				string cardToModify = split[0];

				if (CardDoesNotExistInGameOrApiLoad(cardToModify))
				{
					Logger.LogError(
						$"Can't find card with name \"{cardToModify}\" to add to evolve params. Make sure it's not the displayName!");
					break;
				}

				List<CardInfo> cardsForEvolutions = split[1]
					.Split(',')
					.Select(CardLoader.GetCardByName)
					.ToList();

				EvolveRandomly.CardsWithSpecifiedEvolutions.Add(cardToModify, cardsForEvolutions);
				// Logger.LogMessage($"[{cardToModify}] now [{cardsForEvolutions.Count}] random evolutions");
			}
		}

		// Taken from MADH95
		private bool CardDoesNotExistInGameOrApiLoad(string cardToCheck)
		{
			return !VerifyPluginsAndApiCards(cardToCheck);
		}

		private bool VerifyPluginsAndApiCards(string cardToCheck)
		{
			return Chainloader.PluginInfos.ContainsKey(APIGUID)
			       && Chainloader.PluginInfos.ContainsKey(JSONGUID)
			       && CheckThatAPICardExists(cardToCheck).Result;
		}

		async Task<bool> WaitForItToWork()
		{
			bool succeeded = ScriptableObjectLoader<CardInfo>.allData is not null;
			while (!succeeded)
			{
				// do work
				succeeded = ScriptableObjectLoader<CardInfo>.allData is not null; // if it worked, make as succeeded, else retry
				await Task.Delay(1000); // arbitrary delay
			}
			return succeeded;
		}
		
		[MethodImpl(MethodImplOptions.NoInlining)]
		private async Task<bool> CheckThatAPICardExists(string nameParam)
		{
			bool result = await WaitForItToWork();
			Plugin.Log.LogDebug("WaitForItToWork done:" + result); // true or false
			return CustomCard.cards.Exists(elem => elem.name == nameParam) ||
			       NewCard.cards.Exists(elem => elem.name == nameParam);
		}
	}


	public class EvolveRandomly : Evolve
	{
		public static void AddCardToDictionary(string name, List<string> evolutionCards)
		{
			CardsWithSpecifiedEvolutions.Add(name,
				evolutionCards
					.Select(CardLoader.GetCardByName).ToList()
			);
		}

		public static readonly Dictionary<string, List<CardInfo>> CardsWithSpecifiedEvolutions = new();
		public static Ability ability;
		public override Ability Ability => ability;

		public override CardInfo GetTransformCardInfo()
		{
			if (CardsWithSpecifiedEvolutions.TryGetValue(base.Card.Info.name, out var listOfCards))
			{
				Plugin.Log.LogInfo($"[{base.Card.Info.name}] has random evolutions, choosing from list...");
				return listOfCards[Random.Range(0, listOfCards.Count)];
			}

			Plugin.Log.LogWarning($"[{base.Card.Info.name}] has no random evolutions, choosing pure random card...");
			return CardLoader.GetPureRandomCard();
		}
	}
}