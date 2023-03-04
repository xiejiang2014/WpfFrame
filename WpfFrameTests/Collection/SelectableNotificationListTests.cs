using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfFrame.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfFrame.Demo;

namespace WpfFrame.Collection.Tests
{
    [TestClass()]
    public class SelectableNotificationListTests
    {
        [TestMethod()]
        public void SelectableNotificationListTest()
        {
            var snl = new SelectableNotificationList<SelectableItem>();

            var items = Enumerable
                       .Range(0, 999)
                       .Select(v => SelectableItem.GetNew())
                       .ToArray();

            snl.AddRange(items);

            snl.SelectAll();
        }
    }
}