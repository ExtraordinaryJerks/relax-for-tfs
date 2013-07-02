using Microsoft.Practices.Unity;
using Relax.Repository;
using Relax.Repository.Interfaces;

namespace Relax.Unity
{
    public static class Dependencies
    {
        private static readonly IUnityContainer _container;

        static public void RegisterDefaults()
        {
        }

        static Dependencies()
        {
            _container = new UnityContainer();
            RegisterDefaults();
        }

        public static void Register<T>(T instance)
        {
            _container.RegisterInstance(instance);
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}