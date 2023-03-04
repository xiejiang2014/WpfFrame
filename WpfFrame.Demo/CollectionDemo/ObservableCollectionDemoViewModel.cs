using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using WpfFrame.Collection;

namespace WpfFrame.Demo.CollectionDemo
{
    internal class ObservableCollectionDemoViewModel : BindableBase
    {
        public ObservableCollection<SelectableItem> Items { get; set; } = new();


        #region 添加6个(逐个)

        private DelegateCommand? _add6Command;
        public  DelegateCommand  Add6Command => _add6Command ??= new DelegateCommand(Add6);

        private void Add6()
        {
            for (int i = 0; i < 6; i++)
            {
                Items.Add(SelectableItem.GetNew());
            }
        }

        #endregion

        #region 添加6个(批量)

        private DelegateCommand? _addRange6Command;
        public  DelegateCommand  AddRange6Command => _addRange6Command ??= new DelegateCommand(AddRange6);

        private void AddRange6()
        {
            var newItems = Enumerable
                          .Range(1, 6)
                          .Select(v => SelectableItem.GetNew())
                          .ToArray();

            Items.AddRange(newItems);
        }

        #endregion
    }
}