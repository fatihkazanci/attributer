# AttributerLibrary
Interceptor yapısından faydalanarak oluşturulan bir Attribute kütüphanesidir.Bu kütüphane herhangi bir metodunuzun çalışmadan öncesinde veya çalıştıktan sonrasında istediğiniz bir işlemin gerçekleştirilmesini sağlar.
# Installation
You can start with creating a Attributer object.
```csharp
IExample productBusiness = AttributerPatcher<IExample, Example>.Create();
```

Or you can Dependency Injection if you are going to use it in ASP.Net Core projects. 
```csharp
builder.Services.AddScopedWithAttributer<IUserBusiness, UserBusiness>();
//or
builder.Services.AddTransientWithAttributer<IUserBusiness, UserBusiness>();
//or
builder.Services.AddSingletonWithAttributer<IUserBusiness, UserBusiness>();
```

# Usage

Öncelikle Base Modeli **Attributer** olan bir tane Özel Attribute sınıfımızı oluşturuyoruz.
**Attributer** Base Modelinden override edebileceğimiz 2 adet method bulunmaktadır.
Bu iki method sayesinde oluşturduğunuz Attribute'yi kullanacağınız Methodun veya Class'ın üstüne koyarak, o methodun çağrılmadan önceki veya çağırıdıktan sonraki gerçekleştireceğiniz işlemleri yapılandırmış oluyorsunuz.
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

Örnek olarak bir Methodun veya bir Class'da kullanımı
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
İşlem aşamasında çeşitli bilgilerin tutulduğu nesnedir.

**Method:** Çağırılan Metodun *MethodInfo* işlemlerini yapabileceğiniz bir property

**Arguments:** Çağırılan Metodun parametre değerlerini getiren property

**Result:** Attributenin başarılı şekilde tamamlandığını belirten bir property. Varsayılan değer *true*. Örnek: Eğer **OnBeforeExecute** metodunuda değeri *false* olarak belirtirseniz çağırılmak istenen method çalışmayacaktır.

**Error:** Eğer çağırılan metot bir hata alırsa, hata detaylarını saklayan bir property.

**ServiceProvider:** Eğer ASP.NET Core Dependency Injection kullanıyorsanız, Inject ettiğiniz servisleri çağırmak için kullanılan property.

**CallingParentMethods:** Çalıştırılmak istenen metodun hangi diğer methotlardan çağrıldığını gösteren property.

# AttributerError
Çağırılmak istenen metot hata verdiğinde, hata bilgilerinin tutulduğu nesnedir.
**Exception:** Hata bilgilerinin tutulduğu nesne.
**ErrorLine:** Hatanın gerçekleştiği satır numarası.
**ErrorMethod:** Hatanın olduğu metot.
**CallingParentMethods:** Hatanın olduğu metotun hangi diğer metotlardan çağırıldığını gösteren property.
