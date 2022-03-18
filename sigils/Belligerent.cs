using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using JetBrains.Annotations;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge;

public partial class Plugin
{
	private AbilityManager.FullAbility AddBelligerent()
	{
		// setup ability
		const string rulebookName = "Belligerent";
		const string rulebookDescription =
			"When [creature] dies, create a copy of itself with a cumulative -1 to Power and Health. "
			+ "Has no effect on cards with 1 Health.";

		return SigilUtils.CreateAbility<Belligerent>(
			Resources.ability_belligerent,
			rulebookName,
			rulebookDescription
		);
	}
}

public class Belligerent : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	[CanBeNull]
	private CardModificationInfo MatchingLesserSingletonID()
	{
		Plugin.Log.LogDebug($"Checking mods for card {SigilUtils.GetLogOfCardInSlot(Card)}");
		return Card.Info.Mods.Count == 0
			? null
			: Card.Info.Mods.Find(mod => mod.singletonId.StartsWith($"lesserCard_{base.Card.Info.name}"));
	}

	private CardModificationInfo GetModdedInfo(int timesKilled)
	{
		return new CardModificationInfo
		{
			attackAdjustment = -timesKilled,
			healthAdjustment = -timesKilled,
			nullifyGemsCost = true,
			nameReplacement = "Lesser " + Card.Info.name,
			singletonId = $"lesserCard_{Card.Info.name}_{timesKilled}"
		};
	}

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
	{
		return true;
	}

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		yield return base.PreSuccessfulTriggerSequence();
		yield return new WaitForSeconds(0.3f);

		int timesKilled = 0;
		var belligMod = MatchingLesserSingletonID();

		if (belligMod is not null)
		{
			Plugin.Log.LogDebug(
				$"Belligerent mod exists for [{base.Card.Info.name}] SingletonID [{belligMod.singletonId}]"
			);
			timesKilled = int.Parse(belligMod.singletonId.Split('_')[2]);
		}

		if (Card.MaxHealth > 1)
		{
			Plugin.Log.LogDebug("Cloning Card");
			var lesserCard = (CardInfo)Card.Info.Clone();
			if (belligMod is not null)
			{
				Plugin.Log.LogDebug("Remove existing Belligerent mod");
				lesserCard.Mods.Remove(belligMod);
			}

			Plugin.Log.LogDebug($"Adding updated Belligerent mod, times killed [{timesKilled + 1}]");
			lesserCard.Mods.Add(GetModdedInfo(timesKilled + 1));

			yield return BoardManager.Instance.CreateCardInSlot(lesserCard, base.Card.Slot, 0.3f);
		}
		else
		{
			Plugin.Log.LogDebug($"{SigilUtils.GetLogOfCardInSlot(base.Card)} will not be reviving");
		}

		Plugin.Log.LogDebug($"Max Health [{base.Card.MaxHealth}] minus [{timesKilled}]");
		yield return base.LearnAbility(0.5f);
	}
}
