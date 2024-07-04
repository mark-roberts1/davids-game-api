using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Davids.Game.Models.Leagues;
public class LeagueSearchRequest : PageRequest
{
    public string? Name { get; set; }
    public LeagueType? LeagueType { get; set; }
    public short? CountryId { get; set; }
    public long? SourceId { get; set; }
}
