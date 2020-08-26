using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyIoC;
using IoCSample;

namespace MyIoCTest
{
  [TestClass]
  public class IoCTest
  {
    private Container _container;

    [TestInitialize]
    public void Init()
    {
      _container = new Container(new SimpleActivator());
    }

    [TestMethod]
    public void Not_GenericCreateInstance_ExplicitSet_ConstructorTest()
    {
      _container.AddType(typeof(CustomerBLL));
      _container.AddType(typeof(Logger));
      _container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

      var customerBll = (CustomerBLL)_container.CreateInstance(typeof(CustomerBLL));

      Assert.IsNotNull(customerBll);
      Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL));
    }

    [TestMethod]
    public void GenericCreateInstance_ExplicitSet_ConstructorTest()
    {
      _container.AddType(typeof(CustomerBLL));
      _container.AddType(typeof(Logger));
      _container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

      var customerBll = _container.CreateInstance<CustomerBLL>();

      Assert.IsNotNull(customerBll);
      Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL));
    }


    [TestMethod]
    public void Not_GenericCreateInstance_ExplicitSet_PropertiesTest()
    {
      _container.AddType(typeof(CustomerBLL2));
      _container.AddType(typeof(Logger));
      _container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

      var customerBll = (CustomerBLL2)_container.CreateInstance(typeof(CustomerBLL2));

      Assert.IsNotNull(customerBll);
      Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL2));
      Assert.IsNotNull(customerBll.CustomerDAL);
      Assert.IsNotNull(customerBll.CustomerDAL.GetType() == typeof(CustomerDAL));
      Assert.IsNotNull(customerBll.Logger);
      Assert.IsNotNull(customerBll.Logger.GetType() == typeof(Logger));
    }

    [TestMethod]
    public void GenericCreateInstance_ExplicitSet_PropertiesTest()
    {
      _container.AddType(typeof(CustomerBLL2));
      _container.AddType(typeof(Logger));
      _container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

      var customerBll = _container.CreateInstance<CustomerBLL2>();

      Assert.IsNotNull(customerBll);
      Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL2));
      Assert.IsNotNull(customerBll.CustomerDAL);
      Assert.IsNotNull(customerBll.CustomerDAL.GetType() == typeof(CustomerDAL));
      Assert.IsNotNull(customerBll.Logger);
      Assert.IsNotNull(customerBll.Logger.GetType() == typeof(Logger));
    }

  }
}
