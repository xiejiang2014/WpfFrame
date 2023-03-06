using System.ComponentModel;
using System.Diagnostics;
using WpfFrame.Collection;

namespace WpfFrame.Demo;

public class SelectableItem : ISelectableItem
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private SelectableItem()
    {
    }

    public bool IsSelected { get; set; }

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