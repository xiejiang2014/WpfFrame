////using System;
////using System.Windows.Input;

////namespace WpfFrame
////{
////    public class DelegateCommand : ICommand
////    {
////        public DelegateCommand()
////        {
////        }

////        public DelegateCommand(Action<object> executeAction)
////        {
////            ExecuteAction = executeAction;
////        }

////        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteFunc) : this(executeAction)
////        {
////            CanExecuteFunc = canExecuteFunc;
////        }

////        public event EventHandler CanExecuteChanged;

////        public void OnCanExecuteChanged()
////        {
////            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
////        }

////        public Action<object> ExecuteAction { get; set; }

////        public Func<object, bool> CanExecuteFunc { get; set; }

////        public void Execute(object parameter)
////        {
////            ExecuteAction?.Invoke(parameter);
////        }

////        public bool CanExecute(object parameter)
////        {
////            return CanExecuteFunc == null || CanExecuteFunc(parameter);
////        }
////    }
////}