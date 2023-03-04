using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public event EventHandler<ItemPropertyChangedEventArgs<T>>? ItemPropertyChanged;

    #region 元素的添加删除插入等

    public new void Add(T item)
    {
        var anyItem = this.Any();

        item.PropertyChanged += Item_PropertyChanged;

        base.Add(item);

        if (anyItem != this.Any())
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(AnyItem)));
            Debug.Print("AnyItem 事件被触发 1");
        }
    }


    public override void AddRange(IList<T> items)
    {
        var anyItem = this.Any();

        foreach (var item in items)
        {
            item.PropertyChanged += Item_PropertyChanged;
        }

        base.AddRange(items);

        if (anyItem != this.Any())
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(AnyItem)));
            Debug.Print("AnyItem 事件被触发 2");
        }
    }

    public new void Insert(int index, T item)
    {
        var anyItem = this.Any();

        item.PropertyChanged += Item_PropertyChanged;
        base.Insert(index, item);

        if (anyItem != this.Any())
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(AnyItem)));
            Debug.Print("AnyItem 事件被触发 3");
        }
    }

    public new bool Remove(T item)
    {
        var anyItem = this.Any();

        item.PropertyChanged -= Item_PropertyChanged;

        var r = base.Remove(item);

        if (anyItem != this.Any())
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(AnyItem)));
            Debug.Print("AnyItem 事件被触发 4");
        }

        return r;
    }

    public new void RemoveAt(int index)
    {
        var anyItem = this.Any();

        this[index].PropertyChanged -= Item_PropertyChanged;

        base.RemoveAt(index);

        if (anyItem != this.Any())
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(AnyItem)));
            Debug.Print("AnyItem 事件被触发 5");
        }
    }


    public bool AnyItem => this.Any();

    public new void Clear()
    {
        var anyItem = this.Any();

        foreach (var v in this)
        {
            v.PropertyChanged -= Item_PropertyChanged;
        }

        base.Clear();

        if (anyItem != this.Any())
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(AnyItem)));
            Debug.Print("AnyItem 事件被触发 6");
        }
    }

    #endregion

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


    protected void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
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