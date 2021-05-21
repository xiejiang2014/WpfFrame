using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading;
using System.Windows.Input;

namespace WpfFrame.Commands
{
    /// <summary>
    /// ί������.
    /// </summary>
    public abstract class DelegateCommandBase : ICommand
    {
        /// <summary>
        /// ͬ��������
        /// </summary>
        private readonly SynchronizationContext _synchronizationContext;

        /// <summary>
        /// ���캯��
        /// </summary>
        protected DelegateCommandBase()
        {
            _synchronizationContext = SynchronizationContext.Current;
        }

        #region CanExecute

        /// <summary>
        /// ��������Ƿ����ִ��,�����ؽ��.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        /// <summary>
        /// ��������Ƿ����ִ��,�����ؽ��.
        /// </summary>
        /// <param name="parameter"></param>
        protected abstract bool CanExecute(object parameter);

        /// <summary>
        /// �� <see cref="CanExecute"/> ���Է����仯ʱ����
        /// </summary>
        public virtual event EventHandler CanExecuteChanged;

        /// <summary>
        /// ���� <see cref="ICommand.CanExecuteChanged"/> �¼�,�Ա�����������ж��������յ� CanExecute �Ѹı��֪ͨ.
        /// �˷������������߳�ִ��,���Զ��л�����ʵ������Ĵ����߳���ִ��
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        /// ���� <see cref="ICommand.CanExecuteChanged"/> �¼�,�Ա�����������ж��������յ� CanExecute �Ѹı��֪ͨ.
        /// �˷������������߳�ִ��,���Զ��л�����ʵ������Ĵ����߳���ִ��
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
        /// ���ĵ����Ա��ʽ�б�
        /// </summary>
        private readonly HashSet<string> _observedPropertiesExpressions = new HashSet<string>();

        /// <summary>
        /// ����ʵ���� INotifyPropertyChanged �ӿڵ��������,�����յ����Ա��֪ͨʱ,�Զ����� RaiseCanExecuteChanged �� CanExecute ���Խ��и���.
        /// </summary>
        /// <param name="propertyExpression">���Ա��ʽ. ����: ObservesProperty(() => PropertyName).</param>
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
        /// ִ������
        /// </summary>
        /// <param name="parameter">�������</param>
        void ICommand.Execute(object parameter)
        {
            Execute(parameter);
        }

        /// <summary>
        /// ִ������
        /// </summary>
        /// <param name="parameter">�������</param>
        protected abstract void Execute(object parameter);

        #region IsActive

        private bool _isActive;

        /// <summary>
        /// ����/���������Ƿ��ǿ��õ�.
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
        /// �� IsActive ���Ըı�ʱ����.
        /// </summary>
        public virtual event EventHandler IsActiveChanged;

        /// <summary>
        /// ���� IsActiveChanged �¼�.
        /// </summary>
        protected virtual void OnIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion IsActive
    }
}