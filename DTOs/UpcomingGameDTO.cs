namespace SoxMon2.DTOs
{
    public class UpcomingGameDTO
    {
        public int GamePk { get; set; }
        public string HomeAbbr { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayAbbr { get; set; }
        public string AwayTeamName { get; set; }

        public string HomePitcher { get; set; }
        public string AwayPitcher { get; set; }

        public string GameStatus { get; set; }

        public string StatusBlurb { get; set; }

        public DateTime GameTime { get; set; }
        public long GameTimeUnix { get; set; }

        public string DayNight { get; set; }

        public int? TotalGames { get; set; }


    }
}
