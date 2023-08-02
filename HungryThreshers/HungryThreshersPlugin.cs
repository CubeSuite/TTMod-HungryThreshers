using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using HungryThreshers.Patches;
using UnityEngine;

namespace HungryThreshers
{
    // TODO Review this file and update to your own requirements.

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class HungryThreshersPlugin : BaseUnityPlugin
    {
        private const string MyGUID = "com.equinox.HungryThreshers";
        private const string PluginName = "HungryThreshers";
        private const string VersionString = "1.1.0";

        private const string PauseIfBothFullKey = "PauseIfBothFull";
        public static ConfigEntry<bool> PauseIfBothFull;

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        private void Awake() {
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            PauseIfBothFull = Config.Bind("General", PauseIfBothFullKey, false, new ConfigDescription("When enabled, threshers will not void items if both outputs are full"));
            
            Harmony.PatchAll();
            Log = Logger;
            Harmony.PatchAll(typeof(ThresherInstancePatch));
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
        }
    }
}
