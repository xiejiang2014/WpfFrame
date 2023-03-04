using System.ComponentModel;
using WpfFrame.Collection;

namespace WpfFrame.Demo;

public class SelectableItem : ISelectableItem
{
    private bool                              _isSelected;


    public event PropertyChangedEventHandler? PropertyChanged;

    private SelectableItem()
    {
    }

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