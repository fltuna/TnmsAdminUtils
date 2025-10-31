using Sharp.Shared.Objects;
using Sharp.Shared.Types;
using TnmsAdminUtils.Utils;
using TnmsPluginFoundation.Extensions.Client;
using TnmsPluginFoundation.Models.Command;
using TnmsPluginFoundation.Models.Command.Validators;
using TnmsPluginFoundation.Models.Command.Validators.RangedValidators;

namespace TnmsAdminUtils.Modules.ServerManagement.Commands;

public class Rcon(IServiceProvider provider): TnmsAbstractCommandBase(provider)
{
    public override string CommandName => "rcon";
    public override string CommandDescription => "Executes a specified command in server console.";

    public override TnmsCommandRegistrationType CommandRegistrationType =>
        TnmsCommandRegistrationType.Client;

    protected override ICommandValidator? GetValidator() => new CompositeValidator()
        .Add(new PermissionValidator("tnms.adminutil.management.server.command.rcon", true))
        .Add(new ArgumentCountValidator(1, true));

    protected override ValidationFailureResult OnValidationFailed(ValidationFailureContext context)
    {
        switch (context.Validator)
        {
            case ArgumentCountValidator:
                PrintMessageToServerOrPlayerChat(context.Client, LocalizeWithPluginPrefix(context.Client, "Rcon.Notification.Usage"));
                break;
        }
        
        return ValidationFailureResult.SilentAbort();
    }

    protected override void ExecuteCommand(IGameClient? client, StringCommand commandInfo, ValidatedArguments? validatedArguments)
    {
        if (client == null)
            return;
        
        string commandToExecute = commandInfo.ArgString;
        
        Plugin.SharedSystem.GetModSharp().ServerCommand(commandToExecute);
        
        // client.GetPlayerController()?.PrintToChat(LocalizeWithPluginPrefix(client, "Rcon.Notification.CommandExecuted", commandToExecute));
    
        Plugin.LogAdminActionLocalized(client, "Rcon.Broadcast.CommandExecuted", commandToExecute);

        
        
    }
}