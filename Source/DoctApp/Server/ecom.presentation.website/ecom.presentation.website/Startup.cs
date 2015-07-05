using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ecom.presentation.website.Startup))]
namespace ecom.presentation.website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
