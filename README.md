# AttributerLibrary
Interceptor yapısından faydalanarak oluşturulan bir Attribute kütüphanesidir.Bu kütüphane herhangi bir metodunuzun çalışmadan öncesinde veya çalıştıktan sonrasında istediğiniz bir işlemin gerçekleştirilmesini sağlar.
# Installation
You can start with creating a Attributer object.
```csharp
IExample productBusiness = AttributerPatcher<IExample, Example>.Create();
```

Or you can inject dependency if you are going to use it in ASP.Net Core projects. 
```csharp
builder.Services.AddScopedWithAttributer<IUserBusiness, UserBusiness>();
//or
builder.Services.AddTransientWithAttributer<IUserBusiness, UserBusiness>();
//or
builder.Services.AddSingletonWithAttributer<IUserBusiness, UserBusiness>();
```

# Usage

Öncelikle Base Modeli *Attributer* olan bir tane Özel Attribute sınıfımızı oluşturuyoruz.
**Attributer** Base Modelinden override edebileceğimiz 2 adet method bulunmaktadır.
Bu iki method sayesinde oluşturduğunuz Attribute'yi kullanacağınız Methodun veya Class'ın üstüne koyarak, o methodun çağrılmadan önceki veya çağırıdıktan sonraki gerçekleştireceğiniz işlemleri yapılandırmış oluyorsunuz.
```csharp
internal class LogAttribute : Attributer
    {
        public override void OnAfterExecuted(AttributerContext context)
        {
            //This is Example Attribute
            base.OnAfterExecuted(context);
        }
        
        public override void OnBeforeExecute(AttributerContext context)
        {
            base.OnBeforeExecute(context);
        }
    }
```

Örnek olarak bir Methodun veya bir Class'da kullanımı
```csharp
using BusinessLibrary.Attribute;
using BusinessLibrary.Interface;
using BusinessLibrary.Models;
using Microsoft.AspNetCore.Http;

namespace BusinessLibrary.Business
{
    [Log]
    public class UserBusiness : IUserBusiness
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserBusiness(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public BusinessResult<User> GetUser(int id)
        {
            return CustomPrivateMethod(id);
        }

        private BusinessResult<User> CustomPrivateMethod(int id)
        {
            return new BusinessResult<User>()
            {
                IsSuccess = true,
                ReturnObject = new User()
                {
                    Id = id,
                    Name = "Fatih",
                    Surname = "Kazancı",
                    UserAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"]
                }
            };
        }
    }
}
```

```csharp
using BusinessLibrary.Attribute;
using BusinessLibrary.Interface;
using BusinessLibrary.Models;

namespace BusinessLibrary.Business
{
    public class ProductBusiness : IProductBusiness
    {
        [Log]
        public BusinessResult<Product> GetProduct(int id)
        {
            return new BusinessResult<Product>()
            {
                IsSuccess = true,
                ReturnObject = new Product()
                {
                    Id = id,
                    Name = "Nokia 3310",
                    Price = 30
                }
            };
        }
    }
}
```
