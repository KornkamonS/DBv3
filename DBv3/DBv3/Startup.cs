using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DBv3.Startup))]
namespace DBv3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
