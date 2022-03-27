using FluentMigrator.Runner;
using Polly;

namespace HttpClient
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
            services.AddHttpClient();
           
                       
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