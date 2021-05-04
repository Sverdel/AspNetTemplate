using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetTemplate.Core.Commands;
using AspNetTemplate.Core.Dto;
using CSharpFunctionalExtensions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace AspNetTemplate.Core.Handlers
{
    [UsedImplicitly]
    public class HelloCommandHandler : MediatR.IRequestHandler<HelloCommand, Result<HelloReply>>
    {
        private readonly ILogger _logger;

        public HelloCommandHandler(ILogger<HelloCommandHandler> logger)
        {
            _logger = logger;
        }
        
        public async Task<Result<HelloReply>> Handle(HelloCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return new HelloReply($"Hello {request.Request?.Name}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while processing VAT check");
                return Result.Failure<HelloReply>($"Error while processing VAT check: {e.Message}");
            }
        }
    }
}