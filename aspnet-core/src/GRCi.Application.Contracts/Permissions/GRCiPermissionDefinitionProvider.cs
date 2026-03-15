using GRCi.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace GRCi.Permissions;

public class GRCiPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(GRCiPermissions.GroupName, L("Permission:GRCi"));

        var complianceGroup = myGroup.AddPermission(GRCiPermissions.Compliance.Default, L("Permission:Compliance"));

        var templates = complianceGroup.AddChild(GRCiPermissions.Compliance.Templates.Default, L("Permission:Compliance:Templates"));
        templates.AddChild(GRCiPermissions.Compliance.Templates.Create, L("Permission:Compliance:Templates:Create"));
        templates.AddChild(GRCiPermissions.Compliance.Templates.Edit, L("Permission:Compliance:Templates:Edit"));
        templates.AddChild(GRCiPermissions.Compliance.Templates.Delete, L("Permission:Compliance:Templates:Delete"));
        templates.AddChild(GRCiPermissions.Compliance.Templates.Submit, L("Permission:Compliance:Templates:Submit"));
        templates.AddChild(GRCiPermissions.Compliance.Templates.Approve, L("Permission:Compliance:Templates:Approve"));
        templates.AddChild(GRCiPermissions.Compliance.Templates.Return, L("Permission:Compliance:Templates:Return"));
        templates.AddChild(GRCiPermissions.Compliance.Templates.Archive, L("Permission:Compliance:Templates:Archive"));

        var items = complianceGroup.AddChild(GRCiPermissions.Compliance.Items.Default, L("Permission:Compliance:Items"));
        items.AddChild(GRCiPermissions.Compliance.Items.Manage, L("Permission:Compliance:Items:Manage"));

        var attachments = complianceGroup.AddChild(GRCiPermissions.Compliance.Attachments.Default, L("Permission:Compliance:Attachments"));
        attachments.AddChild(GRCiPermissions.Compliance.Attachments.Manage, L("Permission:Compliance:Attachments:Manage"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<GRCiResource>(name);
    }
}
