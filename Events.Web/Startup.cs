using Microsoft.Owin;
using Owin;
using BackendlessAPI;

[assembly: OwinStartupAttribute(typeof(Events.Web.Startup))]
namespace Events.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Backendless.InitApp("472D764C-B369-5FCF-FF95-397E44002F00", "99BE55B9-7B84-40CF-B6F3-1C3D9D0411CE");
            ConfigureAuth(app);
        }
    }
}
