using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TeploServices.Startup))]
namespace TeploServices
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
