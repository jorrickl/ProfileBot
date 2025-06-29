using MediatR;

namespace ProfileBot.SharedKernel
{
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
            where TCommand : ICommand<TResponse>
    {
    }
}
