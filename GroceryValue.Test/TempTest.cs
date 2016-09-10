using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroceryValue.Test
{
    [TestClass]
    public class TempTest
    {
        [TestMethod]
        public void TempTest_01()
        {
            const string expected = "unknown";
            const string actual = "unknown";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TempTest_02()
        {
            const string strA = "unknown";
            const string strB = "Unknown";
            Assert.AreEqual(0, string.Compare(strA, strB, StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void TempTest_03()
        {
            const string strA = "לא ידוע";
            const string strB = "לא ידוע";
            Assert.AreEqual(0, string.Compare(strA, strB, StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void TempTest_04()
        {
            const int expected = 0;
            const float actual = 0.00F;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TempTest_05()
        {
            const float actual = 0.00F;
            Assert.IsTrue(Math.Abs(actual) <= float.Epsilon);
        }

        [TestMethod]
        public void TempTest_06()
        {
            const string str = "34 unknown 12";
            int actual;
            int.TryParse(str, out actual);
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void TempTest_07()
        {
            const string str = "34 unknown 12";
            bool actual;
            bool.TryParse(str, out actual);
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void TempTest_08()
        {
            const string str = "34 unknown 12";
            IEnumerable<bool> conditions = new[]
            {
                string.Compare(str, "unknown", StringComparison.OrdinalIgnoreCase) != 0,
                string.Compare(str, "לא ידוע", StringComparison.OrdinalIgnoreCase) != 0
            };
            Assert.IsTrue(conditions.All(condition => condition));
        }

        [TestMethod]
        public void TempTest_09()
        {
            const string str = "לא ידוע";
            IEnumerable<bool> conditions = new[]
            {
                string.Compare(str, "unknown", StringComparison.OrdinalIgnoreCase) != 0,
                string.Compare(str, "לא ידוע", StringComparison.OrdinalIgnoreCase) != 0
            };
            Assert.IsFalse(conditions.All(condition => condition));
        }

        [TestMethod]
        public void TempTest_10()
        {
            object sender = new ComboBox();
            Assert.IsTrue(sender.GetType() == typeof(ComboBox));
        }

        [TestMethod]
        public void TempTest_11()
        {
            object sender = new Button();
            Assert.IsFalse(sender.GetType() == typeof(ComboBox));
        }

        [TestMethod]
        public void TempTest_12()
        {
            IEnumerable<bool> conditions = new[]
            {
                false,
                true
            };
            Assert.IsTrue(conditions.Any(condition => condition));
        }

        [TestMethod]
        public void TempTest_13()
        {
            IEnumerable<bool> conditions = new[]
            {
                false,
                false
            };
            Assert.IsFalse(conditions.Any(condition => condition));
        }
    }
}
