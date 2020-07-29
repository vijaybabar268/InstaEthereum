using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InstaEthereum.Startup))]
namespace InstaEthereum
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
