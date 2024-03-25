﻿using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Persistence
{
    public static class DbContextDI
    {
        public static IServiceCollection AddTodoDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContext, ApplicationDbContext>(
                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                }
            );

            services.AddTransient<IContextTransactionCreator, ContextTransactionCreator>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();

            services.AddTransient<IRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();
            services.AddTransient<IRepository<ApplicationUserRole>, BaseRepository<ApplicationUserRole>>();
            services.AddTransient<IRepository<RefreshToken>, BaseRepository<RefreshToken>>();
            return services;
        }
    }
}
