using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using System.Xaml;

namespace Cafe.ViewModels.Base
{
	internal abstract class ViewModel : MarkupExtension, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(field, value)) return false;
			field = value;
			OnPropertyChanged();
			return true;
		}

		#region Overrides of MarkupExtension

		public override object ProvideValue(IServiceProvider sp)
		{
			var value_target_service = sp.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
			var root_object_service = sp.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;

			OnInitialized(
				value_target_service?.TargetObject,
				value_target_service?.TargetProperty,
				root_object_service?.RootObject);

			return this;

		}

		#endregion
		private WeakReference _TargetRef;
		private WeakReference _RootRef;

		public object TargetObject => _TargetRef.Target;

		public object RootObject => _RootRef.Target;

		protected virtual void OnInitialized(object Target, object Property, object Root)
		{
			_TargetRef = new WeakReference(Target);
			_RootRef = new WeakReference(Root);
		}
	}
}
