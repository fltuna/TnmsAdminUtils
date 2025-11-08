using Sharp.Shared.Objects;
using Sharp.Shared.Types;
using TnmsPluginFoundation.Models.Command;
using TnmsPluginFoundation.Models.Command.Validators;
using TnmsPluginFoundation.Models.Command.Validators.RangedValidators;
using TnmsPluginFoundation.Utils.Entity;

namespace TnmsAdminUtils.Modules.InGameManagement.Commands.GameRules;

public class AddTime(IServiceProvider provider) : TnmsAbstractCommandBase(provider)
{
    public override string CommandName => "addtime";
    public override string CommandDescription => "Adds time to the current round timer.";

    public override TnmsCommandRegistrationType CommandRegistrationType =>
        TnmsCommandRegistrationType.Client | TnmsCommandRegistrationType.Server;

    protected override ICommandValidator? GetValidator() => new CompositeValidator()
        .Add(new PermissionValidator("tnms.adminutil.management.ingame.command.addtime", true))
        .Add(new ArgumentCountValidator(1, true))
        .Add(new RangedArgumentValidator<int>(int.MinValue, int.MaxValue, 1, true));

    protected override ValidationFailureResult OnValidationFailed(ValidationFailureContext context)
    {
        switch (context.Validator)
        {
            case ArgumentCountValidator:
                PrintMessageToServerOrPlayerChat(context.Client, LocalizeWithPluginPrefix(context.Client, "AddTime.Notification.Usage"));
                break;
            case PermissionValidator:
                PrintMessageToServerOrPlayerChat(context.Client, LocalizeWithPluginPrefix(context.Client, "Common.ValidationFailure.NotEnoughPermissions"));
                break;
            case IRangedArgumentValidator rangedArgumentValidator:
                PrintMessageToServerOrPlayerChat(context.Client, LocalizeWithPluginPrefix(context.Client, "Common.ValidationFailure.ArgumentIsMustBeInRange", rangedArgumentValidator.ArgumentIndex, rangedArgumentValidator.GetRangeDescription()));
                break;
        }
        
        return ValidationFailureResult.SilentAbort();
    }

    protected override void ExecuteCommand(IGameClient? client, StringCommand commandInfo, ValidatedArguments? validatedArguments)
    {
        int extendSeconds = validatedArguments!.GetArgument<int>(1);

        int currentRoundTimeLimit = GameRulesUtil.GetRoundTime();
        
        int newRoundTimeLimit = currentRoundTimeLimit + extendSeconds;
        

        if (extendSeconds < 0)
        {
            int diffTime = -extendSeconds;
            if (newRoundTimeLimit < 0)
            {
                diffTime = Math.Abs(-extendSeconds - -newRoundTimeLimit);
                newRoundTimeLimit = 0;
            }
            
            Plugin.TnmsLogger.LogAdminActionLocalized(client, "AddTime.Broadcast.TimeShortened", diffTime);
        }
        else
        {
            Plugin.TnmsLogger.LogAdminActionLocalized(client, "AddTime.Broadcast.TimeAdded", extendSeconds);
        }
        
        GameRulesUtil.SetRoundTime(newRoundTimeLimit);
    }
}