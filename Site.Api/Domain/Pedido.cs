using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Site.Api.Domain
{
	public class Pedido
	{
		public int IdPedido { get; set; }
		public int Quantidade { get; set; }
		public int IdProduto { get; set; }
		public int Valor { get; set; }
	}
}
