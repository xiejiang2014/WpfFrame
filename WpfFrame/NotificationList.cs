using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfFrame
{
    /// <summary>
    /// ObservableCollection 的增强版,可提供子对象的属性变化事件.其中只能包含实现了 INotifyPropertyChanged 接口的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NotificationList<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public NotificationList(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        public NotificationList()
        {
        }

        /// <summary>
        /// 列表中对象属性改变时激活.
        /// </summary>
        public event ItemPropertyChangedEventHandler ItemPropertyChanged;

        public delegate void ItemPropertyChangedEventHandler(object sender, ItemPropertyChangedEventArgs e);

        public class ItemPropertyChangedEventArgs : PropertyChangedEventArgs
        {
            public T Item { get; set; }

            public ItemPropertyChangedEventArgs(string propertyName) : base(propertyName)
            {
            }

            public ItemPropertyChangedEventArgs(T item, string propertyName) : base(propertyName)
            {
                Item = item;
            }
        }

        public new void Add(T item)
        {
            item.PropertyChanged += Item_PropertyChanged;
            base.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                item.PropertyChanged += Item_PropertyChanged;
                base.Add(item);
            }
        }

        public new void Insert(int index, T item)
        {
            item.PropertyChanged += Item_PropertyChanged;
            base.Insert(index, item);
        }

        public new bool Remove(T item)
        {
            item.PropertyChanged -= Item_PropertyChanged;
            return base.Remove(item);
        }

        public new void RemoveAt(int index)
        {
            this[index].PropertyChanged -= Item_PropertyChanged;
            base.RemoveAt(index);
        }

        public new void Clear()
        {
            foreach (var v in this)
            {
                v.PropertyChanged -= Item_PropertyChanged;
            }

            base.Clear();
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ItemPropertyChanged?.Invoke(this, new ItemPropertyChangedEventArgs((T)sender, e.PropertyName));
        }
    }
}