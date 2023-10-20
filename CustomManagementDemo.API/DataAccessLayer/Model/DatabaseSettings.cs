namespace CustomManagementDemo.API.DataAccessLayer.Model;

/// <summary>
/// 
/// </summary>
public class DatabaseSettings : IDatabaseSettings
{
    /// <summary>
    /// 
    /// </summary>
    public string ConnectionString { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string DatabaseName { get; set; }
}

/// <summary>
/// 
/// </summary>
public interface IDatabaseSettings
{
    /// <summary>
    /// 
    /// </summary>
    string ConnectionString { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    string DatabaseName { get; set; }
}