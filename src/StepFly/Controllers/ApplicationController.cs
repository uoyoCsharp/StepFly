using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace StepFly.Controllers
{
    [ApiController]
    public abstract class ApplicationController : ControllerBase
    {
        public CancellationToken AbortToken => HttpContext.RequestAborted;
    }
}
