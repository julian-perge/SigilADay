﻿using APIPlugin;
using DiskCardGame;
using SigilADay_julianperge.lib;
using System.Collections;
using UnityEngine;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private NewAbility AddMiasma()
		{
			// setup ability
			const string rulebookName = "Miasma";
			const string desc = "When [creature] dies, spawn a Greater Smoke creature in its place.";
			string dialogue = desc.Replace("[creature]", "this creature");
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, desc);

			info.abilityLearnedDialogue = SigilUtils.SetAbilityInfoDialogue(dialogue);

			Texture2D tex = SigilUtils.LoadTextureFromResource(Properties.Resources.ability_miasma);

			var abIds = SigilUtils.GetAbilityId(info.rulebookName);
			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(Miasma), tex, abIds);
			Miasma.ability = newAbility.ability;

			return newAbility;
		}
	}

	public class Miasma : AbilityBehaviour
	{
		public override Ability Ability => ability;

		public static Ability ability;

		public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
		{
			return true;
		}

		public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
		{
			yield return base.PreSuccessfulTriggerSequence();

			var greaterSmoke = CardSpawner.SpawnPlayableCard(CardLoader.GetCardByName("Smoke_Improved"));

			yield return new WaitForSeconds(1f);
			
			yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(greaterSmoke, base.Card.Slot);

			yield return base.LearnAbility(0.5f);
			yield break;
		}
	}
}