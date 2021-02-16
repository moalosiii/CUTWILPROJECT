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
            //Backendless.InitApp("1611A55F-8DD5-1D10-FFC8-74FC92FBF500", "CA514EC3-053A-42CC-A022-0835EB07FE94");
            Backendless.InitApp("9C1610AF-36D4-9259-FFE4-C159BA4F3F00", "BBBE9986-0BC1-4BB5-8F86-DFDFE3B6B9FB");
            ConfigureAuth(app);
        }
    }
}
