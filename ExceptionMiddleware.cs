using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SgtMapper {
  public class ExceptionMiddleware {
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
      try {
        await _next(context);
      } catch (Exception e) {
        var mappedResponse = SgtMapper.MapException(e);
        JsonConvert.SerializeObject(mappedResponse);
      }
    }
  }
}
