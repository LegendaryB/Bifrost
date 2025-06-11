using GUIFramework;

using HarmonyLib;

namespace Bifrost
{
    public partial class BifrostPlugin
    {
        [HarmonyPatch(typeof(ZNet), nameof(ZNet.RPC_ClientHandshake))]
        public static class RPC_ClientHandshakePatch
        {
            public static void Postfix(
                ZNet __instance,
                ZRpc rpc,
                bool needPassword,
                string serverPasswordSalt)
            {
                if (!needPassword)
                {
                    Logger.LogDebug("Server does not need a password. Connecting..");
                    return;
                }

                if (!Configuraton.Servers.TryGetValue(
                    SelectedServerUniqueKey,
                    out var savedPassword))
                {
                    Logger.LogDebug($"No saved password for server found (serverKey = {SelectedServerUniqueKey}).");

                    var inputField = __instance.m_passwordDialog.GetComponentInChildren<GuiInputField>();

                    if (inputField == null)
                        return;

                    inputField.OnInputSubmit.RemoveAllListeners();
                    inputField.OnInputSubmit.AddListener((password) =>
                    {
                        if (!string.IsNullOrWhiteSpace(password))
                        {
                            Logger.LogDebug($"Set pending password for server (serverKey = {SelectedServerUniqueKey}).");

                            PendingPassword = password;

                            InvokeOnPasswordEntered(
                                __instance,
                                PendingPassword);
                        }
                    });
                }
                else
                {
                    Logger.LogError($"Found saved password for server (serverKey = {SelectedServerUniqueKey})");

                    __instance.m_passwordDialog.gameObject.SetActive(false);

                    InvokeOnPasswordEntered(
                        __instance,
                        savedPassword);
                }
            }

            private static void InvokeOnPasswordEntered(
                ZNet instance,
                string savedPassword)
            {
                var method = AccessTools.Method(
                    typeof(ZNet),
                    "OnPasswordEntered");

                method?.Invoke(
                    instance,
                    new object[] { savedPassword });
            }
        }
    }
}