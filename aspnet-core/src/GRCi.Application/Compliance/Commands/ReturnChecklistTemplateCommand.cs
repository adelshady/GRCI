using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace GRCi.Compliance.Commands;

public class ReturnChecklistTemplateCommand : IRequest
{
    public Guid TemplateId { get; set; }

    [Required]
    public string Comment { get; set; } = null!;
}
