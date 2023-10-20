namespace CustomManagementDemo.API.DomainLayer.Requests;

/// <summary>
/// 
/// </summary>
public class PagedRequest
{
    /// <summary>
    /// Represent the page
    /// </summary>
    /// <example>1</example>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Represent the size of the page
    /// </summary>
    /// <example>10</example>
    public int PageSize { get; set; } = 10;
}