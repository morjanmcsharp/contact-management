using AutoMapper;
using CustomManagementDemo.API.DomainLayer.Entities;
using CustomManagementDemo.API.DomainLayer.Entities.Contacts;
using FluentValidation;

namespace CustomManagementDemo.API.DomainLayer.Requests.Contacts;

/// <summary>
/// 
/// </summary>
public class EditContactRequest : CreateContactRequest
{
    /// <summary>
    /// Represent the primary key of the contact entity
    /// </summary>
    /// <example>3</example>
    public int Id { get; set; }
    
    /// <summary>
    /// Represent a field that can be added, edited and removed dynamically
    /// </summary>
    public List<ExtendedField> ExtendedFields { get; set; } = new List<ExtendedField>();
}

/// <summary>
/// 
/// </summary>
public class EditContactRequestValidator : AbstractValidator<EditContactRequest>
{
    /// <summary>
    /// 
    /// </summary>
    public EditContactRequestValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEqual(0);
        RuleFor(x => x)
            .SetInheritanceValidator(x => x.Add<EditContactRequest>(new CreateContactRequestValidator()));
    }
}

/// <summary>
/// 
/// </summary>
public class EditContactRequestProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public EditContactRequestProfile()
    {
        CreateMap<EditContactRequest, Contact>();
    }
}