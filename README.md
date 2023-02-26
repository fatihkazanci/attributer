# AttributerLibrary
AttributerLibrary is an Attribute library that created by using Interceptor. This library allows any of your methods to be executed before or after they run. 
# Installation
You can start with creating a Attributer object.
```csharp
IExample productBusiness = AttributerPatcher<IExample, Example>.Create();
```

Or you can Dependency Injection if you are going to use it in ASP.Net Core projects. 
```csharp
builder.Services.AddScopedWithAttributer<IExampleBusiness, ExampleBusiness>();
//or
builder.Services.AddTransientWithAttributer<IExampleBusiness, ExampleBusiness>();
//or
builder.Services.AddSingletonWithAttributer<IExampleBusiness, ExampleBusiness>();
```

# Usage

First, create a Special Attribute Class which is a Base Model **Attributer**. There are 2 methods that can be overridden from **Attributer** Base Model. These 2 methods allow you to add the Attribute that created to Method or Class which will be used and with this you can configure the actions you take before or after the method is called.
```csharp
internal class LogAttribute : Attributer
    {
        public override void OnAfterExecuted(AttributerContext context)
        {
            ...
            base.OnAfterExecuted(context);
        }
        
        public override void OnBeforeExecute(AttributerContext context)
        {
            ...
            base.OnBeforeExecute(context);
        }
    }
```

For example, using a Method or a Class
```csharp
using ...

namespace BusinessLibrary.Business
{
    [Log]
    public class ExampleBusiness : IExampleBusiness
    {
        ...
        ...
    }
}
```

```csharp
using ...

namespace BusinessLibrary.Business
{
    public class ExampleBusiness : IExampleBusiness
    {
        [Log]
        public Example GetExample(int id)
        {
            ...
        }
    }
}
```

# AttributerContext
AttributerContextThe is the object where various information is stored.

**Method:** Property where you can execute MethodInfo progresses of the called Method.

**Arguments:** Property which brings parameter values of the called Method. 

**Result:**Property that indicating successful completion of Attribute. The default value is **true**. Example: The Method won’t work if you write the value as **false** in OnBeforeExecute method.  

**Error:** Property that stores error details if the called Method gets an error.

**ServiceProvider:** Property to call the services which was injected in case of using ASP.NET Core Dependency Injection.

**CallingParentMethods:** Property that indicates from which other methods the method to be executed is called.

# AttributerError
AttributerError is the object where the error information is stored when the called method gives an error. 

**Exception:** The object where the error information is stored.

**ErrorLine:** : The line number where the error occurred.

**ErrorMethod:** : The method in which the error occurred.

**CallingParentMethods:** Property that indicates from which other methods the method with the error was called. 
