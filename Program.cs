using FoodRecipes.Interfaces;
using FoodRecipes.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpClient<IForkifyService, ForkifyService>(client =>
{
    var baseUrl = builder.Configuration["ForkifyApi:BaseUrl"] 
        ?? throw new InvalidOperationException("ForkifyApi:BaseUrl não configurada no appsettings.json");
    
    var timeoutSeconds = builder.Configuration.GetValue<int>("ForkifyApi:TimeoutSeconds", 30);
    
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
