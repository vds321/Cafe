using Cafe.Infrastructure.Command.Base;
using Cafe.Service.Interface;
using Cafe.ViewModels.Base;
using Cafe.ViewModels.MainViewModels;
using Storage.Entities;
using Storage.Interface;
using System.Windows;
using System.Windows.Input;


namespace Cafe.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IUserDialog _UserDialog;
        private readonly IRepository<Dish> _Dishes;
        private readonly IRepository<Order> _Orders;
        private readonly IRepository<Composition> _Compositions;
        private readonly IRepository<Product> _Product;
        private readonly IRepository<Category> _Categories;
        #region Свойства

        #region Заголовок главного окна

        private string _Title = $"Программа управления кафе (Версия {Application.ResourceAssembly.GetName().Version})";

        ///<summary>Заголовок главного окна</summary>
        public string Title { get => _Title; set => Set(ref _Title, value); }

        #endregion

        #region Выбор текущего окна

        private ViewModel _CurrentModel;

        ///<summary>Выбор текущего окна</summary>
        public ViewModel CurrentModel { get => _CurrentModel; private set => Set(ref _CurrentModel, value); }

        #endregion

        #endregion

        #region Команды

        #region Command ShowMenuViewCommand

        /// <summary>Показать меню</summary> 
        private ICommand _ShowMenuViewCommand;

        /// <summary>Показать меню</summary>
        public ICommand ShowMenuViewCommand => _ShowMenuViewCommand
            ??= new LambdaCommand(OnShowMenuViewCommandExecuted, CanShowMenuViewCommandExecute);

        /// <summary>Проверка возможности выполнения Показать меню</summary>
        private bool CanShowMenuViewCommandExecute() => CurrentModel?.GetType() != typeof(MenuViewModel);

        /// <summary>Логика выполнения Показать меню</summary>
        private void OnShowMenuViewCommandExecuted()
        {
            CurrentModel = new MenuViewModel(_Dishes, _Orders);
            OnPropertyChanged(nameof(CurrentModel));
        }

        #endregion

        #region Command ShowCafeManageCommand

        /// <summary>Показать управление кафе</summary> 
        private ICommand _ShowCafeManageCommand;

        /// <summary>Показать управление кафе</summary>

        public ICommand ShowCafeManageCommand => _ShowCafeManageCommand
            ??= new LambdaCommand(OnShowCafeManageCommandExecuted, CanShowCafeManageCommandExecute);

        /// <summary>Проверка возможности выполнения Показать управление кафе</summary>
        private bool CanShowCafeManageCommandExecute() => CurrentModel?.GetType() != typeof(ManageCafeViewModel);

        /// <summary>Логика выполнения Показать управление кафе</summary>
        private void OnShowCafeManageCommandExecuted()
        {
            CurrentModel = new ManageCafeViewModel(_Dishes, _Product, _Categories, _Compositions, _UserDialog);
            OnPropertyChanged(nameof(CurrentModel));
        }

        #endregion

        #region Command ShowWareHouse

        /// <summary>Показать управление складом</summary> 
        private ICommand _ShowWareHouse;

        /// <summary>Показать управление складом</summary>

        public ICommand ShowWareHouse => _ShowWareHouse
            ??= new LambdaCommand(OnShowWareHouseExecuted, CanShowWareHouseExecute);

        /// <summary>Проверка возможности выполнения Показать управление складом</summary>
        private bool CanShowWareHouseExecute() => CurrentModel?.GetType() != typeof(WarehouseViewModel);

        /// <summary>Логика выполнения Показать управление складом</summary>
        private void OnShowWareHouseExecuted()
        {
            CurrentModel = new WarehouseViewModel(_Product, _UserDialog);
            OnPropertyChanged(nameof(CurrentModel));
        }

        #endregion

        #region Command OrdersShowCommand

        /// <summary>Показать управление заказами</summary> 
        private ICommand _OrdersShowCommand;

        /// <summary>Показать управление заказами</summary>

        public ICommand OrdersShowCommand => _OrdersShowCommand
            ??= new LambdaCommand(OnOrdersShowCommandExecuted, CanOrdersShowCommandExecute);

        /// <summary>Проверка возможности выполнения Показать управление заказами</summary>
        private bool CanOrdersShowCommandExecute() => CurrentModel?.GetType() != typeof(OrderViewModel);

        /// <summary>Логика выполнения Показать управление заказами</summary>
        private void OnOrdersShowCommandExecuted()
        {
            CurrentModel = new OrderViewModel(_Orders);
            OnPropertyChanged(nameof(CurrentModel));
        }

        #endregion

        #region Command ReportShowCommand : Показать страницу отчетов


        /// <summary>Показать страницу отчетов</summary> 
        private ICommand _ReportShowCommand;

        /// <summary>Показать страницу отчетов</summary>

        public ICommand ReportShowCommand => _ReportShowCommand
            ??= new LambdaCommand(OnReportShowCommandExecuted, CanReportShowCommandExecute);


        /// <summary>Проверка возможности выполнения Показать страницу отчетов</summary>
        private bool CanReportShowCommandExecute() => CurrentModel?.GetType() != typeof(ReportViewModel);

        /// <summary>Логика выполнения Показать страницу отчетов</summary>
        private void OnReportShowCommandExecuted()
        {
            CurrentModel = new ReportViewModel(_Categories, _Product, _Dishes, _Compositions, _Orders);
            OnPropertyChanged(nameof(CurrentModel));
        }


        #endregion

        #endregion

        public MainWindowViewModel(IUserDialog UserDialog, IRepository<Dish> Dish, IRepository<Order> Order,
                                    IRepository<Category> Category, IRepository<Composition> Composition,
                                    IRepository<Product> Product)
        {
            _UserDialog = UserDialog;
            _Dishes = Dish;
            _Orders = Order;
            _Categories = Category;
            _Compositions = Composition;
            _Product = Product;
        }
    }
}
