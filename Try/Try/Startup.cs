using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Try.Startup))]
namespace Try
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
