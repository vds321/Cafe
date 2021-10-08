using Cafe.Infrastructure.Command.Base;
using Cafe.ViewModels.Base;
using Storage.Entities;
using Storage.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Cafe.ViewModels.MainViewModels
{
    class ReportViewModel : ViewModel
    {
        private readonly IRepository<Category> _Category;
        private readonly IRepository<Product> _Product;
        private readonly IRepository<Dish> _Dishes;
        private readonly IRepository<Composition> _Compositions;
        private readonly IRepository<Order> _Order;

        #region Начальная дата

        private DateTime? _StartDate;

        ///<summary>Начальная дата</summary>
        public DateTime? StartDate
        {
            get => _StartDate;
            set => Set(ref _StartDate, value);
        }

        #endregion

        #region Конечная дата

        private DateTime? _StopDate;

        ///<summary>Конечная дата</summary>
        public DateTime? StopDate
        {
            get => _StopDate;
            set => Set(ref _StopDate, value);
        }

        #endregion

        #region Выбранная категория

        private string _SelectedCategory;

        ///<summary>Выбранная категория</summary>
        public string SelectedCategory
        {
            get => _SelectedCategory;
            set => Set(ref _SelectedCategory, value);
        }

        #endregion

        #region Выбранные продукты

        private string _SelectedProducts;

        ///<summary>Выбранные продукты</summary>
        public string SelectedProducts
        {
            get => _SelectedProducts;
            set => Set(ref _SelectedProducts, value);
        }

        #endregion

        #region Список продуктов

        private List<string> _Products;

        ///<summary>Список продуктов</summary>
        public List<string> Products
        {
            get => _Products;
            set => Set(ref _Products, value);
        }

        #endregion

        #region Список категорий

        private List<string> _Categories;

        ///<summary>Список категорий</summary>
        public List<string> Categories
        {
            get => _Categories;
            set => Set(ref _Categories, value);
        }

        #endregion

        #region Свойства и команды для отчета

        #region Список имен блюд для отчета 1

        private ObservableCollection<string> _DishNames_Report1;

        ///<summary>Список имен блюд для отчета 1</summary>
        public ObservableCollection<string> DishNames_Report1
        {
            get => _DishNames_Report1;
            set => Set(ref _DishNames_Report1, value);
        }

        #endregion

        #region Command Report1RunCommand : Обновление отчета 1


        /// <summary>Обновление отчета 1</summary> 
        private ICommand _Report1RunCommand;

        /// <summary>Обновление отчета 1</summary>

        public ICommand Report1RunCommand => _Report1RunCommand
            ??= new LambdaCommand(OnReport1RunCommandExecuted, CanReport1RunCommandExecute);


        /// <summary>Проверка возможности выполнения Обновление отчета 1</summary>
        private bool CanReport1RunCommandExecute() => SelectedProducts.Length > 0;

        /// <summary>Логика выполнения Обновление отчета 1</summary>
        private void OnReport1RunCommandExecuted()
        {
            DishNames_Report1.Clear();
            var prodList = new List<string>();
            SelectedProducts.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(x =>
            {
                prodList.Add(x.Trim());
            });

            var prodListCount = prodList.Count;
            var tempResult = new List<string>();
            foreach (string prod in prodList)
            {
                var dishNames = _Dishes.Items.Where(d => _Compositions.Items
                        .Where(c => c.ProductId == _Product.Items
                            .Where(p => p.Name.Equals(prod))
                            .Select(p => p.Id).FirstOrDefault())
                        .Select(c => c.DishId).Contains(d.Id))
                    .Select(d => d.Name)
                    .ToList();
                tempResult.AddRange(dishNames);
            }

            var results = tempResult.GroupBy(x => x).Select(g => new { Name = g.Key, Count = g.Count() }).Where(d => d.Count == prodListCount).Select(d => d.Name);
            foreach (var result in results)
            {
                DishNames_Report1.Add(result);
            }
        }


        #endregion

        #region Список заказов для отчета 2

        private ObservableCollection<Order> _Orders_Report2;

        ///<summary>Список заказов для отчета 2</summary>
        public ObservableCollection<Order> Orders_Report2
        {
            get => _Orders_Report2;
            set => Set(ref _Orders_Report2, value);
        }

        #endregion

        #region Command Report2RunCommand : Обновление отчета 2


        /// <summary>Обновление отчета 2</summary> 
        private ICommand _Report2RunCommand;

        /// <summary>Обновление отчета 2</summary>

        public ICommand Report2RunCommand => _Report2RunCommand
            ??= new LambdaCommand(OnReport2RunCommandExecuted, CanReport2RunCommandExecute);


        /// <summary>Проверка возможности выполнения Обновление отчета 2</summary>
        private bool CanReport2RunCommandExecute() => !string.IsNullOrEmpty(SelectedCategory) && StartDate != null && StopDate != null;

        /// <summary>Логика выполнения Обновление отчета 2</summary>
        private void OnReport2RunCommandExecuted()
        {
            Orders_Report2.Clear();

            var orderNum = _Order.Items.Where(o => o.OrderDate >= StartDate && o.OrderDate <= StopDate && (_Dishes.Items
                                            .Where(d => d.Category.Name == SelectedCategory)
                                            .Select(d => d.Id))
                                            .Contains(o.DishId))
                                            .Select(o => o.OrderNumber);

            _Order.Items.Where(x => orderNum.Contains(x.OrderNumber))
                                    .Select(x => x)
                                    .ToList()
                                    .ForEach(x =>
                                                {
                                                    Orders_Report2.Add(x);
                                                });
        }

        #endregion

        #region Список продуктов для отчета 3

        private ObservableCollection<string> _ProductsReport3;

        ///<summary>Список продуктов для отчета 3</summary>
        public ObservableCollection<string> ProductsReport3
        {
            get => _ProductsReport3;
            set => Set(ref _ProductsReport3, value);
        }

        #endregion

        #region Command Report3RunCommand : Обновление отчета 3


        /// <summary>Обновление отчета 3</summary> 
        private ICommand _Report3RunCommand;

        /// <summary>Обновление отчета 3</summary>

        public ICommand Report3RunCommand => _Report3RunCommand
            ??= new LambdaCommand(OnReport3RunCommandExecuted, CanReport3RunCommandExecute);


        /// <summary>Проверка возможности выполнения Обновление отчета 3</summary>
        private bool CanReport3RunCommandExecute() => !string.IsNullOrWhiteSpace(SelectedCategory);

        /// <summary>Логика выполнения Обновление отчета 3</summary>
        private void OnReport3RunCommandExecuted()
        {
            ProductsReport3.Clear();
            var categoryId = _Category.Items.Where(c => c.Name == SelectedCategory).Select(c => c.Id).FirstOrDefault();
            var prodIds = new List<int>();
            _Dishes.Items.Where(d => d.CategoryId == categoryId).Select(d => new { CompList = d.Compositions }).ToList().ForEach(
                c =>
                {
                    c.CompList.ToList().ForEach(x =>
                    {
                        prodIds.Add(x.ProductId);
                    });


                });
            prodIds.Distinct().ToList().ForEach(x =>
            {
                ProductsReport3.Add(_Product.Items.Where(p => p.Id == x).Select(p => p.Name).First());
            });
        }


        #endregion

        #region Список имен блюд для отчета 4 

        private ObservableCollection<string> _DishNames_Report4;

        ///<summary>Список имен блюд для отчета 4 </summary>
        public ObservableCollection<string> DishNames_Report4
        {
            get => _DishNames_Report4;
            set => Set(ref _DishNames_Report4, value);
        }

        #endregion

        #region Command Report4RunCommand : Обновление отчета 4


        /// <summary>Обновление отчета 4</summary> 
        private ICommand _Report4RunCommand;

        /// <summary>Обновление отчета 4</summary>

        public ICommand Report4RunCommand => _Report4RunCommand
            ??= new LambdaCommand(OnReport4RunCommandExecuted, CanReport4RunCommandExecute);


        /// <summary>Проверка возможности выполнения Обновление отчета 4</summary>
        private bool CanReport4RunCommandExecute() => StartDate != null && StopDate != null;

        /// <summary>Логика выполнения Обновление отчета 4</summary>
        private void OnReport4RunCommandExecuted()
        {
            DishNames_Report4.Clear();
            var priceAvg = _Dishes.Items.Select(d => d.Price).Average();
            var dishesIds = _Dishes.Items.Where(d => d.Price > priceAvg).Select(d => d.Id).ToList();
            var dishesIds_Ordered = _Order.Items.Where(o => o.OrderDate >= StartDate && o.OrderDate <= StopDate).Select(o => o.DishId).Distinct();
            foreach (var dishesId in dishesIds.Where(dishesId => !dishesIds_Ordered.Contains(dishesId)))
            {
                DishNames_Report4.Add(_Dishes.Items.Where(d => d.Id == dishesId).Select(d => d.Name).First());
            }
        }

        #endregion

        #endregion


        public ReportViewModel(IRepository<Category> categories, IRepository<Product> product,
            IRepository<Dish> dishes, IRepository<Composition> compositions, IRepository<Order> order)
        {
            _Category = categories;
            _Product = product;
            _Dishes = dishes;
            _Compositions = compositions;
            _Order = order;
            Categories = _Category.Items.Select(x => x.Name).ToList();
            Products = _Product.Items.Select(x => x.Name).ToList();
            DishNames_Report1 = new ObservableCollection<string>();
            Orders_Report2 = new ObservableCollection<Order>();
            ProductsReport3 = new ObservableCollection<string>();
            DishNames_Report4 = new ObservableCollection<string>();
        }
    }
}
