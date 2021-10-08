using Cafe.Infrastructure.Command.Base;
using Cafe.ViewModels.Base;
using Storage.Entities;
using Storage.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Cafe.ViewModels.DialogViewModel
{
	internal class AddEditDishViewModel : ViewModel
	{
		private readonly IRepository<Category> _Category;
		private readonly IRepository<Product> _Product;

		#region Свойства

		#region Название окна

		private string _Title;

		///<summary>Название окна</summary>
		public string Title
		{
			get => _Title;
			set => Set(ref _Title, value);
		}

		#endregion

		#region Название редактируемого блюда

		private string _Name;

		///<summary>Название редактируемого блюда</summary>
		public string Name
		{
			get => _Name;
			set => Set(ref _Name, value);
		}

		#endregion

		#region Общий список продуктов

		private List<Product> _Products;

		///<summary>Общий список продуктов</summary>
		public List<Product> Products
		{
			get => _Products;
			set => Set(ref _Products, value);
		}

		#endregion

		#region Общий список категорий

		private List<Category> _Categories;

		///<summary>Общий список категорий</summary>
		public List<Category> Categories
		{
			get => _Categories;
			set => Set(ref _Categories, value);
		}

		#endregion

		#region Категория блюда

		private Category _CategoryCurrent;

		///<summary>Категория блюда</summary>
		public Category CategoryCurrent
		{
			get => _CategoryCurrent;
			set => Set(ref _CategoryCurrent, value);
		}

		#endregion

		#region Состав блюда

		//private Product _ProductCurrent;

		/////<summary>Состав блюда</summary>
		//public Product ProductCurrent
		//{
		//	get => _ProductCurrent;
		//	set
		//	{
		//		Set(ref _ProductCurrent, value);

		//	}
		//}

		#endregion

		#region Цена блюда

		private decimal _Price;

		///<summary>Цена блюда</summary>
		public decimal Price
		{
			get => _Price;
			set => Set(ref _Price, value);
		}

		#endregion

		#region Вес блюда

		private double _Weight;

		///<summary>Вес блюда</summary>
		public double Weight
		{
			get => _Weight;
			set => Set(ref _Weight, value);
		}

		#endregion

		#region Вхождение в меню

		private bool _IsActive;

		///<summary>Вхождение в меню</summary>
		public bool IsActive
		{
			get => _IsActive;
			set => Set(ref _IsActive, value);
		}

		#endregion

		public ObservableCollection<Product> ProductsCurrent { get; set; }
		#endregion

		#region Команды

		#region Command GetLoadedDataCommand - Получение коллекции продуктов выбранного блюда

		/// <summary>Получение коллекции продуктов выбранного блюда</summary> 
		private ICommand _GetLoadedDataCommand;

		/// <summary>Получение коллекции продуктов выбранного блюда</summary>

		public ICommand GetLoadedDataCommand => _GetLoadedDataCommand
			??= new LambdaCommand(OnGetLoadedDataCommandExecuted, CanGetLoadedDataCommandExecute);


		/// <summary>Проверка возможности выполнения Получение коллекции продуктов выбранного блюда</summary>
		private bool CanGetLoadedDataCommandExecute() => true;

		/// <summary>Логика выполнения Получение коллекции продуктов выбранного блюда</summary>
		private void OnGetLoadedDataCommandExecuted()
		{
			Products = new List<Product>();
			Products = _Product.Items.ToList();

			Categories = new List<Category>();
			Categories = _Category.Items.ToList();


			OnPropertyChanged(nameof(Products));
			OnPropertyChanged(nameof(Categories));
		}

		#endregion

		#endregion
		public AddEditDishViewModel(Dish dish, IRepository<Category> category, IRepository<Product> product)
		{
			_Category = category;
			_Product = product;
			Title = dish.Name != null ? "Редактирование блюда" : "Добавление блюда";
			if (dish.Name != null)
			{

				Name = dish.Name;
				CategoryCurrent = dish.Category;
				Price = dish.Price;
				Weight = dish.Weight;
				IsActive = dish.IsActive;
				ProductsCurrent = new ObservableCollection<Product>(dish.Compositions.Select(x => x.Product).ToArray());
			}
			else
			{
				ProductsCurrent = new ObservableCollection<Product>();
				IsActive = true;
			}
		}
	}
}
