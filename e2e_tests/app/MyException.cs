using System;
using SgtMapper;

namespace app {
  public class MyException : Exception {
  }

  [ExceptionMapper]
  public class MyExceptionMapper : ExceptionMapper<MyException> {
    public object Map(MyException exception) {
      return new { Message = "got a simple exception!" };
    }
  }
}
