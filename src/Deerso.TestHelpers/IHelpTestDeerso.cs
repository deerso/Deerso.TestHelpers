using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Deerso.BusinessLayer.Mixins;
using Deerso.Common.Contracts;
using Deerso.Data.Contracts;
using Deerso.Data.Contracts.Repositories;
using Deerso.Domain.Models;
using Moq;
using Ploeh.AutoFixture;
using System.Web.Mvc;
using Deerso.Domain.Websites;

namespace Deerso.TestHelpers
{
    public interface IHelpTestDeerso
    { }

    public static class TestDeersoMixins
    {
        private static IFixture fixture { get; set; }

        static TestDeersoMixins()
        {
            fixture = new Fixture().Customize(new FixtureHelpers.IgnoreVirtualMembersCustomisation());
            MockItemImagesRepo = new Mock<IItemImageRepository>();
        }

        private static Mock<IDeersoWebUnitOfWork> mockUow { get; set; }
        private static Mock<IItemImageRepository> MockItemImagesRepo { get; set; }

        public static Mock<IDeersoWebUnitOfWork> MockUow(this IHelpTestDeerso This)
        {
            mockUow = new Mock<IDeersoWebUnitOfWork>();

            //Addresses
            var mockAddressRepo = new Mock<IRepository<Domain.Models.Address>>();
            mockAddressRepo.Setup(i => i.GetById(It.IsAny<int>())).Returns(fixture.Create<Domain.Models.Address>());
            mockUow.SetupGet(i => i.Addresses).Returns(mockAddressRepo.Object);


            //Item Images
            var itemImagesRepo = new Mock<IItemImageRepository>();
            itemImagesRepo.Setup(i => i.GetBySku(It.IsAny<string>()))
                .Returns(fixture.CreateMany<ItemImages>().AsQueryable());
            mockUow.SetupGet(i => i.ItemImages).Returns(itemImagesRepo.Object);


            //Customers
            var mockCustomersRepo = new Mock<ICustomerRepository>();
            fixture.Customize<Domain.Models.Customer>(c => c.With(i => i.Orders, This.MockOrders(3)));
            //GetById, GetByUserId returns our mock customer
            mockCustomersRepo.Setup(i => i.GetById(It.IsAny<int>())).Returns(fixture.Create<Domain.Models.Customer>());
            mockCustomersRepo.Setup(i => i.GetByUserId(It.IsAny<Guid>())).Returns(fixture.Create<Domain.Models.Customer>());
            //Get all returns our mock customer as a queryable
            mockCustomersRepo.Setup(i => i.GetAll()).Returns(fixture.CreateMany<Domain.Models.Customer>().AsQueryable());
            mockUow.SetupGet(i => i.Customers).Returns(mockCustomersRepo.Object);

            return mockUow;
        }


     
        public static Mock<IRepository<T>> MockStandardRepo<T>(this IHelpTestDeerso This,
            List<T> mockData, Func<int, T> whereIdFunc) where T : class
        {
            var mock = new Mock<IRepository<T>>();

            mock.Setup(i => i.GetAll()).Returns(mockData.AsQueryable());
            mock.Setup(i => i.GetById(It.IsAny<int>()))
                .Returns(whereIdFunc);

            mock.Setup(i => i.Where(It.IsAny<Expression<Func<T, bool>>>()))
                .Returns((Expression<Func<T, bool>> x) => mockData.Where(x.Compile()).AsQueryable());
            return mock;
        }


        public static Fixture Fixture(this IHelpTestDeerso This)
        {
            return fixture as Fixture;
        }

        public static Mock<IWebsiteItemsRepository> MockWebsiteItemsRepository(this IHelpTestDeerso This)
        {
            var mockWir = new Mock<IWebsiteItemsRepository>();

            return mockWir;
        }

        public static Mock<IWebsiteItemsRepository> MockWebsiteItemsRepository(this IHelpTestDeerso This,
            List<WebsiteItem> itemsList)
        {
            var mockWir = This.MockWebsiteItemsRepository();

            mockWir.Setup(i => i.GetAll()).Returns(itemsList.AsQueryable());

            mockWir.Setup(i => i.GetAllForMfr(It.IsAny<int>()))
                .Returns((int i) => itemsList.Where(wi => wi.MfrID == i).AsQueryable());

            mockWir.Setup(i => i.Where(It.IsAny<Expression<Func<WebsiteItem, bool>>>()))
                .Returns((Expression<Func<WebsiteItem, bool>> i) => itemsList.Where(i.Compile()).AsQueryable());
            return mockWir;
        }

        public static Mock<IItemImageRepository> MockItemImagesRepository(this IHelpTestDeerso This)
        {
            var mockImages = new Mock<IItemImageRepository>();
            return mockImages;
        }

        public static Mock<IItemImageRepository> MockItemImagesRepository(this IHelpTestDeerso This,
            List<ItemImages> imagesList)
        {
            var mockImages = This.MockItemImagesRepository();

            mockImages.Setup(i => i.GetAll()).Returns(imagesList.AsQueryable());
            mockImages.Setup(i => i.GetBySku(It.IsAny<string>()))
                .Returns((string sku) => imagesList.Where(x => x.ItemSku == sku).AsQueryable());

            mockImages.Setup(i => i.Where(It.IsAny<Expression<Func<ItemImages, bool>>>()))
                .Returns((Expression<Func<ItemImages, bool>> i) => imagesList.Where(i.Compile()).AsQueryable());
            return mockImages;
        }


        public static Mock<IWebUrlHelper> MockWebUrlHelper(this IHelpTestDeerso This)
        {
            var mock = new Mock<IWebUrlHelper>();

            mock.Setup(i => i.GetMvcUrl(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<UrlHelper>()
                )).Returns("mockedUrl");

            return mock;
        }

        public static Mock<IProductsRepository> MockProductRepository(this IHelpTestDeerso This)
        {
            var mockPis = new Mock<IProductsRepository>();

            mockPis.Setup(i => i.GetProduct(It.IsAny<string>()))
                .Returns((string s) =>
                {
                    var product = This.MockProducts()[0];
                    product.Sku = s;
                    return product.ToDeersoProduct();
                });
            mockPis.Setup(i => i.GetShippingMethodSelectItems())
                .Returns(new List<SelectListItem> {new SelectListItem {Text = "MockShipMethod", Value = "MockValue"}});
            mockPis.Setup(i => i.GetImagesForProduct(It.IsAny<string>()))
                .Returns(fixture.CreateMany<ProductImage>().ToList());


            return mockPis;
        }

        public static List<WebsiteItem> MockProducts(this IHelpTestDeerso This, int count = 1)
        {
            var webItems = fixture.CreateMany<WebsiteItem>(count).ToArray();
            for (var i = 0; i < count; i++)
            {
                var images = fixture.CreateMany<ItemImages>().ToArray();
                webItems[i].Images = images;

                var tags = fixture.Build<ItemTags>().With(f => f.Tag, fixture.Create<Tag>()).CreateMany().ToArray();
                webItems[i].Tags = tags;
            }

            return webItems.ToList();
        }

        public static List<Order> MockOrders(this IHelpTestDeerso This, int count = 1)
        {
            var orders = fixture.CreateMany<Order>(count).ToArray();
            for (var i = 0; i < count; i++)
            {
                var orderDetails = fixture.CreateMany<OrderDetail>().ToArray();
                orders[i].Trackings = new List<Deerso.Domain.Models.Tracking> {fixture.Create<Domain.Models.Tracking>()};
                orders[i].OrderDetails = orderDetails;
            }

            return orders.ToList();
        }
    }
}