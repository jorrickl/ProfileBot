namespace ProfileBot.Application.Activities.Get
{
    public sealed record GetActivitiesResult()
    {
        public required string Message { get; init; }
    }
}
