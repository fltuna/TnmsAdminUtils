using Sharp.Shared.Objects;
using Sharp.Shared.Types;
using TnmsAdminUtils.Utils;
using TnmsPluginFoundation.Extensions.Client;
using TnmsPluginFoundation.Models.Command;
using TnmsPluginFoundation.Models.Command.Validators;

namespace TnmsAdminUtils.Modules.InGameManagement.Commands;

public class Slay(IServiceProvider provider): TnmsAbstractCommandBase(provider)
{
    public override string CommandName => "slay";
    public override string CommandDescription => "Slays a specified player.";

    public override TnmsCommandRegistrationType CommandRegistrationType =>
        TnmsCommandRegistrationType.Client | TnmsCommandRegistrationType.Server;

    protected override ICommandValidator? GetValidator() => new CompositeValidator()
        .Add(new PermissionValidator("tnms.adminutil.management.ingame.command.slay", true))
        .Add(new ArgumentCountValidator(1, true))
        .Add(new ExtendableTargetValidator(1, true));

    protected override ValidationFailureResult OnValidationFailed(ValidationFailureContext context)
    {
        switch (context.Validator)
        {
            case ArgumentCountValidator:
                PrintMessageToServerOrPlayerChat(context.Client, LocalizeWithPluginPrefix(context.Client, "Slap.Notification.Usage"));
                break;
            case PermissionValidator:
                PrintMessageToServerOrPlayerChat(context.Client, LocalizeWithPluginPrefix(context.Client, "Common.ValidationFailure.NotEnoughPermissions"));
                break;
            case ExtendableTargetValidator:
                PrintMessageToServerOrPlayerChat(context.Client, LocalizeWithPluginPrefix(context.Client, "Common.ValidationFailure.NoValidTargetsFound"));
                break;
        }
        
        return ValidationFailureResult.SilentAbort();
    }

    protected override void ExecuteCommand(IGameClient? client, StringCommand commandInfo, ValidatedArguments? validatedArguments)
    {
        var targets = validatedArguments!.GetArgument<List<IGameClient>>(1)!;

        foreach (var gameClient in targets)
        {
            gameClient.GetPlayerController()?.GetPlayerPawn()?.Slay();
        }
        
        

        string targetName;
        if (targets.Count > 1)
        {
            targetName = $"{targets.Count} players";
        }
        else
        {
            targetName = $"{targets[0].Name}";
        }
        
        Plugin.LogAdminActionLocalized(client, $"Slay.Broadcast.Slayed", targetName);
    }
}