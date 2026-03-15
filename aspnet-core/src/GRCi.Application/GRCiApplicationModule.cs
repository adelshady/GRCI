using GRCi.Compliance.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace GRCi;

[DependsOn(
    typeof(GRCiDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(GRCiApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class GRCiApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<GRCiApplicationModule>();
        });

        context.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GRCiApplicationModule).Assembly));

        context.Services.AddTransient<IWorkflowManager, WorkflowManager>();
        context.Services.AddTransient<IAttachmentService, AttachmentService>();
    }
}
