using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.General.Interfaces;
using Infrastructure.General.Implementation;

namespace EmployeeManagement3.Helper
{
    public static class ServiceExtensions
    {

        public static IServiceCollection RegisterAppServices(this IServiceCollection services, AppSettings appSettings)
        {

            services.AddScoped<IDapperContext>(x => new DapperContext(appSettings));


            services.AddScoped<IBaseRepository<Employees>, BaseRepository<Employees>>();

 
           

            return services;
        }
  
    }
}
