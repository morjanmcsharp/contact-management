using AutoMapper;
using CustomManagementDemo.API.DataAccessLayer;
using CustomManagementDemo.API.DataAccessLayer.Services;
using CustomManagementDemo.API.DomainLayer.Constants;
using CustomManagementDemo.API.DomainLayer.Entities;
using CustomManagementDemo.API.DomainLayer.Entities.Companies;
using CustomManagementDemo.API.DomainLayer.Entities.Contacts;
using CustomManagementDemo.API.DomainLayer.Enums;
using CustomManagementDemo.API.DomainLayer.Requests;
using CustomManagementDemo.API.DomainLayer.Requests.Companies;
using CustomManagementDemo.API.DomainLayer.Responses.Companies;
using FluentValidation;
using MongoDB.Driver;

namespace CustomManagementDemo.API.ServiceLayer.Companies;

/// <summary>
/// 
/// </summary>
public class CompanyService : ICompanyService
{
    private readonly IMapper _mapper;
    private readonly NumericIdGenerator _numericIdGenerator;

    #region Ctor & props

    private readonly IMongoCollection<Company> _companyCollection;
    private readonly IMongoCollection<Contact> _contactCollection;

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="numericIdGenerator"></param>
    public CompanyService(ApplicationDbContext context, IMapper mapper, NumericIdGenerator numericIdGenerator)
    {
        _mapper = mapper;
        _numericIdGenerator = numericIdGenerator;
        _companyCollection = context.GetCollection<Company>(CollectionNames.CompanyCollection);
        _contactCollection = context.GetCollection<Contact>(CollectionNames.ContactCollection);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Create a new column for the company table.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="extendedField"></param>
    /// <returns></returns>
    public async Task<CompanyResponse> AddFieldAsync(int id, ExtendedField extendedField)
    {
        var company = await _companyCollection.FindAsync(x => x.Id == id);
        var existingCompany = await company.FirstOrDefaultAsync();

        if (existingCompany != null)
        {
            FieldProcess(extendedField, existingCompany);

            await _companyCollection.ReplaceOneAsync(x => x.Id == id, existingCompany);
        }

        var mappedResponse = _mapper.Map<Company, CompanyResponse>(existingCompany);
        
        return mappedResponse;
    }

    /// <summary>
    /// Return a list of filtered fields.
    /// </summary>
    /// <param name="pagedRequest"></param>
    /// <param name="filterRequest"></param>
    /// <returns></returns>
    public async Task<List<CompanyResponse>> GetFiltered(PagedRequest pagedRequest, FilterRequest filterRequest)
    {
        var filter = FilterProcess(filterRequest.FilterName, filterRequest.FilterValue, filterRequest.FilterExtendedFields);

        var skip = (pagedRequest.Page - 1) * pagedRequest.PageSize;

        var companies = await _companyCollection.Find(filter)
            .Skip(skip)
            .Limit(pagedRequest.PageSize)
            .ToListAsync();
        
        var mappedResponse = _mapper.Map<List<Company>, List<CompanyResponse>>(companies);
        
        return mappedResponse;
    }

    /// <summary>
    /// Return a list of all companies
    /// </summary>
    /// <returns></returns>
    public async Task<List<CompanyResponse>> GetAllAsync(PagedRequest pagedRequest)
    {
        var skip = (pagedRequest.Page - 1) * pagedRequest.PageSize;
        
        var companies = await _companyCollection.Find(company => true)
            .Skip(skip)
            .Limit(pagedRequest.PageSize)
            .ToListAsync();

        var companyResponse = _mapper.Map<List<Company>, List<CompanyResponse>>(companies);

        return companyResponse;
    }

    /// <summary>
    /// Return a company specified by a unique id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<CompanyResponse> GetByIdAsync(int id)
    {
        var company = await _companyCollection.FindAsync(x => x.Id == id);

        var companyResponse = _mapper.Map<Company, CompanyResponse>(await company.FirstOrDefaultAsync());

        return companyResponse;
    }

    /// <summary>
    /// Create a new company
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task CreateAsync(CreateCompanyRequest request)
    {
        await ValidateCreateRequest(request);
        
        await ValidateNameExists(request.Name);

        await ValidateContactExists(request.ContactId);
        
        var mappedEntity = _mapper.Map<CreateCompanyRequest, Company>(request);
        
        // Here we generate an id as an auto-incremented for the entity.
        mappedEntity.Id = _numericIdGenerator.GenerateId(CollectionNames.CompanyCollection);
        
        await _companyCollection.InsertOneAsync(mappedEntity);
    }
    
    /// <summary>
    /// Edit an existing company
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task EditAsync(EditCompanyRequest request)
    {
        await ValidateEditRequest(request);

        await ValidateNameExists(request.Name, request.Id);
        
        await ValidateContactExists(request.ContactId);

        var mappedEntity = _mapper.Map<EditCompanyRequest, Company>(request);
        
        FieldsProcess(mappedEntity);
        
        await _companyCollection.ReplaceOneAsync(x=>x.Id == mappedEntity.Id, mappedEntity);
    }

    /// <summary>
    /// Delete an existing company specified by its unique id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int id)
    {
        await _companyCollection.DeleteOneAsync(x => x.Id == id);
    }

    #endregion

    #region Utilities

    private async Task ValidateCreateRequest(CreateCompanyRequest request)
    {
        var validator = new CreateCompanyRequestValidator();
        await validator.ValidateAndThrowAsync(request);
    }
    
    private async Task ValidateEditRequest(EditCompanyRequest request)
    {
        var validator = new EditCompanyRequestValidator();
        await validator.ValidateAndThrowAsync(request);
    }
    private async Task ValidateNameExists(string name, int? id = null)
    {
        if (String.IsNullOrEmpty(name)) return;

        var normalizedName = name.ToLower().Trim();
        
        // Check if the name is unique among companies
        var filter = Builders<Company>.Filter.Where(c => c.Name.Trim().ToLower() == normalizedName) 
                     & Builders<Company>.Filter.Ne(c => c.Id, id);

        
        if (await _companyCollection.Find(filter).AnyAsync())
        {
            // Throw an exception or handle the validation error accordingly
            throw new InvalidOperationException("Company with the same name already exists.");
        }
    }

    private async Task ValidateContactExists(int contactId)
    {
        var filter = Builders<Contact>.Filter.Eq(x => x.Id, contactId);
        
        var contact = await _contactCollection.Find(filter).FirstOrDefaultAsync();

        if (contact == null)
        {
            // Throw an exception or handle the validation error accordingly
            throw new InvalidOperationException("Contact does not exists.");
        }
    }
    
    private FilterDefinition<Company> FilterProcess(string filterName = null, string filterValue = null, bool filterExtendedFields = false)
    {
        var filter = Builders<Company>.Filter.Empty;
        
        if (filterExtendedFields)
        {
            // Extended fields filter
            filter = Builders<Company>.Filter.ElemMatch(x => x.ExtendedFields, Builders<ExtendedField>.Filter.Eq(filterName, filterValue));
        }
        else
        {
            // Standard fields filter
            filter = Builders<Company>.Filter.Eq(filterName, filterValue);
        }

        return filter;
    }
    
    private void FieldsProcess(Company company)
    {
        foreach (var extendedField in company.ExtendedFields)
        {
            switch (extendedField.Type)
            {
                case FieldType.Text:
                    extendedField.NumberValue = null;
                    extendedField.DateValue = null;
                    return;
                case FieldType.Number:
                    extendedField.DateValue = null;
                    extendedField.TextValue = null;
                    return;
                case FieldType.Date:
                    extendedField.NumberValue = null;
                    extendedField.TextValue = null;
                    return;
            }
        }
    }

    private void FieldProcess(ExtendedField extendedField, Company company)
    {
        switch (extendedField.Type)
        {
            case FieldType.Text:
                extendedField.NumberValue = null;
                extendedField.DateValue = null;
                break;
            case FieldType.Number:
                extendedField.DateValue = null;
                extendedField.TextValue = null;
                break;
            case FieldType.Date:
                extendedField.NumberValue = null;
                extendedField.TextValue = null;
                break;
        }

        company.ExtendedFields.Add(extendedField);


        #endregion
    }
}