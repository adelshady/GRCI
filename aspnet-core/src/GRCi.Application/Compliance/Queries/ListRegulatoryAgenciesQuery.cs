using System.Collections.Generic;
using GRCi.Compliance.Dtos;
using MediatR;

namespace GRCi.Compliance.Queries;

public class ListRegulatoryAgenciesQuery : IRequest<List<RegulatoryAgencyDto>>
{
    public bool ActiveOnly { get; set; } = true;
}
