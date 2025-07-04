namespace ProfileBot.Domain.Runescape
{
    public record Activity
    {
        public string Date { get; init; }
        public string Details { get; init; }
        public string Text { get; init; }

        public Activity()
        {
            Date = string.Empty;
            Details = string.Empty;
            Text = string.Empty;
        }
    }
}