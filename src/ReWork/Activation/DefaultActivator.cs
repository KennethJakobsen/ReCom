using System;
using System.Collections.Generic;
using ReWork.Activation.LifeTime;
using ReWork.Exceptions;

namespace ReWork.Activation
{
    public class DefaultActivator : IDisposable, IActivator
    {
        private Dictionary<Type, ServiceRegistration> _registrations = new Dictionary<Type, ServiceRegistration>();

        public void Register<TInterface, TInstance>() where TInstance : TInterface
        {
            _registrations.Add(typeof(TInterface), new ServiceRegistration(typeof(TInstance), new TransientLifeTime()));
        }
        public void Register<TInterface, TInstance>(ILifeTime lifetime) where TInstance : TInterface
        {
            _registrations.Add(typeof(TInterface), new ServiceRegistration(typeof(TInstance), lifetime));
        }

        public void Register<T>()
        {
            _registrations.Add(typeof(T), new ServiceRegistration(typeof(T), new TransientLifeTime()));
        }
        public void Register<T>(ILifeTime lifeTime)
        {
            _registrations.Add(typeof(T), new ServiceRegistration(typeof(T), lifeTime));
        }

        public void Register<T>(object instance)
        {
            _registrations.Add(typeof(T), new ServiceRegistration(instance));
        }

        public void Register(object instance)
        {
            _registrations.Add(instance.GetType(), new ServiceRegistration(instance));
        }

        public void Register<T>(Func<object> factory, ILifeTime lifetime)
        {
            _registrations.Add(typeof(T), new ServiceRegistration( factory, lifetime));
        }

        public T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }
        public object GetInstance(Type t)
        {
            return GetInstance(t, new HashSet<Type>());
        }

        public bool HasRegistration(Type t)
        {
            return _registrations.ContainsKey(t);
        }
        public object GetInstance(Type t, HashSet<Type> registered)
        {

            if (!_registrations.ContainsKey(t))
                throw new ActivatorServiceNotFoundException($"Type: {t} was not found in activator");

            var reg = _registrations[t];
            if (reg.HasInstance) return reg.Instance;
            var instance = reg.HasFactory ? reg.Factory() : Instantiate(reg, registered);

            SaveInstance(t, instance);
            return instance;
        }

        private object Instantiate(ServiceRegistration reg, HashSet<Type> registered)
        {
            if (registered.Contains(reg.ConcreteType))
                throw new ActivatorCircularDependencyException($"Circular dependency detected: {reg.ConcreteType}");

            registered.Add(reg.ConcreteType);
            var ctors = reg.ConcreteType.GetConstructors();

            if (ctors.Length == 0)
                return System.Activator.CreateInstance(reg.ConcreteType);

            var firstCtor = ctors[0];
            var ctorParams = firstCtor.GetParameters();
            var ctorParamLength = ctorParams.Length;
            var instances = new object[ctorParamLength];

            if (ctorParamLength == 0)
                return System.Activator.CreateInstance(reg.ConcreteType);

            for (var i = 0; i < ctorParamLength; i++)
                instances[i] = GetInstance(ctorParams[i].ParameterType, new HashSet<Type>(registered));

            registered = null;

            return firstCtor.Invoke(instances);
        }

        private void SaveInstance(Type id, object instance)
        {
            var reg = _registrations[id];
            if (reg.LifeTime is ActivatorLifeTime)
                reg.Instance = instance;
        }
        public void Dispose()
        {
            _registrations = null;
        }
    }
}
