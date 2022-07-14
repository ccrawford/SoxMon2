namespace SoxMon2.DTOs
{
    public class LastGameNextGameDTO
    {
        public string LastGamePk { get; set; }
        public string NextGamePk { get; set; }
        public int MinutesToNextGame { get; set; }
        public int MinutesSinceLastGame { get; set; }

    }
}
