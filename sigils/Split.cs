using System.Collections;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

// using UnityEngine.UIElements;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private NewAbility AddSplit()
		{
			// setup ability
			const string rulebookName = "Split";
			const string rulebookDescription =
				"When [creature] is played, create a copy an open space on your side of the field.";
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, true);

			Texture2D defaultTexture = SigilUtils.LoadTextureFromResource(Resources.ability_split);
			
			var abIds = SigilUtils.GetAbilityId(info.rulebookName);
			NewAbility newAbility = new NewAbility(info, typeof(Split), defaultTexture, abIds);
			
			// set ability to behaviour class
			Split.ability = newAbility.ability;

			return newAbility;
		}
	}

	public class Split : AbilityBehaviour
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
				Plugin.Log.LogDebug(
					$"All slots are full, not spawning a copy of [{base.Card.Info.name}]");
			}
			else
			{
				Plugin.Log.LogDebug("Starting copy sequence");
				// now only check those filtered cards that have terrain traits
				foreach (var slot in slotsWithCards.Where(slot => slot && !slot.Card))
				{
					// >= 0 AND <= 2
					string cardToSpawn = base.Card.Info.name;
					Plugin.Log.LogDebug($"-> Spawning [{cardToSpawn}] in slot [{slot.name}]");
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