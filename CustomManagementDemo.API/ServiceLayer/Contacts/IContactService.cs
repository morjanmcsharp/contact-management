using CustomManagementDemo.API.DomainLayer.Entities;
using CustomManagementDemo.API.DomainLayer.Requests;
using CustomManagementDemo.API.DomainLayer.Requests.Contacts;
using CustomManagementDemo.API.DomainLayer.Responses.Contacts;

namespace CustomManagementDemo.API.ServiceLayer.Contacts;

/// <summary>
/// 
/// </summary>
public interface IContactService
{
    /// <summary>
    /// Create a new column for the contact table.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="extendedField"></param>
    /// <returns></returns>
    Task<ContactResponse> AddFieldAsync(int id, ExtendedField extendedField);

    /// <summary>
    /// Return a list of filtered fields and values
    /// </summary>
    /// <param name="pagedRequest"></param>
    /// <param name="filterRequest"></param>
    /// <returns></returns>
    Task<List<ContactResponse>> GetFiltered(PagedRequest pagedRequest, FilterRequest filterRequest);
    
    /// <summary>
    /// Return a list of all contacts
    /// </summary>
    /// <param name="pagedRequest"></param>
    /// <returns></returns>
    Task<List<ContactResponse>> GetAllAsync(PagedRequest pagedRequest);
    
    /// <summary>
    /// Return a contact specified by a unique id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ContactResponse> GetByIdAsync(int id);
    
    /// <summary>
    /// Create a new contact
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task CreateAsync(CreateContactRequest request);
    
    /// <summary>
    /// Edit an existing contact
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task EditAsync(EditContactRequest request);
    
    /// <summary>
    /// Delete an existing contact specified by its unique id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(int id);
}