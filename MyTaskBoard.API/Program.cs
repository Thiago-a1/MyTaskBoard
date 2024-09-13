using MyTaskBoard.API.Routes;
using MyTaskBoard.API.Services;
using MyTaskBoard.TokenService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .WithHeaders("content-type")
                   .AllowAnyOrigin()
                   .AllowAnyMethod();
        });
});

//swagger
builder.Services.AddSwaggerService();

//Configura JWT
builder.Services.AddServiceToken(builder.Configuration);

//Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.RoutePrefix = "swagger";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseCors("");

//app.UseHttpsRedirection();

UserRoutes.MapUserRoutes(app);
AuthRoutes.MapAuthRoutes(app);
BoardRoutes.MapBoardRoutes(app);
TaskRoutes.MapTaskRoutes(app);

if (app.Environment.IsDevelopment())
{
    app.Run();
} else if (app.Environment.IsProduction())
{
    app.Run($"http://localhost:5100");
}