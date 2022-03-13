using AutoMapper;
using MetricsAgent.DAL;
using FluentMigrator.Runner;

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

        }
    }
}