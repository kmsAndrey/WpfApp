using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AksenovNewTeleTeth.ViewModels
{
        public class UserCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            private Action _execute;

            public UserCommand(Action execute)
            {
                _execute = execute;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                _execute.Invoke();
            }
        }
    }
