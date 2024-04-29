using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalAPIPeliculas;
using MinimalAPIPeliculas.Endpoints;
using MinimalAPIPeliculas.Entidades;
using MinimalAPIPeliculas.Repositorios;
using MinimalAPIPeliculas.Servicios;
using MinimalAPIPeliculas.Utilidades;

var builder = WebApplication.CreateBuilder(args);
var ambiente = builder.Configuration.GetValue<string>("ambiente");
var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!;
//Aquí van los servicios (antes del app builder)

builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
opciones.UseSqlServer("name=DefaultConnection"));

builder.Services.AddCors(options =>
        options.AddDefaultPolicy(configuration=>
    {
        configuration.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();  
    }));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOutputCache();
builder.Services.AddScoped<IRepositorioGeneros, RepositorioGeneros>();
builder.Services.AddScoped<IRepositorioActores, RepositorioActores>();
builder.Services.AddScoped<IRepositorioPeliculas,RepositorioPeliculas>();
builder.Services.AddScoped<IRepositorioComentarios, RepositorioComentarios>();
builder.Services.AddScoped<IRepositorioErrores, RepositorioErrores>();
builder.Services.AddScoped<IAlmacenadorArchivos,AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddProblemDetails();

//Requiere el jwt bearer
builder.Services.AddAuthentication().AddJwtBearer(opciones=>
opciones.TokenValidationParameters=new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    //Para una sóla llave
    //IssuerSigningKey = Llaves.ObtenerLlave(builder.Configuration).First(),
    //Multiples llaves
    IssuerSigningKeys = Llaves.ObtenerTodasLasLlaves(builder.Configuration),
    ClockSkew = TimeSpan.Zero
});
builder.Services.AddAuthorization();

//Configuración del identity (para el sistema de usuarios)
builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();


//*************************************************** fin del área de servicios

//Middlewares después del app builder
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//Servicios para el manejo de errores, su builder es Add.ProblemDetails()
app.UseExceptionHandler(exceptionHandlerApp => exceptionHandlerApp.Run(async context =>
{
    //Obtener los datos del error (que serán guardados)
    var exceptionHandleFeature = context.Features.Get<IExceptionHandlerFeature>();
    var excepcion = exceptionHandleFeature?.Error!;
    var error = new Error();

    error.Fecha = DateTime.UtcNow;
    error.MensajeDeError = excepcion.Message;
    error.StackTrace = excepcion.StackTrace;

    var repositorio = context.RequestServices.GetRequiredService<IRepositorioErrores>();
    await repositorio.Crear(error);


    await TypedResults.BadRequest(new 
    { 
        tipo = "error", mensaje = "Ha ocurrido un mensaje de error inesperado", estatus = 500 
    }).ExecuteAsync(context);
}));
app.UseStatusCodePages();


//Servicio Personalizado para acceso a la interfaz de repositorios (inversión de dependencias)
app.UseStaticFiles();

app.UseCors();
app.UseOutputCache();
//app.MapGet("/", () =>"En "+ ambiente+" Hello World!");
//app.MapGet("/other-page", () =>  "Hello other page!");


app.MapGet("/error", () =>
{
    throw new InvalidOperationException("error de ejemplo");
});

app.MapGroup("/generos").MapGeneros();
app.MapGroup("/actores").MapActores();
app.MapGroup("/peliculas").MapPeliculas();
app.MapGroup("/pelicula/{peliculaId:int}/comentarios").MapComentarios();

//*************************************************

app.Run();

