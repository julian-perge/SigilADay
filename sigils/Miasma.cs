﻿using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private NewAbility AddMiasma()
		{
			// setup ability
			const string rulebookName = "Miasma";
			const string rulebookDescription = "When [creature] dies, spawn a Greater Smoke creature in its place.";
			
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, true, 2);
			
			Texture2D tex = SigilUtils.LoadTextureFromResource(Resources.ability_miasma);

			var abIds = SigilUtils.GetAbilityId(info.rulebookName);
			
			NewAbility newAbility = new NewAbility(info, typeof(Miasma), tex, abIds);
			
			// set ability to behaviour class
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

			var greaterSmoke = CardLoader.GetCardByName("Smoke_Improved");

			yield return new WaitForSeconds(1f);
			
			yield return Singleton<BoardManager>.Instance.CreateCardInSlot(greaterSmoke, base.Card.Slot);

			yield return base.LearnAbility(0.5f);
			yield break;
		}
	}
}