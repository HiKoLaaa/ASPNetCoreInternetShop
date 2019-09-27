using InternetShop.Models.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InternetShop.Models.ViewModels
{
	public class SessionCart
	{
		private List<CartLine> _lineCollection;

		[JsonIgnore]
		public ISession Session { get; set; }

		public SessionCart()
		{
			_lineCollection = new List<CartLine>();
		}

		public void AddItem(Product product, int quantity)
		{
			CartLine line = _lineCollection
				.Where(p => p.Product.ID == product.ID)
				.FirstOrDefault();

			if (line == null)
			{
				_lineCollection.Add(new CartLine
				{
					Product = product,
					Quantity = quantity
				});
			}
			else
			{
				line.Quantity += quantity;
			}

			SetCart(this);
		}

		public void RemoveLine(Product product)
		{
			_lineCollection.RemoveAll(l => l.Product.ID == product.ID);
			SetCart(this);
		}

		public decimal ComputeTotalValue() =>
			_lineCollection.Sum(e => e.Product.Price * e.Quantity);

		public decimal ComputeDiscoutValue(int discount)
		{
			decimal priceWithoutDiscont = ComputeTotalValue();
			return priceWithoutDiscont - priceWithoutDiscont * ((decimal)discount / 100);
		}

		public void Clear()
		{
			_lineCollection.Clear();
			Session?.Remove("Cart");
		}

		public IEnumerable<CartLine> Lines => _lineCollection;

		public static SessionCart GetCart(IServiceProvider serviceProvider)
		{
			IHttpContextAccessor httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
			ISession session = httpContextAccessor?.HttpContext.Session;
			var stringCart = session.GetString("Cart");
			SessionCart cart = stringCart == null ? new SessionCart() :
				JsonConvert.DeserializeObject<SessionCart>(stringCart);

			cart.Session = session;
			return cart;
		}

		public static void SetCart(SessionCart cart) =>
			cart.Session?.SetString("Cart", JsonConvert.SerializeObject(cart));
	}
}