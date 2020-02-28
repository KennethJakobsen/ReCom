using ReCom.Activation;

namespace ReCom.Config
{
    public class Configure
    {
        public static ReWorkConfigurer With(IActivator activator)
        {
            var bootstrapper = new Bootstrapper(activator);

            var strappedActivator = bootstrapper.RegisterServices();
            return new ReWorkConfigurer(strappedActivator);
        }
    }
}
