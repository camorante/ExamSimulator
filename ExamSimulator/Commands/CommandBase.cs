using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExamSimulator.Commands
{
    public class CommandBase : ICommand
    {
        private Action<object> execAction;
        private Func<object, bool> canExecFunc;

        public CommandBase(Action<object> execAction)
        {
            this.execAction = execAction;
            this.canExecFunc = null;
        }//Fin de constructor.

        public CommandBase(Action<object> execAction, Func<object, bool> canExecFunc)
        {
            this.execAction = execAction;
            this.canExecFunc = canExecFunc;
        }//Fin de constructor.

        public bool CanExecute(object parameter)
        {
            if (canExecFunc != null)
            {
                return canExecFunc.Invoke(parameter);
            }//Fin de if.
            else
            {
                return true;
            }//Fin de else.
        }//Fin de CanExecute.

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (execAction != null)
            {
                execAction.Invoke(parameter);
            }//Fin de if.
        }//Fin de Execute.
    }
}
