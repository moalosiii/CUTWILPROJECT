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
            Backendless.InitApp("6EE0A024-A6AD-5429-FFCD-BAE35223BD00", "C4E3C54F-3CAD-4F38-910D-D8AAB7FD6420");
            ConfigureAuth(app);
        }
    }
}
