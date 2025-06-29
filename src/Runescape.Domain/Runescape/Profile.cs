namespace Runescape.Domain.Domain
{
    public class Profile
    {
        public long Magic { get; set; }
        public int QuestsStarted { get; set; }
        public int TotalSkill { get; set; }
        public int QuestsComplete { get; set; }
        public int QuestsNotStarted { get; set; }
        public long TotalXp { get; set; }
        public long Ranged { get; set; }
        public required Activity[] Activities { get; set; }
        public required Skillvalue[] SkillValues { get; set; }
        public required string Name { get; set; }
        public required string Rank { get; set; }
        public long Melee { get; set; }
        public int CombatLevel { get; set; }
        public required string LoggedIn { get; set; }

        public Profile()
        {
            Activities = [];
            SkillValues = [];
            Name = string.Empty;
            Rank = string.Empty;
            LoggedIn = string.Empty;
        }
    }
}