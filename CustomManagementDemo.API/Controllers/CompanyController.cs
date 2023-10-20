using CustomManagementDemo.API.DomainLayer.Entities;
using CustomManagementDemo.API.DomainLayer.Requests;
using CustomManagementDemo.API.DomainLayer.Requests.Companies;
using CustomManagementDemo.API.DomainLayer.Responses.Companies;
using CustomManagementDemo.API.ServiceLayer.Companies;
using Microsoft.AspNetCore.Mvc;

namespace CustomManagementDemo.API.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/[Controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="companyService"></param>
    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }
    
    /// <summary>
    /// Add a new field to a specific company
    /// </summary>
    /// <remarks>
    /// The extended field has a three types { text = 1, number = 2, date = 3 };
    /// TextValue should be filled when choosing text type,
    /// NumberValue should be filled when choosing number type,
    /// DateValue should be filled when choosing date type
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="extendedField"></param>
    /// <returns></returns>
    [HttpPost("{id}/extended-field")]
    public async Task<ActionResult<CompanyResponse>> AddField([FromRoute] int id, [FromBody] ExtendedField extendedField)
    {
        var company = await _companyService.AddFieldAsync(id, extendedField);

        return Ok(company);
    }
    
    /// <summary>
    /// Return a list of companies using filtered fields.
    /// </summary>
    /// <remarks>The filterName should be in PascalCase and the filterValue should be exactly the same</remarks>
    /// <param name="pagedRequest"></param>
    /// <param name="filterRequest"></param>
    /// <returns></returns>
    [HttpGet("filter")]
    public async Task<ActionResult<IList<CompanyResponse>>> GetFiltered([FromQuery] PagedRequest pagedRequest, [FromQuery] FilterRequest filterRequest)
    {
        var companies = await _companyService.GetFiltered(pagedRequest, filterRequest);

        return Ok(companies);
    }

    /// <summary>
    /// Return a list of all companies
    /// </summary>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<ActionResult<List<CompanyResponse>>> GetAll([FromQuery] PagedRequest pagedRequest)
    {
        var companies = await _companyService.GetAllAsync(pagedRequest);

        return Ok(companies);
    }

    /// <summary>
    /// Return a company specified by its unique id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyResponse>> GetById([FromRoute] int id)
    {
        var company = await _companyService.GetByIdAsync(id);

        if (company == null)
        {
            return NotFound();
        }

        return Ok(company);
    }

    /// <summary>
    /// Add a new company
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCompanyRequest request)
    {
         await _companyService.CreateAsync(request);

        return Ok();
    }

    /// <summary>
    /// Update an existing company specified by its unique id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] EditCompanyRequest request)
    {
        var company = await _companyService.GetByIdAsync(id);

        if (company == null)
        {
            return NotFound();
        }
        
        await _companyService.EditAsync(request);

        return NoContent();
    }

    /// <summary>
    /// Remove a company specified by its unique id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _companyService.DeleteAsync(id);
        
        return NoContent();
    }
}