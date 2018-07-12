using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReWork.Activation;
using ReWork.Activation.LifeTime;
using ReWork.Exceptions;
using Shouldly;

namespace Rework.Tests.Unit.Activator
{
    [TestClass]
    public class DefaultActivatorTests
    {
        private DefaultActivator _sut;

        [TestInitialize]
        public void Setup()
        {
            _sut = new DefaultActivator();
        }

        [TestMethod]
        public void CanGetExistingInstance()
        {
            var instance = new Test1();
            _sut.Register(instance);

            var result = _sut.GetInstance<Test1>();
            result.ShouldNotBeNull();
            result.ShouldBe(instance);
        }

        [TestMethod]
        public void CanGetExistingInstanceFromInterface()
        {
            var instance = new Test1();
            _sut.Register<Interface1>(instance);

            var result = _sut.GetInstance<Interface1>();
            result.ShouldNotBeNull();
            result.ShouldBe(instance);
        }

        [TestMethod]
        public void CanGetInstanceFromInterfaceWithoutDependencies()
        {
            _sut.Register<Interface1, Test1>();

            var result = _sut.GetInstance<Interface1>();
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Test1>();
        }

        [TestMethod]
        public void CanUseActivatorLifeTime()
        {
            _sut.Register<Interface1, Test1>(new ActivatorLifeTime());

            var result1 = _sut.GetInstance<Interface1>();
            result1.ShouldNotBeNull();
            result1.ShouldBeOfType<Test1>();

            var result2 = _sut.GetInstance<Interface1>();
            result2.ShouldNotBeNull();
            result2.ShouldBeOfType<Test1>();

            result1.ShouldBeSameAs(result2);
        }


        [TestMethod]
        public void CanUseTransientLifeTime()
        {
            _sut.Register<Interface1, Test1>(new TransientLifeTime());

            var result1 = _sut.GetInstance<Interface1>();
            result1.ShouldNotBeNull();
            result1.ShouldBeOfType<Test1>();

            var result2 = _sut.GetInstance<Interface1>();
            result2.ShouldNotBeNull();
            result2.ShouldBeOfType<Test1>();

            result1.ShouldNotBeSameAs(result2);
        }

        [TestMethod]
        public void CanGetInstanceFromFromType()
        {
            _sut.Register<Test1>();

            var result = _sut.GetInstance<Test1>();
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Test1>();
        }


        [TestMethod]
        public void CanGetInstanceFromInterfaceWithDependencies()
        {
            _sut.Register<Interface1, Test1>();
            _sut.Register<Interface2, Test2>();
            _sut.Register<Interface3, Test3>();
            _sut.Register<Interface4, Test4>();
            _sut.Register<Interface5, Test5>();

            var result = _sut.GetInstance<Interface2>();
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Test2>();
        }

        [TestMethod]
        public void CanFailGracefullyWithDependencies()
        {
            
            _sut.Register<Interface2, Test2>();
            _sut.Register<Interface3, Test3>();
            _sut.Register<Interface4, Test4>();
            _sut.Register<Interface5, Test5>();

             Should.Throw<ActivatorServiceNotFoundException>(() => _sut.GetInstance<Interface2>());
            
        }

        [TestMethod]
        public void CanCanDetectCircularDependencies()
        {
            _sut.Register<Interface6, Test6>();
            _sut.Register<Interface7, Test7>();
            _sut.Register<Interface8, Test8>();

            Should.Throw<ActivatorCircularDependencyException>(() => _sut.GetInstance<Interface6>());
        }

        [TestMethod]
        public void CanResolveFactory()
        {
            _sut.Register<Interface1>(() => new Test1(), false);

            var result = _sut.GetInstance<Interface1>();
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Test1>();
        }

        [TestMethod]
        public void CanKeepFactoryInstanceAlive()
        {
            _sut.Register<Interface1>(() => new Test1(), true);

            var result1 = _sut.GetInstance<Interface1>();
            var result2 = _sut.GetInstance<Interface1>();

            result1.ShouldNotBeNull();
            result1.ShouldBeOfType<Test1>();

            result2.ShouldNotBeNull();
            result2.ShouldBeOfType<Test1>();

            result2.ShouldBeSameAs(result1);
        }


    }



    interface Interface1 { }
    class Test1 : Interface1 {}
    interface Interface2 { }

    class Test2 : Interface2
    {
        public Test2(Interface1 i1, Interface3 i3, Interface5 i5)
        {
            
        }
    }
    interface Interface3 { }

    class Test3 : Interface3
    {
        public Test3(Interface4 i4)
        {
            
        }
    }
    interface Interface4 { }

    class Test4 : Interface4
    {
        public Test4()
        {
            
        }
        public Test4(Interface5 i5)
        {
            
        }
    }
    interface Interface5 { }
    class Test5 : Interface5 { }
    interface Interface6 { }
    class Test6 : Interface6
    {
        public Test6(Interface7 i2)
        {
            
        }
    }

    interface Interface7 { }
    class Test7 : Interface7
    {
        public Test7(Interface8 i2)
        {

        }
    }

    interface Interface8 { }
    class Test8 : Interface8
    {
        public Test8(Interface6 i2)
        {

        }
    }

}
