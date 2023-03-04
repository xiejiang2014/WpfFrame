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

    private bool _isInBatchOperation;

    /// <summary>
    /// 全选
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public void SelectAll()
    {
        if (!CanMultipleSelect)
        {
            throw new InvalidOperationException($"仅多选模式下才能使用 SelectAll() 方法,请设置 {nameof(CanMultipleSelect)} 为 true");
        }

        if (!this.Any())
        {
            return;
        }

        var anyChanged          = false;
        var areMultipleSelected = AreMultipleSelected;
        var isAnySelected       = IsAnySelected;
        var firstSelectedItem   = FirstSelectedItem;

        var selectionChangedEventArgs = SelectionChanged is null
            ? null
            : new SelectionChangedEventArgs
              {
                  Selected = new List<ISelectableItem>()
              };


        try
        {
            _isInBatchOperation = true;

            foreach (var selectableItem in this.Where(v => !v.IsSelected))
            {
                selectableItem.IsSelected = true;

                if (selectionChangedEventArgs is not null)
                {
                    selectionChangedEventArgs.Selected!.Add(selectableItem);
                }

                anyChanged = true;
            }
        }
        finally
        {
            _isInBatchOperation = false;
        }

        if (anyChanged)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAllSelected)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(CountOfSelected)));

            if (areMultipleSelected != AreMultipleSelected)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(AreMultipleSelected)));
            }

            if (isAnySelected != IsAnySelected)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAnySelected)));
            }

            if (!ReferenceEquals(firstSelectedItem, FirstSelectedItem))
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(FirstSelectedItem)));
            }


            SelectionChanged?.Invoke(this, selectionChangedEventArgs!);
        }
    }

    /// <summary>
    /// 全不选
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public void UnselectAll()
    {
        if (!this.Any())
        {
            return;
        }

        var anyChanged          = false;
        var areMultipleSelected = AreMultipleSelected;

        var selectionChangedEventArgs = SelectionChanged is null
            ? null
            : new SelectionChangedEventArgs
              {
                  Unselected = new List<ISelectableItem>()
              };

        try
        {
            _isInBatchOperation = true;

            foreach (var selectableItem in this.Where(v => v.IsSelected))
            {
                selectableItem.IsSelected = false;

                if (selectionChangedEventArgs is not null)
                {
                    selectionChangedEventArgs.Unselected!.Add(selectableItem);
                }

                anyChanged = true;
            }
        }
        finally
        {
            _isInBatchOperation = false;
        }

        if (anyChanged)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAllSelected)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(CountOfSelected)));

            if (areMultipleSelected != AreMultipleSelected)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(AreMultipleSelected)));
            }
            
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAnySelected)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(FirstSelectedItem)));
  
            SelectionChanged?.Invoke(this, selectionChangedEventArgs!);
        }
    }


    protected override void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        //todo 如果有10W+项 要进行一次全选  这里会运行多少次?

        if (!_isInBatchOperation && sender is ISelectableItem selectableItem)
        {
            if (e.PropertyName == nameof(selectableItem.IsSelected))
            {
                Debug.Print("SelectableNotificationList.OnItemPropertyChanged");

                if (selectableItem.IsSelected == false)
                {
                    if (_isAllSelected)
                    {
                        //有任意一项未选,那么肯定不是全选了
                        _isAllSelected = false;
                        OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAllSelected)));
                    }
                }
                else
                {
                    //检查是否全选了  //todo 这里调用是否太过频繁
                    if (this.All(v => v.IsSelected))
                    {
                        if (!_isAllSelected)
                        {
                            _isAllSelected = true;
                            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAllSelected)));
                        }
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
        if (!CanMultipleSelect     &&
            e.NewItems is not null &&
            e.NewItems.Count != 0  &&
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
    public bool AreMultipleSelected => this.Where(v => v.IsSelected)
                                           .Take(2)
                                           .Count() > 1;

    /// <summary>
    /// 当前是否单选
    /// </summary>
    public bool IsSingleSelected => CountOfSelected == 1;

    /// <summary>
    /// 当前是否选中了至少1项
    /// </summary>
    public bool IsAnySelected => this.Any(v => v.IsSelected);


    private bool _isAllSelected;

    public bool IsAllSelected
    {
        get => _isAllSelected;
        set
        {
            _isAllSelected = value;

            if (value)
            {
                SelectAll();
            }
            else
            {
                UnselectAll();
            }
        }
    }

    public IEnumerable<T> SelectedItems => this.Where(v => v.IsSelected);

    public T? FirstSelectedItem => this.FirstOrDefault(v => v.IsSelected);
}