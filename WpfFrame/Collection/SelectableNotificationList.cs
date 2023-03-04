using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace WpfFrame.Collection;

public class SelectableNotificationList<T> : NotificationList<T>, ISelectableNotificationList<T>
    where T : class, ISelectableItem
{
    public event EventHandler<SelectionChangedEventArgs>? SelectionChanged;

    public SelectableNotificationList(IEnumerable<T> collection) : base(collection)
    {
    }

    public SelectableNotificationList()
    {
    }

    protected override void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is ISelectableItem selectableItem)
        {
            if (e.PropertyName == nameof(selectableItem.IsSelected))
            {
                if (selectableItem.IsSelected == false)
                {
                    //有任意一项未选,那么肯定不是全选了
                    _isAllSelectedEnable = false;
                    IsAllSelected = false;
                    _isAllSelectedEnable = true;
                }
                else
                {
                    //检查是否全选了  //todo 这里调用是否太过频繁
                    if (this.All(v => v.IsSelected))
                    {
                        _isAllSelectedEnable = false;
                        IsAllSelected = true;
                        _isAllSelectedEnable = true;
                    }


                    //如果是单选状态,那么要将其它所有项都设为未选状态  //todo 这里是否会重复触发多次子对象的事件变化?
                    if (!CanMultipleSelect)
                    {
                        foreach (var item in this)
                        {
                            if (!ReferenceEquals(item, selectableItem))
                            {
                                if (item is not null)
                                {
                                    item.IsSelected = false;
                                }
                            }
                        }
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs(nameof(CountOfSelected)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(AreMultipleSelected)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSingleSelected)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAnySelected)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(FirstSelectedItem)));

                if (SelectionChanged is not null)
                {
                    var selectionChangedEventArgs = new SelectionChangedEventArgs();
                    if (selectableItem.IsSelected)
                    {
                        selectionChangedEventArgs.Selected = new List<ISelectableItem>() { selectableItem };
                    }
                    else
                    {
                        selectionChangedEventArgs.Unselected = new List<ISelectableItem>() { selectableItem };
                    }


                    SelectionChanged?.Invoke(this, selectionChangedEventArgs);
                }
            }
        }
    }


    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        //在单选模式下,如果添加了新项,并且原本已存在选中项,那么新增的项不能是选中的
        if (!CanMultipleSelect &&
            e.NewItems is not null &&
            e.NewItems.Count != 0 &&
            AreMultipleSelected)
        {
            var isFirstSelectedSkipped = false;

            for (var i = 0; i < Count; i++)
            {
                if (this[i].IsSelected)
                {
                    if (isFirstSelectedSkipped)
                    {
                        isFirstSelectedSkipped = true;
                        continue;
                    }

                    this[i].IsSelected = false;
                }
            }
        }

        base.OnCollectionChanged(e);
        OnPropertyChanged(new PropertyChangedEventArgs(nameof(CountOfSelected)));
        OnPropertyChanged(new PropertyChangedEventArgs(nameof(AreMultipleSelected)));
        OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSingleSelected)));
        OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAnySelected)));
        OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAllSelected)));
    }

    /// <summary>
    /// 是否允许多选,默认是
    /// </summary>
    public bool CanMultipleSelect { get; set; } = true;


    /// <summary>
    /// 当前选中项数量
    /// </summary>
    public int CountOfSelected => this.Count(v => v.IsSelected);

    /// <summary>
    /// 当前是否多选
    /// </summary>
    public bool AreMultipleSelected => CountOfSelected > 1;

    /// <summary>
    /// 当前是否单选
    /// </summary>
    public bool IsSingleSelected => CountOfSelected == 1;

    /// <summary>
    /// 当前是否选中了至少1项
    /// </summary>
    public bool IsAnySelected => CountOfSelected >= 1;


    private bool _isAllSelectedEnable = true;

    private bool _isAllSelected;

    public bool IsAllSelected
    {
        get => _isAllSelected;
        set
        {
            _isAllSelected = value;

            if (_isAllSelectedEnable)
            {
                foreach (var item in this)
                {
                    item.IsSelected = value;
                }
            }

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAllSelected)));
        }
    }

    public IEnumerable<T> SelectedItems => this.Where(v => v.IsSelected);

    public T? FirstSelectedItem => this.FirstOrDefault(v => v.IsSelected);
}