using BaseApi.Extensions;
using Api.Extensions;

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
builder.Services.AddMongoContext(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Main API V1");
});

app.UseAuthorization();

app.MapControllers();
await app.RunAsync();