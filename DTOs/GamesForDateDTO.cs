namespace SoxMon2.DTOs
{
    public class GamesForDateDTO
    {
        public string? HomeAbbr { get; set; }
        public string? AwayAbbr { get; set; }
        public string? Inning { get; set; }
        public int? HomeTeamRuns { get; set; }
        public int? AwayTeamRuns { get; set; }
 //       public DateTime? GameTime { get; set; }
        public string? GameStatus { get; set; }

        public long? GameTimeUnix { get; set; }
    }
}
