using CustomManagementDemo.API.DataAccessLayer.Services;
using CustomManagementDemo.API.ServiceLayer.Companies;
using CustomManagementDemo.API.ServiceLayer.Contacts;
using MongoDB.Driver;

namespace CustomManagementDemo.API.Extensions.Services;

/// <summary>
/// 
/// </summary>
public static class ServicesExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICompanyService, CompanyService>();
        serviceCollection.AddScoped<IContactService, ContactService>();
        serviceCollection.AddScoped<NumericIdGenerator>();
    }
}