using TnmsAdminUtils.Modules.InGameManagement.Commands;
using TnmsAdminUtils.Modules.InGameManagement.Commands.GameRules;
using TnmsAdminUtils.Modules.InGameManagement.Commands.Teleports;
using TnmsPluginFoundation.Models.Plugin;

namespace TnmsAdminUtils.Modules.InGameManagement;

public class InGameManagementCommands(IServiceProvider serviceProvider) : PluginModuleBase(serviceProvider)
{
    public override string PluginModuleName => "InGameManagementCommands";
    public override string ModuleChatPrefix => "";
    protected override bool UseTranslationKeyInModuleChatPrefix => false;

    protected override void OnAllModulesLoaded()
    {
        RegisterTnmsCommand<TerminateRound>();
        RegisterTnmsCommand<Slap>();
        RegisterTnmsCommand<Slay>();
        
        RegisterTnmsCommand<Goto>();
        RegisterTnmsCommand<Bring>();
        RegisterTnmsCommand<Send>();
        RegisterTnmsCommand<AddTime>();
    }
}