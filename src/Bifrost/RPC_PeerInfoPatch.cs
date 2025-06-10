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
                    Configuraton.Items[SelectedServerUniqueKey] = PendingPassword;
                    Logger.LogInfo($"[Bifrost] Saved password for {SelectedServerUniqueKey}");

                    SelectedServerUniqueKey = null;
                    PendingPassword = null;

                    SaveConfiguration();
                }
            }
        }
    }
}