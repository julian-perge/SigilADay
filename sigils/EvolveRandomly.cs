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
		private static string _configCards;

		private NewAbility AddRandomEvolve()
		{ 
			InitializeRandomEvolveCardListConfig();
			
			// setup ability
			const string rulebookName = "Evolve Randomly";
			const string rulebookDescription = "A card bearing this sigil will grow into a random form after 1 turn on the board.";
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
			_configCards = Config.Bind(PluginName, "Cards to evolve from", "Squirrel,Skunk,Shark",  
				new ConfigDescription( "Set list of possible cards to evolve into. Separated by commas.")).Value;
			Log.LogDebug($"CardList value is [{_configCards}]");

			if (!string.IsNullOrEmpty(_configCards))
			{
				Log.LogDebug($"RandomEvolveCardList is not empty: [{_configCards}]");
				var splitCardList = _configCards
					.Split(',')
					.Select(str => str.Trim()).ToList();
				foreach (var cardName in splitCardList)
				{
					bool cardExistsInGame = ScriptableObjectLoader<CardInfo>.AllData.Exists(info => info.name == cardName);

					bool cardExistsInApi = TryCheckAPICards(cardName);

					if (!cardExistsInGame && !cardExistsInApi)
					{
						Logger.LogError(
							$"Can't find card with name \"{name}\" to add to evolve params. Make sure it's not the displayName!");
						break;
					}

					EvolveRandomly.ListOfPossibleCardsToTransformInto.Add(CardLoader.GetCardByName(cardName));
					Logger.LogMessage($"[{cardName}] added to list of possible cards to evolve into");
				}
			}
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
		public static List<CardInfo> ListOfPossibleCardsToTransformInto = new List<CardInfo>();
		public static Ability ability;
		public override Ability Ability => ability;

		public override CardInfo GetTransformCardInfo()
		{
			if (ListOfPossibleCardsToTransformInto.Count != 0)
			{
				Plugin.Log.LogDebug($"ListOfPossibleCardsToTransformInto is not empty, choosing from list...");
				return ListOfPossibleCardsToTransformInto[
					Random.Range(0, ListOfPossibleCardsToTransformInto.Count)
				];
			}

			Plugin.Log.LogDebug($"ListOfPossibleCardsToTransformInto is empty, choosing random card...");
			return CardLoader.GetPureRandomCard();
		}
	}
}