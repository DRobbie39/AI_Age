namespace AI_Age_FrontEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl");

            // Đăng ký HttpClient với cấu hình BaseAddress
            builder.Services.AddHttpClient("ApiClient", client =>
            {
                if (string.IsNullOrEmpty(apiBaseUrl))
                {
                    throw new InvalidOperationException("ApiSettings:BaseUrl is not configured in appsettings.json");
                }
                client.BaseAddress = new Uri(apiBaseUrl);
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();

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
                name: "Admin",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
