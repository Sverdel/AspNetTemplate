using AspNetTemplate.Core.Dto;
using CSharpFunctionalExtensions;
using MediatR;

namespace AspNetTemplate.Core.Commands
{
    /// <summary>
    /// <see cref="Handlers.HelloCommandHandler"/>
    /// </summary>
    public class HelloCommand : IRequest<Result<HelloReply>>
    {
        public HelloCommand(HelloRequest request)
        {
            Request = request;
        }

        public HelloRequest Request { get; }
    }
}