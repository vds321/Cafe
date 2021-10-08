using Cafe.Infrastructure.Command.Base;
using System.Windows;

namespace Cafe.Infrastructure.Command
{
    internal class CloseCommand : CommandBase
    {

        #region Overrides of LambdaCommand

        protected override bool CanExecute(object p) => true;
        #endregion

        #region Overrides of LambdaCommand

        protected override void Execute(object p) => (p as Window ?? App.FocusedWindow ?? App.ActivedWindow ?? Application.Current.MainWindow)?.Close();

        #endregion


    }
}