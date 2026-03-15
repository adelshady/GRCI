using System;
using System.Threading.Tasks;
using GRCi.Compliance.RegulatoryAgencies;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace GRCi.Compliance;

public class RegulatoryAgencyDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<RegulatoryAgency, Guid> _repository;
    private readonly IGuidGenerator _guidGenerator;

    public RegulatoryAgencyDataSeedContributor(
        IRepository<RegulatoryAgency, Guid> repository,
        IGuidGenerator guidGenerator)
    {
        _repository = repository;
        _guidGenerator = guidGenerator;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        var agencies = new[]
        {
            ("Saudi Central Bank",                                   "البنك المركزي السعودي",              "SAMA"),
            ("Zakat, Tax and Customs Authority",                     "هيئة الزكاة والضريبة والجمارك",      "ZATCA"),
            ("National Cybersecurity Authority",                     "الهيئة الوطنية للأمن السيبراني",     "NCA"),
            ("Communications and Information Technology Commission", "هيئة الاتصالات وتقنية المعلومات",   "CITC"),
            ("Capital Market Authority",                             "هيئة السوق المالية",                 "CMA"),
            ("Ministry of Health",                                   "وزارة الصحة",                        "MOH"),
        };

        foreach (var (nameEn, nameAr, code) in agencies)
        {
            var existing = await _repository.FindAsync(a => a.Code == code);
            if (existing == null)
            {
                await _repository.InsertAsync(
                    new RegulatoryAgency(_guidGenerator.Create(), nameEn, nameAr, code),
                    autoSave: true);
            }
            else if (existing.NameAr == null || existing.NameAr.Contains('?'))
            {
                existing.Update(nameEn, nameAr, code);
                await _repository.UpdateAsync(existing, autoSave: true);
            }
        }
    }
}
