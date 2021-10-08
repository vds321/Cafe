using Cafe.Infrastructure.Command.Base;
using Cafe.Service.Interface;
using Cafe.ViewModels.Base;
using Storage.Entities;
using Storage.Interface;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;


namespace Cafe.ViewModels.MainViewModels
{
    internal class ManageCafeViewModel : ViewModel
    {
        private readonly IRepository<Product> _Product;
        private readonly IRepository<Dish> _Dish;
        private readonly IRepository<Category> _Category;
        private readonly IRepository<Composition> _Compositions;
        private readonly IUserDialog _UserDialog;

        #region Свойства

        #region Список блюд

        private ObservableCollection<Dish> _Dishes;

        ///<summary>Список блюд</summary>
        public ObservableCollection<Dish> Dishes
        {
            get => _Dishes;
            set
            {
                Set(ref _Dishes, value);
                OnPropertyChanged(nameof(SelectedDish));
            }
        }

        #endregion

        #region Выбранное блюдо

        private Dish _SelectedDish;

        ///<summary>Выбранное блюдо</summary>
        public Dish SelectedDish
        {
            get => _SelectedDish;
            set
            {
                Set(ref _SelectedDish, value);
                if (_SelectedDish != null)
                {
                    _SelectedDishProducts = new ObservableCollection<Product>();
                    var productIds = _SelectedDish.Compositions.Select(x => x.ProductId);
                    foreach (var productId in productIds)
                    {
                        _SelectedDishProducts.Add(_Product.Items.Where(x => x.Id == productId).Select(x => x).FirstOrDefault());
                    }

                }
                OnPropertyChanged(nameof(SelectedDishProducts));
            }
        }

        #endregion

        #region Состав выбранного блюда

        private ObservableCollection<Product> _SelectedDishProducts;

        ///<summary>Состав выбранного блюда</summary>
        public ObservableCollection<Product> SelectedDishProducts
        {
            get => _SelectedDishProducts;
            set => Set(ref _SelectedDishProducts, value);
        }

        #endregion

        #endregion

        #region Команды

        #region Command AddDishCommand


        /// <summary>Добавить блюдо</summary> 
        private ICommand _AddDishCommand;

        /// <summary>Добавить блюдо</summary>

        public ICommand AddDishCommand => _AddDishCommand
            ??= new LambdaCommand(OnAddDishCommandExecuted, CanAddDishCommandExecute);


        /// <summary>Проверка возможности выполнения Добавить блюдо</summary>
        private bool CanAddDishCommandExecute() => true;

        /// <summary>Логика выполнения Добавить блюдо</summary>
        private void OnAddDishCommandExecuted()
        {
            var new_dish = new Dish();
            if (!_UserDialog.EditDish(new_dish, _Category, _Product)) return;
            if (_Dish.Items.Select(d => d.Name.Equals(new_dish.Name)).Count() != 0)
            {
                _UserDialog.MessageError($"Данное название блюда \"{new_dish.Name}\" уже существует.", "Ошибка данных");
                return;
            }

            _Dish.Add(new_dish);
            new_dish.Compositions.ToList().ForEach(x =>
            {
                x.DishId = _Dish.Items.Where(y => y.Name == new_dish.Name).Select(y => y.Id).First();
            });
            var t = _Compositions.AddRange(new_dish.Compositions);
            Dishes = new ObservableCollection<Dish>(_Dish.Items.ToArray());

            OnPropertyChanged(nameof(Dishes));
            OnPropertyChanged(nameof(SelectedDishProducts));
        }
        #endregion

        #region Command RemoveDishCommand


        /// <summary>Удалить блюдо</summary> 
        private ICommand _RemoveDishCommand;

        /// <summary>Удалить блюдо</summary>

        public ICommand RemoveDishCommand => _RemoveDishCommand
            ??= new LambdaCommand(OnRemoveDishCommandExecuted, CanRemoveDishCommandExecute);


        /// <summary>Проверка возможности выполнения Удалить блюдо</summary>
        private bool CanRemoveDishCommandExecute() => SelectedDish != null;

        /// <summary>Логика выполнения Удалить блюдо</summary>
        private void OnRemoveDishCommandExecuted()
        {
            var dishToRemove = SelectedDish;
            if (!_UserDialog.ConfirmWarning($"Удаление блюда \"{dishToRemove.Name}\" из меню!\n Вы согласны?", "Внимание!")) return;
            _Dish.Remove(dishToRemove.Id);
            _Dishes.Remove(dishToRemove);
            SelectedDish = null;
            SelectedDishProducts.Clear();
        }


        #endregion

        #region Command EditDishCommand


        /// <summary>Редактировать блюдо</summary> 
        private ICommand _EditDishCommand;

        /// <summary>Редактировать блюдо</summary>

        public ICommand EditDishCommand => _EditDishCommand
            ??= new LambdaCommand(OnEditDishCommandExecuted, CanEditDishCommandExecute);


        /// <summary>Проверка возможности выполнения Редактировать блюдо</summary>
        private bool CanEditDishCommandExecute() => SelectedDish != null;

        /// <summary>Логика выполнения Редактировать блюдо</summary>
        private void OnEditDishCommandExecuted()
        {
            var old_Name = SelectedDish.Name;
            if (!_UserDialog.EditDish(SelectedDish, _Category, _Product)) return;
            var new_Name = SelectedDish.Name;
            if (!old_Name.Equals(new_Name) && _Dish.Items.Select(p => p.Name.Equals(new_Name)).Count() != 0)
            {
                _UserDialog.MessageError($"Данное название блюда \"{new_Name}\" уже существует.", "Ошибка данных");
                return;
            }

            _Dish.Update(SelectedDish);

            Dishes = new ObservableCollection<Dish>(_Dish.Items.ToArray());
            OnPropertyChanged(nameof(Dishes));
            OnPropertyChanged(nameof(SelectedDishProducts));
            var selectedId = SelectedDish.Id;
            SelectedDish = _Dishes.Where(x => x.Id == selectedId).Select(x => x).First();
        }


        #endregion

        #endregion
        public ManageCafeViewModel(IRepository<Dish> dish, IRepository<Product> product,
            IRepository<Category> categories, IRepository<Composition> compositions, IUserDialog userDialog)
        {
            _Product = product;
            _Dish = dish;
            _Category = categories;
            _Compositions = compositions;
            _UserDialog = userDialog;
            Dishes = new ObservableCollection<Dish>(_Dish.Items.OrderByDescending(x => x.Category).ToArray());
        }
    }
}
