using Sharp.Shared.Objects;
using Sharp.Shared.Types;
using TnmsPluginFoundation.Extensions.Client;
using TnmsPluginFoundation.Models.Command;
using TnmsPluginFoundation.Models.Command.Validators;

namespace TnmsAdminUtils.Modules.InGameManagement.Commands.Teleports;

public class Goto(IServiceProvider provider): TnmsAbstractCommandBase(provider)
{
    public override string CommandName => "goto";
    public override string CommandDescription => "Teleport to player.";

    public override TnmsCommandRegistrationType CommandRegistrationType =>
        TnmsCommandRegistrationType.Client;

    protected override ICommandValidator? GetValidator() => new CompositeValidator()
        .Add(new PermissionValidator("tnms.adminutil.management.ingame.command.goto", true))
        .Add(new ArgumentCountValidator(1, true))
        .Add(new ExtendableTargetValidator(1, true));

    protected override ValidationFailureResult OnValidationFailed(ValidationFailureContext context)
    {
        switch (context.Validator)
        {
            case ArgumentCountValidator:
                PrintMessageToServerOrPlayerChat(context.Client, LocalizeWithPluginPrefix(context.Client, "Teleport.Notification.Goto.Usage"));
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

        if (targets.Count > 1)
        {
            PrintMessageToServerOrPlayerChat(client, LocalizeWithPluginPrefix(client, "Common.ValidationFailure.MultipleTargetsFound"));
            return;
        }
        
        var targetPawn = targets[0].GetPlayerController()?.GetPlayerPawn();
        
        if (targetPawn == null)
        {
            PrintMessageToServerOrPlayerChat(client, LocalizeWithPluginPrefix(client, "Common.ValidationFailure.NoValidTargetsFound"));
            return;
        }
        
        executorPawn.Teleport(targetPawn.GetAbsOrigin());
        
        Plugin.TnmsLogger.LogAdminActionLocalized(client, "Teleport.Broadcast.Goto", targetPawn.GetController()!.PlayerName);
    }
}