using System;
using System.Collections.Generic;
using ReWork.Activation.LifeTime;

namespace ReWork.Activation
{
    public interface IActivator
    {
        object GetInstance(Type t);
        object GetInstance(Type t, HashSet<Type> registered);
        T GetInstance<T>();
        void Register(object instance);
        void Register<T>(Func<object> factory, ILifeTime lifetime);
        void Register<T>(object instance);
        void Register<T>();
        void Register<T>(ILifeTime lifeTime);
        void Register<TInterface, TInstance>(ILifeTime lifeTime) where TInstance : TInterface;
        void Register<TInterface, TInstance>() where TInstance : TInterface;
        bool HasRegistration(Type t);
    }
}