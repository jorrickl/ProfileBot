namespace ProfileBot.Application.Activities.Get
{
    public sealed record GetActivitiesResult()
    {
        public required string Activities { get; init; }
    }
}
