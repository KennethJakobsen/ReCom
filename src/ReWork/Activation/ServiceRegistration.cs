using System;
using System.Collections.Generic;
using System.Text;
using ReWork.Activation.LifeTime;

namespace ReWork.Activation
{
    public class ServiceRegistration
    {
        public ServiceRegistration(Type concreteType, ILifeTime lifeTime)
        {
            ConcreteType = concreteType;
            LifeTime = lifeTime;
        }
        public ServiceRegistration(Func<object> factory, ILifeTime lifeTime)
        {
            Factory = factory;
            LifeTime = lifeTime;
        }
        public ServiceRegistration(object instance)
        {
            Instance = instance;
        }

        public bool HasInstance => Instance != null;
        public bool HasFactory => Factory != null;
        public Type ConcreteType { get; set; }
        public object Instance { get; set; }
        public Func<object> Factory { get; set; }
        public ILifeTime LifeTime { get; set; }
    }
}
