using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using InscryptionAPI;

namespace SigilADay_julianperge;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency(InscryptionAPIPlugin.ModGUID)]
public partial class Plugin : BaseUnityPlugin
{
	public const string PluginGuid = "julianperge.inscryption.sigiladay";
	public const string PluginName = "SigilADay_julianperge";
	private const string PluginVersion = "1.22";

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

	private void Start()
	{
		Log.LogDebug("SigilADay_julianperge Start() begin");
		Nest.TutorCards = InitializeConfigNest();
		InitializeEvolveRandomlyCardListConfig();
		Log.LogDebug("SigilADay_julianperge Start() end");
	}
}
