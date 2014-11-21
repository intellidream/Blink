using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Blink.Data.Tests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void TestElements()
        {
            var c00 = new Composite<Element>();
            var c01 = new Composite<Element>();
            var s = new Material<string>(MaterialTypes.Text);

            c00.Add(c01);
            c00.Add(s);
        }
    }
}
