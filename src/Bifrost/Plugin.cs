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
        private const string PluginIdentifier = "legendaryb.valheim.Bifrost";

        private static new ManualLogSource Logger { get; set; }
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
                    Logger.LogDebug("Configuration file does not exist.");
                    return null;
                }

                Logger.LogDebug("Configuration file was found.");

                var content = File.ReadAllText(configurationPath);

                Logger.LogDebug("Configuration file was read.");

                var configuration = JsonConvert.DeserializeObject<BifrostConfiguration>(content);

                Logger.LogInfo("Configuration file was loaded.");

                return configuration;
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

                Logger.LogInfo("Saved configuration file.");
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
