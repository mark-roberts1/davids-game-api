using Davids.Game.Countries;
using Davids.Game.IdentityProviders;
using Davids.Game.Leagues;
using Davids.Game.Lists;
using Davids.Game.Pools;
using Davids.Game.Statistics;
using Davids.Game.Teams;
using Davids.Game.Users;
using Davids.Game.Venues;

namespace Davids.Game;

public interface IRepository
{
    public ICountriesRepository Countries { get; }
    public IIdentityProvidersRepository IdentityProviders { get; }
    public ILeaguesRepository Leagues { get; }
    public IListsRepository Lists { get; }
    public IPoolsRepository Pools { get; }
    public IStatisticsRepository Statistics { get; }
    public ITeamsRepository Teams { get; }
    public IUsersRepository Users { get; }
    public IVenuesRepository Venues { get; }
}
