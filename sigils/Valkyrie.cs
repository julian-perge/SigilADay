using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using UnityEngine;
using static SigilADay_julianperge.Plugin;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge;

public partial class Plugin
{
	public static readonly Vector2 DefaultVector2 = new Vector2(0.5f, 0.5f);
	public static readonly Rect DefaultCardArtRect = new Rect(0.0f, 0.0f, 114.0f, 94.0f);

	public const string NameNorseWarrior = $"{PluginGuid}_NorseWarrior";

	private void AddNorseWarrior()
	{
		var defaultTexture = SigilUtils.LoadTextureFromResource(Resources.card_norse_warrior);

		const string displayName = "Norse Warrior";
		const string desc = "Odin would be proud!";

		CardInfo cardInfo = ScriptableObject.CreateInstance<CardInfo>();
		cardInfo.name = NameNorseWarrior;
		cardInfo.displayedName = displayName;
		cardInfo.baseAttack = 1;
		cardInfo.baseHealth = 1;
		cardInfo.description = desc;
		cardInfo.cost = 1;
		cardInfo.portraitTex = Sprite.Create(defaultTexture, DefaultCardArtRect, DefaultVector2);
		cardInfo.abilities = new List<Ability> { Ability.Brittle };

		CardManager.Add(PluginGuid, cardInfo);
	}

	private AbilityManager.FullAbility AddFlightOfTheValkyrie()
	{
		AddNorseWarrior();

		const string rulebookName = "Flight of the Valkyrie";
		string rulebookDescription =
			"When [creature] is played, fill all open slots on your side of the field with 1/1 Norse Warriors. "
			+ $"[define:{NameNorseWarrior}]";

		return SigilUtils.CreateAbility<FlightOfTheValkyrie>(
			Resources.ability_fotv,
			rulebookName,
			rulebookDescription,
			3
		);
	}
}

public class FlightOfTheValkyrie : AbilityBehaviour
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

		var openSlots = BoardManager.Instance.GetSlots(!Card.OpponentCard)
			.Where(slot => !slot.Card)
			.ToList();

		base.Card.Anim.StrongNegationEffect();
		if (!openSlots.IsNullOrEmpty())
		{
			foreach (var slot in openSlots)
			{
				// >= 0 AND <= 2
				yield return BoardManager.Instance.CreateCardInSlot(CardLoader.GetCardByName(NameNorseWarrior), slot);
				break;
			}

			ViewManager.Instance.SwitchToView(View.Board);
			yield return new WaitForSeconds(0.2f);
		}

		yield return base.LearnAbility(0.25f);
	}
}
