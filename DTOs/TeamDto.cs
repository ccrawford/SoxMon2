namespace SoxMon2.DTOs
{
    public class TeamDto
    {

        /// <summary>
        /// The team's name (eg: "Jays").
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Team hometown.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Team ID from the API (used for building other calls).
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The league ID (used for building other league calls).
        /// </summary>
        public int? LeagueId { get; set; }


        /// <summary>
        /// Division ID from the API (used for building other calls).
        /// </summary>
        public int? DivId { get; set; }
        public string? Abbr { get; set; }
    }
}
