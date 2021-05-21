using System;

namespace WpfFrame.DataGrid
{
    /// <summary>
    /// 指定属性在System.Windows.Controls.DataGrid中自动生成列时,列的顺序
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DataGridColumnOrderAttribute : Attribute
    {
        public static readonly DataGridColumnOrderAttribute Default = new DataGridColumnOrderAttribute(0);

        private int _order;

        public DataGridColumnOrderAttribute(int order)
        {
            _order = order;
        }

        public virtual int DataGridColumnOrder => _order;

        protected int DataGridColumnOrderValue
        {
            get => _order;
            set => _order = value;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            return obj is DataGridColumnOrderAttribute other && other.DataGridColumnOrder == DataGridColumnOrder;
        }

        public override int GetHashCode()
        {
            return DataGridColumnOrder.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return Equals(Default);
        }
    }
}