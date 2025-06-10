using BepInEx;
using BepInEx.Logging;

using Bifrost.Configuration;

using HarmonyLib;

using Newtonsoft.Json;

using System;
using System.IO;

namespace Bifrost
{
    [BepInPlugin(PluginIdentifier, PluginName, "1.0.0")]
    public partial class BifrostPlugin : BaseUnityPlugin
    {
        private const string PluginName = "Bifrost";
        private const string PluginIdentifier = "throwingbits.valheim.Bifrost";

        public static ManualLogSource Logger = null;

        private static BifrostConfiguration Configuraton { get; set; }
        private static string SelectedServerUniqueKey { get; set; }
        private static string PendingPassword { get; set; }

        public void Awake()
        {
            Logger = Logger ?? base.Logger;
            Configuraton = LoadConfiguration() ?? new BifrostConfiguration();

            Harmony.CreateAndPatchAll(typeof(BifrostPlugin).Assembly);
        }

        private static BifrostConfiguration LoadConfiguration()
        {
            try
            {
                var configurationPath = GetConfigurationFilePath();

                if (!File.Exists(configurationPath))
                {
                    Logger.LogInfo("Configuration file does not exist.");
                    return null;
                }

                var content = File.ReadAllText(configurationPath);

                return JsonConvert.DeserializeObject<BifrostConfiguration>(content);
            }
            catch (Exception)
            {
                Logger.LogError("Failed to load configuration file.");
                return null;
            }
        }

        private static void SaveConfiguration()
        {
            try
            {
                var configurationPath = GetConfigurationFilePath();
                var content = JsonConvert.SerializeObject(Configuraton, Formatting.Indented);

                File.WriteAllText(
                    configurationPath,
                    content);
            }
            catch (Exception)
            {
                Logger.LogError("Failed to save configuration file.");
            }
        }

        private static string GetConfigurationFilePath()
        {
            return Path.Combine(
                Paths.ConfigPath,
                $"{PluginIdentifier}.json");
        }
    }
}
