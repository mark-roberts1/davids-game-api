namespace Davids.Game.Data;

public partial class SurfaceType
{
    public short Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Venue> Venues { get; set; } = new List<Venue>();
}
