using BackgroundServiceSampleWeb.BackgroundServices;
using BackgroundServiceSampleWeb.Queue;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddHostedService<ClassSampleIHostedService>();
//builder.Services.AddHostedService<ClassSampleBackgroundService>();

builder.Services.AddHostedService<QueueHostedService>();
builder.Services.AddSingleton<IBackgroundTaskQueue>(p=>
{
    return new BackgroundTaskQueue(5);
});
builder.Services.AddSingleton<SamplePruducer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
