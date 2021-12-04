using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private static NewAbility AddBrimstone()
		{
			// setup ability
			const string rulebookName = "Brimstone";
			const string rulebookDescription =
				"Does not affect Terrain or Pelts. When [creature] damages another card, any overkill damage is set to 1. " +
				"If no card is queued to take overkill damage, your opponent takes 1 damage instead.";
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, true);

			Texture2D tex = SigilUtils.LoadTextureFromResource(Resources.ability_brimstone);

			var abIds = SigilUtils.GetAbilityId(info.rulebookName);
			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(Brimstone), tex, abIds);
			Brimstone._Ability = newAbility.ability;

			return newAbility;
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
				attackingSlotIsPlayerCard && attackingSlot.Card.Info.HasAbility(Brimstone._Ability);
			if (damage > 0 && attackingSlotHasBrimstone)
			{
				Plugin.Log.LogDebug($"{SigilUtils.GetLogOfCardInSlot(attackingSlot.Card)} - Setting damage to 1 for Brimstone");
				damage = 1;
			}
		}

		// [HarmonyPostfix]
		// public static IEnumerator Postfix(IEnumerator values)
		// {
		// 	// 1. WaitForSeconds 0.1f
		// 	// 2. WaitForSeconds 0.3f
		// 	// 3. WaitForSeconds 0.3f
		// 	// 4. ShowCardBlocked if queuedCard has PreventAttack
		// 	// 4. PreOverkillDamage if queuedCard has PreventAttack
		//
		// 	Plugin.Log.LogDebug($"CombatPhaseManager Postfix, values.Current [{values.Current}]");
		// 	while (values.MoveNext())
		// 	{
		// 		var iter = values.Current;
		// 		Plugin.Log.LogDebug($"-> Value [{iter}]");
		// 		yield return iter;
		// 	}
		// }
	}
	
	// on attack ended will be AFTER DealOverkillDamage
	// Scenario 1. I would deal 10 damage to the creature opposing me, and 1 to the creature behind it
	// Scenario 2. I would deal 10 damage to the creature opposing me, and 1 to Leshy if no card exists behind the opposing card
	// Scenario 3. I would deal 10 damage to the terrain card, and no damage done to the card behind it or Leshy
	// Scenario 4. No card exists for attacking, therefore no extra overkill damage is done  
	public class Brimstone : AbilityBehaviour
	{
		public static Ability _Ability;
		public override Ability Ability => _Ability;
		private bool willDealOverkillDamage;

		public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
			PlayableCard killer)
		{
			return killer == base.Card;
		}

		public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
			PlayableCard killer)
		{
			yield return base.PreSuccessfulTriggerSequence();
			if (willDealOverkillDamage)
			{
				Plugin.Log.LogDebug(
					$"[OnOtherCardDie] {SigilUtils.GetLogOfCardInSlot(base.Card)} with Brimstone dealing 1 damage directly.");
				yield return base.LearnAbility(0.25f);
				yield return Singleton<LifeManager>.Instance.ShowDamageSequence(1, 1, false);
			}

			yield break;
		}

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
			this.willDealOverkillDamage = slot.Card
			                              && !Singleton<BoardManager>.Instance.GetCardQueuedForSlot(slot)
			                              && base.Card.Attack > slot.Card.Health;
			yield break;
		}
	}
}