namespace Runescape.Domain.Domain
{
    public class Activity
    {
        public required string Date { get; set; }
        public required string Details { get; set; }
        public required string Text { get; set; }

        public Activity()
        {
            Date = string.Empty;
            Details = string.Empty;
            Text = string.Empty;
        }
    }
}