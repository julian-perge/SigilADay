using System.Collections;
using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using SigilADay_julianperge.lib;
using UnityEngine;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private NewAbility AddNest()
		{
			const string rulebookName = "Nest";
			const string description = "When attacked, [creature] will tutor for a card from a specific draw pile.";

			// setup ability
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, description, 3);
			
			List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
			DialogueEvent.Line line = new DialogueEvent.Line
			{
				text = "When attacked, this creature will tutor for a card from a specific draw pile."
			};
			lines.Add(line);
			info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

			// get and load artwork
			Texture2D sigilTex = new Texture2D(2, 2);
			sigilTex.LoadImage(Properties.Resources.ability_nest);

			// set ability to behavior class
			var abIds = SigilUtils.GetAbilityId(info.rulebookName);
			NewAbility newAbility = new NewAbility(info, typeof(Nest), sigilTex, abIds);
			Nest.ability = newAbility.ability;

			return newAbility;
		}
	}

	public class Nest : AbilityBehaviour
	{
		public override Ability Ability => ability;

		public static Ability ability;

		private List<CardInfo> _originalDeckCards;

		// todo: list of cards that are mediocre/decent
		private static readonly List<CardInfo> TutorCards = new List<CardInfo>
		{
			CardLoader.GetCardByName("Tail_Insect"),
			CardLoader.GetCardByName("RingWorm"),
			CardLoader.GetCardByName("Bee"),
			CardLoader.GetCardByName("RingWorm")
		};

		public override bool RespondsToTakeDamage(PlayableCard source)
		{
			// on first hit, assign original deck cards
			_originalDeckCards ??= Singleton<CardDrawPiles>.Instance.Deck.cards;
			return true;
		}

		public override IEnumerator OnTakeDamage(PlayableCard source)
		{
			yield return base.PreSuccessfulTriggerSequence();
			base.Card.Anim.StrongNegationEffect();
			yield return new WaitForSeconds(0.4f);

			// set the current deck to a copy of the tutored cards
			Singleton<CardDrawPiles>.Instance.Deck.cards = new List<CardInfo>(TutorCards);

			// now tutor
			yield return Singleton<CardDrawPiles>.Instance.Deck.Tutor();

			// after tutor, call the 3D pile (not regular CardDrawPiles) to destroy the cards
			// this is needed due to the fact that after tutoring, the game will set the deck to the list of tutored cards
			yield return Singleton<CardDrawPiles3D>.Instance.pile.DestroyCards();

			// now set the deck cards back to the original cards
			Singleton<CardDrawPiles>.Instance.Deck.cards = _originalDeckCards;

			// call SpawnCards to correctly show how many cards are left.
			yield return Singleton<CardDrawPiles3D>.Instance.pile.SpawnCards(_originalDeckCards.Count, 0.5f);

			yield return base.LearnAbility(0.5f);
			Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
			yield break;
		}
	}
}