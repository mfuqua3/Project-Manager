using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ApiController : Controller
{
    
}