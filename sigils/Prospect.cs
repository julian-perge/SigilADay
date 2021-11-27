using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private NewAbility AddProspect()
		{
			AddGoldenNugget();
			// setup ability
			const string rulebookName = "Prospect";
			const string rulebookDescription =
				"When [creature] attacks another creature, that creature turns into a Gold Nugget. 1 gold tooth is found upon being destroyed.";
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription);

			Texture2D tex = SigilUtils.LoadTextureFromResource(Resources.ability_prospect);

			var abIds = SigilUtils.GetAbilityId(info.rulebookName);
			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(Prospect), tex, abIds);
			Prospect.ability = newAbility.ability;

			return newAbility;
		}

		private NewAbility AddGoldenNugget()
		{
			const string rulebookName = "Golden Nugget";
			const string rulebookDescription = "1 gold tooth is found upon being destroyed.";
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription);

			var abId = SigilUtils.GetAbilityId(info.rulebookName);

			NewAbility newAbilityNugget = new NewAbility(info, typeof(GoldenNugget), Texture2D.blackTexture, abId);
			GoldenNugget.ability = newAbilityNugget.ability;

			return newAbilityNugget;
		}
	}

	public class GoldenNugget : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability => ability;

		public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
		{
			return true;
		}

		public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
		{
			AudioController.Instance.PlaySound3D("sharp_object_up#1", MixerGroup.ExplorationBGM, Vector3.zero, 1f, 0f,
				new AudioParams.Pitch(AudioParams.Pitch.Variation.Small));
			yield return new WaitForSeconds(0.25f);
			RunState.Run.currency++;
			yield return new WaitForSeconds(0.25f);
			AudioController.Instance.PlaySound3D("metal_drop", MixerGroup.ExplorationSFX, Vector3.zero, 0.6f, 0f,
				new AudioParams.Pitch(0.7f), null, new AudioParams.Randomization());
			yield return new WaitForSeconds(0.25f);
		}

		public static CardInfo GetModdedNugget()
		{
			Plugin.Log.LogDebug($"Creating modified nugget");
			var moddedNugget = CardLoader.GetCardByName("GoldNugget");
			CardModificationInfo modInfo = new CardModificationInfo
			{
				abilities = new List<Ability> { GoldenNugget.ability },
				nonCopyable = true,
				nameReplacement = "Golden Nugget"
			};
			moddedNugget.Mods.Add(modInfo);
			return moddedNugget;
		}
	}

	public class Prospect : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability => ability;
		private List<CardSlot> slotsToSpawnNuggets = new();

		public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
		{
			Plugin.Log.LogDebug($"Attacker slot is equal to base.Card.Slot [{attacker.Slot == Card.Slot}]");
			Plugin.Log.LogDebug($"Attacker [{attacker.Info.name}] slot.Card is valid [{(bool)slot.Card}]");
			return attacker.Slot == Card.Slot && slot.Card;
		}

		public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
		{
			Plugin.Log.LogDebug($"Assigning [{slot.Card.Info.name}] slot to list of card slots");
			// assign card being attacked to variable
			if (!slot.Card.Info.name.Equals("GoldNugget"))
			{
				slotsToSpawnNuggets.Add(slot);
			}
			yield break;
		}

		public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
		{
			return attacker == base.Card;
		}

		public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
		{
			if (!target.Dead)
			{
				Plugin.Log.LogDebug($"Killing card [{target.Info.name}]");
				yield return target.Die(false);
				yield return new WaitForSeconds(0.25f);
			}

			yield break;
		}

		public override bool RespondsToAttackEnded()
		{
			return slotsToSpawnNuggets.Count > 0;
		}

		public override IEnumerator OnAttackEnded()
		{
			yield return PreSuccessfulTriggerSequence();

			Plugin.Log.LogDebug($"Setting view to board");
			Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);

			foreach (var slot in slotsToSpawnNuggets)
			{
				// Plugin.Log.LogDebug($"Card [{card.Info.name}] is still alive? [{!card.Dead}]");

				yield return new WaitForSeconds(0.25f);
				AudioController.Instance.PlaySound3D("metal_object_hit#1", MixerGroup.TableObjectsSFX,
					Vector3.zero, 1f, 0f, new AudioParams.Pitch(AudioParams.Pitch.Variation.Medium));

				yield return new WaitForSeconds(0.25f);
				yield return Singleton<BoardManager>.Instance.CreateCardInSlot(GoldenNugget.GetModdedNugget(), slot);
			}
			
			slotsToSpawnNuggets.Clear();
			yield return LearnAbility(0.5f);
			yield return new WaitForSeconds(0.2f);
		}
	}
}