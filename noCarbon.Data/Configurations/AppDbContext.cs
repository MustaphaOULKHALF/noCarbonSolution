using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using noCarbon.Core;
using noCarbon.Core.Domain.Functions;
using noCarbon.Data.Mapping;
using System.Data;
using System.Data.Common;

namespace noCarbon.Data.Configurations;

public class AppDbContext : DbContext
{

    public AppDbContext() : base() { }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 30));
            optionsBuilder.UseMySql("Server=localhost;User Id=appuser;Password=p@$$w0rd;Database=noCarbon", serverVersion);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerBuilder());
        modelBuilder.ApplyConfiguration(new CustomerRefreshTokenBuilder());
        modelBuilder.ApplyConfiguration(new CategoryBuilder());
        modelBuilder.ApplyConfiguration(new EventBuilder());
        modelBuilder.ApplyConfiguration(new SponsorshipBuilder());
        modelBuilder.ApplyConfiguration(new ActionsBuilder());
        modelBuilder.ApplyConfiguration(new HistoricBuilder());
        modelBuilder.Entity<Historic_Result>().HasNoKey();
        modelBuilder.Entity<GetBalance_Result>().HasNoKey();
        modelBuilder.Entity<GetLeaderboard_Result>().HasNoKey();
        modelBuilder.Entity<GetWeeklyTrend_Result>().HasNoKey();
        modelBuilder.Entity<GetYearlyTrend_Result>().HasNoKey();
    }
    #region Utilities
    /// <summary>
    /// Modify the input SQL query by adding passed parameters
    /// </summary>
    /// <param name="sql">The raw SQL query</param>
    /// <param name="parameters">The values to be assigned to parameters</param>
    /// <returns>Modified raw SQL query</returns>
    protected virtual string CreateSqlWithParameters(string sql, params object[] parameters)
    {
        //add parameters to sql
        for (var i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
        {
            if (parameters[i] is not DbParameter parameter)
                continue;

            sql = $"{sql}{(i > 0 ? "," : string.Empty)} @{parameter.ParameterName}";

            //whether parameter is output
            if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output)
                sql = $"{sql} output";
        }

        return sql;
    }
    /// <summary>
    /// Modify the input SQL query by adding passed parameters
    /// </summary>
    /// <param name="sql">The raw SQL query</param>
    /// <param name="parameters">The values to be assigned to parameters</param>
    /// <returns>Modified raw SQL query</returns>
    protected virtual string GetExecutedSqlQuery(string sql, params object[] parameters)
    {
        //add parameters to sql
        for (var i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
        {
            if (parameters[i] is not DbParameter parameter)
                continue;
            if (parameter.IsNullable && parameter.Value == null)
                sql = sql.Replace(parameter.ParameterName, "NULL");
            else
            {
                if (parameter.DbType == DbType.Guid)
                    sql = sql.Replace(parameter.ParameterName, string.Format("'{0}'", parameter.Value.ToString()));
                if (parameter.DbType == DbType.Int32)
                    sql = sql.Replace(parameter.ParameterName, parameter.Value.ToString());
            }
        }

        return sql;
    }
    #endregion

    #region Methods

    /// <summary>
    /// Creates a DbSet that can be used to query and save instances of entity
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <returns>A set for the given entity type</returns>
    public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
    {
        return base.Set<TEntity>();
    }
    /// <summary>
    /// Generate a script to create all tables for the current model
    /// </summary>
    /// <returns>A SQL script</returns>
    public virtual string GenerateCreateScript()
    {
        return Database.GenerateCreateScript();
    }

    /// <summary>
    /// Creates a LINQ query for the entity based on a raw SQL query
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="sql">The raw SQL query</param>
    /// <param name="parameters">The values to be assigned to parameters</param>
    /// <returns>An IQueryable representing the raw SQL query</returns>
    public virtual IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseEntity
    {
        return Set<TEntity>().FromSqlRaw(CreateSqlWithParameters(sql, parameters), parameters);
    }

    /// <summary>
    /// Executes the given SQL against the database
    /// </summary>
    /// <param name="sql">The SQL to execute</param>
    /// <param name="doNotEnsureTransaction">true - the transaction creation is not ensured; false - the transaction creation is ensured.</param>
    /// <param name="timeout">The timeout to use for command. Note that the command timeout is distinct from the connection timeout, which is commonly set on the database connection string</param>
    /// <param name="parameters">Parameters to use with the SQL</param>
    /// <returns>The number of rows affected</returns>
    public virtual int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
    {
        //set specific command timeout
        var previousTimeout = Database.GetCommandTimeout();
        Database.SetCommandTimeout(timeout);

        var result = 0;
        if (!doNotEnsureTransaction)
        {
            //use with transaction
            using var transaction = Database.BeginTransaction();
            result = Database.ExecuteSqlRaw(sql, parameters);
            transaction.Commit();
        }
        else
            result = Database.ExecuteSqlRaw(sql, parameters);

        //return previous timeout back
        Database.SetCommandTimeout(previousTimeout);

        return result;
    }

    /// <summary>
    /// Detach an entity from the context
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="entity">Entity</param>
    public virtual void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var entityEntry = Entry(entity);
        if (entityEntry == null)
            return;

        //set the entity is not being tracked by the context
        entityEntry.State = EntityState.Detached;
    }

    /// <summary>
    /// Saves all changes made in this context to the database
    /// </summary>
    /// <returns>The number of state entries written to the database</returns>
    public virtual async Task SaveChangesAsync() => await this.SaveChangesAsync();
    public IQueryable<Historic_Result> GetHistoric(Guid? CustomerId, int? CategoryId = null, int? ActionId = null)
    {
        var ParametreCustomerId = new SqlParameter
        {
            IsNullable = true,
            DbType = DbType.Guid,
            Value = CustomerId ?? null,
            SourceColumnNullMapping = true,
            ParameterName = "@CustomerId"
        };
        var ParametreCategoryId = new SqlParameter
        {
            IsNullable = true,
            DbType = DbType.Int32,
            Value = CategoryId ?? null,
            SourceColumnNullMapping = true,
            ParameterName = "@CategoryId"
        };
        var ParametreActionId = new SqlParameter
        {
            IsNullable = true,
            DbType = DbType.Int32,
            Value = ActionId ?? null,
            SourceColumnNullMapping = true,
            ParameterName = "@ActionId"
        };
        var sql = GetExecutedSqlQuery("SELECT * FROM [dbo].[GetHistoric](@CustomerId,@CategoryId,@ActionId)", ParametreCustomerId, ParametreCategoryId, ParametreActionId);
        var result = base.Set<Historic_Result>().FromSqlRaw(sql).ToList();
        return result.AsQueryable();
    }
    public async Task<GetBalance_Result> GetBalance(Guid CustomerId)
    {
        var ParametreCustomerId = new SqlParameter
        {
            IsNullable = false,
            DbType = DbType.Guid,
            Value = CustomerId,
            SourceColumnNullMapping = true,
            ParameterName = "@CustomerId"
        };

        var sql = GetExecutedSqlQuery("CALL GetBalance(@CustomerId)", ParametreCustomerId);
        var result = await base.Set<GetBalance_Result>().FromSqlRaw(sql).ToListAsync();
        return result.FirstOrDefault();
    }
    public async Task<IQueryable<GetLeaderboard_Result>> GetLeaderboard()
    {
        var result = await base.Set<GetLeaderboard_Result>().FromSqlRaw("SELECT * FROM [dbo].[GetLeaderboard] ()").ToListAsync();
        return result.AsQueryable();
    }
    public async Task<IQueryable<GetWeeklyTrend_Result>> GetMyWeeklyTrend(Guid CustomerId)
    {
        var ParametreCustomerId = new SqlParameter
        {
            IsNullable = false,
            DbType = DbType.Guid,
            Value = CustomerId,
            SourceColumnNullMapping = true,
            ParameterName = "@CustomerId"
        };

        var sql = GetExecutedSqlQuery("SELECT * FROM [dbo].[GetMyWeeklyTrend](@CustomerId)", ParametreCustomerId);
        return (await base.Set<GetWeeklyTrend_Result>().FromSqlRaw(sql).ToListAsync()).AsQueryable();
    }
    public async Task<IQueryable<GetYearlyTrend_Result>> GetYearlyTrend(Guid CustomerId)
    {
        var ParametreCustomerId = new SqlParameter
        {
            IsNullable = false,
            DbType = DbType.Guid,
            Value = CustomerId,
            SourceColumnNullMapping = true,
            ParameterName = "@CustomerId"
        };

        var sql = GetExecutedSqlQuery("SELECT * FROM [dbo].[GetYearlyTrend](@CustomerId)", ParametreCustomerId);
        return (await base.Set<GetYearlyTrend_Result>().FromSqlRaw(sql).ToListAsync()).AsQueryable();
    }
    #endregion
}