using CustomManagementDemo.API.DomainLayer.Enums;

namespace CustomManagementDemo.API.DomainLayer.Entities;

/// <summary>
/// 
/// </summary>
public class ExtendedField
{
    public string Name { get; set; }
    public FieldType Type { get; set; }
    public string TextValue { get; set; }
    public DateTime? DateValue { get; set; }
    public int? NumberValue { get; set; }
}