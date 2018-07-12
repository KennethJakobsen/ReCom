using System;
using System.Collections.Generic;
using ReWork.Activation.LifeTime;
using ReWork.Exceptions;

namespace ReWork.Activation
{
    public class DefaultActivator : IDisposable, IActivator
    {
        private Dictionary<Type, object> _instances = new Dictionary<Type, object>();
        private Dictionary<Type, ServiceRegistration> _nonInstatiated = new Dictionary<Type, ServiceRegistration>();
        private Dictionary<Type, ServiceFactory> _factories = new Dictionary<Type, ServiceFactory>();

        public void Register<TInterface, TInstance>() where TInstance : TInterface
        {
            _nonInstatiated.Add(typeof(TInterface), new ServiceRegistration(typeof(TInstance), new TransientLifeTime()));
        }
        public void Register<TInterface, TInstance>(ILifeTime lifetime) where TInstance : TInterface
        {
            _nonInstatiated.Add(typeof(TInterface), new ServiceRegistration(typeof(TInstance), lifetime));
        }

        public void Register<T>()
        {
            _nonInstatiated.Add(typeof(T), new ServiceRegistration(typeof(T), new TransientLifeTime()));
        }
        public void Register<T>(ILifeTime lifeTime)
        {
            _nonInstatiated.Add(typeof(T), new ServiceRegistration(typeof(T), lifeTime));
        }

        public void Register<T>(object instance)
        {
            _instances.Add(typeof(T), instance);
        }

        public void Register(object instance)
        {
            _instances.Add(instance.GetType(), instance);
        }

        public void Register<T>(Func<object> factory, bool keepAlive)
        {
            _factories.Add(typeof(T), new ServiceFactory(keepAlive, factory));
        }

        public T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }
        public object GetInstance(Type t)
        {
            return GetInstance(t, new HashSet<Type>());
        }

        private object InvokeFactory(Type t)
        {
            var serviceFactory = _factories[t];
            var instance = serviceFactory.Create();

            if (!serviceFactory.KeepAlive)
                return instance;

            _instances.Add(t, instance);
            return instance;
        }

        public bool HasRegistration(Type t)
        {
            return _instances.ContainsKey(t) || _factories.ContainsKey(t) ||
                   _nonInstatiated.ContainsKey(t);
        }
        public object GetInstance(Type t, HashSet<Type> registered)
        {
            if (_instances.ContainsKey(t)) return _instances[t];
            if (_factories.ContainsKey(t)) return InvokeFactory(t);
            if (_nonInstatiated.ContainsKey(t))
            {
                var instance = Instantiate(_nonInstatiated[t], registered);
                SaveInstance(t, instance, _nonInstatiated[t].LifeTime);
                return instance;
            }
            throw new ActivatorServiceNotFoundException($"Type: {t} was not found in activator");
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

        private void SaveInstance(Type id, object instance, ILifeTime lifeTime)
        {
            if (lifeTime is ActivatorLifeTime)
                _instances.Add(id, instance);
        }
        public void Dispose()
        {
            _instances = null;
            _nonInstatiated = null;
            _factories = null;
        }
    }
}
