using AliLotfiStrategy;
using ChatChannel.Domain.Model.Contracts;
using ChatChannel.Domain.Model.Enums;
using ChatChannel.Infraustructure.EfRepository;
using ChatChannel.Infraustructure.MongoRepository;
using ChatChannel.Infraustructure.Substructure.Utils;
using ChatChannel.Infraustructure.UnitOfWork;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatChannel.Infraustructure.DI
{
    public static class InfraustructureRegistration
    {
        public static IServiceCollection InfrauRegister(this IServiceCollection services, IConfiguration _config)
        {
            var connectionStringsConfiguration = _config.GetSection(nameof(ConnectionStrings));
            services.Configure<ConnectionStrings>(connectionStringsConfiguration);
            var connectionStrings = connectionStringsConfiguration.Get<ConnectionStrings>();

            services.Configure<MongoSetting>(_config.GetSection("MongoDbSettings"));
            services.Configure<DbSettings>(_config.GetSection("IsSqlOrNoSqlSetting"));



            services.AddDbContext<ChatDbContext>(options =>
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder()
                {
                    DataSource = connectionStrings!.Server,
                    InitialCatalog = connectionStrings!.Database,
                    TrustServerCertificate = true,
                    IntegratedSecurity = true,
                };

                options.UseSqlServer(connectionStringBuilder.ConnectionString);
            });
            services.AddScopedLazy<IUserRepository, MongoUserRepository, DatabaseTypes>(DatabaseTypes.MongoDb);
            services.AddScopedLazy<IUserRepository, UserRepository, DatabaseTypes>(DatabaseTypes.SqlServer);
            //services.AddScopedLazy<IUserRepository, MongoUserRepository, DatabaseTypes>(DatabaseTypes.MongoDb);
            services.AddScopedLazy<IUnitOfWork, UnitOfwork.UnitOfWork, DatabaseTypes>(DatabaseTypes.SqlServer);
            services.AddScopedLazy<IUnitOfWork, UnitOfwork.MongoUnitOfWork, DatabaseTypes>(DatabaseTypes.MongoDb);
            services.AddScoped<IStrategy<IUnitOfWork, DatabaseTypes>, StrategyRepository<IUnitOfWork, DatabaseTypes>>();
            services.AddScoped<IStrategy<IUserRepository, DatabaseTypes>, StrategyRepository<IUserRepository, DatabaseTypes>>();

            services.AddScoped<IUnitOfWork, UnitOfwork.UnitOfWork>();
            //services.AddScoped<IUserRepositoryStrategy, UserRepositoryStrategy>();
            return services;
        }
    }
}
