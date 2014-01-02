using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Deerso.Common.Contracts;
using Deerso.Domain.Interfaces;
using Deerso.Domain.Websites;

namespace Deerso.TestHelpers.Fakes
{
    public class FakeProductsRepo : IProductsRepository
    {
        List<DeersoProduct> FakeItems { get; set; }
        public FakeProductsRepo(List<DeersoProduct> fakeItems = null)
        {
            FakeItems = fakeItems ?? new List<DeersoProduct>();
        }
        public IEnumerable<IProduct> GetProductsForBrowsePage(int amount, int? catId, int? mfrId, out int totalPages,
            int? pageNum = 1)
        {
            totalPages = 0;
            return null;
        }

        public IEnumerable<IProduct> GetProducts(List<string> skusList)
        {
            return null;
        }


        public IEnumerable<IProduct> GetProductsForHomePage(int amount)
        {
            return null;
        }

        public IEnumerable<IProduct> GetProducts(Expression<Func<ProductBase, bool>> wherePredicate)
        {
            return null;
        }


        public IEnumerable<IProduct> GetDeersoProducts(Expression<Func<DeersoProduct, bool>> wherePredicate)
        {
            return FakeItems.Where(wherePredicate.Compile());
        }

        public IEnumerable<IProduct> GetFamilyHardwareProducts(
            Expression<Func<FamilyHardwareProduct, bool>> wherePredicate)
        {
            return null;
        }

        public Dictionary<IProduct, List<string>> GetProductsWithImages(List<string> skuList)
        {
            return null;
        }
        public KeyValuePair<IProduct, List<string>> GetProductWithImage(string sku)
        {
            return default(KeyValuePair<IProduct, List<string>>);

        }

        public IProduct GetProduct(string sku)
        {
            return null;
        }


        public List<ProductImage> GetImagesForProduct(string sku)
        {
            return null;
        }


        public List<SelectListItem> GetShippingMethodSelectItems()
        {
            return null;
        }

        public IProduct ApplyBusinessRules(IProduct product)
        {
            return null;
        }


        public IEnumerable<IProduct> ApplyBusinessRules(IEnumerable<IProduct> products)
        {
            return default(IEnumerable<IProduct>);
        }

        public decimal GetShippingAmount(IProduct product)
        {
            return Decimal.Zero;
        }

        public decimal GetShippingAmount(Dictionary<IProduct, int> itemQuantityDictionary)
        {
            return decimal.Zero;
        }


        public Dictionary<int, string> GetTags(string sku)
        {
            throw new Exception();
        }

        public Dictionary<int, string> GetTags(IProduct product)
        {
            throw new NotImplementedException();
        }
    }
}