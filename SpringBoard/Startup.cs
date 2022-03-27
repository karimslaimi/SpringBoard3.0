using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SpringBoard.Data;
using SpringBoard.Domaine;
using SpringBoard.Service;
using Swashbuckle.Swagger;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SpringBoard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabContext>();
            services.AddControllers();


            services.AddSwaggerGen(setup =>
            {
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

            });

            services.AddIdentity<Utilisateur, IdentityRole>()
                    .AddEntityFrameworkStores<DatabContext>()
                    .AddDefaultTokenProviders();



                services.Configure<IdentityOptions>(options =>
                {
                // Default Password settings.
                options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                });
                // Adding Authentication  


                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                // Adding Jwt Bearer  
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["JWT:ValidAudience"],
                        ValidIssuer = Configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                    };
                });



                services.AddScoped<IServiceUser, ServiceUser>();

            }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {


                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();

                    app.UseSwagger();

                    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseDeveloperExceptionPage();



                app.UseRouting();

                app.UseAuthentication();

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

                await CreateRoles(app.ApplicationServices);

            }


            async Task CreateRoles(IServiceProvider services)
            {
                using (var scope = services.CreateScope())
                {



                    var RoleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));
                    var userManager = (UserManager<Utilisateur>)scope.ServiceProvider.GetService(typeof(UserManager<Utilisateur>));


                    IdentityResult roleResult;


                    var roleCheck = await RoleManager.RoleExistsAsync("RH");
                    var roleCheck1 = await RoleManager.RoleExistsAsync("Commercial");
                    var roleCheck2 = await RoleManager.RoleExistsAsync("Consultant");
                    var roleCheck3 = await RoleManager.RoleExistsAsync("Administrateur");


                    if (!roleCheck)
                    {
                        roleResult = await RoleManager.CreateAsync(new IdentityRole("RH"));

                    }
                    if (!roleCheck1)
                    {
                        roleResult = await RoleManager.CreateAsync(new IdentityRole("Commercial"));

                    }
                    if (!roleCheck2)
                    {

                        roleResult = await RoleManager.CreateAsync(new IdentityRole("Consultant"));
                    }
                    if (!roleCheck3)
                    {
                        roleResult = await RoleManager.CreateAsync(new IdentityRole("Administrateur"));

                    }

                    var checkuser = await userManager.FindByEmailAsync("admin@admin.com");

                    if (checkuser == null)
                    {
                        var usr = new Administrateur
                        {
                            LastName = "admin",
                            Firstname = "admin",
                            UserName = "admin@admin.com",
                            Email = "admin@admin.com"
                        };
                        var chkUser = await userManager.CreateAsync(usr, "admin123");
                        if (chkUser.Succeeded)
                        {
                            var result1 = await userManager.AddToRoleAsync(usr, "Administrateur");
                        }
                    }


                }

            }
        }
    }
