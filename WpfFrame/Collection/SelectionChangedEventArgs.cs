using System;
using System.Collections.Generic;

namespace WpfFrame.Collection;

/// <summary>
/// 选择更改事件参数
/// </summary>
public class SelectionChangedEventArgs : EventArgs
{
    /// <summary>
    /// 在本次选中变化事件中,新的被选中项
    /// </summary>
    public List<ISelectableItem>? Selected { get; set; }

    /// <summary>
    /// 在本次选中变化事件中,新的未被选中项
    /// </summary>
    public List<ISelectableItem>? Unselected { get; set; }
}