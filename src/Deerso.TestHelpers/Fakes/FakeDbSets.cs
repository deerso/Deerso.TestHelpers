using System.Linq;
using Deerso.Data.Tests.Fakes;
using Deerso.Domain.Websites;

namespace Deerso.TestHelpers.Fakes
{
    public class CouponFake : FakeDbSet<Coupon>
    {
        public override Coupon Find(params object[] keyValues)
        {
            return this.SingleOrDefault(x => x.Id == (int)keyValues.FirstOrDefault());
        }
    }

    public class CouponUsesFakeDbSet : FakeDbSet<CouponUses>
    {
        public override CouponUses Find(params object[] keyValues)
        {
            return this.SingleOrDefault(x => x.Id == (int)keyValues.FirstOrDefault());
        }
    }

    public class DeersoProductFakeDbSet : FakeDbSet<DeersoProduct>
    {
        public override DeersoProduct Find(params object[] keyValues)
        {
            return this.SingleOrDefault(x => x.Sku == (string)keyValues.FirstOrDefault());
        }
    }
}