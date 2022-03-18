using DiskCardGame;
using Random = UnityEngine.Random;

namespace SigilADay_julianperge;

public partial class Plugin
{
	private const string CardsForNest = "Cards for Nest";

	public bool IsCompleteRandomEnabled()
		=> Config.Bind(
				CardsForNest,
				"Pure randomness?",
				false,
				"If set to true, this will set the tutored deck to a random set of cards. Overrides all other settings here."
			)
			.Value;

	public bool IsBonesSearchEnabled()
		=> Config.Bind(
				CardsForNest,
				"Allow searching for Bones cost cards?",
				true,
				"If set to true, this will include bone cost cards to be added to the tutored deck."
			)
			.Value;

	public bool IsBloodSearchEnabled()
		=> Config.Bind(
				CardsForNest,
				"Allow searching for Blood cost cards?",
				true,
				"If set to true, this will include blood cost cards to be added to the tutored deck."
			)
			.Value;

	public int GetNumCardsForDrawPile()
		=> Config.Bind(
				CardsForNest,
				"How many cards to have in draw pile to search for?",
				4,
				"Max value is 10"
			)
			.Value;

	public string GetListOfCardsOrCostForNest()
		=> Config.Bind(
				CardsForNest,
				"List of cards",
				"Skunk,Shark,Wolf",
				"Set list of possible cards to tutor for. Comma separated."
				+ "\nIf a number is set, will randomly generate cards of that blood cost."
				+ "\nIf allowing to search for bones cards is set to true, all bones cards will be searched for due to the limited number of bones cards in Act 1."
				+ "\ne.g. 3 will generate cards of only 3 blood cost type."
				+ "\nIf the cards added below is greater than 10, only those first 10 will be chosen"
				+ "\nIf you have set the draw pile number higher than the list provided, random cards will be added until total has been reached."
			)
			.Value;

	private List<CardInfo> InitializeConfigNest()
	{
		int randomSeed = SaveManager.SaveFile.randomSeed;
		List<CardInfo> listOfCards = new List<CardInfo>();
		int numberOfCardsForDeck = GetNumCardsForDrawPile();
		if (IsCompleteRandomEnabled())
		{
			Log.LogDebug($"Complete randomness is enabled for Nest! Generating [{numberOfCardsForDeck}]");
			for (int i = 0; i < numberOfCardsForDeck; i++)
			{
				listOfCards.Add(CardLoader.GetRandomChoosableCard(randomSeed));
			}
		}
		else
		{
			bool isBloodSearchEnabled = IsBloodSearchEnabled();
			bool isBonesSearchEnabled = IsBonesSearchEnabled();
			string listOfCardsOrNumberCost = GetListOfCardsOrCostForNest();
			bool isCardListANumber = int.TryParse(listOfCardsOrNumberCost, out var bloodCost);
			for (int i = 0; i < numberOfCardsForDeck; i++)
			{
				if (isCardListANumber)
				{
					randomSeed = AddCardsBasedOnBloodAndBones(
						isBloodSearchEnabled,
						isBonesSearchEnabled,
						listOfCards,
						randomSeed,
						bloodCost
					);
				}
				else
				{
					var splitCards = listOfCardsOrNumberCost.Split(',').Select(CardLoader.GetCardByName).ToList();
					listOfCards.AddRange(splitCards);
					break;
				}
			}

			if (numberOfCardsForDeck > listOfCards.Count)
			{
				Log.LogDebug($"Generating an extra [{numberOfCardsForDeck - listOfCards.Count}] cards to fill up deck");
				listOfCards.AddRange(
					GetRandomBloodOrBonesCards(
						ref randomSeed,
						numberOfCardsForDeck - listOfCards.Count
					)
				);
			}
		}

		return listOfCards;
	}

	private static int AddCardsBasedOnBloodAndBones(
		bool isBloodSearchEnabled,
		bool isBonesSearchEnabled,
		List<CardInfo> listOfCards,
		int randomSeed,
		int bloodCost
	)
	{
		if (isBloodSearchEnabled && isBonesSearchEnabled)
		{
			listOfCards.Add(GetRandomBloodOrBonesCard(ref randomSeed, bloodCost));
		}
		else if (isBonesSearchEnabled)
		{
			listOfCards.Add(CardLoader.GetRandomChoosableBonesCard(randomSeed++));
		}
		else if (isBloodSearchEnabled)
		{
			listOfCards.Add(CardLoader.GetRandomChoosableCardWithCost(randomSeed++, bloodCost));
		}
		else
		{
			listOfCards.Add(CardLoader.GetRandomChoosableCard(randomSeed++));
		}

		return randomSeed;
	}

	private static List<CardInfo> GetRandomBloodOrBonesCards(ref int randomSeed, int count, int bloodCost = -1)
	{
		var cards = new List<CardInfo>();
		for (int i = 0; i < count; i++)
		{
			cards.Add(GetRandomBloodOrBonesCard(ref randomSeed, bloodCost));
		}

		return cards;
	}

	private static CardInfo GetRandomBloodOrBonesCard(ref int randomSeed, int bloodCost = -1)
	{
		if (bloodCost == -1)
		{
			bloodCost = Math.Max(0, Random.RandomRangeInt(1, 5));
		}

		return Random.RandomRangeInt(0, int.MaxValue) % 2 == 0
			? CardLoader.GetRandomChoosableCardWithCost(randomSeed++, bloodCost)
			: CardLoader.GetRandomChoosableBonesCard(randomSeed++);
	}
}
