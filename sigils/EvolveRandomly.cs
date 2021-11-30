using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using APIPlugin;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using DiskCardGame;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		public int GetNumCards()
			=> Config.Bind(PluginName, "Sets of cards to modify", 1).Value;

		public string GetCard(int index)
			=> Config.Bind(PluginName, "Card" + (index + 1),
				"Geck-Skunk,RavenEgg",
				"Set list of possible cards to evolve into.\n" +
				"Card to set specified evolutions to, has a hyphen before the other cards.\n"
				+ "e.g. Geck-Skink,Raven will set the Geck card with 2 possible evolutions.").Value;

		private NewAbility AddRandomEvolve()
		{
			InitializeRandomEvolveCardListConfig();

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

		private void InitializeRandomEvolveCardListConfig()
		{
			int numOfCards = GetNumCards();

			for (var i = 0; i < numOfCards; i++)
			{
				string cardWithHyphenAndCommas = GetCard(i);
				Plugin.Log.LogDebug($"Card #[{i + 1}] has [{cardWithHyphenAndCommas}]");

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
			bool cardExistsInGame = ScriptableObjectLoader<CardInfo>.AllData.Exists(info => info.name == cardToCheck);
			bool cardExistsInApi = TryCheckAPICards(cardToCheck);
			return !cardExistsInGame && !cardExistsInApi;
		}

		private bool TryCheckAPICards(string _name)
		{
			return Chainloader.PluginInfos.ContainsKey(APIGUID) && CheckForAPICard(_name);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool CheckForAPICard(string nameParam)
			=> CustomCard.cards.Exists(elem => elem.name == nameParam) ||
			   NewCard.cards.Exists(elem => elem.name == nameParam);
	}


	public class EvolveRandomly : Evolve
	{
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