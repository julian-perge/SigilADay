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
		private NewAbility AddBelligerent()
		{
			// setup ability
			const string rulebookName = "Belligerent";
			const string rulebookDescription =
				"When [creature] dies, create a copy of itself with a cumulative -1 to Power and Health. Has no effect on cards with 1 Health.";
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription);

			Texture2D tex = SigilUtils.LoadTextureFromResource(Resources.ability_belligerent);

			var abIds = SigilUtils.GetAbilityId(info.rulebookName);
			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(Belligerent), tex, abIds);
			Belligerent.ability = newAbility.ability;

			return newAbility;
		}
	}

	public class Belligerent : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability => ability;

		private Dictionary<string, int> timesKilled = new Dictionary<string, int>();

		public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
		{
			return true;
		}

		public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
		{
			if (base.Card.MaxHealth > 1)
			{
				CardInfo cardByName = CardLoader.GetCardByName(base.Card.Info.name);
				CardModificationInfo cardModificationInfo = new CardModificationInfo
				{
					attackAdjustment = cardByName.Attack - 1,
					healthAdjustment = cardByName.Health - 1,
					nullifyGemsCost = true,
					nameReplacement = "Lesser " + base.Card.Info.name,
					abilities = new List<Ability> { ability },
					singletonId = "lesserCard" + base.Card.Info + timesKilled.Count
				};
				cardByName.Mods.Add(cardModificationInfo);
				yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardByName, base.Card.Slot);
			}

			yield break;
		}
	}
}