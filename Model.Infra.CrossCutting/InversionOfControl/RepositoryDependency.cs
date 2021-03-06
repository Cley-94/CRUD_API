using Model.Domain.Interfaces;
using Model.Infra.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Model.Infra.CrossCutting.InversionOfControl
{
    public static class RepositoryDependency
    {
        public static void AddRepositoryDependency(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryUser, UserRepository>();
        }
    }
}
