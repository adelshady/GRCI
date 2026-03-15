using AutoMapper;
using GRCi.Compliance.Attachments;
using GRCi.Compliance.ChecklistTemplates;
using GRCi.Compliance.Dtos;
using GRCi.Compliance.Workflow;

namespace GRCi;

public class GRCiApplicationAutoMapperProfile : Profile
{
    public GRCiApplicationAutoMapperProfile()
    {
        CreateMap<ChecklistTemplate, ChecklistTemplateDto>()
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items))
            .ForMember(d => d.WorkflowHistory, opt => opt.Ignore());

        CreateMap<ChecklistItem, ChecklistItemDto>();

        CreateMap<WorkflowActionHistory, WorkflowActionHistoryDto>();
    }
}
