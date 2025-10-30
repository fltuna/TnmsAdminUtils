using Sharp.Shared.Objects;
using Sharp.Shared.Types;
using TnmsAdminUtils.Utils;
using TnmsPluginFoundation.Extensions.Client;
using TnmsPluginFoundation.Models.Command;
using TnmsPluginFoundation.Models.Command.Validators;

namespace TnmsAdminUtils.Modules.InGameManagement.Commands.Teleports;

public class Bring(IServiceProvider provider): TnmsAbstractCommandBase(provider)
{
    public override string CommandName => "bring";
    public override string CommandDescription => "Bring a player to executor.";

    public override TnmsCommandRegistrationType CommandRegistrationType =>
        TnmsCommandRegistrationType.Client;

    protected override ICommandValidator? GetValidator() => new CompositeValidator()
        .Add(new PermissionValidator("tnms.adminutil.management.ingame.command.bring", true))
        .Add(new ArgumentCountValidator(1, true))
        .Add(new ExtendableTargetValidator(1, true));

    protected override ValidationFailureResult OnValidationFailed(ValidationFailureContext context)
    {
        switch (context.Validator)
        {
            case ArgumentCountValidator:
                PrintMessageToServerOrPlayerChat(context.Client, LocalizeWithPluginPrefix(context.Client, "Teleport.Notification.Bring.Usage"));
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
        var executorPawn = client?.GetPlayerPawn();
        if (executorPawn == null)
            return;
        
        var targets = validatedArguments!.GetArgument<List<IGameClient>>(1)!;

        foreach (var gameClient in targets)
        {
            var pawn = gameClient.GetPlayerController()?.GetPlayerPawn();
            
            if (pawn == null)
                continue;

            pawn.Teleport(executorPawn.GetAbsOrigin());
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
        
        Plugin.LogAdminActionLocalized(client, "Teleport.Broadcast.Bring", targetName);
    }
}