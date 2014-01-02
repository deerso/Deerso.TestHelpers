using System.Collections.Generic;
using Deerso.Data.Tests.Fakes;
using Deerso.Domain.Websites;

namespace Deerso.TestHelpers.Fakes
{
    public class FakeContext : FakeDbContext
    {
        public FakeContext()
        {
            AddFakeDbSet<Coupon, CouponFake>();
            AddFakeDbSet<CouponUses, CouponUsesFakeDbSet>();
            AddFakeDbSet<DeersoProduct, DeersoProductFakeDbSet>();
        }
        public void AddFakeData<T>(List<T> fakeData) where T : class
        {
            fakeData.ForEach(x => this.Set<T>().Add(x));
        }
    } 
}