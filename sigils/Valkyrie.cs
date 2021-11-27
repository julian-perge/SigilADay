using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using SigilADay_julianperge.lib;
using UnityEngine;
using static SigilADay_julianperge.Plugin;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private void AddNorseWarrior()
		{
			Texture2D defaultTexture =
				SigilUtils.LoadTextureFromResource(SigilADay_julianperge.Properties.Resources.card_norse_warrior);

			const string name = "NorseWarrior";
			const string displayName = "Norse Warrior";
			const string desc = "Odin would be proud!";

			NewCard.Add(name, displayName, 1, 1,
				null, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexture, abilities: new List<Ability>() { Ability.Brittle }
			);
		}

		private NewAbility AddFlightOfTheValkyrie()
		{
			AddNorseWarrior();

			const string rulebookName = "Flight of the Valkyrie";
			const string rulebookDescription =
				"When [creature] is played, fill all open slots on your side of the field with 1/1 Norse Warriors.";

			// setup ability
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, true, 3);

			// get and load artwork
			Texture2D sigilTex = SigilUtils.LoadTextureFromResource(SigilADay_julianperge.Properties.Resources.ability_fotv);

			var abId = SigilUtils.GetAbilityId(info.rulebookName);
			
			NewAbility newAbility = new NewAbility(info, typeof(FlightOfTheValkyrie), sigilTex, abId);
			
			FlightOfTheValkyrie.ability = newAbility.ability;

			return newAbility;
		}
	}

	public class FlightOfTheValkyrie : AbilityBehaviour
	{
		public override Ability Ability => ability;

		public static Ability ability;

		public override bool RespondsToResolveOnBoard()
		{
			return true;
		}

		public override IEnumerator OnResolveOnBoard()
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return new WaitForSeconds(0.2f);

			// only get a list of player slots
			var slotsWithCards = Singleton<BoardManager>.Instance.GetSlots(true);

			base.Card.Anim.StrongNegationEffect();
			if (slotsWithCards.TrueForAll(slot => slot && slot.Card))
			{
				Log.LogDebug($"All slots are full, not spawning a copy of [{base.Card.Info.name}]");
			}
			else
			{
				Log.LogDebug("Starting copy sequence");
				// now only check those filtered cards that have terrain traits
				foreach (var slot in slotsWithCards.Where(slot => slot && !slot.Card))
				{
					// >= 0 AND <= 2
					string cardToSpawn = base.Card.Info.name;
					Log.LogDebug($"-> Spawning [{cardToSpawn}] in slot [{slot.name}]");
					PlayableCard copy = CardSpawner.SpawnPlayableCard(CardLoader.GetCardByName(cardToSpawn));
					yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(copy, slot);
					break;
				}

				Singleton<ViewManager>.Instance.SwitchToView(View.Board);
				yield return new WaitForSeconds(0.2f);
			}

			yield return base.LearnAbility(0.25f);
			yield break;
		}
	}
}