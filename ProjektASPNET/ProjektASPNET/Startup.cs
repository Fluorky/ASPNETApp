using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjektASPNET.Startup))]
namespace ProjektASPNET
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
