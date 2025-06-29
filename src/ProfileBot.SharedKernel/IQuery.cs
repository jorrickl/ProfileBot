using MediatR;

namespace ProfileBot.SharedKernel
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}