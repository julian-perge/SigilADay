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
		
		
	}
}