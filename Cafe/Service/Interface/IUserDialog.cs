using Storage.Entities;
using Storage.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cafe.Service.Interface
{
	public interface IUserDialog
	{
		bool EditDish(Dish dish, IRepository<Category> categoryRepository,
								 IRepository<Product> productsRepository);
		bool EditProduct(Product product);

		bool MessageInformation(string Information, string Caption);
		bool ConfirmWarning(string Warning, string Caption);
		bool MessageError(string Error, string Caption);

	}
}
