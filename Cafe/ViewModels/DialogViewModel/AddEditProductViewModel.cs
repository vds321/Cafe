using Cafe.ViewModels.Base;
using Storage.Entities;

namespace Cafe.ViewModels.DialogViewModel
{
    internal class AddEditProductViewModel : ViewModel
    {

        #region Название окна

        ///<summary>Название окна</summary>
        public string Title { get; }

        #endregion

        #region Название продукта

        private string _Name;

        ///<summary>Название продукта</summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        #endregion

        #region Остаток на складе

        private double _Remain;

        ///<summary>Остаток на складе</summary>
        public double Remain
        {
            get => _Remain;
            set => Set(ref _Remain, value);
        }

        #endregion
        public AddEditProductViewModel(Product product)
        {
            Title = product.Name == null ? "Добавить продукт" : "Редактировать продукт";
            Name = product?.Name;
            Remain = product.Remains;
        }
    }
}
