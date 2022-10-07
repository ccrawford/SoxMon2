namespace SoxMon2.DTOs
{
    public class WildcardDTO
    {
        public int teamId { get; set; }
        public int leagueId { get; set; }
        public string? wildCardRank { get; set; }
        
        public string? wildCardGamesBack { get; set; }

        public bool? clinched { get; set; }
        public string? magicNumber { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }

        
    }
}
