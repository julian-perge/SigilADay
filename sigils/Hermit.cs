using System.Collections;
using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;

namespace SigilADay_julianperge
{

	public partial class Plugin
	{
		
		private NewSpecialAbility AddHermit()
		{
			var sId = SigilUtils.GetSpecialAbilityId("Hermit");

			var specialAbility = new NewSpecialAbility(typeof(Hermit), sId);
			Hermit.specialTriggeredAbility = specialAbility.specialTriggeredAbility;
			
			return specialAbility;
		}
		
	}
	
	public class Hermit : SpecialCardBehaviour
	{
		public static SpecialTriggeredAbility specialTriggeredAbility;
		
		private Transform pack;
		
		public override bool RespondsToResolveOnBoard()
		{
			return true;
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x00045A87 File Offset: 0x00043C87
		public override IEnumerator OnResolveOnBoard()
		{
			yield return new WaitForSeconds(0.4f);
			
			Vector3 b = new Vector3(-0.25f, 0.75f, 0f);
			
			Plugin.Log.LogDebug($"Tweening position for Vector3 b variable");
			Tween.Position(base.PlayableCard.transform, base.PlayableCard.Slot.transform.position + b, 3f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
			
			Vector3 cardRot = base.PlayableCard.transform.rotation.eulerAngles;
			Plugin.Log.LogDebug($"cardRot var [{cardRot.ToString()}]");
			
			Plugin.Log.LogDebug($"Rotating card");
			Tween.Rotate(base.PlayableCard.transform, new Vector3(0f, 0f, 25f), Space.World, 3f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
			
			GameObject gameObject = Object.Instantiate<GameObject>(ResourceBank.Get<GameObject>("Prefabs/Cards/SpecificCardModels/CardPack"));
			
			this.pack = gameObject.transform;
			Vector3 position = base.PlayableCard.Slot.transform.position;
			Vector3 position2 = position + Vector3.forward * 8f;
			
			Plugin.Log.LogDebug($"Setting pack position to position2 Vector3 var");
			this.pack.position = position2;
			Plugin.Log.LogDebug($"-> Tweening position");
			Tween.Position(this.pack, position, 3f, 0f, Tween.EaseOut, Tween.LoopType.None, null, null, true);
			
			yield return new WaitForSeconds(0.1f);

			Plugin.Log.LogDebug($"Rotating card with cardRot Vector3");
			Tween.Rotation(base.PlayableCard.transform, cardRot, 3f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
			
			base.PlayableCard.SlotHeightOffset = 0.17f;

			Plugin.Log.LogDebug($"Assigning base card to base slot");
			yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.PlayableCard, base.PlayableCard.Slot, 3f, null, true);
			
			yield return new WaitUntil(() => !Tween.activeTweens.Exists((TweenBase t) => t.targetInstanceID == base.PlayableCard.transform.GetInstanceID()));
			
			this.pack.SetParent(base.PlayableCard.transform);
			yield break;
		}
		
	}
	
	
}