using HarmonyLib;

using System.Text.RegularExpressions;

namespace Bifrost
{
    public partial class BifrostPlugin
    {
        [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.OnJoinStart))]
        public static class FejdStartupOnJoinStartPatch
        {
            public static void Prefix(FejdStartup __instance)
            {
                SelectedServerUniqueKey = null;
                PendingPassword = null;

                if (!TryGetServerStatus(
                    __instance,
                    out var serverStatus))
                {
                    Logger.LogError("Failed to get server status object.");
                    return;
                }

                Logger.LogDebug("Got server status object.");

                SelectedServerUniqueKey = GenerateServerUniqueKey(
                    serverStatus.m_joinData);

                if (string.IsNullOrWhiteSpace(SelectedServerUniqueKey))
                    Logger.LogError("Failed to get server key.");

                Logger.LogDebug("Got unique server key.");
            }
        }

        private static string GenerateServerUniqueKey(ServerJoinData m_joinData)
        {
            if (m_joinData == null)
                return null;

            string key;

            switch (m_joinData)
            {
                case ServerJoinDataDedicated dedicated:
                    key = $"dedicated:{dedicated.m_host}:{dedicated.m_port}";
                    break;
                case ServerJoinDataPlayFabUser playfab:
                    key = $"playfab:{playfab.m_remotePlayerId}";
                    break;
                case ServerJoinDataSteamUser steam:
                    key = $"steam:{steam.m_joinUserID}";
                    break;
                default:
                    key = "unknown";
                    break;
            }

            return Regex
                .Replace(key, @"[^\w\.-]", "_")
                .Trim('_');
        }

        private static bool TryGetServerStatus(
            FejdStartup __instance,
            out ServerStatus status)
        {
            status = null;

            var field = AccessTools.Field(
                typeof(FejdStartup),
                "m_joinServer");

            if (field == null)
                return false;

            status = field.GetValue(__instance) as ServerStatus;

            return status != null;
        }
    }
}
