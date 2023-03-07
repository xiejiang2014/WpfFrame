using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfFrame.Collection;

namespace WpfFrame.Demo
{
    internal class SelectableNotificationListDemoViewModel
    {

        public SelectableNotificationList<SelectableItem> Items { get; }=new();


        public SelectableNotificationListDemoViewModel()
        {
            var items = Enumerable
                       .Range(1, 1000)
                       .Select(v => SelectableItem.GetNew())
                       .ToArray();

            Items.AddRange(items);
        }
    }
}
