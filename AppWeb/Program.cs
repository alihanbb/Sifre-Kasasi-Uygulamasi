using AppRepository.Context;
using AppRepository.Entities;
using AppRepository.Repositories;
using AppServices.Abstract;
using AppServices.Concrete;
using AppWeb.Authtantication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCookieAuth();
builder.Services.AddDbContext<AuthtakeDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Authtake_Connection"));
});
builder.Services.AddScoped(typeof(IMemberRepository<>), typeof(MemberRepository<>));
builder.Services.AddScoped<IRegisterService , RegisterService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IEncrypthonService, EncrypthonService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStatusCodePagesWithReExecute("/ErrorPage/Index", "?code={0}");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}")
    .WithStaticAssets();
app.Run();
