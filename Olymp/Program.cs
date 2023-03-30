using Olymp;

await Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(
        builder =>
        {
            builder.UseStartup<Startup>();
            builder.UseUrls("http://localhost:8080");
        }
        ).Build().RunAsync();