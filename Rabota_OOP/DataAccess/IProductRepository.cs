using System.Collections.Generic;
using Marketplace.Domain;

namespace Marketplace.DataAccess
{
    /// <summary>
    /// Абстракция хранилища товаров.
    /// </summary>
    public interface IProductRepository
    {
        void Save(IEnumerable<Product> products);

        List<Product> Load();
    }
}
