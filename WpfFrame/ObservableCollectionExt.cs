using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfFrame
{
    public static class ObservableCollectionExt
    {
        public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                observableCollection.Add(item);
            }
        }
    }
}