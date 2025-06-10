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
                    return;

                if (!Configuraton.Servers.TryGetValue(SelectedServerUniqueKey, out var item))
                {
                    var inputField = __instance.m_passwordDialog.GetComponentInChildren<GuiInputField>();

                    if (inputField == null)
                        return;

                    inputField.OnInputSubmit.RemoveAllListeners();
                    inputField.OnInputSubmit.AddListener((password) =>
                    {
                        if (!string.IsNullOrWhiteSpace(password))
                        {
                            Configuraton.Servers.Add(SelectedServerUniqueKey, password);

                            Logger.LogInfo($"Saved password for {SelectedServerUniqueKey}");

                            PendingPassword = password;

                            InvokeOnPasswordEntered(
                                __instance,
                                password);
                        }
                    });
                }
                else
                {
                    __instance.m_passwordDialog.gameObject.SetActive(false);

                    InvokeOnPasswordEntered(
                        __instance,
                        item);
                }
            }

            private static void InvokeOnPasswordEntered(
                ZNet instance,
                string password)
            {
                var method = AccessTools.Method(
                    typeof(ZNet),
                    "OnPasswordEntered");

                method?.Invoke(
                    instance,
                    new object[] { password });
            }
        }
    }
}