using Cafe.Models.Base;
using System;
using System.Windows;

namespace Cafe.Models
{
    internal class CurrentOrder : NotifyPropertyChanged
    {
        #region Название блюда

        ///<summary>Название блюда</summary>
        public string Name { get; set; }

        #endregion

        #region Количество блюд одного типа

        private int _qty;

        public int Qty
        {
            get => _qty;
            set
            {
                if (value > 2)
                {
                    MessageBox.Show("Один заказ может содержать не более 2 блюд одного наименования", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _qty = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        #endregion

        #region Цена блюда

        public decimal Price { get; set; }

        #endregion

        #region Общая стоимость блюд одного типа

        public decimal Total => Qty * Price;


        #endregion
    }
}
