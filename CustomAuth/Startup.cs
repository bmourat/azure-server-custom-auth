using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CustomAuth.Startup))]

namespace CustomAuth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}