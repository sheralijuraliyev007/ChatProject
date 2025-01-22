using Chat.Api.Context;
using Chat.Api.Helpers;
using Chat.Api.Managers;
using Chat.Api.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Bearer. : \"Authorization: Bearer { token } \"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var jwtParam = builder.Configuration.GetSection("JwtParameters").Get<JwtParameters>();


    var key = System.Text.Encoding.UTF32.GetBytes(jwtParam.Key);
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = jwtParam.Issuer,
        ValidateIssuer = true,
        ValidAudience = jwtParam.Audience,
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true
    };
});


builder.Services.AddMemoryCache();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IUserChatRepository, UserChatRepository>();
builder.Services.AddScoped<IMessageRepository,MessageRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<UserManager>();
builder.Services.AddScoped<ChatManager>();
builder.Services.AddScoped<JWTManager>();
builder.Services.AddScoped<MessageManager>();
builder.Services.AddScoped<UserHelper>();
builder.Services.AddScoped<MemoryCacheManager>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ChatDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ChatDb"));
});





//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policy =>
//    {
//        policy.AllowAnyMethod();
//        policy.AllowAnyOrigin();
//        policy.AllowAnyHeader();
//    });
//});

var app = builder.Build();


app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

//app.UseCors();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();



app.UseAuthorization();

app.MapControllers();

app.Run();
