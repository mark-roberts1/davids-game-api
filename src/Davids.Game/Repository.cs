using Davids.Game.Countries;
using Davids.Game.DependencyInjection;
using Davids.Game.IdentityProviders;
using Davids.Game.Leagues;
using Davids.Game.Lists;
using Davids.Game.Pools;
using Davids.Game.Statistics;
using Davids.Game.Teams;
using Davids.Game.Users;
using Davids.Game.Venues;

namespace Davids.Game;

[Service<IRepository>]
internal record Repository(
    ICountriesRepository Countries,
    IIdentityProvidersRepository IdentityProviders,
    ILeaguesRepository Leagues,
    IListsRepository Lists,
    IPoolsRepository Pools,
    IStatisticsRepository Statistics,
    ITeamsRepository Teams,
    IUsersRepository Users,
    IVenuesRepository Venues) : IRepository;
