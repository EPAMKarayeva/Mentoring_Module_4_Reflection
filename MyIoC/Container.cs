using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyIoC
{
	public class Container
	{
    private readonly IActivator _activator;
    private readonly IDictionary<Type, Type> typesList;

    public Container(IActivator activator)
    {
      _activator = activator;
      typesList = new Dictionary<Type, Type>();
    }
    public void AddAssembly(Assembly assembly)
		{
      var types = assembly.ExportedTypes;

      foreach (var type in types)
      {
        var constructorImportAttribute = type.GetCustomAttribute<ImportConstructorAttribute>();

        if (constructorImportAttribute != null || HasImportProperties(type))
        {
          typesList.Add(type, type);
        }

        var exportAttributes = type.GetCustomAttributes<ExportAttribute>();

        foreach (var exportAttribute in exportAttributes)
        {
          typesList.Add(exportAttribute.Contract ?? type, type);
        }
      }
    }

		public void AddType(Type type)
		{
			typesList.Add(type, type);
		}

		public void AddType(Type type, Type baseType)
		{
			typesList.Add(baseType, type);
		}

		public object CreateInstance(Type type)
		{
			var instance = ConstructInstanceType(type);
			return instance;
		}

		public T CreateInstance<T>()
		{
			var type = typeof(T);
			var instance = (T)ConstructInstanceType(type);
			return instance;
		 }

    private object ConstructInstanceType(Type type)
    {
      if (!typesList.ContainsKey(type))
      {
        throw new Exception($"Cannot create instance of {type.FullName}");
      }

      Type newType = typesList[type];
      ConstructorInfo constructorInfo = GetConstructor(newType);
      object instance = CreateFromConstructor(newType, constructorInfo);

      if (newType.GetCustomAttribute<ImportConstructorAttribute>() != null)
      {
        return instance;
      }

      ResolveProperties(newType, instance);
      return instance;
    }

    private bool HasImportProperties(Type type)
    {
      var propertiesInfo = GetPropertiesRequiedImport(type);
      return propertiesInfo.Any();
    }

    private IEnumerable<PropertyInfo> GetPropertiesRequiedImport(Type type)
    {
      return type.GetProperties().Where(p => p.GetCustomAttribute<ImportAttribute>() != null);
    }

    private void ResolveProperties(Type type, object instance)
    {
      var propertiesInfo = GetPropertiesRequiedImport(type);

      foreach (var property in propertiesInfo)
      {
        var resolvedProperty = ConstructInstanceType(property.PropertyType);
        property.SetValue(instance, resolvedProperty);
      }
    }

    private ConstructorInfo GetConstructor(Type type)
    {
      ConstructorInfo[] constructors = type.GetConstructors();

      if (constructors.Length == 0)
      {
        throw new Exception($"There are no public constructors for type {type.FullName}");
      }

      return constructors.First();
    }

    private object CreateFromConstructor(Type type, ConstructorInfo constructorInfo)
    {
      ParameterInfo[] parameters = constructorInfo.GetParameters();
      List<object> parametersInstances = new List<object>(parameters.Length);
      Array.ForEach(parameters, p => parametersInstances.Add(ConstructInstanceType(p.ParameterType)));

      object instance = _activator.CreateInstance(type, parametersInstances.ToArray());
      return instance;
    }
  }
}
