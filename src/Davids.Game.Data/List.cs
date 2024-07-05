namespace Davids.Game.Data;

public partial class List
{
    public long Id { get; set; }

    public long? PreviousListId { get; set; }

    public long UserPoolId { get; set; }

    public virtual ICollection<List> InversePreviousList { get; set; } = new List<List>();

    public virtual ICollection<ListEntry> ListEntries { get; set; } = new List<ListEntry>();

    public virtual List? PreviousList { get; set; }

    public virtual UserPool UserPool { get; set; } = null!;
}
