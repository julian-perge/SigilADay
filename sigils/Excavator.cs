using System.Collections;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private NewAbility AddExcavator()
		{
			// setup ability
			const string rulebookName = "Excavator";
			const string rulebookDescription =
				"When [creature] is played, remove all Terrain cards on your side of the field. For each card removed, place a Squirrel in your hand.";
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, rulebookDescription, true);

			Texture2D tex = SigilUtils.LoadTextureFromResource(Resources.ability_excavator);

			var abIds = SigilUtils.GetAbilityId(info.rulebookName);
			NewAbility newAbility = new NewAbility(info, typeof(Excavator), tex, abIds);
			
			// set ability to behaviour class
			Excavator.ability = newAbility.ability;

			return newAbility;
		}
	}

	public class Excavator : AbilityBehaviour
	{
		public override Ability Ability => ability;

		public static Ability ability;

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