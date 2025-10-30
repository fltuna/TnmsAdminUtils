using Microsoft.Extensions.Logging;
using Sharp.Shared.Objects;
using TnmsPluginFoundation;
using TnmsPluginFoundation.Extensions.Client;
using TnmsPluginFoundation.Models.Plugin;
using TnmsPluginFoundation.Utils.Entity;

namespace TnmsAdminUtils.Utils;

public static class AdminActionUtil
{
    public static void LogAdminAction(this TnmsPlugin plugin, IGameClient? executor,
        string actionDescription)
    {
        string log = $"[AdminAction] {PlayerUtil.GetPlayerName(executor)} ({executor?.SteamId.ToString() ?? "N/A"}) performed action: {actionDescription}";
        
        plugin.Logger.LogInformation(log);
        foreach (var gameClient in plugin.SharedSystem.GetModSharp().GetIServer().GetGameClients())
        {
            if (gameClient.IsFakeClient || gameClient.IsHltv)
                continue;
            
            // TODO: add permission check for manipulating details shown in admin action log
            // Also Translation
            
            gameClient.GetPlayerController()?.PrintToChat(log);
        }
    }
}