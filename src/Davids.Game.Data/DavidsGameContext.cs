using Microsoft.EntityFrameworkCore;

namespace Davids.Game.Data;

public partial class DavidsGameContext : DbContext
{
    public DavidsGameContext(DbContextOptions<DavidsGameContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<IdentityProvider> IdentityProviders { get; set; }

    public virtual DbSet<League> Leagues { get; set; }

    public virtual DbSet<List> Lists { get; set; }

    public virtual DbSet<ListEntry> ListEntries { get; set; }

    public virtual DbSet<Pool> Pools { get; set; }

    public virtual DbSet<Statistic> Statistics { get; set; }

    public virtual DbSet<StatisticDataType> StatisticDataTypes { get; set; }

    public virtual DbSet<StatisticType> StatisticTypes { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamSeasonLeague> TeamSeasonLeagues { get; set; }

    public virtual DbSet<TeamSeasonStatistic> TeamSeasonStatistics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAttribute> UserAttributes { get; set; }

    public virtual DbSet<UserPool> UserPools { get; set; }

    public virtual DbSet<Venue> Venues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("country_pkey");

            entity.ToTable("country", "game");

            entity.HasIndex(e => e.Name, "ix_country__name");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasColumnType("character varying")
                .HasColumnName("code");
            entity.Property(e => e.FlagLink)
                .HasColumnType("character varying")
                .HasColumnName("flag_link");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<IdentityProvider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("identity_provider_pkey");

            entity.ToTable("identity_provider", "game");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<League>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("league_pkey");

            entity.ToTable("league", "game");

            entity.HasIndex(e => e.SourceId, "ix_league__source_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.LogoLink)
                .HasColumnType("character varying")
                .HasColumnName("logo_link");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.SourceId).HasColumnName("source_id");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.Country).WithMany(p => p.Leagues)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_league_country");
        });

        modelBuilder.Entity<List>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("list_pkey");

            entity.ToTable("list", "game");

            entity.HasIndex(e => e.UserPoolId, "ix_list__user_pool_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PreviousListId).HasColumnName("previous_list_id");
            entity.Property(e => e.UserPoolId).HasColumnName("user_pool_id");

            entity.HasOne(d => d.PreviousList).WithMany(p => p.InversePreviousList)
                .HasForeignKey(d => d.PreviousListId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_list_list");

            entity.HasOne(d => d.UserPool).WithMany(p => p.Lists)
                .HasForeignKey(d => d.UserPoolId)
                .HasConstraintName("fk_list_user_pool");
        });

        modelBuilder.Entity<ListEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("list_entry_pkey");

            entity.ToTable("list_entry", "game");

            entity.HasIndex(e => e.ListId, "ix_list_entry__list_id");

            entity.HasIndex(e => e.TeamId, "ix_list_entry__team_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ListId).HasColumnName("list_id");
            entity.Property(e => e.TeamId).HasColumnName("team_id");

            entity.HasOne(d => d.List).WithMany(p => p.ListEntries)
                .HasForeignKey(d => d.ListId)
                .HasConstraintName("fk_list_entry_list");

            entity.HasOne(d => d.Team).WithMany(p => p.ListEntries)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("fk_list_entry_team");

            entity.HasMany(d => d.Statistics).WithMany(p => p.ListEntries)
                .UsingEntity<Dictionary<string, object>>(
                    "ListEntryStatistic",
                    r => r.HasOne<Statistic>().WithMany()
                        .HasForeignKey("StatisticId")
                        .HasConstraintName("fk_list_entry_statistic_statistic"),
                    l => l.HasOne<ListEntry>().WithMany()
                        .HasForeignKey("ListEntryId")
                        .HasConstraintName("fk_list_entry_statistic_list_entry"),
                    j =>
                    {
                        j.HasKey("ListEntryId", "StatisticId").HasName("list_entry_statistic_pkey");
                        j.ToTable("list_entry_statistic", "game");
                        j.IndexerProperty<long>("ListEntryId").HasColumnName("list_entry_id");
                        j.IndexerProperty<long>("StatisticId").HasColumnName("statistic_id");
                    });
        });

        modelBuilder.Entity<Pool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pool_pkey");

            entity.ToTable("pool", "game");

            entity.HasIndex(e => e.DiscordServerId, "ix_pool__discord_server_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiscordServerId)
                .HasColumnType("character varying")
                .HasColumnName("discord_server_id");
            entity.Property(e => e.JoinCode)
                .HasColumnType("character varying")
                .HasColumnName("join_code");
            entity.Property(e => e.LeagueId).HasColumnName("league_id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Season)
                .HasColumnType("character varying")
                .HasColumnName("season");

            entity.HasOne(d => d.League).WithMany(p => p.Pools)
                .HasForeignKey(d => d.LeagueId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_pool_league");
        });

        modelBuilder.Entity<Statistic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("statistic_pkey");

            entity.ToTable("statistic", "game");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatisticDataTypeId).HasColumnName("statistic_data_type_id");
            entity.Property(e => e.StatisticTypeId).HasColumnName("statistic_type_id");
            entity.Property(e => e.Value)
                .HasColumnType("character varying")
                .HasColumnName("value");

            entity.HasOne(d => d.StatisticDataType).WithMany(p => p.Statistics)
                .HasForeignKey(d => d.StatisticDataTypeId)
                .HasConstraintName("fk_statistic_statistic_data_type");

            entity.HasOne(d => d.StatisticType).WithMany(p => p.Statistics)
                .HasForeignKey(d => d.StatisticTypeId)
                .HasConstraintName("fk_statistic_statistic_type");
        });

        modelBuilder.Entity<StatisticDataType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("statistic_data_type_pkey");

            entity.ToTable("statistic_data_type", "game");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<StatisticType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("statistic_type_pkey");

            entity.ToTable("statistic_type", "game");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("team_pkey");

            entity.ToTable("team", "game");

            entity.HasIndex(e => e.SourceId, "ix_team__source_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasColumnType("character varying")
                .HasColumnName("code");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.LogoLink)
                .HasColumnType("character varying")
                .HasColumnName("logo_link");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.SourceId).HasColumnName("source_id");

            entity.HasOne(d => d.Country).WithMany(p => p.Teams)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_team_country");

            entity.HasMany(d => d.Venues).WithMany(p => p.Teams)
                .UsingEntity<Dictionary<string, object>>(
                    "TeamVenue",
                    r => r.HasOne<Venue>().WithMany()
                        .HasForeignKey("VenueId")
                        .HasConstraintName("fk_team_venue_venue"),
                    l => l.HasOne<Team>().WithMany()
                        .HasForeignKey("TeamId")
                        .HasConstraintName("fk_team_venue_team"),
                    j =>
                    {
                        j.HasKey("TeamId", "VenueId").HasName("team_venue_pkey");
                        j.ToTable("team_venue", "game");
                        j.IndexerProperty<long>("TeamId").HasColumnName("team_id");
                        j.IndexerProperty<long>("VenueId").HasColumnName("venue_id");
                    });
        });

        modelBuilder.Entity<TeamSeasonLeague>(entity =>
        {
            entity.HasKey(e => new { e.LeagueId, e.TeamId, e.Season }).HasName("team_season_league_pkey");

            entity.ToTable("team_season_league", "game");

            entity.Property(e => e.LeagueId).HasColumnName("league_id");
            entity.Property(e => e.TeamId).HasColumnName("team_id");
            entity.Property(e => e.Season)
                .HasColumnType("character varying")
                .HasColumnName("season");

            entity.HasOne(d => d.League).WithMany(p => p.TeamSeasonLeagues)
                .HasForeignKey(d => d.LeagueId)
                .HasConstraintName("fk_team_season_league_league");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamSeasonLeagues)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("fk_team_season_league_team");
        });

        modelBuilder.Entity<TeamSeasonStatistic>(entity =>
        {
            entity.HasKey(e => new { e.TeamId, e.Season, e.StatisticId }).HasName("team_season_statistic_pkey");

            entity.ToTable("team_season_statistic", "game");

            entity.Property(e => e.TeamId).HasColumnName("team_id");
            entity.Property(e => e.Season)
                .HasColumnType("character varying")
                .HasColumnName("season");
            entity.Property(e => e.StatisticId).HasColumnName("statistic_id");

            entity.HasOne(d => d.Statistic).WithMany(p => p.TeamSeasonStatistics)
                .HasForeignKey(d => d.StatisticId)
                .HasConstraintName("fk_team_season_statistic_statistic");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamSeasonStatistics)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("fk_team_season_statistic_team");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.ToTable("user", "game");

            entity.HasIndex(e => new { e.IdentityProviderId, e.ExternalId }, "ix_user__identity_provider_id__external_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExternalId)
                .HasColumnType("character varying")
                .HasColumnName("external_id");
            entity.Property(e => e.IdentityProviderId).HasColumnName("identity_provider_id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.HasOne(d => d.IdentityProvider).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdentityProviderId)
                .HasConstraintName("fk_user_identity_provider");
        });

        modelBuilder.Entity<UserAttribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_attributes_pkey");

            entity.ToTable("user_attributes", "game");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<UserPool>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pool_pkey");

            entity.ToTable("user_pool", "game");

            entity.HasIndex(e => new { e.PoolId, e.UserId }, "ix_user_pool__pool_id__user_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attributes).HasColumnName("attributes");
            entity.Property(e => e.PoolId).HasColumnName("pool_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Pool).WithMany(p => p.UserPools)
                .HasForeignKey(d => d.PoolId)
                .HasConstraintName("fk_user_pool_pool");

            entity.HasOne(d => d.User).WithMany(p => p.UserPools)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_pool_user");
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("venue_pkey");

            entity.ToTable("venue", "game");

            entity.HasIndex(e => e.SourceId, "ix_venue__source_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.ImageLink)
                .HasColumnType("character varying")
                .HasColumnName("image_link");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.SourceId).HasColumnName("source_id");
            entity.Property(e => e.Surface).HasColumnName("surface");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
