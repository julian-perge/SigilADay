using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using SigilADay_julianperge.lib;
using UnityEngine;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private NewAbility AddExcavator()
		{
			// setup ability
			const string rulebookName = "Excavator";
			const string desc =
				"When played, remove all Terrain cards on your side of the field. For each card removed, place a Squirrel in your hand.";
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, desc);
			
			List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
			DialogueEvent.Line line = new DialogueEvent.Line
			{
				text = "When played, remove all Terrain cards on your side of the field. For each card removed, place a Squirrel in your hand."
			};
			lines.Add(line);
			info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(SigilADay_julianperge.Properties.Resources.ability_excavator);

			var abIds = SigilUtils.GetAbilityId(info.rulebookName);
			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(Excavator), tex, abIds);
			Excavator.ability = newAbility.ability;

			return newAbility;
		}
	}

	public class Excavator : CustomAbilityBehaviour
	{
		public override bool RespondsToResolveOnBoard()
		{
			return true;
		}

		public override IEnumerator OnResolveOnBoard()
		{
			yield return base.PreSuccessfulTriggerSequence();

			// only get a list of slots that are not null and have cards in them
			var slotsWithCards = Singleton<BoardManager>
				.Instance
				.GetSlots(true)
				.Where(slot => slot && slot.Card);

			Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
			// all the boulders need to die before we switch to our hand view to spawn the Squirrels
			int numberOfCardsToSpawn = 0;

			// now only check those filtered cards that have terrain traits
			foreach (var slot in slotsWithCards.Where(slot => slot.Card.Info.HasTrait(Trait.Terrain)))
			{
				numberOfCardsToSpawn++;
				yield return new WaitForSeconds(0.1f);
				yield return slot.Card.Die(false, base.Card);
			}

			// this is copied from BeesOnHit Ability
			// switch to hand view
			if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
			{
				yield return new WaitForSeconds(0.2f);
				Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
				yield return new WaitForSeconds(0.2f);
			}

			for (int i = 0; i < numberOfCardsToSpawn; i++)
			{
				// Spawn squirrel in your hand
				yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName("Squirrel"), null);
			}

			yield return base.LearnAbility(0.25f);
			yield break;
		}
	}
}