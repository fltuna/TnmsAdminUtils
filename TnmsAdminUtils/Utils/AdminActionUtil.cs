using Microsoft.Extensions.Logging;
using Sharp.Shared.Objects;
using TnmsPluginFoundation;
using TnmsPluginFoundation.Extensions.Client;
using TnmsPluginFoundation.Models.Plugin;
using TnmsPluginFoundation.Utils.Entity;

namespace TnmsAdminUtils.Utils;

public static class AdminActionUtil
{
    public static void LogAdminActionLocalized(this TnmsPlugin plugin, IGameClient? executor,
        string actionDescription, params object[] descriptionParams)
    {
        string localizedActionDescription = plugin.LocalizeString(actionDescription, descriptionParams);
        string log = $"[AdminAction] {PlayerUtil.GetPlayerName(executor)} ({executor?.SteamId.ToString() ?? "N/A"}) performed action: {localizedActionDescription}";
        
        plugin.Logger.LogInformation(log);
        foreach (var gameClient in plugin.SharedSystem.GetModSharp().GetIServer().GetGameClients())
        {
            if (gameClient.IsFakeClient || gameClient.IsHltv)
                continue;
            
            // TODO: add permission check for manipulating details shown in admin action log
            
            var msg =
                $"{plugin.LocalizeStringForPlayer(gameClient, plugin.PluginPrefix)} {PlayerUtil.GetPlayerName(executor)}: {plugin.LocalizeStringForPlayer(gameClient, actionDescription, descriptionParams)}";
            
            gameClient.GetPlayerController()?.PrintToChat(msg);
        }
    }
    public static void LogAdminAction(this TnmsPlugin plugin, IGameClient? executor,
        string actionDescription)
    {
        string log = $"[AdminAction] {PlayerUtil.GetPlayerName(executor)} ({executor?.SteamId.ToString() ?? "N/A"}) performed action: {actionDescription}";
        plugin.Logger.LogInformation(log);
    }
}