using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using UnityEngine;
using Resources = SigilADay_julianperge.Properties.Resources;

namespace SigilADay_julianperge;

public partial class Plugin
{
	private AbilityManager.FullAbility AddHermit()
	{
		const string rulebookName = "Hermit";
		const string rulebookDescription =
			"At the start of your turn, if [creature] is next to a Terrain or Pelt card, creature will hide under it.";

		return SigilUtils.CreateAbility<Hermit>(
			Resources.ability_hermit,
			rulebookName,
			rulebookDescription
		);
	}
}

public class Hermit : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;
	private Transform _pack;

	public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		return !base.Card.FaceDown && attacker == base.Card && Card.CanAttackDirectly(slot);
	}

	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		CardSlot toLeft = BoardManager.Instance.GetAdjacent(base.Card.Slot, true);
		CardSlot toRight = BoardManager.Instance.GetAdjacent(base.Card.Slot, false);
		if (SigilUtils.IsNonLivingCard.Invoke(toRight.Card))
		{
			var toRightSlotTransform = toRight.transform.position;
			yield return new WaitForSeconds(0.4f);

			Vector3 b = new Vector3(-0.25f, 0.75f, 0f);

			Plugin.Log.LogDebug("Tweening position for Vector3 b variable");
			Tween.Position(toRight.Card.transform, toRightSlotTransform + b, 3f, 0f, Tween.EaseInOut);

			Vector3 cardRot = toRight.Card.transform.rotation.eulerAngles;
			Plugin.Log.LogDebug($"cardRot var [{cardRot.ToString()}]");

			Plugin.Log.LogDebug("Rotating card");
			Tween.Rotate(toRight.Card.transform, new Vector3(0f, 0f, 25f), Space.World, 3f, 0f, Tween.EaseInOut);

			GameObject gameObject = base.Card.gameObject;

			this._pack = gameObject.transform;
			Vector3 position = toRightSlotTransform;
			Vector3 position2 = position + Vector3.forward * 8f;

			Plugin.Log.LogDebug("Setting pack position to position2 Vector3 var");
			this._pack.position = position2;
			Plugin.Log.LogDebug("-> Tweening position");
			Tween.Position(base.Card.transform, position, 3f, 0f, Tween.EaseOut);

			yield return new WaitForSeconds(0.1f);

			Plugin.Log.LogDebug("Rotating card with cardRot Vector3");
			Tween.Rotation(toRight.Card.transform, cardRot, 3f, 0f, Tween.EaseInOut);

			toRight.Card.SlotHeightOffset = 0.17f;

			Plugin.Log.LogDebug("Assigning base card to base slot");
			yield return BoardManager.Instance.AssignCardToSlot(toRight.Card, toRight.Card.Slot, 3f);

			Plugin.Log.LogDebug("Waiting until active tween no longer exists in nonliving card");
			yield return new WaitUntil(() =>
				!Tween.activeTweens.Exists(t => t.targetInstanceID == toRight.Card.transform.GetInstanceID()));

			Plugin.Log.LogDebug("Setting base card face down like submerge");
			base.Card.SetFaceDown(true);

			Plugin.Log.LogDebug("Setting base card with parent of nonliving card");
			base.Card.transform.SetParent(this._pack);
		}
	}
}
