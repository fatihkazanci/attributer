
using AttributerLibrary;
using BusinessLibrary.Business;
using BusinessLibrary.Interface;
using BusinessLibrary.Models;

IProductBusiness productBusiness = AttributerPatcher<IProductBusiness, ProductBusiness>.Create();
Console.WriteLine("Enter your Product Id");
int requestId = Convert.ToInt32(Console.ReadLine());
BusinessResult<Product> currentUserResponse = productBusiness.GetProduct(requestId);
if (currentUserResponse.IsSuccess)
{
    string? fullName = currentUserResponse?.ReturnObject?.Name;
    Console.WriteLine(fullName);
}
else
{
    Console.WriteLine("Product not found");
}