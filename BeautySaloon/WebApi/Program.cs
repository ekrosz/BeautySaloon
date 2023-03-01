using BeautySaloon.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}

// TODO:
// 1. ћетод дл€ получени€ доступных абонементов/услуг по клиенту дл€ создани€ записи
// 2. Ћокализованные исключени€
// 3. ƒо конца протестировать и отладить все методы