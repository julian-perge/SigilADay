using System.Collections;
using APIPlugin;
using DiskCardGame;
using SigilADay_julianperge.Properties;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		private NewAbility AddBelligerent()
		{
			// setup ability
			string rulebookName = $"[{PluginName}] Belligerent";
			const string rulebookDescription =
				"When [creature] dies, create a copy of itself with a cumulative -1 to Power and Health. " +
				"Has no effect on cards with 1 Health.";

			return SigilUtils.CreateAbility(
				typeof(Belligerent),
				Resources.ability_belligerent,
				rulebookName,
				rulebookDescription
			);
		}
	}

	public class Belligerent : AbilityBehaviour
	{
		public static Ability ability;
		public override Ability Ability => ability;

		private int timesKilled = 0;

		private CardModificationInfo GetModdedInfo()
		{
			var cardModificationInfo = new CardModificationInfo
			{
				attackAdjustment = -timesKilled,
				healthAdjustment = -timesKilled,
				nullifyGemsCost = true,
				nameReplacement = "Lesser " + base.Card.Info.name,
				singletonId = "lesserCard" + base.Card.Info + timesKilled
			};

			return cardModificationInfo;
		}

		public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
		{
			return true;
		}

		public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
		{
			timesKilled++;
			Plugin.Log.LogDebug($"{SigilUtils.GetLogOfCardInSlot(base.Card)} Times killed [{timesKilled}]");
			if (base.Card.MaxHealth - timesKilled > 1)
			{
				Plugin.Log.LogDebug($"Max Health [{base.Card.MaxHealth}] minus [{timesKilled}]");

				CardInfo cardWithMods = base.Card.Info.Clone() as CardInfo;
				cardWithMods.Mods.Add(GetModdedInfo());

				Plugin.Log.LogDebug($"Added temp mods to cloned card.");

				yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardWithMods, base.Card.Slot);
			}

			yield break;
		}
	}
}