using System.Globalization;
using AutoMapper;
using CustomManagementDemo.API.DataAccessLayer;
using CustomManagementDemo.API.DataAccessLayer.Services;
using CustomManagementDemo.API.DomainLayer.Constants;
using CustomManagementDemo.API.DomainLayer.Entities;
using CustomManagementDemo.API.DomainLayer.Entities.Companies;
using CustomManagementDemo.API.DomainLayer.Entities.Contacts;
using CustomManagementDemo.API.DomainLayer.Enums;
using CustomManagementDemo.API.DomainLayer.Requests;
using CustomManagementDemo.API.DomainLayer.Requests.Contacts;
using CustomManagementDemo.API.DomainLayer.Responses.Companies;
using CustomManagementDemo.API.DomainLayer.Responses.Contacts;
using FluentValidation;
using MongoDB.Driver;

namespace CustomManagementDemo.API.ServiceLayer.Contacts;

/// <summary>
/// 
/// </summary>
public class ContactService : IContactService
{
    private readonly IMapper _mapper;
    private readonly NumericIdGenerator _numericIdGenerator;

    #region Ctor & props

    private readonly IMongoCollection<Contact> _contactCollection;
    private readonly IMongoCollection<Company> _companyCollection;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="numericIdGenerator"></param>
    public ContactService(ApplicationDbContext context, IMapper mapper, NumericIdGenerator numericIdGenerator)
    {
        _mapper = mapper;
        _numericIdGenerator = numericIdGenerator;
        
        _companyCollection = context.GetCollection<Company>(CollectionNames.CompanyCollection);
        _contactCollection = context.GetCollection<Contact>(CollectionNames.ContactCollection);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Create a new column for the contact table.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="extendedField"></param>
    /// <returns></returns>
    public async Task<ContactResponse> AddFieldAsync(int id, ExtendedField extendedField)
    {
        var contact = await _contactCollection.FindAsync(x => x.Id == id);
        var existingContact = await contact.FirstOrDefaultAsync();

        if (existingContact != null)
        {
            FieldProcess(extendedField, existingContact);

            await _contactCollection.ReplaceOneAsync(x => x.Id == id, existingContact);
        }

        var mappedResponse = _mapper.Map<Contact, ContactResponse>(existingContact);

        await MapRelatedCompanies(mappedResponse);

        return mappedResponse;
    }

    /// <summary>
    /// Return a list of filtered fields and values
    /// </summary>
    /// <param name="pagedRequest"></param>
    /// <param name="filterRequest"></param>
    /// <returns></returns>
    public async Task<List<ContactResponse>> GetFiltered(PagedRequest pagedRequest, FilterRequest filterRequest)
    {
        var filter = FilterProcess(filterRequest.FilterName, filterRequest.FilterValue, filterRequest.FilterExtendedFields);
        
        var skip = (pagedRequest.Page - 1) * pagedRequest.PageSize;
        
        var contacts = await _contactCollection.Find(filter)
            .Skip(skip)
            .Limit(pagedRequest.PageSize)
            .ToListAsync();

        var mappedResponse = _mapper.Map<List<Contact>, List<ContactResponse>>(contacts);

        await MapRelatedCompanies(mappedResponse);

        return mappedResponse;
    }

    /// <summary>
    /// Return a list of all contacts
    /// </summary>
    /// <param name="pagedRequest"></param>
    /// <returns></returns>
    public async Task<List<ContactResponse>> GetAllAsync(PagedRequest pagedRequest)
    {
        var skip = (pagedRequest.Page - 1) * pagedRequest.PageSize;

        var contacts = await _contactCollection.Find(contact => true)
            .Skip(skip)
            .Limit(pagedRequest.PageSize)
            .ToListAsync();
        
        var contactResponse = _mapper.Map<List<Contact>, List<ContactResponse>>(contacts);
        
        await MapRelatedCompanies(contactResponse);

        return contactResponse;
    }

    /// <summary>
    /// Return a contact specified by a unique id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ContactResponse> GetByIdAsync(int id)
    {
        var contact = await _contactCollection.FindAsync(x => x.Id == id);

        var contactResponse = _mapper.Map<Contact, ContactResponse>(await contact.FirstOrDefaultAsync());

        await MapRelatedCompanies(contactResponse);

        return contactResponse;
    }

    /// <summary>
    /// Create a new contact
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task CreateAsync(CreateContactRequest request)
    {
        await ValidateCreateRequest(request);
        
        await ValidateNameExists(request.Name);
        
        var mappedEntity = _mapper.Map<CreateContactRequest, Contact>(request);
        
        // Here we generate an id as an auto-incremented for the entity.
        mappedEntity.Id = _numericIdGenerator.GenerateId(CollectionNames.ContactCollection);
        
        await _contactCollection.InsertOneAsync(mappedEntity);
    }

    /// <summary>
    /// Edit an existing contact
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task EditAsync(EditContactRequest request)
    {
        await ValidateEditRequest(request);
        
        await ValidateNameExists(request.Name, request.Id);
        
        var mappedEntity = _mapper.Map<CreateContactRequest, Contact>(request);

        FieldsProcess(mappedEntity);
        
        await _contactCollection.ReplaceOneAsync(x=>x.Id == mappedEntity.Id, mappedEntity);
    }

    /// <summary>
    /// Delete an existing contact specified by its unique id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int id)
    {
        await _contactCollection.DeleteOneAsync(x => x.Id == id);
    }

    #endregion

    #region Utilities

    private async Task ValidateNameExists(string name, int? id = null)
    {
        if (String.IsNullOrEmpty(name)) return;

        var normalizedName = name.ToLower().Trim();
        
        // Check if the name is unique among companies
        var filter = Builders<Contact>.Filter.Where(c => c.Name.Trim().ToLower() == normalizedName) 
                     & Builders<Contact>.Filter.Ne(c => c.Id, id);

        if (await _contactCollection.Find(filter).AnyAsync())
        {
            // Throw an exception or handle the validation error accordingly
            throw new InvalidOperationException("Contact with the same name already exists.");
        }
    }
    
    private async Task MapRelatedCompanies(ContactResponse contact)
    {
            var relatedCompanies = await _companyCollection.FindAsync(x => x.ContactId == contact.Id);
            var mappedCompanyResponse = _mapper.Map<List<Company>, List<CompanyResponse>>(await relatedCompanies.ToListAsync());
            contact.Companies = mappedCompanyResponse;
    }
    
    private async Task MapRelatedCompanies(IEnumerable<ContactResponse> contacts)
    {
        foreach (var contact in contacts)
        {
            var relatedCompanies = await _companyCollection.FindAsync(x => x.ContactId == contact.Id);
            var mappedCompanyResponse = _mapper.Map<List<Company>, List<CompanyResponse>>(await relatedCompanies.ToListAsync());
            contact.Companies = mappedCompanyResponse;
        }
    }
    
    private FilterDefinition<Contact> FilterProcess(string filterName = null, string filterValue = null, bool filterExtendedFields = false)
    {
        var filter = Builders<Contact>.Filter.Empty;
        
        if (filterExtendedFields)
        {
            // Extended fields filter
            filter = Builders<Contact>.Filter.ElemMatch(x => x.ExtendedFields, Builders<ExtendedField>.Filter.Eq(filterName, filterValue));
        }
        else
        {
            filter = Builders<Contact>.Filter.Eq(filterName, filterValue);
        }

        return filter;
    }

    private void FieldsProcess(Contact contact)
    {
        foreach (var extendedField in contact.ExtendedFields)
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

    private void FieldProcess(ExtendedField extendedField, Contact contact)
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
            default:
                throw new ArgumentOutOfRangeException();
        }

        contact.ExtendedFields.Add(extendedField);
    }
    
    private async Task ValidateCreateRequest(CreateContactRequest request)
    {
        var validator = new CreateContactRequestValidator();
        await validator.ValidateAndThrowAsync(request);
    }
    
    private async Task ValidateEditRequest(EditContactRequest request)
    {
        var validator = new EditContactRequestValidator();

        await validator.ValidateAndThrowAsync(request);
    }
    
    #endregion
    
    
}