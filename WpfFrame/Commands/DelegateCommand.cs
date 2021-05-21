using System;
using System.Linq.Expressions;

namespace WpfFrame.Commands
{
    /// <summary>
    /// ί������.
    /// </summary>
    /// <see cref="DelegateCommandBase"/>
    /// <see cref="DelegateCommand{T}"/>
    public class DelegateCommand : DelegateCommandBase
    {
        private readonly Action _executeMethod;
        private Func<bool> _canExecuteMethod;

        /// <summary>
        /// ���캯��.
        /// </summary>
        /// <param name="executeMethod">��ί��ִ�еķ���.</param>
        public DelegateCommand(Action executeMethod)
            : this(executeMethod, () => true)
        {
        }

        /// <summary>
        /// ���캯��.
        /// </summary>
        /// <param name="executeMethod">��ί��ִ�еķ���.</param>
        /// <param name="canExecuteMethod">�жϱ�ί��ִ�еķ����Ƿ���õķ���</param>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _executeMethod = executeMethod ??
                             throw new ArgumentNullException(nameof(executeMethod));

            _canExecuteMethod = canExecuteMethod;
        }

        #region Execute

        /// <summary>
        /// ִ������
        /// </summary>
        /// <param name="parameter">�������</param>
        protected override void Execute(object parameter)
        {
            Execute();
        }

        ///<summary>
        /// ִ������
        ///</summary>
        public void Execute()
        {
            _executeMethod();
        }

        #endregion Execute

        #region CanExecute

        /// <summary>
        /// ��������Ƿ����ִ��,�����ؽ��.
        /// </summary>
        /// <param name="parameter">������.</param>
        protected override bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        /// <summary>
        /// ��������Ƿ����ִ��,�����ؽ��.
        /// </summary>
        public bool CanExecute()
        {
            return _canExecuteMethod == null || _canExecuteMethod();
        }

        /// <summary>
        /// ����ʵ���� INotifyPropertyChanged �ӿڵ��������,�����յ����Ա��֪ͨʱ,�Զ����� RaiseCanExecuteChanged �� CanExecute ���Խ��и���.
        /// </summary>
        /// <param name="propertyExpression">���Ա��ʽ. ����: ObservesProperty(() => PropertyName).</param>
        /// <returns>��ǰ DelegateCommand ��ʵ��</returns>
        public DelegateCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
        {
            ObservesPropertyInternal(propertyExpression);
            return this;
        }

        /// <summary>
        /// �����ܹ����������ܷ�ִ�е�����.����,���������ʵ���� INotifyPropertyChanged,��ô���Զ����� RaiseCanExecuteChanged �� CanExecute ���Խ��и���.
        /// Observes a property that is used to determine if this command can execute,
        /// </summary>
        /// <param name="canExecuteExpression">���Ա��ʽ. ����: ObservesProperty(() => PropertyName).</param>
        /// <returns>��ǰ DelegateCommand ��ʵ��</returns>
        public DelegateCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            _canExecuteMethod = canExecuteExpression.Compile();
            ObservesPropertyInternal(canExecuteExpression);
            return this;
        }

        #endregion CanExecute
    }
}