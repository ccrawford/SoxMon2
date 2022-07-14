namespace SoxMon2.DTOs
{
    public class LastGameDTO
    {
        public bool IsHomeGame { get; set; }
        public string HomeShort { get; set; }
        public string AwayShort { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        public string WinnerShort { get; set; }

        public string DisplayString { get; set; }



    }
}
