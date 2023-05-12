using PloomesBackend.Config.Mappings;
using PloomesBackend.Data.Queries;
using PloomesBackend.Data.Repository;
using PloomesBackend.Data.Util;
using PloomesBackend.Security.Authentication;
using PloomesBackend.Security.Extensions;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(BasicAuthenticationDefaults.SchemaName)
    .AddBasicAuthentication();

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("EmailSenha", authBuilder =>
    {
        authBuilder.RequireClaim(ClaimTypes.Email);
    });
});

builder.Services.AddAutoMapper(typeof(DataToViewModelMappingProfile));
builder.Services.AddAutoMapper(typeof(ViewModelToDataMappingProfile));

builder.Services.AddTransient<IConnectionStringGetter, ConnectionStringGetter>(
    c => new ConnectionStringGetter(builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IUsuarioQueryBuilder, UsuarioQueryBuilder>();
builder.Services.AddTransient<IClienteQueryBuilder, ClienteQueryBuilder>();
builder.Services.AddTransient<IClienteRepository, ClienteRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddBasicAuthenticationDocumentation();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
