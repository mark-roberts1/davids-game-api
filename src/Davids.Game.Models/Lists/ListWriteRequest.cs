namespace Davids.Game.Models.Lists;
public class ListWriteRequest
{
    public long? PreviousListId { get; set; }

    public long PoolId { get; set; }

    public IEnumerable<ListEntryWriteRequest> Entries { get; set; } = [];
}
