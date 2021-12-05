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
		private NewAbility AddNest()
		{
			string rulebookName = $"[{PluginName}] Nest";
			const string rulebookDescription = "When [creature] is struck, you may select a card from a specific draw pile.";

			return SigilUtils.CreateAbility(
				typeof(Nest),
				Resources.ability_nest,
				rulebookName,
				rulebookDescription,
				3
			);
		}
	}

	public class Nest : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability => ability;

		private List<CardInfo> _originalDeckCards;

		// todo: list of cards that are mediocre/decent
		public static List<CardInfo> TutorCards = new();

		public override bool RespondsToTakeDamage(PlayableCard source)
		{
			// on first hit, assign original deck cards
			_originalDeckCards ??= Singleton<CardDrawPiles>.Instance.Deck.cards;
			return true;
		}

		public override IEnumerator OnTakeDamage(PlayableCard source)
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return base.LearnAbility(0.5f);
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

			Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
			yield break;
		}
	}
}