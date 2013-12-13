﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Deerso.TestHelpers
{
    public class FixtureHelpers
    {
        /// <summary>
        /// Customisation to ignore the virtual members in the class - helps ignoring the navigational 
        /// properties and makes it quicker to generate objects when you don't care about these
        /// </summary>
        public class IgnoreVirtualMembers : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }

                var pi = request as PropertyInfo;
                if (pi == null)
                {
                    return new NoSpecimen(request);
                }

                if (pi.GetGetMethod().IsVirtual)
                {
                    return null;
                }
                return new NoSpecimen(request);
            }
        }
        public class IgnoreVirtualMembersCustomisation : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customizations.Add(new IgnoreVirtualMembers());
            }
        }
    }
}
