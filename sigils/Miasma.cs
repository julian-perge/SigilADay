using System.Collections;
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
			const string rulebookDescription = "When [creature] dies, spawn a Greater Smoke in its place.";

			return SigilUtils.CreateAbility<Miasma>(
				Resources.ability_miasma,
				rulebookName,
				rulebookDescription,
				2
			);
		}
	}

	public class Miasma : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability => ability;

		public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
		{
			return true;
		}

		public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return base.LearnAbility(0.5f);

			var greaterSmoke = CardLoader.GetCardByName("Smoke_Improved");

			yield return new WaitForSeconds(1f);

			yield return Singleton<BoardManager>.Instance.CreateCardInSlot(greaterSmoke, base.Card.Slot);
			yield break;
		}
	}
}