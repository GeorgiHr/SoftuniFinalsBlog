using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SoftuniFinalsBlog.Startup))]
namespace SoftuniFinalsBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
