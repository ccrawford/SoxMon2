
namespace SoxMon2.DTOs
{
    public class AllDivisionsStandingsDTO
    {
        public int DivisionId { get; set; }
        public string? DivisionName { get; set; }
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }

        public StandingsDto[]? Standings { get; set; } = Array.Empty<StandingsDto>();
    }
}
