using ChatApp.BusinessLogicLayer.Abstract;
using ChatApp.BusinessLogicLayer.Concrete;
using ChatApp.BusinessLogicLayer.DTOs;
using ChatApp.BusinessLogicLayer.Validators;
using ChatApp.DataAccesLayer.Abstract;
using ChatApp.DataAccesLayer.Concrete;
using ChatApp.DataAccesLayer.Data;
using ChatApp.EntitiesLayer.Model;
using ChatApp.PresentationLayer.Hubs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddIdentity<AppUser, AppUserRole>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IValidator<UserInformationDTO>, UserInformationValidator>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IMessageService,MessageService>();
builder.Services.AddScoped<IOnlineUsersService,OnlineUsersService>();
builder.Services.AddScoped<IUserGroupService,UserGroupService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/Index";


});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/chatHub");
app.Run();
