using System.Collections;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private NewAbility AddSplit()
		{
			// setup ability
			const string rulebookName = "Split";
			const string rulebookDescription =
				"When [creature] is played, create a copy on an open space on your side of the field.";

			return SigilUtils.CreateAbility<Split>(
				Resources.ability_split,
				rulebookName,
				rulebookDescription
			);
		}
	}

	public class Split : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability => ability;

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
			if (slotsWithCards.TrueForAll(slot => slot.Card))
			{
				Plugin.Log.LogDebug(
					$"All slots are full, not spawning a copy of [{base.Card.Info.name}]");
			}
			else
			{
				Plugin.Log.LogDebug("Starting copy sequence");

				// now only check if a card exists in that slot
				foreach (var slot in slotsWithCards.Where(slot => !slot.Card))
				{
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
