using AutoMapper;
using CustomManagementDemo.API.DomainLayer.Entities;
using CustomManagementDemo.API.DomainLayer.Entities.Companies;
using FluentValidation;

namespace CustomManagementDemo.API.DomainLayer.Requests.Companies;

/// <summary>
/// 
/// </summary>
public class EditCompanyRequest : CreateCompanyRequest
{
    /// <summary>
    /// Represent the primary key of the company entity
    /// </summary>
    /// <example>5</example>
    public int Id { get; set; }
    
    /// <summary>
    /// Represent a field that can be added, edited and removed dynamically
    /// </summary>
    public List<ExtendedField> ExtendedFields { get; set; } = new List<ExtendedField>();
}
/// <summary>
/// 
/// </summary>
public class EditCompanyRequestValidator : AbstractValidator<EditCompanyRequest>
{
    /// <summary>
    /// 
    /// </summary>
    public EditCompanyRequestValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEqual(0);
        RuleFor(x => x)
            .SetInheritanceValidator(x => x.Add<EditCompanyRequest>(new CreateCompanyRequestValidator()));
    }
}

/// <summary>
/// 
/// </summary>
public class EditCompanyRequestProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public EditCompanyRequestProfile()
    {
        CreateMap<EditCompanyRequest, Company>();
    }
}