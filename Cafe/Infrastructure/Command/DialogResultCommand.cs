using Cafe.Infrastructure.Command.Base;
using System;

namespace Cafe.Infrastructure.Command
{
	class DialogResultCommand : CommandBase
	{
		#region Overrides of CommandBase

		private bool? DialogResult { get; set; }
		protected override void Execute(object parameter)
		{
			var window = App.CurrentWindow;

			var dialog_result = DialogResult;
			if (parameter != null)
				dialog_result = Convert.ToBoolean(parameter);

			window.DialogResult = dialog_result;
			window.Close();
		}

		#endregion

		#region Overrides of CommandBase

		protected override bool CanExecute(object parameter) => App.CurrentWindow != null;

		#endregion
	}
}
