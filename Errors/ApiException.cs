using System;

namespace DatingAppServer.Errors;

/// <summary>
/// Model/Entity class for Error formed by api exceptions.
/// Instead of normal constructor we are using new syntax here.
/// </summary>
/// <param name="statusCode">Mandatory parameter</param>
/// <param name="message">Mandatory parameter</param>
/// <param name="details">Optional parameter</param>
public class ApiException(int statusCode, string message, string? details)
{
    /// <summary>
    /// Normal Constructor of Class
    /// </summary>
    /*
    public ApiException(int statusCode, string message, string? details)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }    
    */

    ///Assigning values when passed via constructor
    public int StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = message;
    public string? Details { get; set; } = details;
}
