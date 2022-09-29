namespace noCarbon.API.Models;
/// <summary>
/// Represent register request 
/// </summary>
public class AddCustomerInput
{
    /// <summary>
    /// Get or set username
    /// </summary>
    /// <example>moulkhalf</example>
    public string UserName { get; set; }
    /// <summary>
    /// Get or set mail
    /// </summary>
    /// <example>oulkhalfmustapha@gmail.com</example>
    public string Mail { get; set; }
    /// <summary>
    /// Get or set password
    /// </summary>
    /// <example>123456</example>
    public string Password { get; set; }
}
