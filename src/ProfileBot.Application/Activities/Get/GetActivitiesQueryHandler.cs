using Ardalis.Result;
using ProfileBot.Application.Interfaces;
using ProfileBot.Infrastructure.Interfaces;
using ProfileBot.SharedKernel;

namespace ProfileBot.Application.Activities.Get
{
    public sealed class GetActivitiesQueryHandler(IProfileClient profileClient, IActivityFormatter activityFormatter)
        : IQueryHandler<GetActivitiesQuery, Result<GetActivitiesResult>>
    {
        public async Task<Result<GetActivitiesResult>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
        {
            var profile = await profileClient.GetProfileAsync(request.Username).ConfigureAwait(false);

            if (profile is null)
            {
                return Result.NotFound($"Profile not found for RSN: {request.Username}!");
            }

            var formattedActivities = activityFormatter.FormatActivities(profile);

            return Result.Success(new GetActivitiesResult()
            {
                Message = formattedActivities
            });
        }
    }
}
