using System;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppServer.Controllers;

/// <summary>
/// Class <c>BaseApiController</c> acts as the controller class which contains common attributes and implementations.
/// </summary>

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{

}
