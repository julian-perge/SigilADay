﻿using System.Collections;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using SigilADay_julianperge.Properties;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private static NewAbility AddBrimstone()
		{
			// setup ability
			const string rulebookName = "Brimstone";
			const string rulebookDescription =
				"Does not affect Terrain or Pelts. " +
				"When [creature] damages another card, overkill damage will always happen, but damage is set to 1. " +
				"If no card is queued to take overkill damage, your opponent takes 1 damage instead.";

			return SigilUtils.CreateAbility<Brimstone>(
				Resources.ability_brimstone,
				rulebookName,
				rulebookDescription
			);
		}
	}

	[HarmonyPatch(typeof(CombatPhaseManager), nameof(CombatPhaseManager.DealOverkillDamage))]
	public class DealOverkillDamageBrimstonePatch
	{
		[HarmonyPrefix]
		public static void CheckCardForBrimstoneAbilityPatch(ref int damage, CardSlot attackingSlot, CardSlot opposingSlot)
		{
			bool attackingSlotIsPlayerCard = attackingSlot.Card is not null && attackingSlot.IsPlayerSlot;
			bool attackingSlotHasBrimstone =
				attackingSlotIsPlayerCard && attackingSlot.Card.Info.HasAbility(Brimstone.ability);
			if (attackingSlotHasBrimstone)
			{
				Plugin.Log.LogDebug($"{SigilUtils.GetLogOfCardInSlot(attackingSlot.Card)} - Setting damage to 1 for Brimstone");
				damage = 1;
			}
		}
	}

	// on attack ended will be AFTER DealOverkillDamage
	// Scenario 1. I would deal 10 damage to the creature opposing me, and 1 to the creature behind it
	// Scenario 2. I would deal 10 damage to the creature opposing me, and 1 to Leshy if no card exists behind the opposing card
	// Scenario 3. I would deal 10 damage to the terrain card, and no damage done to the card behind it or Leshy
	// Scenario 4. No card exists for attacking, therefore no extra overkill damage is done
	public class Brimstone : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability => ability;

		private bool willDealDamageToOpponent;

    // happens BEFORE overkill damage
    public override bool RespondsToCardGettingAttacked(PlayableCard source)
    {
      return source == base.Card.Slot.opposingSlot.Card;
    }

    public override IEnumerator OnCardGettingAttacked(PlayableCard card)
    {
      yield return base.PreSuccessfulTriggerSequence();
      if (willDealDamageToOpponent)
      {
        Plugin.Log.LogDebug(
          $"[OnCardGettingAttacked] {SigilUtils.GetLogOfCardInSlot(base.Card)} with Brimstone dealing 1 damage directly.");
        // yield return base.LearnAbility(0.25f);
        yield return Singleton<LifeManager>.Instance.ShowDamageSequence(1, 1, false);
      }
    }

    // happens BEFORE OnCardGettingAttacked AND overkill damage
		public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
		{
			Plugin.Log.LogDebug(
				$"[RespondsToSlotTargetedForAttack] Attacker is equal to base.Card? [{attacker == Card}] Slot [{attacker.Slot.Index}]");
			return attacker == Card;
		}

		public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
		{
			// Plugin.Log.LogDebug($"[OnSlotTargetedForAttack] Setting {SigilUtils.GetLogOfCardInSlot(attacker)} startedAttack to true");
			// card exists in opposing slot
			// AND, no card exists in queued slot BEHIND slot that was targeted
			this.willDealDamageToOpponent = slot.Card && !Singleton<BoardManager>.Instance.GetCardQueuedForSlot(slot);
			yield break;
		}
	}
}
