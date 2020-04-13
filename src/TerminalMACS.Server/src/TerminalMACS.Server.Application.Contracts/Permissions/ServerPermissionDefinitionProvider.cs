using TerminalMACS.Server.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace TerminalMACS.Server.Permissions
{
    public class ServerPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(ServerPermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(ServerPermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ServerResource>(name);
        }
    }
}
