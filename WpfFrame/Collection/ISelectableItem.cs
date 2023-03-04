using System.ComponentModel;

namespace WpfFrame.Collection;

public interface ISelectableItem : INotifyPropertyChanged
{
    bool IsSelected { get; set; }
}