using Cafe.Models;
using Cafe.ViewModels.Base;
using Storage.Entities;
using Storage.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Cafe.ViewModels.MainViewModels
{
    internal class OrderViewModel : ViewModel
    {
        private IRepository<Order> _OrderRepository;
        private IEnumerable<Order> _Orders;

        #region Свойства

        #region Список заказов

        private IEnumerable<OrderHistory> _OrdersHistory;

        ///<summary>Список заказов</summary>
        public IEnumerable<OrderHistory> OrdersHistory
        {
            get => _OrdersHistory;
            set => Set(ref _OrdersHistory, value);
        }

        #endregion

        #region Подробная информация о заказе

        private IList<Order> _OrderDetail;

        ///<summary>Подробная информация о заказе</summary>
        public IList<Order> OrderDetail
        {
            get => _OrderDetail;
            set => Set(ref _OrderDetail, value);
        }

        #endregion

        #region Выбранный заказ

        private OrderHistory _SelectedOrder;

        ///<summary>Выбранный заказ</summary>
        public OrderHistory SelectedOrder
        {
            get => _SelectedOrder;
            set
            {
                Set(ref _SelectedOrder, value);
                OrderDetail = new List<Order>();
                if (value != null)
                {
                    foreach (var order in _Orders)
                    {
                        if (order.OrderNumber == value.OrderId)
                        {
                            OrderDetail.Add(order);
                        }
                    }
                    OnPropertyChanged(nameof(OrderDetail));
                }
            }
        }

        #endregion

        #endregion
        public OrderViewModel(IRepository<Order> orderRepository)
        {
            _OrderRepository = orderRepository;
            _Orders = _OrderRepository.Items;
            _OrdersHistory = _OrderRepository.Items.ToList().GroupBy(o => o.OrderNumber)
                .Select(g => new OrderHistory()
                {
                    OrderId = g.Select(o => o.OrderNumber).First(),
                    OrderDate = g.Select(o => o.OrderDate).First(),
                    OrderSum = g.Where(o => o.OrderNumber.Equals(g.Key)).Sum(o => o.Dish.Price * o.Qty)
                }).OrderBy(x => x.OrderDate);
        }
    }
}

