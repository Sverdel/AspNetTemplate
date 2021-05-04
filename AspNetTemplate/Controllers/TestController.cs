using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using AspNetTemplate.Core.Commands;
using AspNetTemplate.Core.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetTemplate.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json )]
    [Produces(MediaTypeNames.Application.Json )]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("test")]
        public async Task<ActionResult<HelloReply>> TestAsync(HelloRequest request, CancellationToken cancellation)
        {
            var result = await _mediator.Send(new HelloCommand(request), cancellation);

            if (result.IsSuccess)
            {
                return result.Value;
            }

            return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
        }
    }
}