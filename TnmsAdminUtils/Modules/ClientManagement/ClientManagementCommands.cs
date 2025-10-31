using TnmsAdminUtils.Modules.ClientManagement.Commands;
using TnmsPluginFoundation.Models.Plugin;

namespace TnmsAdminUtils.Modules.ClientManagement;

public class ClientManagementCommands(IServiceProvider serviceProvider) : PluginModuleBase(serviceProvider)
{
    public override string PluginModuleName => "ClientManagementCommands";
    public override string ModuleChatPrefix => "";
    protected override bool UseTranslationKeyInModuleChatPrefix => false;

    protected override void OnAllModulesLoaded()
    {
        RegisterTnmsCommand<QueryCvar>();
    }
}