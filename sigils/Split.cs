using System.Collections;
using System.Collections.Generic;
using System.Linq;
using APIPlugin;
using DiskCardGame;
using SigilADay.lib;
using UnityEngine;

namespace SigilADay
{
	public partial class Plugin
	{
		public NewAbility AddSplit()
		{
			// setup ability
			const string rulebookName = "Split";
			const string desc =
				"When played, [creature] will create a copy of itself in an open space on your side of the field.";
			AbilityInfo info = SigilUtils.CreateInfoWithDefaultSettings(rulebookName, desc);

			// lines
			List<DialogueEvent.Line> lines = new List<DialogueEvent.Line>();
			DialogueEvent.Line line = new DialogueEvent.Line
			{
				text = "When played, this creature will create a copy of itself in an open space on your side of the field."
			};
			lines.Add(line);
			info.abilityLearnedDialogue = new DialogueEvent.LineSet(lines);

			// texture
			Texture2D tex = SigilUtils.LoadImageAndGetTexture("ability_split.png");

			var abIds = AbilityIdentifier.GetAbilityIdentifier(PluginGuid, info.rulebookName);
			// set ability to behavior class
			NewAbility newAbility = new NewAbility(info, typeof(Split), tex, abIds);
			Split.ability = newAbility.ability;

			return newAbility;
		}
	}

	public class Split : CustomAbilityBehaviour
	{
		public override bool RespondsToResolveOnBoard()
		{
			return true;
		}

		public override IEnumerator OnResolveOnBoard()
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return new WaitForSeconds(0.2f);

			// only get a list of player slots
			var slotsWithCards = Singleton<BoardManager>.Instance.GetSlots(true);

			base.Card.Anim.StrongNegationEffect();
			if (slotsWithCards.TrueForAll(slot => slot && slot.Card))
			{
				Plugin.Log.LogDebug($"All slots are full, not spawning a copy of [{base.Card.Info.name}]");
			}
			else
			{
				Plugin.Log.LogDebug("Starting copy sequence");
				// now only check those filtered cards that have terrain traits
				foreach (var slot in slotsWithCards.Where(slot => slot && !slot.Card))
				{
					// >= 0 AND <= 2
					string cardToSpawn = base.Card.Info.name;
					Plugin.Log.LogDebug($"-> Spawning [{cardToSpawn}] in slot [{slot.name}]");
					PlayableCard copy = CardSpawner.SpawnPlayableCard(CardLoader.GetCardByName(cardToSpawn));
					yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(copy, slot);
					break;
				}

				Singleton<ViewManager>.Instance.SwitchToView(View.Board);
				yield return new WaitForSeconds(0.2f);
			}

			yield return base.LearnAbility(0.25f);
			yield break;
		}
	}
}