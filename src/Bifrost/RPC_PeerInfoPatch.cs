using HarmonyLib;

namespace Bifrost
{
    public partial class BifrostPlugin
    {
        [HarmonyPatch(typeof(ZNet), nameof(ZNet.RPC_PeerInfo))]
        public static class RPC_PeerInfoPatch
        {
            public static void Prefix(
                ZNet __instance,
                ZRpc rpc)
            {
                if (!string.IsNullOrWhiteSpace(PendingPassword) &&
                    !string.IsNullOrWhiteSpace(SelectedServerUniqueKey))
                {
                    Configuraton.Servers[SelectedServerUniqueKey] = PendingPassword;
                    Logger.LogDebug($"Saved password for server (serverKey = {SelectedServerUniqueKey}).");

                    SelectedServerUniqueKey = null;
                    PendingPassword = null;

                    SaveConfiguration();
                }
            }
        }
    }
}