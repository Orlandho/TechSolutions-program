using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechSolutions_program.Data;
using TechSolutions_program.Data.Repositories;
using TechSolutions_program.Models;
using TechSolutions_program.Services.Implementations;
using TechSolutions_program.Services.Interfaces;
using TechSolutions_program.Services.Strategies;

namespace TechSolutions_program
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // CAMBIADO: RequireConfirmedAccount = false para permitir login inmediato
            builder.Services.AddDefaultIdentity<Usuario>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddScoped<IProyectoService, ProyectoService>();
            builder.Services.AddScoped<ITareaService, TareaService>();
            builder.Services.AddScoped<IReporteService, ReporteService>();
            builder.Services.AddScoped<IReporteStrategy, PdfReporteStrategy>();
            builder.Services.AddScoped<IReporteStrategy, ExcelReporteStrategy>();
            builder.Services.AddScoped<IdentityService>();
            builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // ====================================================================================
            // INICIALIZACIÓN DE DATOS: ROLES Y USUARIOS DE PRUEBA
            // ====================================================================================
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                // 1. Crear roles en la base de datos
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Lider", "Desarrollador", "Administrador" };

                foreach (var role in roles)
                {
                    if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                    {
                        roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                    }
                }

                // 2. Crear usuarios de prueba en la base de datos
                var userManager = services.GetRequiredService<UserManager<Usuario>>();
                
                // Usuario Administrador
                string adminEmail = "admin@techsolutions.com";
                string adminPassword = "Admin123!";
                
                if (userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult() == null)
                {
                    var adminUser = new Usuario
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        NombreCompleto = "Administrador del Sistema",
                        CodigoEmpleado = "ADM001",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(adminUser, adminPassword).GetAwaiter().GetResult();
                    
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(adminUser, "Administrador").GetAwaiter().GetResult();
                    }
                }
                
                // Usuario Líder
                string liderEmail = "lider@techsolutions.com";
                string liderPassword = "Lider123!";
                
                if (userManager.FindByEmailAsync(liderEmail).GetAwaiter().GetResult() == null)
                {
                    var liderUser = new Usuario
                    {
                        UserName = liderEmail,
                        Email = liderEmail,
                        NombreCompleto = "Juan Pérez - Líder de Proyecto",
                        CodigoEmpleado = "LID001",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(liderUser, liderPassword).GetAwaiter().GetResult();
                    
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(liderUser, "Lider").GetAwaiter().GetResult();
                    }
                }
                
                // Usuario Desarrollador
                string devEmail = "desarrollador@techsolutions.com";
                string devPassword = "Dev123!";
                
                if (userManager.FindByEmailAsync(devEmail).GetAwaiter().GetResult() == null)
                {
                    var devUser = new Usuario
                    {
                        UserName = devEmail,
                        Email = devEmail,
                        NombreCompleto = "María García - Desarrolladora",
                        CodigoEmpleado = "DEV001",
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(devUser, devPassword).GetAwaiter().GetResult();
                    
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(devUser, "Desarrollador").GetAwaiter().GetResult();
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Autenticacion/Login");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            
            // CAMBIADO: Ruta por defecto ahora es Login, no Home/Index
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Autenticacion}/{action=Login}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
