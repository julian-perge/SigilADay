using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge;

public partial class Plugin
{
	private AbilityManager.FullAbility AddSplit()
	{
		// setup ability
		const string rulebookName = "Split";
		const string rulebookDescription =
			"When [creature] is played, create a copy on an open space on your side of the field.";

		return SigilUtils.CreateAbility<Split>(
			Resources.ability_split,
			rulebookName,
			rulebookDescription
		);
	}
}

public class Split : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		yield return base.PreSuccessfulTriggerSequence();
		yield return new WaitForSeconds(0.2f);

		// only get a list of player slots
		var openSlots = BoardManager.Instance.GetSlots(!base.Card.OpponentCard)
			.Where(slot => !slot.Card)
			.ToList();

		base.Card.Anim.StrongNegationEffect();
		if (!openSlots.IsNullOrEmpty())
		{
			// now only check if a card exists in that slot
			CardSlot slotToSpawnIn = openSlots.Randomize().First();
			Plugin.Log.LogDebug($"-> Spawning [{Card.Info.name}] in slot [{slotToSpawnIn.name}]");
			yield return BoardManager.Instance.CreateCardInSlot(CardLoader.GetCardByName(Card.Info.name), slotToSpawnIn);

			ViewManager.Instance.SwitchToView(View.Board);
			yield return new WaitForSeconds(0.2f);
		}

		yield return base.LearnAbility(0.25f);
	}
}
