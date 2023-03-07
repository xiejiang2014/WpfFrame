using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using WpfFrame.Demo;

namespace WpfFrame.Collection.Tests
{
    [TestClass()]
    public class SelectableNotificationListTests
    {
        [TestMethod()]
        public void SelectableNotificationListTest()
        {
            Debug.Print($"=====添加1000");
            var snl = new SelectableNotificationList<SelectableItem>();

            var items = Enumerable
                       .Range(1, 10)
                       .Select(v => SelectableItem.GetNew())
                       .ToArray();

            snl.AddRange(items);

            Debug.Print($"=====全选");

            snl.SelectAll();

            Debug.Print($"=====全不选");

            snl.UnselectAll();

            Debug.Print($"=====全选");

            snl.SelectAll();

            Debug.Print($"=====进入单选模式");

            snl.CanMultipleSelect = false;

            Debug.Print($"=====取消首个选定");

            snl[0].IsSelected = false;
            //snl[0].IsSelected = true;
        }
    }
}