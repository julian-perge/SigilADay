using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private static NewAbility AddCannibal()
		{
			// setup ability
			string rulebookName = $"Cannibal";
			const string rulebookDescription =
				"At the end of your turn, [creature] will steal 1 health from adjacent creatures of the same tribe.";
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, true);

			return SigilUtils.CreateAbility(
				typeof(Cannibal),
				Resources.ability_cannibal,
				rulebookName,
				rulebookDescription
			);
		}
	}

	public class Cannibal : AbilityBehaviour
	{
		public static Ability ability;

		public override Ability Ability => ability;

		// 1. turn ending
		// 2. does the card with this sigl respond?
		// 3. Getting adjacent slots of the card with the sigil
		// 4. do nothing on slots that have no cards

		public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
			return playerTurnEnd && !base.Card.Dead;
		}

		public override IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			yield return base.PreSuccessfulTriggerSequence();

			CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot, true);
			CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot, false);
			int healthToSteal = 0;

			bool toLeftHasMatchingTribe = AdjacentSlotHasMatchingTribe(toLeft);
			bool toRightHasMatchingTribe = AdjacentSlotHasMatchingTribe(toRight);

			// [X] = card with this ability
			// O O [X] X, toLeft will not be null, but card will be. toRight will not be null and card will not be null
			// [X] O O O, toLeft will be null since no slot exists after the farthest left slot

			if (toLeftHasMatchingTribe)
			{
				Plugin.Log.LogDebug(
					$"ToLeft exists, adjCard [{toLeft.Card.Info.name}] in slot [{toLeft}] will have 1 health stolen");
				healthToSteal++;
				yield return FriendlyCardTakesDamage(toLeft);
			}

			if (toRightHasMatchingTribe)
			{
				Plugin.Log.LogDebug(
					$"ToRight exists, adjCard [{toRight.Card.Info.name}] in slot [{toRight}] will have 1 health stolen");
				healthToSteal++;
				yield return FriendlyCardTakesDamage(toRight);
			}

			Plugin.Log.LogDebug($"Cannibal will steal [{healthToSteal}] from his friends!");
			CardModificationInfo mods = new CardModificationInfo
			{
				healthAdjustment = healthToSteal
			};

			yield return new WaitForSeconds(0.5f);
			// Add health adjustment to card with Cannibal
			base.Card.AddTemporaryMod(mods);

			yield return base.LearnAbility(0.5f);
			yield break;
		}

		private IEnumerator FriendlyCardTakesDamage(CardSlot friendlySlot)
		{
			Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
			yield return new WaitForSeconds(1f);
			// friendly cards cannot attack other friendly slots so this animation looks a little goofy
			base.Card.Anim.PlayAttackAnimation(base.Card.IsFlyingAttackingReach(), friendlySlot, null);
			yield return new WaitForSeconds(0.5f);
			yield return friendlySlot.Card.TakeDamage(1, base.Card);
			yield return new WaitForSeconds(0.1f);
		}

		private bool AdjacentSlotHasMatchingTribe(CardSlot adjSlot)
		{
			return (adjSlot && adjSlot.Card)
			       && ((base.Card.Info.tribes.Count == 0 && adjSlot.Card.Info.tribes.Count == 0)
			           || base.Card.Info.tribes.Exists(adjSlot.Card.Info.IsOfTribe));
		}
	}
}