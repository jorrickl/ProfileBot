using MediatR;

namespace ProfileBot.SharedKernel
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
