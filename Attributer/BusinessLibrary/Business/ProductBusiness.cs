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
