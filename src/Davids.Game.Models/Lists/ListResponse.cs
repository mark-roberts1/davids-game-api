namespace Davids.Game.Models.Lists;

public class ListResponse
{
    public long Id { get; set; }

    public long? PreviousListId { get; set; }

    public ListResponse? PreviousList { get; set; }

    public long UserPoolId { get; set; }

    public IEnumerable<ListEntryResponse> Entries { get; set; } = [];
}
