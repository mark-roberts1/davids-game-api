namespace Davids.Game.Models.Lists;
public class ListWriteRequest
{
    public long? PreviousListId { get; set; }

    public ListResponse? PreviousList { get; set; }

    public long UserPoolId { get; set; }

    public int Version
    {
        get
        {
            int self = 1;

            return self + PreviousList?.Version ?? 0;
        }
    }

    public IEnumerable<ListEntryWriteRequest> Entries { get; set; } = [];
}
