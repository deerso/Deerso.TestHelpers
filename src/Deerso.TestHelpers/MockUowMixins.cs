using System;
using System.Linq.Expressions;
using Deerso.Data.Contracts;
using Moq;

namespace Deerso.TestHelpers
{
    public static class MockUowMixins
    {
        /// <summary>
        /// Takes a property of the unit of work, and the repository, and sets it up so 
        /// the uow uses the mockRepo instead of whatever is currently implemented
        /// </summary>
        public static Mock<IDeersoWebUnitOfWork> SetupMockRepository<T>(this Mock<IDeersoWebUnitOfWork> This, Expression<Func<IDeersoWebUnitOfWork, T>> property, T mockRepo) where T : class
        {
            This.SetupGet(property).Returns(mockRepo);
            return This;
        } 
    }
}