using Cafe.Infrastructure.Command.Base;
using Cafe.Service.Interface;
using Cafe.ViewModels.Base;
using Storage.Entities;
using Storage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Cafe.ViewModels.MainViewModels
{
    internal class WarehouseViewModel : ViewModel
    {
        private readonly IRepository<Product> _Repository;
        private readonly IUserDialog _UserDialog;

        #region Свойства

        #region Список продуктов

        private List<Product> _Products;

        ///<summary>Список продуктов</summary>
        public List<Product> Products
        {
            get => _Products;
            set => Set(ref _Products, value);
        }

        #endregion

        #region Выбранный продукт

        private Product _SelectedProduct;

        ///<summary>Выбранный продукт</summary>
        public Product SelectedProduct
        {
            get => _SelectedProduct;
            set
            {
                Set(ref _SelectedProduct, value);
                SelectedProductRemains = SelectedProduct?.Remains ?? 0;
                SelectedProductName = SelectedProduct?.Name;
                OnPropertyChanged(nameof(SelectedProductName));
                OnPropertyChanged(nameof(SelectedProductRemains));
            }
        }

        private string _SelectedProductName;
        public string SelectedProductName
        {
            get => _SelectedProductName;
            set => Set(ref _SelectedProductName, value);
        }


        private double _SelectedProductRemains;
        public double SelectedProductRemains
        {
            get => _SelectedProductRemains;
            set => Set(ref _SelectedProductRemains, value);
        }

        #endregion

        #endregion

        #region Команды

        #region Command ChangeRemainCommand


        /// <summary>Изменить запас выбранного продукта</summary> 
        private ICommand _ChangeRemainCommand;

        /// <summary>Изменить запас выбранного продукта</summary>

        public ICommand ChangeRemainCommand => _ChangeRemainCommand
            ??= new LambdaCommand(OnChangeRemainCommandExecuted, CanChangeRemainCommandExecute);


        /// <summary>Проверка возможности выполнения Изменить запас выбранного продукта</summary>
        private bool CanChangeRemainCommandExecute() => SelectedProduct != null && Math.Abs(SelectedProduct.Remains - SelectedProductRemains) > 0.01;

        /// <summary>Логика выполнения Изменить запас выбранного продукта</summary>
        private void OnChangeRemainCommandExecuted()
        {
            SelectedProduct.Remains = Math.Round(SelectedProductRemains, 1);
            _Repository.Update(SelectedProduct);
            Products = _Repository.Items.ToList();
            OnPropertyChanged(nameof(Products));
        }


        #endregion

        #region Command AddNewProductCommand


        /// <summary>Добавить новый продукт</summary> 
        private ICommand _AddNewProductCommand;

        /// <summary>Добавить новый продукт</summary>

        public ICommand AddNewProductCommand => _AddNewProductCommand
            ??= new LambdaCommand(OnAddNewProductCommandExecuted, CanAddNewProductCommandExecute);


        /// <summary>Проверка возможности выполнения Добавить новый продукт</summary>
        private bool CanAddNewProductCommandExecute() => true;

        /// <summary>Логика выполнения Добавить новый продукт</summary>
        private void OnAddNewProductCommandExecuted()
        {
            var new_product = new Product();
            if (!_UserDialog.EditProduct(new_product)) return;
            if (_Repository.Items.Select(p => p.Name.Equals(new_product.Name)).Count() != 0)
            {
                _UserDialog.MessageError($"Данное название продукта \"{new_product.Name}\" уже существует.", "Ошибка данных");
                return;
            }
            _Repository.Add(new_product);
            Products = _Repository.Items.ToList();
            SelectedProductRemains = new_product.Remains;
            SelectedProductName = new_product.Name;
            OnPropertyChanged(nameof(Products));
            OnPropertyChanged(nameof(SelectedProductName));
            OnPropertyChanged(nameof(SelectedProductRemains));
        }

        #endregion

        #region Command EditSelectedProductCommand

        /// <summary>Редактировать выбранный продукт</summary> 
        private ICommand _EditSelectedProductCommand;

        /// <summary>Редактировать выбранный продукт</summary>

        public ICommand EditSelectedProductCommand => _EditSelectedProductCommand
            ??= new LambdaCommand(OnEditSelectedProductCommandExecuted, CanEditSelectedProductCommandExecute);


        /// <summary>Проверка возможности выполнения Редактировать выбранный продукт</summary>
        private bool CanEditSelectedProductCommandExecute() => SelectedProduct != null;

        /// <summary>Логика выполнения Редактировать выбранный продукт</summary>
        private void OnEditSelectedProductCommandExecuted()
        {
            var old_Name = SelectedProduct.Name;
            if (!_UserDialog.EditProduct(SelectedProduct)) return;
            var new_Name = SelectedProduct.Name;
            if (!old_Name.Equals(new_Name) && _Repository.Items.Select(p => p.Name.Equals(new_Name)).Count() != 0)
            {
                _UserDialog.MessageError($"Данное название продукта \"{new_Name}\" уже существует.", "Ошибка данных");
                return;
            }
            _Repository.Update(SelectedProduct);
            Products = _Repository.Items.ToList();
            SelectedProductRemains = SelectedProduct.Remains;
            SelectedProductName = SelectedProduct.Name;
            OnPropertyChanged(nameof(Products));
            OnPropertyChanged(nameof(SelectedProductName));
            OnPropertyChanged(nameof(SelectedProductRemains));
        }


        #endregion

        #region Command RemoveProductCommand


        /// <summary>Удалить продукт</summary> 
        private ICommand _RemoveProductCommand;

        /// <summary>Удалить продукт</summary>

        public ICommand RemoveProductCommand => _RemoveProductCommand
            ??= new LambdaCommand(OnRemoveProductCommandExecuted, CanRemoveProductCommandExecute);


        /// <summary>Проверка возможности выполнения Удалить продукт</summary>
        private bool CanRemoveProductCommandExecute() => SelectedProduct != null && SelectedProduct.Composition.Count == 0;

        /// <summary>Логика выполнения Удалить продукт</summary>
        private void OnRemoveProductCommandExecuted()
        {
            var id = SelectedProduct.Id;
            var result = _UserDialog.ConfirmWarning($"Вы уверены, что хотите удалить \"{SelectedProduct.Name}\"?", "Внимание!");
            if (!result) return;
            _Repository.Remove(id);
            Products = _Repository.Items.ToList();
            OnPropertyChanged(nameof(Products));
        }


        #endregion

        #endregion

        public WarehouseViewModel(IRepository<Product> repository, IUserDialog userDialog)
        {
            _Repository = repository;
            _UserDialog = userDialog;
            Products = _Repository.Items.OrderBy(x => x.Name).ToList();
        }
    }
}
