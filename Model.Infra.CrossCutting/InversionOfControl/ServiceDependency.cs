using Model.Service.Services;
using Model.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Model.Infra.CrossCutting.InversionOfControl
{
    public static class ServiceDependency
    {
        public static void AddServiceDependency(this IServiceCollection services)
        {
            services.AddScoped<IServiceUser, UserService>();
        }
    }
}
