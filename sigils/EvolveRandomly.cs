using DiskCardGame;
using InscryptionAPI.Card;
using SigilADay_julianperge.Properties;

namespace SigilADay_julianperge;

public partial class Plugin
{
	private AbilityManager.FullAbility AddEvolveRandomly()
	{
		// setup ability
		const string rulebookName = "Evolve Randomly";
		const string rulebookDescription =
			"A card bearing this sigil will grow into a random form after 1 turn on the board.";

		return SigilUtils.CreateAbility<EvolveRandomly>(
			Resources.ability_randomEvolve,
			rulebookName,
			rulebookDescription
		);
	}
}

public class EvolveRandomly : Evolve
{
	public static void AddCardToDictionary(string name, List<string> evolutionCards)
	{
		CardsWithSpecifiedEvolutions.Add(
			name,
			evolutionCards
				.Select(CardLoader.GetCardByName)
				.ToList()
		);
	}

	public static readonly Dictionary<string, List<CardInfo>> CardsWithSpecifiedEvolutions = new();
	public static Ability ability;
	public override Ability Ability => ability;

	public override CardInfo GetTransformCardInfo()
	{
		if (CardsWithSpecifiedEvolutions.TryGetValue(base.Card.Info.name, out var listOfCards))
		{
			Plugin.Log.LogInfo($"[{base.Card.Info.name}] has random evolutions, choosing from list...");
			return listOfCards[UnityEngine.Random.Range(0, listOfCards.Count)];
		}

		Plugin.Log.LogWarning($"[{base.Card.Info.name}] has no random evolutions, choosing pure random card...");
		return CardLoader.GetRandomChoosableCard(SaveManager.SaveFile.randomSeed);
	}
}
