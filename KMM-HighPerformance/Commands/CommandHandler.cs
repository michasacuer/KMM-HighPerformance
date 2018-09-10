using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KMM_HighPerformance.Commands
{
    class CommandHandler : ICommand
    {
        private Action action;
        private bool canExecute;

        public CommandHandler(Action method, bool CanExecute)
        {
            action = method;
            canExecute = CanExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            action();
        }
    }
}
