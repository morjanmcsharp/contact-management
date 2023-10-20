
using CustomManagementDemo.API.DomainLayer.Requests.Companies;
using CustomManagementDemo.API.DomainLayer.Requests.Contacts;
using FluentValidation;

namespace CustomManagementDemo.API.Extensions.Services;

/// <summary>
/// 
/// </summary>
public static class FluentValidationsExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void AddFluentValidations(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddValidatorsFromAssemblyContaining<CreateCompanyRequestValidator>();
        serviceCollection.AddValidatorsFromAssemblyContaining<EditCompanyRequestValidator>();
        serviceCollection.AddValidatorsFromAssemblyContaining<CreateContactRequestValidator>();
        serviceCollection.AddValidatorsFromAssemblyContaining<EditContactRequestValidator>();
    }
}