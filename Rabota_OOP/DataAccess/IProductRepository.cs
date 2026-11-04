using System;
using System.Collections.Generic;
using System.Text;
using Marketplace.Domain;

namespace Marketplace.DataAccess
{
    public interface IProductRepository
    {
        void Save(IEnumerable<Product> products);
        List<Product> Load();
    }
}
