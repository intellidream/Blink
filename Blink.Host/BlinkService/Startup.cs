using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BlinkAzure.Startup))]

namespace BlinkAzure
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}