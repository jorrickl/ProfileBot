using Ardalis.Result;
using ProfileBot.Infrastructure.Interfaces;
using ProfileBot.SharedKernel;

namespace ProfileBot.Application.Activities.Get
{
    public sealed class GetProfileQueryHandler(IProfileClient profileClient)
        : IQueryHandler<GetProfileQuery, Result<GetProfileResult>>
    {
        public async Task<Result<GetProfileResult>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await profileClient.GetProfileAsync(request.Username).ConfigureAwait(false);

            if (profile is null)
            {
                return Result.NotFound($"Profile not found for RSN: {request.Username}!");
            }

            return Result.Success(new GetProfileResult()
            {
                UserProfile = profile
            });
        }
    }
}
