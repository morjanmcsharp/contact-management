using System.Data;
using AutoMapper;
using CustomManagementDemo.API.DomainLayer.Entities.Companies;
using FluentValidation;

namespace CustomManagementDemo.API.DomainLayer.Requests.Companies;

/// <summary>
/// 
/// </summary>
public class CreateCompanyRequest
{
    /// <summary>
    /// Represent the unique name of the company
    /// </summary>
    /// <example>Workium</example>
    public string Name { get; set; }
    
    /// <summary>
    /// Represent the number of the employees in the company
    /// </summary>
    /// <example>50</example>
    public int EmployeesCount { get; set; }
    
    /// <summary>
    /// Represent the foreign key of the contact
    /// </summary>
    /// <example>3</example>
    public int ContactId { get; set; }
}

/// <summary>
/// 
/// </summary>
public class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
{
    /// <summary>
    /// 
    /// </summary>
    public CreateCompanyRequestValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty();
    }
}

/// <summary>
/// 
/// </summary>
public class CreateCompanyRequestProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public CreateCompanyRequestProfile()
    {
        CreateMap<CreateCompanyRequest, Company>();
    }
}