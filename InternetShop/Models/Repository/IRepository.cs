using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShop.Models.Repository
{
	public interface IRepository<T> where T : class
	{
		IEnumerable<T> GetAllItems();

		T GetItem(Guid id);

		void AddItem(T item);

		void UpdateItem(T item);

		void DeleteItem(T item);
	}
}