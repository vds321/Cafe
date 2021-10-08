using Cafe.Service.Interface;
using Cafe.ViewModels.DialogViewModel;
using Cafe.Views.DialogView;
using Storage.Entities;
using Storage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Cafe.Service
{
	internal class UserDialogService : IUserDialog
	{
		#region Implementation of IUserDialog


		public bool EditProduct(Product product)
		{
			var edit_product_model = new AddEditProductViewModel(product);
			var edit_product_window = new AddEditProductView()
			{
				DataContext = edit_product_model,
				Owner = App.ActivedWindow,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};
			if (edit_product_window.ShowDialog() != true) return false;
			bool IsCorrect = !string.IsNullOrWhiteSpace(edit_product_model.Name) &&
							 edit_product_model.Remain != 0.0;
			if (!IsCorrect) return !MessageError("Пустые поля или нулевые значения на допускаются!", "Ошибка");

			product.Name = edit_product_model.Name;
			product.Remains = Math.Round(edit_product_model.Remain, 1);
			return true;
		}


		public bool EditDish(Dish dish, IRepository<Category> categoryRepository, IRepository<Product> productRepository)
		{
			var edit_dish_model = new AddEditDishViewModel(dish, categoryRepository, productRepository);
			var edit_dish_window = new AddEditDishView()
			{
				DataContext = edit_dish_model,
				Owner = App.ActivedWindow,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};
			if (edit_dish_window.ShowDialog() != true) return false;
			bool IsCorrect = !string.IsNullOrWhiteSpace(edit_dish_model.Name) &&
							 edit_dish_model.Price != 0 &&
							 edit_dish_model.Weight != 0.0 &&
							 edit_dish_model.ProductsCurrent.Count > 0 &&
							 edit_dish_model.CategoryCurrent != null;

			if (!IsCorrect) return !MessageError("Пустые поля, нулевые значения на допускаются!", "Ошибка");

			var productIds = edit_dish_model.ProductsCurrent.Select(x => x.Id).ToArray();
			if (dish.Compositions == null)
			{
				dish.Compositions = new List<Composition>();
				foreach (var productId in productIds)
				{
					dish.Compositions.Add(new Composition() { ProductId = productId });
				}
			}
			else
			{
				var compositions_to_remove = dish.Compositions.Where(x => !productIds.Contains(x.ProductId));
				foreach (var composition_to_remove in compositions_to_remove)
				{
					dish.Compositions.Remove(composition_to_remove);
				}

				foreach (var productId in productIds)
				{
					if (dish.Compositions.Any(x => x.ProductId == productId)) continue;
					dish.Compositions.Add(new Composition() { DishId = dish.Id, ProductId = productId });
				}
			}

			dish.Name = edit_dish_model.Name;
			dish.Category = edit_dish_model.CategoryCurrent;
			dish.Price = edit_dish_model.Price;
			dish.Weight = edit_dish_model.Weight;
			dish.IsActive = edit_dish_model.IsActive;
			return true;
		}


		public bool MessageInformation(string Information, string Caption) =>
			MessageBox.Show(Information, Caption, MessageBoxButton.OK, MessageBoxImage.Information)
			== MessageBoxResult.OK;

		public bool ConfirmWarning(string Warning, string Caption) =>
			MessageBox.Show(Warning, Caption, MessageBoxButton.YesNo, MessageBoxImage.Warning)
			== MessageBoxResult.Yes;

		public bool MessageError(string Error, string Caption) =>
			MessageBox.Show(Error, Caption, MessageBoxButton.OK, MessageBoxImage.Error)
			== MessageBoxResult.OK;


		#endregion
	}
}
