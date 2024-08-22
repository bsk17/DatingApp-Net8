using System;
using DatingAppServer.Data;
using DatingAppServer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppServer.Controllers;

/// <summary>
/// BuggyController is a dummy controller.
/// Purposefully throws error so that error handling can be practiced at a higher level.
/// {baseurl}/api/buggy/
/// </summary>
/// <param name="context">Dependency Injection of DBContext using new dotnet syntax</param>
public class BuggyController(DataContext context) : BaseApiController
{
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetAuth()
    {
        return "Secret Text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var thing = context.Users.Find(-1);
        if (thing == null)
        {
            return NotFound();
        }
        return thing;
    }

    [HttpGet("server-error")]
    public ActionResult<AppUser> GetServerError()
    {
        /*
        try
        {
           var thing = context.Users.Find(-1) ?? throw new Exception("Some Server error     happened");
           return thing;
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Computer syas no!");
        }

        //we can add the try catch to handle error at every single actionresult, but that would be too tedious
        hence we create a custom middleware for handling all such kinds of exceptions 
        go to Errors -> ApiExceptions.cs (for Error Model) and Middleware -> ExceptionMiddleware (the logic to perform when error is encountered) and 
        program.cs -> app.UseMiddleware<ExceptionMiddleware>(); to utilise the middleware
        */

        var thing = context.Users.Find(-1) ?? throw new Exception("Some Server error happened");
        return thing;

    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This was not a good request!");
    }
}
