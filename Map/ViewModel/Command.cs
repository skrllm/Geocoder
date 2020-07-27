using System;
using System.Windows.Input;

namespace Map.ViewModel
{
    class Command : ICommand
    {
        Action<object> executeMethod;
        Func<object, bool> canexecuteMethod;


        public Command(Action<object> executeMethod, Func<object, bool> canexecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canexecuteMethod = canexecuteMethod;

        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //  throw new NotImplementedException();
            return true;
        }

        public void Execute(object parameter)
        {
            executeMethod(parameter);
        }
    }
}

