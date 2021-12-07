using System.Collections.Generic;
using System.Linq;
using DiskCardGame;

namespace SigilADay_julianperge
{
	public partial class Plugin
	{
		public int GetNumCardsForRandomEvolve()
			=> Config.Bind("EvolveRandomly", "Sets of cards to modify", 1).Value;

		public string GetCardForRandomEvolve(int index)
			=> Config.Bind("EvolveRandomly", "Card" + (index + 1),
				"Geck-Skunk,RavenEgg",
				"Set list of possible cards to evolve into.\n" +
				"Card to set specified evolutions to, has a hyphen before the other cards.\n"
				+ "e.g. Geck-Skink,Raven will set the Geck card with 2 possible evolutions.").Value;

		private void InitializeEvolveRandomlyCardListConfig()
		{
			int numOfCards = GetNumCardsForRandomEvolve();

			for (var i = 0; i < numOfCards; i++)
			{
				string cardWithHyphenAndCommas = GetCardForRandomEvolve(i);
				Plugin.Log.LogDebug($"Card #[{i + 1}] has [{cardWithHyphenAndCommas}]");

				string[] split = cardWithHyphenAndCommas.Split('-');

				string cardToModify = split[0];

				if (ConfigUtils.CardDoesNotExistInGameOrApiLoad(cardToModify))
				{
					Plugin.Log.LogError(
						$"Can't find card with name [{cardToModify}] to add to evolve params. Make sure it's not the displayName!");
					break;
				}

				List<CardInfo> cardsForEvolutions = split[1]
					.Split(',')
					.Select(CardLoader.GetCardByName)
					.ToList();

				EvolveRandomly.CardsWithSpecifiedEvolutions.Add(cardToModify, cardsForEvolutions);
				// Logger.LogMessage($"[{cardToModify}] now [{cardsForEvolutions.Count}] random evolutions");
			}
		}

	}
}