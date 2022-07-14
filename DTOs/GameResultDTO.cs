namespace SoxMon2.DTOs
{
    public class GameResultDTO
    {

        public string? Display { get; set; }
        public string? HomeAbbr { get; set; }
        public string? AwayAbbr { get; set; }

        public int HomeRuns { get; set; }
        public int HomeHits { get; set; }
        public int HomeErrors { get; set; }

        public int AwayRuns { get; set; }
        public int AwayHits { get; set; }
        public int AwayErrors { get; set; }

        public string GameDayOfWeek { get; set; }

        public string GameState { get; set; }

        public DateTime GameTime { get; set; }

        public string DayNight { get; set; }

    }
}
