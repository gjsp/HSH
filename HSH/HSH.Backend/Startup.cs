using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HSH.Backend.Startup))]
namespace HSH.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
