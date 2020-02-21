using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;

namespace SgtMapper {

  public static class SgtMapper {

    private static Dictionary<Type, Type> exceptionMappers = new Dictionary<Type, Type>();
    private static IServiceProvider serviceProvider;

    public static void WithSgtMapper(this IApplicationBuilder app) {
      serviceProvider = app.ApplicationServices;
      exceptionMappers = AppDomain.CurrentDomain
          .GetAssemblies()
          .SelectMany(assembly => assembly.GetTypes())
          .Where(t => t.GetCustomAttributes(typeof(ExceptionMapperAttribute), true).Length > 0)
          .Where(t => t.IsGenericType)
          .ToDictionary(t => t.GetGenericArguments()[0], t => t);

      app.UseMiddleware<ExceptionMiddleware>();
    }

    public static object MapException(Exception e) {

      if (!exceptionMappers.TryGetValue(e.GetType(), out var mapperType)) {
        throw new Exception($"Could not find the an exception mapper for type '{e.GetType()}'. Make sure your exception mappers are correctly annotated.");
      }

      var mapper = serviceProvider.GetService(mapperType);
      if (mapper == null) {
        try {
          mapper = Activator.CreateInstance(mapperType);
        } catch (Exception) {
          throw new Exception($"Could not instantiate mapper for type '{e.GetType()}'.");
        }
      }

      var methodInfo = mapperType.GetMethod("Map");
      var genericMethod = methodInfo.MakeGenericMethod(e.GetType());
      return genericMethod.Invoke(mapper, new[] { e });
    }
  }
}
