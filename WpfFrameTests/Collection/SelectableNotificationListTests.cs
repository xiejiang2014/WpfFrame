using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfFrame.Collection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfFrame.Collection.Tests
{
    internal class SelectableItem : ISelectableItem
    {
        private bool                              _isSelected;
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        public int Index { get; set; }

        public override string ToString()
        {
            return Index.ToString();
        }

        private static int _index;

        public static SelectableItem GetNew()
        {
            var result = new SelectableItem() { Index = _index };
            _index++;

            return result;
        }
    }

    [TestClass()]
    public class SelectableNotificationListTests
    {
        [TestMethod()]
        public void SelectableNotificationListTest()
        {
            var snl = new SelectableNotificationList<SelectableItem>();

            for (int i = 0; i < 50000; i++)
            {
                var item = new SelectableItem();
                snl.Add(item);
            }
        }
    }
}