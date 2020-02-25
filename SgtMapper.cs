using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace SgtMapper {

  public static class SgtMapper {

    private static Dictionary<Type, Type> exceptionMappers = new Dictionary<Type, Type>();
    private static IServiceProvider serviceProvider;
    private static ILogger<ExceptionMiddleware> logger = new Logger<ExceptionMiddleware>(new LoggerFactory());
    private static bool silent;

    public static void WithSgtMapper(this IApplicationBuilder app, bool suppressUnknownExceptionTypeLogging = false) {
      serviceProvider = app.ApplicationServices;
      exceptionMappers = AppDomain.CurrentDomain
          .GetAssemblies()
          .SelectMany(assembly => assembly.GetTypes())
          .Where(t => t.GetCustomAttributes(typeof(ExceptionMapperAttribute), true).Length > 0)
          .Where(t => t.GetInterfaces()[0].GetGenericArguments().Length > 0) // TODO use correct interface, not necessarily first one
          .ToDictionary(t => t.GetInterfaces()[0].GetGenericArguments()[0], t => t);
      silent = !suppressUnknownExceptionTypeLogging;
      app.UseMiddleware<ExceptionMiddleware>();
    }

    public static object MapException(Exception e) {

      if (!exceptionMappers.TryGetValue(e.GetType(), out var mapperType)) {
        if (!silent) {
          logger.LogInformation($"SgtMapper: Could not find an exception mapper for '${e.GetType()}'. " +
                                "If this is not intended, make sure that your exception types are properly annotated. " +
                                "To suppress this message, configure SgtMapper using 'WithSgtMapper(true)'.");
        }
        throw e;
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
      return methodInfo.Invoke(mapper, new[] { e });
    }
  }
}
