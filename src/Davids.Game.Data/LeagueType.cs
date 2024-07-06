namespace Davids.Game.Data;

public partial class LeagueType
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<League> Leagues { get; set; } = new List<League>();
}
