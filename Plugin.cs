using BepInEx;
using BepInEx.Logging;
using SigilADay_julianperge.lib;
using UnityEngine;

// using UnityEngine;

namespace SigilADay_julianperge
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public partial class Plugin : BaseUnityPlugin
	{
		public const string PluginGuid = "julianperge.inscryption.sigiladay";
		private const string PluginName = "SigilADay_julianperge";
		private const string PluginVersion = "1.1.0";
		public static AssetBundle ArtworkAssets;

		internal static ManualLogSource Log;

		private void Awake()
		{
			Log = base.Logger;
			
			// ArtworkAssets = AssetBundle.LoadFromFile(SigilUtils.GetFullPathOfFile("artwork_sigils"));
			
			AddExcavator();
			// AddNest();;
			// AddSplit();
		}
	}
}