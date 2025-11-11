using ChatChannel.Domain.Model.Contracts;
using ChatChannel.Infraustructure.Repository;
using ChatChannel.Infraustructure.Substructure.Utils;
using ChatChannel.Infraustructure.UnitOfwork;
using ChatChannel.Infraustructure.UnitOfWork;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Infraustructure.DI
{
    public static class InfraustructureRegistration
    {
        public static IServiceCollection InfrauRegister(this IServiceCollection services , IConfiguration _config)
        {
            var connectionStringsConfiguration = _config.GetSection(nameof(ConnectionStrings));
            services.Configure<ConnectionStrings>(connectionStringsConfiguration);
            var connectionStrings = connectionStringsConfiguration.Get<ConnectionStrings>();


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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork,UnitOfwork.UnitOfWork>();
            return services;
        }
    }
}
