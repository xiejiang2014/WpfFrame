using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WpfFrame.Commands
{
    /// <summary>
    /// 委托命令.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateCommand<T> : DelegateCommandBase
    {
        private readonly Action<T> _executeMethod;
        private Func<T, bool> _canExecuteMethod;

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="executeMethod">被委托执行的方法.</param>
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, o => true)
        {
        }

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="executeMethod">被委托执行的方法.</param>
        /// <param name="canExecuteMethod">判断被委托执行的方法是否可用的方法.</param>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            if (executeMethod == null)
                throw new ArgumentNullException(nameof(executeMethod));

            TypeInfo genericTypeInfo = typeof(T).GetTypeInfo();

            // DelegateCommand allows object or Nullable<>.
            // note: Nullable<> is a struct so we cannot use a class constraint.
            if (genericTypeInfo.IsValueType)
            {
                if (!genericTypeInfo.IsGenericType ||
                    !typeof(Nullable<>).GetTypeInfo().IsAssignableFrom(genericTypeInfo.GetGenericTypeDefinition().GetTypeInfo()))
                {
                    throw new InvalidCastException($"无效的 DelegateCommand<T> 泛型类型 {genericTypeInfo.Name}");
                }
            }

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        #region Execute

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameter">命令参数</param>
        protected override void Execute(object parameter)
        {
            Execute((T)parameter);
        }

        ///<summary>
        ///执行命令.
        ///</summary>
        ///<param name="parameter">命令参数.</param>
        public void Execute(T parameter)
        {
            _executeMethod(parameter);
        }

        #endregion Execute

        #region CanExecute

        ///<summary>
        ///检测命令是否可以执行,并返回结果.
        ///</summary>
        ///<param name="parameter">检测参数</param>
        protected override bool CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        ///<summary>
        ///检测命令是否可以执行,并返回结果.
        ///</summary>
        ///<param name="parameter">检测参数</param>
        ///<returns>
        ///</returns>
        public bool CanExecute(T parameter)
        {
            return _canExecuteMethod(parameter);
        }

        /// <summary>
        /// 订阅实现了 INotifyPropertyChanged 接口的类的属性,并在收到属性变更通知时,自动调用 RaiseCanExecuteChanged 对 CanExecute 属性进行更新.
        /// </summary>
        /// <param name="propertyExpression">属性表达式. 例如: ObservesProperty(() => PropertyName).</param>
        /// <returns>当前 DelegateCommand 的实例</returns>
        public DelegateCommand<T> ObservesProperty<TType>(Expression<Func<TType>> propertyExpression)
        {
            ObservesPropertyInternal(propertyExpression);
            return this;
        }

        /// <summary>
        /// 订阅能够决定命令能否执行的属性.并且,如果该属性实现了 INotifyPropertyChanged,那么会自动调用 RaiseCanExecuteChanged 对 CanExecute 属性进行更新.
        /// </summary>
        /// <param name="canExecuteExpression">属性表达式. 例如: ObservesProperty(() => PropertyName).</param>
        /// <returns>当前 DelegateCommand 的实例</returns>
        public DelegateCommand<T> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(canExecuteExpression.Body, Expression.Parameter(typeof(T), "o"));
            _canExecuteMethod = expression.Compile();
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }

        #endregion CanExecute
    }
}