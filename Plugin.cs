using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SigilADay_julianperge
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency(APIGUID)]
	[BepInDependency(JSONGUID)]
	public partial class Plugin : BaseUnityPlugin
	{
		public const string APIGUID = "cyantist.inscryption.api";
		public const string JSONGUID = "MADH.inscryption.JSONLoader";

		public const string PluginGuid = "julianperge.inscryption.sigiladay";
		public const string PluginName = "SigilADay_julianperge";
		private const string PluginVersion = "1.21.6";

		internal static ManualLogSource Log;

		private void Awake()
		{
			Log = base.Logger;
			AddBelligerent();
			AddBrimstone();
			AddCannibal();
			AddEvolveRandomly();
			AddExcavator();
			// AddAbilityExodia();
			// AddHermit();
			AddFlightOfTheValkyrie();
			AddMiasma();
			AddNest();
			AddProspect();
			AddSplit();

			var harmony = new Harmony(PluginGuid);
			harmony.PatchAll();
		}

		private async void Start()
		{
			Log.LogDebug($"SigilADay_julianperge Start() begin");
			Nest.TutorCards = await Task.Run(InitializeConfigNest);
			await Task.Run(InitializeEvolveRandomlyCardListConfig);
			Log.LogDebug($"SigilADay_julianperge Start() end");
		}
	}
}
