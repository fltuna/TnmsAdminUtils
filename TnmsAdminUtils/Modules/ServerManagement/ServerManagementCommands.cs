using TnmsAdminUtils.Modules.InGameManagement.Commands;
using TnmsAdminUtils.Modules.ServerManagement.Commands;
using TnmsPluginFoundation.Models.Plugin;

namespace TnmsAdminUtils.Modules.ServerManagement;

public class ServerManagementCommands(IServiceProvider serviceProvider) : PluginModuleBase(serviceProvider)
{
    public override string PluginModuleName => "ServerManagementCommands";
    public override string ModuleChatPrefix => "";
    protected override bool UseTranslationKeyInModuleChatPrefix => false;

    protected override void OnAllModulesLoaded()
    {
        RegisterTnmsCommand<Rcon>();
        RegisterTnmsCommand<Cvar>();
    }
}