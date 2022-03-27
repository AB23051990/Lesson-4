using AutoMapper;
using MetricsAgent.DAL;
using FluentMigrator.Runner;
using MetricsAgent.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Polly;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace MetricsAgent
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        private const string ConnectionString = @"Data Source=metrics.db;Version=3;";
        // ���� ����� ���������� ������ ����������. ���������� ��� ���        ���������� ����� � ���������
        public void ConfigureServices(IServiceCollection services)
        {
            // ��������� �������
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            // ��������� ���� ������
            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton(new JobSchedule(
            jobType: typeof(CpuMetricJob),
            cronExpression: "0/5 * * * * ?")); // ��������� ������ 5 ������
            services.AddSingleton(new JobSchedule(
            jobType: typeof(RamMetricJob),
            cronExpression: "0/5 * * * * ?"));
            services.AddHostedService<QuartzHostedService>();

            services.AddHttpClient();

            services.AddSwaggerGen();

            services.AddControllers();
            services.AddSingleton<ICpuMetricsRepository,
            CpuMetricsRepository>();
            var mapperConfiguration = new MapperConfiguration(mp =>
            mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
            services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
            // ��������� ��������� SQLite
            .AddSQLite()
            // ������������� ������ �����������
            .WithGlobalConnectionString(ConnectionString)
            // ������������, ��� ������ ������ � ����������
            .ScanIn(typeof(Startup).Assembly).For.Migrations()
            ).AddLogging(lb => lb
            .AddFluentMigratorConsole());



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API ������� ������ ����� ������",
                    Description = "����� ����� �������� � api ������ �������",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Kadyrov",
                        Email = string.Empty,
                        Url = new Uri("https://kremlin.ru"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "����� �������, ��� ����� ��������� �� ������������",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                // ��������� ����, �� �������� ����� ����� ����������� ���  Swagger UI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
        // ���� ����� ���������� ������ ����������. ���������� ��� ���   ��������� ��������� HTTP-��������
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
        IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // ��������� ��������
            migrationRunner.MigrateUp();
            // ��������� middleware � �������� ��� ��������� Swagger-��������.
            app.UseSwagger();
            // ��������� middleware ��� ��������� swagger-ui
            // ��������� �������� Swagger JSON (���� ���������� �� ��������������� �������������,�� ������� ����� �������� UI).
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API ������� ������ �����������");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}