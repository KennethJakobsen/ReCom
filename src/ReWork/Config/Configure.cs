using ReWork.Activation;

namespace ReWork.Config
{
    public class Configure
    {
        public static ReWorkConfigurer With(IActivator activator)
        {
            Bootstrapper bootstrapper;
            if (activator == null)
                bootstrapper = new Bootstrapper();
            bootstrapper = new Bootstrapper(activator);

            var strappedActivator = bootstrapper.RegisterServices();

            return strappedActivator.GetInstance<ReWorkConfigurer>();
        }
    }
}
