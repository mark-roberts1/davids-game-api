using Davids.Game.Models.UserPools;

namespace Davids.Game.Models.Lists;

public class ListResponse
{
    public long Id { get; set; }

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

    public IEnumerable<ListEntryResponse> Entries { get; set; } = [];

    public UserPoolResponse UserPool { get; set; } = null!;
}
