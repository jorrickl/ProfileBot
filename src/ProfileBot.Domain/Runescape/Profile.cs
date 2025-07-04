namespace ProfileBot.Domain.Runescape
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
        public Activity[] Activities { get; set; } = [];
        public Skillvalue[] SkillValues { get; set; } = [];
        public string Name { get; set; } = string.Empty;
        public string Rank { get; set; } = string.Empty;
        public long Melee { get; set; }
        public int CombatLevel { get; set; }
        public string LoggedIn { get; set; } = string.Empty;
    }
}