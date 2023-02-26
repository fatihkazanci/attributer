using BusinessLibrary.Models;

namespace BusinessLibrary.Interface
{
    public interface IProductBusiness
    {
        BusinessResult<Product> GetProduct(int id);
    }
}
