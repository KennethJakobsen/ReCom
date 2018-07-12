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

        public Type ConcreteType { get;  }
        public ILifeTime LifeTime { get;  }
    }
}
