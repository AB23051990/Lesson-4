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
        // Этот метод вызывается средой выполнения. Используем его для        добавления служб в контейнер
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
            // Добавляем поддержку SQLite
            .AddSQLite()
            // Устанавливаем строку подключения
            .WithGlobalConnectionString(ConnectionString)
            // Подсказываем, где искать классы с миграциями
            .ScanIn(typeof(Startup).Assembly).For.Migrations()
            ).AddLogging(lb => lb
            .AddFluentMigratorConsole());
        }
        // Этот метод вызывается средой выполнения. Используем его для   настройки конвейера HTTP-запросов
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
            // Запускаем миграции
            migrationRunner.MigrateUp();

        }
    }
}