using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace WpfFrame.Collection;

public class ItemPropertyChangedEventArgs<T> : PropertyChangedEventArgs
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public T Item { get; }

    public ItemPropertyChangedEventArgs(T item, string propertyName) : base(propertyName)
    {
        Item = item;
    }
}

public enum ItemPropertyChangedNotificationTypes
{
    All,
    AllowList,
    BlockList
}

/// <summary>
/// ObservableCollection 的增强版,可提供子对象的属性变化事件.其中只能包含实现了 INotifyPropertyChanged 接口的对象
/// </summary>
/// <typeparam name="T"></typeparam>
public class NotificationList<T> : ObservableCollection<T> where T : class, INotifyPropertyChanged
{
    public NotificationList(IEnumerable<T> collection) : base(collection)
    {
    }

    public NotificationList()
    {
    }


    /// <summary>
    /// 列表中对象属性改变时激活.
    /// </summary>
    public event EventHandler<ItemPropertyChangedEventArgs<T>>? ItemPropertyChanged;

    #region 元素的添加删除插入等

    public override void AddRange(IList<T> range)
    {
        var anyItem = this.Any();
        base.AddRange(range);

        foreach (var item in range)
        {
            item.PropertyChanged += Item_PropertyChanged;
            Debug.Print($"{item} 加入列表 开始监听事件. 位置50");
        }


        if (anyItem != this.Any())
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(AnyItem)));
            Debug.Print($"AnyItem 事件被触发 位置1 值:{AnyItem}");
        }
    }

    /// <summary>
    /// Called by base class Collection&lt;T&gt; when the list is being cleared;
    /// raises a CollectionChanged event to any listeners.
    /// </summary>
    protected override void ClearItems()
    {
        var anyItem = this.Any();
        foreach (var v in this)
        {
            v.PropertyChanged -= Item_PropertyChanged;
        }

        base.ClearItems();

        if (anyItem != this.Any())
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(AnyItem)));
            Debug.Print($"AnyItem 事件被触发 位置2 值:{AnyItem}");
        }
    }

    /// <summary>
    /// Called by base class Collection&lt;T&gt; when an item is removed from list;
    /// raises a CollectionChanged event to any listeners.
    /// </summary>
    protected override void RemoveItem(int index)
    {
        var item = this[index];
        item.PropertyChanged -= Item_PropertyChanged;
        base.RemoveItem(index);

        if (!this.Any())
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(AnyItem)));
            Debug.Print($"AnyItem 事件被触发 位置3 值:{AnyItem}");
        }
    }

    /// <summary>
    /// Called by base class Collection&lt;T&gt; when an item is added to list;
    /// raises a CollectionChanged event to any listeners.
    /// </summary>
    protected override void InsertItem(int index, T item)
    {
        var anyItem = this.Any();
        base.InsertItem(index, item);
        item.PropertyChanged += Item_PropertyChanged;
        Debug.Print($"{item} 加入列表 开始监听事件. 位置51");
        if (anyItem != this.Any())
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(AnyItem)));
            Debug.Print($"AnyItem 事件被触发 位置4 值:{AnyItem}");
        }
    }

    protected override void SetItem(int index, T item)
    {
        T originalItem = this[index];
        originalItem.PropertyChanged -= Item_PropertyChanged;
        base.SetItem(index, item);
        item.PropertyChanged += Item_PropertyChanged;
        Debug.Print($"{item} 加入列表 开始监听事件. 位置52");
    }

    #endregion

    public bool AnyItem => this.Any();

    /// <summary>
    /// 是否对子对象的属性变更进行通知,默认 true ,某些时候可以临时设为 false 以避免不必要的通知提高性能.
    /// </summary>
    public bool NotifyItemPropertyChanged { get; set; } = true;

    /// <summary>
    /// 一个属性名称的列表,可以允许或阻止指定名称的属性发生变更时触发通知
    /// </summary>
    // ReSharper disable once CollectionNeverUpdated.Global
    public List<string> PropertyNamesList { get; } = new();

    /// <summary>
    /// 对之元素属性变更进行通知方式
    /// </summary>
    public ItemPropertyChangedNotificationTypes ItemPropertyChangedNotificationType { get; set; } = new();


    private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        //将 item 的属性更改通过事件通知到外部
        if (NotifyItemPropertyChanged       &&
            ItemPropertyChanged is not null &&
            sender is T item                &&
            e.PropertyName is { } propertyName)
        {
            var args = new ItemPropertyChangedEventArgs<T>(item, propertyName);

            switch (ItemPropertyChangedNotificationType)
            {
                case ItemPropertyChangedNotificationTypes.All:
                    ItemPropertyChanged.Invoke(this, args);

                    break;

                case ItemPropertyChangedNotificationTypes.AllowList:

                    if (PropertyNamesList.Any(v => v == e.PropertyName))
                    {
                        ItemPropertyChanged.Invoke(this, args);
                    }

                    break;

                case ItemPropertyChangedNotificationTypes.BlockList:

                    if (PropertyNamesList.All(v => v != e.PropertyName))
                    {
                        ItemPropertyChanged.Invoke(this, args);
                    }

                    break;
            }
        }

        OnItemPropertyChanged(sender, e); //交给子类继续处理
    }


    protected virtual void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
    }
}