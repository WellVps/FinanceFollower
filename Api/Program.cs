using BaseApi.Extensions;
using Api.Extensions;
using Api.QueueConsumers;
using Api.Utils;

Type[] queueConsumers = [
    typeof(UpdateLastPriceConsumer)
];

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddValueObjectTypes();
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerAuthentication();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddJsonConfigs();
builder.Services.AddRabbitMQServices(builder.Configuration, queueConsumers);
builder.Services.AddMongoContext(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

MessageBrokerConfig.SubscribeConsumers(app, builder.Configuration);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Main API V1");
});
app.UseCors();

app.UseAuthorization();

app.MapControllers();
await app.RunAsync();