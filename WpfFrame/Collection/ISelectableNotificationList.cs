using System.Collections.Generic;
using System;

namespace WpfFrame.Collection;

public interface ISelectableNotificationList
{
    /// <summary>
    /// 是否允许多选,默认是
    /// </summary>
    bool CanMultipleSelect { get; set; }

    /// <summary>
    /// 当前选中项数量
    /// </summary>
    int CountOfSelected { get; }

    /// <summary>
    /// 当前是否多选
    /// </summary>
    bool AreMultipleSelected { get; }

    /// <summary>
    /// 当前是否单选
    /// </summary>
    bool IsSingleSelected { get; }

    /// <summary>
    /// 当前是否选中了至少1项
    /// </summary>
    bool IsAnySelected { get; }

    /// <summary>
    /// 当前是否所有的项都被选中了
    /// </summary>
    bool IsAllSelected { get; set; }

    /// <summary>
    /// 是否有任何项
    /// </summary>
    bool AnyItem { get; }

    /// <summary>
    /// 项总数
    /// </summary>
    int Count { get; }
}

public interface ISelectableNotificationList<T> : ISelectableNotificationList
{
    /// <summary>
    /// 列表中对象属性改变时激活.
    /// </summary>
    public event EventHandler<ItemPropertyChangedEventArgs<T>> ItemPropertyChanged;

    IEnumerable<T> SelectedItems { get; }

    T? FirstSelectedItem { get; }
}