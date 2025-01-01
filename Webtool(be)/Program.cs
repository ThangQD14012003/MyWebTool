using WebTool.Services;
using System.Text;
using WebTool.ProgressHub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//---------CODETHEM
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
builder.Services.AddCors(option =>
{
    option.AddPolicy("MyCors", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
    });
});
builder.Services.AddSignalR();
builder.Services.AddScoped<ExcelServices>();
builder.Services.AddScoped<TokenServices>();
builder.Services.AddScoped<QrCodeServices>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
// code them
app.UseCors("MyCors");
app.MapHub<HubSignalR>("/Hub-SignalR"); // dùng cái trên này cũng đc

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//    endpoints.MapHub<HubSignalR>("/Hub-SignalR");
//    // Các endpoints khác
//});
app.Run();
