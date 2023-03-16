using BeautySaloon.Api.Services;
using Radzen;
using Refit;
using WebApp.Handlers;
using WebApp.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddTransient<CustomRefitErrorHandler>();
builder.Services.AddTransient<TokenStorageHandler>();
builder.Services.AddTransient<HeaderPropagationHandler>();

builder.Services.AddSingleton<ITokenStorage, TokenStorage>();

builder.Services.AddRefitClient<IUserHttpClient>()
    .ConfigureHttpClient(_ => _.BaseAddress = new Uri("http://localhost:40001"))
    .AddHttpMessageHandler<CustomRefitErrorHandler>()
    .AddHttpMessageHandler<HeaderPropagationHandler>();

builder.Services.AddRefitClient<IPersonHttpClient>()
    .ConfigureHttpClient(_ => _.BaseAddress = new Uri("http://localhost:40001"))
    .AddHttpMessageHandler<CustomRefitErrorHandler>()
    .AddHttpMessageHandler<HeaderPropagationHandler>();

builder.Services.AddRefitClient<IAuthHttpClient>()
    .ConfigureHttpClient(_ => _.BaseAddress = new Uri("http://localhost:40001"))
    .AddHttpMessageHandler<CustomRefitErrorHandler>()
    .AddHttpMessageHandler<TokenStorageHandler>();

builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<TooltipService>();

builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
