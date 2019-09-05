﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternetShop.Models.Repository
{
	public class Order
	{
		[Key]
		public Guid ID { get; set; }

		[ForeignKey("FK_Order_Customer")]
		public Guid CustomerID { get; set; }

		[Required]
		public DateTime OrderDate { get; set; }

		[Required]
		public DateTime ShipmentDate { get; set; }

		[Required]
		public int OrderNumber { get; set; }

		[Required]
		public int ProductCount { get; set; }

		[Required]
		public string StatusID { get; set; }

		public Customer Customer { get; set; }

		public List<OrderProduct> Products { get; set; }
	}
}