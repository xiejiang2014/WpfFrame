using System;
using System.Linq.Expressions;

namespace WpfFrame.Commands
{
    /// <summary>
    /// 委托命令.
    /// </summary>
    /// <see cref="DelegateCommandBase"/>
    /// <see cref="DelegateCommand{T}"/>
    public class DelegateCommand : DelegateCommandBase
    {
        private readonly Action _executeMethod;
        private Func<bool> _canExecuteMethod;

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="executeMethod">被委托执行的方法.</param>
        public DelegateCommand(Action executeMethod)
            : this(executeMethod, () => true)
        {
        }

        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="executeMethod">被委托执行的方法.</param>
        /// <param name="canExecuteMethod">判断被委托执行的方法是否可用的方法</param>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _executeMethod = executeMethod ??
                             throw new ArgumentNullException(nameof(executeMethod));

            _canExecuteMethod = canExecuteMethod;
        }

        #region Execute

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameter">命令参数</param>
        protected override void Execute(object parameter)
        {
            Execute();
        }

        ///<summary>
        /// 执行命令
        ///</summary>
        public void Execute()
        {
            _executeMethod();
        }

        #endregion Execute

        #region CanExecute

        /// <summary>
        /// 检测命令是否可以执行,并返回结果.
        /// </summary>
        /// <param name="parameter">检测参数.</param>
        protected override bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        /// <summary>
        /// 检测命令是否可以执行,并返回结果.
        /// </summary>
        public bool CanExecute()
        {
            return _canExecuteMethod == null || _canExecuteMethod();
        }

        /// <summary>
        /// 订阅实现了 INotifyPropertyChanged 接口的类的属性,并在收到属性变更通知时,自动调用 RaiseCanExecuteChanged 对 CanExecute 属性进行更新.
        /// </summary>
        /// <param name="propertyExpression">属性表达式. 例如: ObservesProperty(() => PropertyName).</param>
        /// <returns>当前 DelegateCommand 的实例</returns>
        public DelegateCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
        {
            ObservesPropertyInternal(propertyExpression);
            return this;
        }

        /// <summary>
        /// 订阅能够决定命令能否执行的属性.并且,如果该属性实现了 INotifyPropertyChanged,那么会自动调用 RaiseCanExecuteChanged 对 CanExecute 属性进行更新.
        /// Observes a property that is used to determine if this command can execute,
        /// </summary>
        /// <param name="canExecuteExpression">属性表达式. 例如: ObservesProperty(() => PropertyName).</param>
        /// <returns>当前 DelegateCommand 的实例</returns>
        public DelegateCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            _canExecuteMethod = canExecuteExpression.Compile();
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }

        #endregion CanExecute
    }
}