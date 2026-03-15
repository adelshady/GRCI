using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GRCi.Compliance.Dtos;
using GRCi.Compliance.Queries;
using GRCi.Compliance.RegulatoryAgencies;
using MediatR;
using Volo.Abp.Domain.Repositories;

namespace GRCi.Compliance.Handlers;

public class ListRegulatoryAgenciesHandler : IRequestHandler<ListRegulatoryAgenciesQuery, List<RegulatoryAgencyDto>>
{
    private readonly IRepository<RegulatoryAgency, System.Guid> _repository;

    public ListRegulatoryAgenciesHandler(IRepository<RegulatoryAgency, System.Guid> repository)
    {
        _repository = repository;
    }

    public async Task<List<RegulatoryAgencyDto>> Handle(ListRegulatoryAgenciesQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetListAsync(
            x => !request.ActiveOnly || x.IsActive,
            cancellationToken: cancellationToken);

        return items
            .OrderBy(x => x.NameEn)
            .Select(x => new RegulatoryAgencyDto
            {
                Id = x.Id,
                NameEn = x.NameEn,
                NameAr = x.NameAr,
                Code = x.Code,
                IsActive = x.IsActive,
            })
            .ToList();
    }
}
