using Cafe.Infrastructure.Command.Base;
using Cafe.Models;
using Cafe.ViewModels.Base;
using Storage.Entities;
using Storage.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Cafe.ViewModels.MainViewModels
{
    internal class MenuViewModel : ViewModel
    {
        private readonly IRepository<Dish> _DishRepository;
        private readonly IRepository<Order> _OrderRepository;

        #region Свойства

        #region Список меню

        private List<Dish> _Dishes;

        ///<summary>Список меню</summary>
        public List<Dish> Dishes
        {
            get => _Dishes;
            set => Set(ref _Dishes, value);
        }

        #endregion

        #region Выбранный пункт меню

        private Dish _SelectedDish;

        ///<summary>Выбранный пункт меню</summary>
        public Dish SelectedDish
        {
            get => _SelectedDish;
            set => Set(ref _SelectedDish, value);
        }

        #endregion

        #region Список заказов

        private ObservableCollection<CurrentOrder> _CurrentOrders;

        ///<summary>Список заказов</summary>
        public ObservableCollection<CurrentOrder> CurrentOrders
        {
            get => _CurrentOrders;
            set => Set(ref _CurrentOrders, value);
        }

        #endregion

        #region Полная сумма заказа

        ///<summary>Полная сумма заказа</summary>
        public decimal TotalOrderSum => CurrentOrders.Sum(currentOrder => currentOrder.Total);

        #endregion

        #endregion

        #region Команды

        #region Command AddToOrderCommand


        ///// <summary>Добавить в заказ</summary> 
        private ICommand _AddToOrderCommand;

        ///// <summary>Добавить в заказ</summary>

        public ICommand AddToOrderCommand => _AddToOrderCommand
            ??= new LambdaCommand(OnAddToOrderCommandExecuted, CanAddToOrderCommandExecute);


        /// <summary>Проверка возможности выполнения Добавить в заказ</summary>
        private bool CanAddToOrderCommandExecute(object p) => p is Dish;


        /// <summary>Логика выполнения Добавить в заказ</summary>
        private void OnAddToOrderCommandExecuted(object p)
        {
            if (p is not Dish dish) return;

            SelectedDish = dish;
            OnPropertyChanged(nameof(SelectedDish));
            if (CurrentOrders.Count == 6 && !CurrentOrders.Any(x => x.Name.Equals(SelectedDish.Name)))
            {
                MessageBox.Show("Заказ не может содержать более 6 различных блюд.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (CurrentOrders.Any(x => x.Name.Equals(SelectedDish.Name)))
            {
                CurrentOrders.First(x => x.Name.Equals(SelectedDish.Name)).Qty++;
            }
            else
            {
                CurrentOrders.Add(new CurrentOrder()
                {
                    Name = SelectedDish.Name,
                    Qty = 1,
                    Price = SelectedDish.Price
                });
            }
            OnPropertyChanged(nameof(TotalOrderSum));
        }


        #endregion

        #region Command DelFormOrderCommand


        /// <summary>Убрать из заказа</summary> 
        private ICommand _DelFormOrderCommand;

        /// <summary>Убрать из заказа</summary>

        public ICommand DelFormOrderCommand => _DelFormOrderCommand
            ??= new LambdaCommand(OnDelFormOrderCommandExecuted, CanDelFormOrderCommandExecute);


        /// <summary>Проверка возможности выполнения Убрать из заказа</summary>
        private bool CanDelFormOrderCommandExecute(object p) => p is Dish dish && CurrentOrders.Any(x => x.Name.Equals(dish.Name));

        /// <summary>Логика выполнения Убрать из заказа</summary>
        private void OnDelFormOrderCommandExecuted(object p)
        {
            if (p is not Dish dish) return;
            SelectedDish = dish;
            OnPropertyChanged(nameof(SelectedDish));
            var order = CurrentOrders.First(x => x.Name.Equals(SelectedDish.Name));
            switch (order.Qty)
            {
                case 1:
                    _ = CurrentOrders.Remove(order);
                    break;
                default:
                    order.Qty--;
                    break;
            }
            OnPropertyChanged(nameof(TotalOrderSum));
        }


        #endregion

        #region Command ClearCurrentOrdersCommand


        /// <summary>Очистка текущего заказа</summary> 
        private ICommand _ClearCurrentOrdersCommand;

        /// <summary>Очистка текущего заказа</summary>

        public ICommand ClearCurrentOrdersCommand => _ClearCurrentOrdersCommand
            ??= new LambdaCommand(OnClearCurrentOrdersCommandExecuted, CanClearCurrentOrdersCommandExecute);


        /// <summary>Проверка возможности выполнения Очистка текущего заказа</summary>
        private bool CanClearCurrentOrdersCommandExecute() => CurrentOrders.Any();

        /// <summary>Логика выполнения Очистка текущего заказа</summary>
        private void OnClearCurrentOrdersCommandExecuted()
        {
            CurrentOrders.Clear();
            OnPropertyChanged(nameof(TotalOrderSum));
        }

        #endregion

        #region Command CompleteOrderCommand


        /// <summary>Оформить заказ</summary> 
        private ICommand _CompleteOrderCommand;

        /// <summary>Оформить заказ</summary>

        public ICommand CompleteOrderCommand => _CompleteOrderCommand
            ??= new LambdaCommand(OnCompleteOrderCommandExecuted, CanCompleteOrderCommandExecute);


        /// <summary>Проверка возможности выполнения Оформить заказ</summary>
        private bool CanCompleteOrderCommandExecute() => CurrentOrders.Any();

        /// <summary>Логика выполнения Оформить заказ</summary>
        private void OnCompleteOrderCommandExecuted()
        {
            var lastOrderId = _OrderRepository.Items.Select(x => x.OrderNumber).Max();
            foreach (var order in CurrentOrders)
            {
                var newOrder = _OrderRepository.Add(new Order()
                {
                    OrderNumber = lastOrderId + 1,
                    OrderDate = DateTime.Now,
                    Qty = order.Qty,
                    DishId = _Dishes.Where(d => d.Name.Equals(order.Name)).Select(d => d.Id).FirstOrDefault()
                });
            }
            CurrentOrders.Clear();
            OnPropertyChanged(nameof(TotalOrderSum));
            MessageBox.Show("Заказ успешно сохранен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        #endregion

        #endregion

        public MenuViewModel(IRepository<Dish> dish, IRepository<Order> order)
        {
            _DishRepository = dish;
            _OrderRepository = order;
            Dishes = _DishRepository.Items.Where(x => x.IsActive).OrderByDescending(x => x.Category).ToList();
            CurrentOrders = new ObservableCollection<CurrentOrder>();
        }

        public MenuViewModel() { }
    }
}
