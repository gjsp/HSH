using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HSH.Member.Startup))]
namespace HSH.Member
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
