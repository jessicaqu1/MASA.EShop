var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Settings>(builder.Configuration);
builder.Services.AddCallerService();

//var identityUrl = app.Configuration.GetValue<string>("urls:identity");
var identityUrl = "";
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(options =>
{
    options.Authority = identityUrl;
    options.RequireHttpsMetadata = false;
    options.Audience = "masa_eshop_open";
});

var app = builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "MASA EShop - HTTP API",
            Version = "v1",
            Description = "The EShop Service HTTP API"
        });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the bearer scheme",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey
        });
        //options.AddSecurityRequirement(new OpenApiSecurityRequirement
        //{
        //    {
        //        new OpenApiSecurityScheme
        //        {
        //            Reference = new OpenApiReference
        //            {
        //                Id = "Bearer",
        //                Type = ReferenceType.Schema
        //            }
        //        },new List<string>()
        //    }
        //});
    })
    .AddServices(builder);

app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MASA EShop Service HTTP API v1");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/hello", [Authorize] () => "hello").WithName("WelcomeMessage").WithMetadata(new SwaggerOperationAttribute("123", "333333333"));

app.Run();
