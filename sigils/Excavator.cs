using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge;

public partial class Plugin
{
	private AbilityManager.FullAbility AddExcavator()
	{
		// setup ability
		const string rulebookName = "Excavator";
		const string rulebookDescription =
			"When [creature] is played, remove all Terrain cards on your side of the field. " +
			"For each card removed, place a Squirrel in your hand.";

		return SigilUtils.CreateAbility<Excavator>(
			Resources.ability_excavator,
			rulebookName,
			rulebookDescription
		);
	}
}

public class Excavator : AbilityBehaviour
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

		// only get a list of slots that are not null and have cards in them
		var slotsContainingTerrain = BoardManager.Instance.PlayerSlotsCopy
			.Where(slot => slot && slot.Card && slot.Card.Info.HasTrait(Trait.Terrain))
			.ToList();

		ViewManager.Instance.SwitchToView(View.Board);
		// all the boulders need to die before we switch to our hand view to spawn the Squirrels

		// now only check those filtered cards that have terrain traits
		foreach (var slot in slotsContainingTerrain)
		{
			yield return new WaitForSeconds(0.1f);
			yield return slot.Card.Die(false, base.Card);
		}

		// this is copied from BeesOnHit Ability
		// switch to hand view
		if (ViewManager.Instance.CurrentView != View.Default)
		{
			yield return new WaitForSeconds(0.2f);
			ViewManager.Instance.SwitchToView(View.Default);
			yield return new WaitForSeconds(0.2f);
		}

		for (int i = 0; i < slotsContainingTerrain.Count; i++)
		{
			// Spawn squirrel in your hand
			yield return CardSpawner.Instance.SpawnCardToHand(CardLoader.GetCardByName("Squirrel"), null);
		}

		yield return base.LearnAbility(0.25f);
	}
}
