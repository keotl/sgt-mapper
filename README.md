# Sgt. Mapper's Lonely Exceptions Band
Uncomplicated exception mapping for ASP.NET Core. 

## What does it do
SgtMapper is installed as a middleware in your application which automatically
catches uncaught exceptions and maps it to a response using one of the user-defined
exception mappers.

## How to use it
1. Add the nuget package.
```bash
dotnet app package SgtMapper
```

2. Configure SgtMapper in your Startup.cs.
```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        ...
        app.WithSgtMapper();
    }
```

3. Define custom exception mappers.
```csharp
[ExceptionMapper]
  public class MyExceptionMapper : ExceptionMapper<MyException> {
    public object Map(MyException exception) {
      return new { Message = "got a simple exception!" };
    }
  }
```

## Advanced Settings
None! SgtMapper is designed to be as simple to use as possible. There is nothing
more to configure than what is described in this README. Exception mappers will
be automatically detected in your project, and will be instantiated directly, or
using ASP.NET's dependency injection mechanism, should it require constructor
parameters.

## Running the tests
Sgt. Mapper has an [Anachronos](https://pypi.org/project/anachronos/) testing suite which can be found in the `e2e_tests` directory. Running the tests requires python3.6 or later. 

```bash
pip install -r e2e_tests/requirements.txt
sh run_tests.sh
```
