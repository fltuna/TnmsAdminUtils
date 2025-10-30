using Microsoft.Extensions.Configuration;
using Sharp.Shared;
using TnmsAdminUtils.Modules.InGameManagement;
using TnmsAdminUtils.Modules.ServerManagement;
using TnmsPluginFoundation;

namespace TnmsAdminUtils;

public class TnmsAdminUtils(
    ISharedSystem sharedSystem,
    string dllPath,
    string sharpPath,
    Version? version,
    IConfiguration coreConfiguration,
    bool hotReload)
    : TnmsPlugin(sharedSystem, dllPath, sharpPath, version, coreConfiguration, hotReload)
{
    public override string DisplayName => "TnmsAdminUtils";
    public override string DisplayAuthor => "faketuna";
    public override string BaseCfgDirectoryPath => "";
    public override string ConVarConfigPath => "TnmsAdminUtils/convars.cfg";
    public override string PluginPrefix => "Plugin.Prefix";
    public override bool UseTranslationKeyInPluginPrefix => true;

    protected override void TnmsOnPluginLoad(bool hotReload)
    {
        RegisterModule<InGameManagementCommands>();
        RegisterModule<ServerManagementCommands>();
    }
}