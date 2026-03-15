using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GRCi.Compliance.ChecklistTemplates;
using GRCi.Compliance.Enums;
using GRCi.Compliance.Repositories;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace GRCi.EntityFrameworkCore.Compliance.Repositories;

public class ChecklistTemplateRepository :
    EfCoreRepository<GRCiDbContext, ChecklistTemplate, Guid>,
    IChecklistTemplateRepository
{
    public ChecklistTemplateRepository(IDbContextProvider<GRCiDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<ChecklistTemplate?> GetWithItemsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.ChecklistTemplates
            .Include(t => t.Items)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<List<ChecklistTemplate>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string? search = null,
        ChecklistTemplateStatus? status = null,
        Guid? regulatoryAgencyId = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = BuildQuery(dbContext, search, status, regulatoryAgencyId);

        return await query
            .OrderByDescending(t => t.CreationTime)
            .Skip(skipCount)
            .Take(maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public async Task<long> GetCountAsync(
        string? search = null,
        ChecklistTemplateStatus? status = null,
        Guid? regulatoryAgencyId = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        return await BuildQuery(dbContext, search, status, regulatoryAgencyId)
            .LongCountAsync(cancellationToken);
    }

    public async Task<int> GetMaxCodeNumberAsync(CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var maxCode = await dbContext.ChecklistTemplates
            .IgnoreQueryFilters()
            .Select(t => t.Code)
            .OrderByDescending(c => c)
            .FirstOrDefaultAsync(cancellationToken);

        if (maxCode == null) return 0;

        var parts = maxCode.Split('-');
        return int.TryParse(parts.LastOrDefault(), out var num) ? num : 0;
    }

    private static IQueryable<ChecklistTemplate> BuildQuery(
        GRCiDbContext dbContext,
        string? search,
        ChecklistTemplateStatus? status,
        Guid? regulatoryAgencyId)
    {
        var query = dbContext.ChecklistTemplates.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(t =>
                t.Code.Contains(search) ||
                t.NameEn.Contains(search) ||
                (t.NameAr != null && t.NameAr.Contains(search)));

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (regulatoryAgencyId.HasValue)
            query = query.Where(t => t.RegulatoryAgencyId == regulatoryAgencyId.Value);

        return query;
    }
}
