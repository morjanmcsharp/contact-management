using CustomManagementDemo.API.DomainLayer.Entities;
using CustomManagementDemo.API.DomainLayer.Requests;
using CustomManagementDemo.API.DomainLayer.Requests.Companies;
using CustomManagementDemo.API.DomainLayer.Responses.Companies;

namespace CustomManagementDemo.API.ServiceLayer.Companies;

/// <summary>
/// 
/// </summary>
public interface ICompanyService
{
    /// <summary>
    /// Create a new column for the company table.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="extendedField"></param>
    /// <returns></returns>
    Task<CompanyResponse> AddFieldAsync(int id, ExtendedField extendedField);

    /// <summary>
    /// Return a list of filtered fields.
    /// </summary>
    /// <param name="pagedRequest"></param>
    /// <param name="filterRequest"></param>
    /// <returns></returns>
    Task<List<CompanyResponse>> GetFiltered(PagedRequest pagedRequest, FilterRequest filterRequest);
    
    /// <summary>
    /// Return a list of all companies
    /// </summary>
    /// <param name="pagedRequest"></param>
    /// <returns></returns>
    Task<List<CompanyResponse>> GetAllAsync(PagedRequest pagedRequest);
    
    /// <summary>
    /// Return a company specified by a unique id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<CompanyResponse> GetByIdAsync(int id);
    
    /// <summary>
    /// Create a new company
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task CreateAsync(CreateCompanyRequest request);
    
    /// <summary>
    /// Edit an existing company
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task EditAsync(EditCompanyRequest request);
    
    /// <summary>
    /// Delete an existing company specified by its unique id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(int id);
}