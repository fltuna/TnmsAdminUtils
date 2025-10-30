using TnmsAdminUtils.Modules.InGameManagement.Commands;
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
    }
}