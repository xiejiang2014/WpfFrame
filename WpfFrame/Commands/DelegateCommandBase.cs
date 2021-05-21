using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading;
using System.Windows.Input;

namespace WpfFrame.Commands
{
    /// <summary>
    /// 委托命令.
    /// </summary>
    public abstract class DelegateCommandBase : ICommand
    {
        /// <summary>
        /// 同步上下文
        /// </summary>
        private readonly SynchronizationContext _synchronizationContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected DelegateCommandBase()
        {
            _synchronizationContext = SynchronizationContext.Current;
        }

        #region CanExecute

        /// <summary>
        /// 检测命令是否可以执行,并返回结果.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        /// <summary>
        /// 检测命令是否可以执行,并返回结果.
        /// </summary>
        /// <param name="parameter"></param>
        protected abstract bool CanExecute(object parameter);

        /// <summary>
        /// 当 <see cref="CanExecute"/> 属性发生变化时触发
        /// </summary>
        public virtual event EventHandler CanExecuteChanged;

        /// <summary>
        /// 触发 <see cref="ICommand.CanExecuteChanged"/> 事件,以便让命令的所有订阅者能收到 CanExecute 已改变的通知.
        /// 此方法可在任意线程执行,会自动切换到本实例对象的创建线程上执行
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        /// 触发 <see cref="ICommand.CanExecuteChanged"/> 事件,以便让命令的所有订阅者能收到 CanExecute 已改变的通知.
        /// 此方法可在任意线程执行,会自动切换到本实例对象的创建线程上执行
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                if (_synchronizationContext != null && _synchronizationContext != SynchronizationContext.Current)
                {
                    _synchronizationContext.Post(o => handler.Invoke(this, EventArgs.Empty), null);
                }
                else
                {
                    handler.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 订阅的属性表达式列表
        /// </summary>
        private readonly HashSet<string> _observedPropertiesExpressions = new HashSet<string>();

        /// <summary>
        /// 订阅实现了 INotifyPropertyChanged 接口的类的属性,并在收到属性变更通知时,自动调用 RaiseCanExecuteChanged 对 CanExecute 属性进行更新.
        /// </summary>
        /// <param name="propertyExpression">属性表达式. 例如: ObservesProperty(() => PropertyName).</param>
        protected internal void ObservesPropertyInternal<T>(Expression<Func<T>> propertyExpression)
        {
            if (_observedPropertiesExpressions.Contains(propertyExpression.ToString()))
            {
                throw new ArgumentException(
                    $@"{propertyExpression} is already being observed.",
                    nameof(propertyExpression)
                );
            }

            _observedPropertiesExpressions.Add(propertyExpression.ToString());
            PropertyObserver.Observes(propertyExpression, RaiseCanExecuteChanged);
        }

        #endregion CanExecute

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameter">命令参数</param>
        void ICommand.Execute(object parameter)
        {
            Execute(parameter);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="parameter">命令参数</param>
        protected abstract void Execute(object parameter);

        #region IsActive

        private bool _isActive;

        /// <summary>
        /// 设置/返回命令是否是可用的.
        /// </summary>
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive == value) return;
                _isActive = value;
                OnIsActiveChanged();
            }
        }

        /// <summary>
        /// 当 IsActive 属性改变时触发.
        /// </summary>
        public virtual event EventHandler IsActiveChanged;

        /// <summary>
        /// 触发 IsActiveChanged 事件.
        /// </summary>
        protected virtual void OnIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion IsActive
    }
}