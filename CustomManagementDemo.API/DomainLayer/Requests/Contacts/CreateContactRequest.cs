using AutoMapper;
using CustomManagementDemo.API.DomainLayer.Entities.Contacts;
using FluentValidation;

namespace CustomManagementDemo.API.DomainLayer.Requests.Contacts;

/// <summary>
/// 
/// </summary>
public class CreateContactRequest
{
    /// <summary>
    /// Represent the unique name of the contact
    /// </summary>
    /// <example>Morjan</example>
    public string Name { get; set; }
}

/// <summary>
/// 
/// </summary>
public class CreateContactRequestValidator : AbstractValidator<CreateContactRequest>
{
    /// <summary>
    /// 
    /// </summary>
    public CreateContactRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
    }
}

/// <summary>
/// 
/// </summary>
public class CreateContactRequestProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public CreateContactRequestProfile()
    {
        CreateMap<CreateContactRequest, Contact>();
    }
}