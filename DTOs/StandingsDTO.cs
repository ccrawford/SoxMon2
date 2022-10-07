namespace SoxMon2.DTOs
{
    public class StandingsDto
    {
        public string? displayText { get; set; }
        public int teamId { get; set; }

        public string? divisionRank { get; set; }
        public string? pct { get; set; }

        public string? gamesBack { get; set; }

        public string? teamName { get; set; }

        public int wins { get; set; }
        public int losses { get; set; }

    }
}
