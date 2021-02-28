using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Team1CentricAplication.Startup))]
namespace Team1CentricAplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
