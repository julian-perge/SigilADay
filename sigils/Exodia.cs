using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private NewAbility AddAbilityExodia()
		{
			AddCardExodiaArms();
			
			const string rulebookName = "Obliterate";
			const string rulebookDescription
				= "When [creature] has the left and right arm cards of Exodia on each respective side, you win the match.";

			return SigilUtils.CreateAbility<Exodia>(
				Resources.ability_exodia,
				rulebookName,
				rulebookDescription,
				2
			);
		}
		
		private static void AddCardExodiaArms()
		{
		string NameLeftArm = $"{PluginName}_Left_Arm_Of_Exodia";
		string NameRightArm = $"{PluginName}_Right_Arm_Of_Exodia";
		
			List<CardMetaCategory> metaCategories = new()
			{
				CardMetaCategory.TraderOffer
			};

			Texture2D defaultTexLeftArm = SigilUtils.LoadTextureFromResource(Resources.card_exodia_left_arm);

			Texture2D defaultTexRightArm = SigilUtils.LoadTextureFromResource(Resources.card_exodia_right_arm);

			var displayNameLeft = "Left Arm Of Exodia";
			var displayNameRight = "Right Arm Of Exodia";

			var desc = "One of the arms of The Forbidden One.";

			// Left Arm
			NewCard.Add(NameLeftArm, displayNameLeft, 0, 1,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexLeftArm, onePerDeck: true
			);

			// Right Arm
			NewCard.Add(NameRightArm, displayNameRight, 0, 1,
				metaCategories, CardComplexity.Simple, CardTemple.Nature,
				desc, bloodCost: 1, defaultTex: defaultTexRightArm, onePerDeck: true
			);
		}
	}

	public class Exodia : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability => ability;

		private bool rightArmInCorrectSlot;
		private bool leftArmInCorrectSlot;

		public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			return base.Card.Slot != otherCard.Slot && !otherCard.Dead && otherCard.Slot.IsPlayerSlot;
		}

		public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
		{
			yield return base.PreSuccessfulTriggerSequence();

			if (this.DoAdjacentSlotsHaveArms(otherCard))
			{
				Plugin.Log.LogDebug($"EXODIA IS GOING TO OBLITERATE");
				Singleton<TextDisplayer>.Instance.StartCoroutine(
					Singleton<TextDisplayer>.Instance.ShowUntilInput("EXODIA, OBLITERATE!", -0.65f, 0.4f,
						Emotion.Laughter, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Bonelord)
				);
				yield return new WaitForSeconds(1f);

				int dmgToWin = Singleton<LifeManager>.Instance.DamageUntilPlayerWin;
				// show damage on scale. Code taken from Pliers and modified the dmg done
				yield return Singleton<LifeManager>.Instance.ShowDamageSequence(
					dmgToWin,
					1, false, 0.25f,
					ResourceBank.Get<GameObject>("Prefabs/Environment/ScaleWeights/Weight_RealTooth"),
					0.25f);
			}

			yield return base.LearnAbility(0.5f);
			yield break;
		}

		private bool DoAdjacentSlotsHaveArms(PlayableCard otherCard)
		{
			CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
			CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
			// the slots to check are switched since facing Exodia, his right arm will be on th left side
			// RIGHT ARM, EXODIA, LEFT ARM
			if (otherCard.name.Contains("Right Arm") && otherCard.Slot == toLeft)
			{
				Plugin.Log.LogDebug($"Setting right arm slot to true");
				rightArmInCorrectSlot = true;
			}

			if (otherCard.name.Contains("Left Arm") && otherCard.Slot == toRight)
			{
				Plugin.Log.LogDebug($"Setting left arm slot to true");
				leftArmInCorrectSlot = true;
			}

			return leftArmInCorrectSlot && rightArmInCorrectSlot;
		}
	}
}