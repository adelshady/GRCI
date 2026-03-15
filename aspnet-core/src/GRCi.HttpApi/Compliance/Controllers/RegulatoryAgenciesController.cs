using System.Collections.Generic;
using System.Threading.Tasks;
using GRCi.Compliance.Dtos;
using GRCi.Compliance.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace GRCi.Controllers.Compliance;

[ApiController]
[Route("api/compliance/agencies")]
[Authorize]
public class RegulatoryAgenciesController : AbpControllerBase
{
    private readonly IMediator _mediator;

    public RegulatoryAgenciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public Task<List<RegulatoryAgencyDto>> GetListAsync([FromQuery] bool activeOnly = true)
    {
        return _mediator.Send(new ListRegulatoryAgenciesQuery { ActiveOnly = activeOnly });
    }
}
