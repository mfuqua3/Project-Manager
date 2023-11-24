using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Common.Validation;

namespace ProjectManager.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ValidateRequests]
public class ApiController : ControllerBase
{
    
}