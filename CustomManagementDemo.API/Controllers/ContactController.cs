using CustomManagementDemo.API.DomainLayer.Entities;
using CustomManagementDemo.API.DomainLayer.Requests;
using CustomManagementDemo.API.DomainLayer.Requests.Contacts;
using CustomManagementDemo.API.DomainLayer.Responses.Contacts;
using CustomManagementDemo.API.ServiceLayer.Contacts;
using Microsoft.AspNetCore.Mvc;

namespace CustomManagementDemo.API.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/[Controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="contactService"></param>
    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }
    
    /// <summary>
    /// Add a new field to a specific contact
    /// </summary>
    /// <remarks>
    /// The extended field has a three types { text = 1, number = 2, date = 3 };
    /// TextValue should be filled when choosing text type,
    /// NumberValue should be filled when choosing number type,
    /// DateValue should be filled when choosing date type</remarks>
    /// <param name="id"></param>
    /// <param name="extendedField"></param>
    /// <returns></returns>
    [HttpPost("{id}/extended-field")]
    public async Task<ActionResult<ContactResponse>> AddField([FromRoute] int id, [FromBody] ExtendedField extendedField)
    {
        var contact = await _contactService.AddFieldAsync(id, extendedField);

        return Ok(contact);
    }
    
    /// <summary>
    /// Return a list of filtered fields.
    /// </summary>
    /// <remarks>The filterName should be in PascalCase and the filterValue should be exactly the same</remarks>
    /// <param name="pagedRequest"></param>
    /// <param name="filterRequest"></param>
    /// <returns></returns>
    [HttpGet("filter")]
    public async Task<ActionResult<IList<ContactResponse>>> GetFiltered([FromQuery] PagedRequest pagedRequest, [FromQuery] FilterRequest filterRequest)
    {
        var contacts = await _contactService.GetFiltered(pagedRequest, filterRequest);

        return Ok(contacts);
    }

    /// <summary>
    /// Return a list of all contacts
    /// </summary>
    /// <param name="pagedRequest"></param>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<ActionResult<List<ContactResponse>>> GetAll([FromQuery] PagedRequest pagedRequest)
    {
        var companies = await _contactService.GetAllAsync(pagedRequest);

        return Ok(companies);
    }

    /// <summary>
    /// Return a contact specified by its unique id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactResponse>> GetById([FromRoute] int id)
    {
        var company = await _contactService.GetByIdAsync(id);

        if (company == null)
        {
            return NotFound();
        }

        return Ok(company);
    }

    /// <summary>
    /// Add a new contact
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateContactRequest request)
    {
        await _contactService.CreateAsync(request);

        return Ok();
    }

    /// <summary>
    /// Update an existing contact specified by its unique id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ContactResponse>> EditAsync([FromRoute] int id, [FromBody] EditContactRequest request)
    {
        var contact = _contactService.GetByIdAsync(id);

        if (contact == null)
        {
            return NotFound();
        }
        
        await _contactService.EditAsync(request);

        return NoContent();
    }

    /// <summary>
    /// Remove a contact specified by its unique id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _contactService.DeleteAsync(id);

        return NoContent();
    }
}