using System;
using System.Windows;

namespace Xiejiang.DataGridEx.WPF
{
    /// <summary>
    /// 指定属性在System.Windows.Controls.DataGrid中自动生成列时,文本内容的水平对齐方式
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DataGridColumnHorizontalAlignmentAttribute : Attribute
    {
        public HorizontalAlignment HorizontalAlignment { get; set; }

        public DataGridColumnHorizontalAlignmentAttribute(HorizontalAlignment horizontalAlignment)
        {
            HorizontalAlignment = horizontalAlignment;
        }
    }
}