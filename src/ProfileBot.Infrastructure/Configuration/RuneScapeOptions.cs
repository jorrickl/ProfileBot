using System.ComponentModel.DataAnnotations;

namespace ProfileBot.Infrastructure.Configuration
{
    internal sealed class RuneScapeOptions
    {
        public const string ConfigurationSectionName = "RuneScape";

        [Required]
        public required string ProfileBaseUrl { get; set; }
    }
}
