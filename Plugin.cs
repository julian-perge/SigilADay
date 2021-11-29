using BepInEx;
using BepInEx.Logging;

namespace SigilADay_julianperge
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency(APIGUID, BepInDependency.DependencyFlags.HardDependency)]
	public partial class Plugin : BaseUnityPlugin
	{
		public const string APIGUID = "cyantist.inscryption.api";
		public const string PluginGuid = "julianperge.inscryption.sigiladay";
		private const string PluginName = "SigilADay_julianperge";
		private const string PluginVersion = "1.6.3";

		internal static ManualLogSource Log;

		private void Awake()
		{
			Log = base.Logger;

			AddRandomEvolve();
			// AddBelligerent();
			AddCannibal();
			AddExcavator();
			AddFlightOfTheValkyrie();
			AddMiasma();
			AddNest();
			AddProspect();
			AddSplit();
		}
	}
}