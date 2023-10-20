namespace CustomManagementDemo.API.DomainLayer.Requests;

/// <summary>
/// 
/// </summary>
public class FilterRequest
{
    /// <summary>
    /// Represent the name of the field to be filtered
    /// </summary>
    /// <example>Name</example>
    public string FilterName { get; set; }
    
    /// <summary>
    /// Represent the value of the field to be filtered
    /// </summary>
    /// <example>Morjan</example>
    public string FilterValue { get; set; }

    /// <summary>
    /// Represent whether it is going to be the standard fields or the extended fields.
    /// </summary>
    /// <example>BirthDate</example>
    public bool FilterExtendedFields { get; set; } = false;
}