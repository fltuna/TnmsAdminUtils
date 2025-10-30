using Sharp.Shared.Objects;
using Sharp.Shared.Types;
using TnmsAdminUtils.Utils;
using TnmsPluginFoundation.Extensions.Client;
using TnmsPluginFoundation.Models.Command;
using TnmsPluginFoundation.Models.Command.Validators;
using TnmsPluginFoundation.Models.Command.Validators.RangedValidators;

namespace TnmsAdminUtils.Modules.InGameManagement.Commands;

public class Slap(IServiceProvider provider): TnmsAbstractCommandBase(provider)
{
    public override string CommandName => "slap";
    public override string CommandDescription => "Slaps a specified player.";

    public override TnmsCommandRegistrationType CommandRegistrationType =>
        TnmsCommandRegistrationType.Client | TnmsCommandRegistrationType.Server;

    protected override ICommandValidator? GetValidator() => new CompositeValidator()
        .Add(new PermissionValidator("tnms.adminutil.command.slay", true))
        .Add(new ExtendableTargetValidator(1, true))
        .Add(new RangedArgumentValidator<int>(0, int.MaxValue, 2, 0, true));

    protected override ValidationFailureResult OnValidationFailed(ValidationFailureContext context)
    {
        switch (context.Validator)
        {
            case PermissionValidator permissionValidator:
                PrintMessageToServerOrPlayerChat(context.Client, "You do not have permission to use this command.");
                break;
            case ExtendableTargetValidator extendableTargetValidator:
                PrintMessageToServerOrPlayerChat(context.Client, "No valid target found to slap.");
                break;
        }
        
        return ValidationFailureResult.SilentAbort();
    }

    protected override void ExecuteCommand(IGameClient? client, StringCommand commandInfo, ValidatedArguments? validatedArguments)
    {
        var targets = validatedArguments!.GetArgument<List<IGameClient>>(1)!;
        int damage = validatedArguments.GetArgument<int>(2);

        foreach (var gameClient in targets)
        {
            var pawn = gameClient?.GetPlayerController()?.GetPlayerPawn();
            
            if (pawn == null)
                continue;
            
            if (damage > 0)
            {
                int newHp = Math.Max(0, pawn.Health - damage);

                if (newHp > 0)
                {
                    pawn.Health = newHp;
                }
                else
                {
                    pawn.Slay();
                }
            }
            
            var rnd = Random.Shared;
            float dx = (rnd.Next(180) + 50) * (rnd.Next(2) == 1 ? -1 : 1);
            float dy = (rnd.Next(180) + 50) * (rnd.Next(2) == 1 ? -1 : 1);
            float dz = rnd.Next(200) + 100;
            pawn.ApplyAbsVelocityImpulse(new Vector(dx, dy, dz));
            
            // TODO Play slap sound, and sound can be specified in config later
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

        if (damage > 0)
        {
            Plugin.LogAdminAction(client, $"Slapped {targetName} with {damage} damage.");
        }
        else
        {
            Plugin.LogAdminAction(client, $"Slapped {targetName}.");
        }
        
    }
}