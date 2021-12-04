using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SigilADay_julianperge
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency(APIGUID, BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency(JSONGUID, BepInDependency.DependencyFlags.HardDependency)]
	public partial class Plugin : BaseUnityPlugin
	{
		public const string APIGUID = "cyantist.inscryption.api";
		public const string JSONGUID = "MADH.inscryption.JSONLoader";
		
		public const string PluginGuid = "julianperge.inscryption.sigiladay";
		private const string PluginName = "SigilADay_julianperge";
		private const string PluginVersion = "1.7.0";

		internal static ManualLogSource Log;

		private void Awake()
		{
			Log = base.Logger;
			// AddBelligerent();
			AddBrimstone();
			AddCannibal();
			AddEvolveRandomly();
			AddExcavator();
			// AddHermit();
			AddFlightOfTheValkyrie();
			AddMiasma();
			AddNest();
			AddProspect();
			AddSplit();

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}

		private void Start()
		{
			Log.LogDebug($"SigilADay_julianperge Start() begin");
			InitializeEvolveRandomlyCardListConfig();
			Nest.TutorCards = InitializeConfigNest();
			Log.LogDebug($"SigilADay_julianperge Start() end");
		}
	}
}