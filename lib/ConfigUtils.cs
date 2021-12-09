using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using APIPlugin;
using BepInEx.Bootstrap;
using DiskCardGame;

namespace SigilADay_julianperge
{
	public static class ConfigUtils
	{
		public static bool CardDoesNotExistInGameOrApiLoad(string cardToCheck)
		{
			return !VerifyPluginsAndApiCards(cardToCheck);
		}

		private static bool VerifyPluginsAndApiCards(string cardToCheck)
		{
			return Chainloader.PluginInfos.ContainsKey(Plugin.APIGUID)
			       && Chainloader.PluginInfos.ContainsKey(Plugin.JSONGUID)
			       && CheckThatCardExists(cardToCheck).Result;
		}

		public static async Task<bool> WaitForAllCardsToBeLoaded()
		{
			bool succeeded = ScriptableObjectLoader<CardInfo>.allData is not null;
			while (!succeeded)
			{
				// do work
				succeeded = ScriptableObjectLoader<CardInfo>.allData is not null; // if it worked, make as succeeded, else retry
				await Task.Delay(1000); // arbitrary delay
			}

			return succeeded;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static async Task<bool> CheckThatCardExists(string nameOfCard)
		{
			bool result = await WaitForAllCardsToBeLoaded();
			Plugin.Log.LogDebug("All cards have been loaded! " + result);

			bool MatchesNameOfCard(CardInfo card) => card.name == nameOfCard;

			return ScriptableObjectLoader<CardInfo>.allData.Exists(MatchesNameOfCard) ||
			       CustomCard.cards.Exists(elem => elem.name == nameOfCard) ||
			       NewCard.cards.Exists(MatchesNameOfCard);
		}
	}
}
