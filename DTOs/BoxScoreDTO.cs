namespace SoxMon2.DTOs
{
    public class BoxScoreDTO
    {
        public string HomeAbbr { get; set; }
        public string AwayAbbr { get; set; }

        public string HomeLineScore { get; set; }

        public string AwayLineScore { get; set; }

        public int CurInning { get; set; }

        public string? InningState { get; set; }

        public string? InningHalf { get; set; }

        public int HometeamRunsGame { get; set; }
        public int AwayteamRunsGame { get; set; }

        public int HometeamHitsGame { get; set; }
        public int AwayteamHitsGame { get; set; }

        public int HometeamErrorsGame { get; set; }
        public int AwayteamErrorsGame { get; set; }

        public string Pitcher { get; set; }
        public string Batter { get; set; }

        public int Outs { get; set; }

        public string GameStatus { get; set; }

        public string StatusBlurb { get; set; }

        public DateTime GameTime { get; set; }

        public string DayNight { get; set; }

        public string DoW { get; set; }

        public bool ManOnFirst { get; set; } = false;
        public bool ManOnSecond { get; set; } = false;
        public bool ManOnThird { get; set; } = false;

        public string? LastComment { get; set; }


    }
}
