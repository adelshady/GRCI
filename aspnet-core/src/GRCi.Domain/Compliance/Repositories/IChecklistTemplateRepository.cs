using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GRCi.Compliance.ChecklistTemplates;
using GRCi.Compliance.Enums;
using Volo.Abp.Domain.Repositories;

namespace GRCi.Compliance.Repositories;

public interface IChecklistTemplateRepository : IRepository<ChecklistTemplate, Guid>
{
    Task<ChecklistTemplate?> GetWithItemsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<ChecklistTemplate>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string? search = null,
        ChecklistTemplateStatus? status = null,
        Guid? regulatoryAgencyId = null,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        string? search = null,
        ChecklistTemplateStatus? status = null,
        Guid? regulatoryAgencyId = null,
        CancellationToken cancellationToken = default);

    Task<int> GetMaxCodeNumberAsync(CancellationToken cancellationToken = default);
}
