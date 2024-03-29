﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Xiejiang.DataGridEx.WPF.Sample
{
    public class DataItem
    {
        #region ColumnOrder

        //使用 DataGridColumnOrderAttribute 设置自动生成列的顺序
        //Use the DataGridColumnOrderAttribute to set the order in which columns are automatically generated

        [DataGridColumnOrder(3)]
        public string ColumnA { get; set; }
        
        [DataGridColumnOrder(2)]
        public bool ColumnB { get; set; }

        [DataGridColumnOrder(1)]
        public DateTime ColumnC { get; set; }

        #endregion ColumnOrder

        //使用 DisplayNameAttribute 设置自动生成列的显示名称
        //Use DisplayNameAttribute to set the display name(Header) of the automatically generated column
        [DisplayName("The DisplayName :)")]
        public string ColumnD { get; set; }

        //使用 DoNotAutoGenerateDataGridColumnAttribute 阻止自动生成列
        //Block auto generate column with DoNotAutoGenerateDataGridColumnAttribute
        [DoNotAutoGenerateDataGridColumn]
        public string ColumnE { get; set; }
    }


    public class DemoViewModel
    {


        public List<DataItem> DemoList { get; set; } = new()
                                                       {
                                                           new DataItem()
                                                           {
                                                               ColumnA = "DemoItem1",
                                                               ColumnB = true,
                                                               ColumnC = DateTime.Now,
                                                               ColumnD = "text",
                                                               ColumnE = "text"
                                                           }
                                                       };
    }
}