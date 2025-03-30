using BaseApi.Extensions;
using JobsAPI.Extensions;

Type[] queueConsumers = [];

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddValueObjectTypes();
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddHangFireConfigs(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerAuthentication();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddJsonConfigs();
builder.Services.AddRabbitMQServices(builder.Configuration, queueConsumers);
builder.Services.AddMongoContexts(builder.Configuration);
builder.Services.AddServices();

var app = builder.Build();

var environment = app.Configuration["Environment"] ?? "Development";
if (environment == "Development")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors( x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseHttpsRedirection();
app.ScheduleBackgroundJobs();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();