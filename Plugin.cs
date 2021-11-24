using BepInEx;
using BepInEx.Logging;

namespace SigilADay
{
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
  public partial class Plugin : BaseUnityPlugin
  {
    private const string PluginGuid = "julianperge.inscryption.sigiladay";
    private const string PluginName = "SigilADay_julianperge";
    private const string PluginVersion = "1.0.0.0";

    internal static ManualLogSource Log;

    private void Awake()
    {
      Log = base.Logger;

      AddExcavator();
      AddNest();
      AddSplit();

      // AddBloodGuzzler();
      // AddLeech();
      // AddRegen1();
      // AddRegen2();
      // AddRegen3();
      // AddRegenFull();
      // AddPoisonous();
      // AddThickShell();
      // AddBonePicker();
      // AddNutritious();
      // AddTransient();
      //AddSilence();

      // ChangeRingworm();
    }
  }
}
