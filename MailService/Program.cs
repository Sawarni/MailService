using MailService;

var builder = WebApplication.CreateBuilder(args);

var app = builder.ConfigureServices().Build().SetupMiddleware();
app.Run();
