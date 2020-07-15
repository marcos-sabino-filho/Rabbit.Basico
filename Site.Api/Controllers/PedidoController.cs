using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Site.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
		private ILogger<PedidoController> _log;

		public PedidoController(ILogger<PedidoController> log)
		{
			_log = log;
		}

		[HttpPost("gerarPedido")]
		public IActionResult Gerar(Domain.Pedido pedido)
		{
			try
			{
				#region [ Fila ]

				var factory = new ConnectionFactory() { HostName = "localhost" };
				using (var connection = factory.CreateConnection())
				using (var channel = connection.CreateModel())
				{
					channel.QueueDeclare(queue: "filaPedidos",
										 durable: false,
										 exclusive: false,
										 autoDelete: false,
										 arguments: null);

					string objetoSerializado = JsonConvert.SerializeObject(pedido);
					var body = Encoding.UTF8.GetBytes(objetoSerializado);

					channel.BasicPublish(exchange: "",
										 routingKey: "filaPedidos",
										 basicProperties: null,
										 body: body);
					//Console.WriteLine(" [x] Sent {0}", message);
				}

				#endregion

				return Accepted(pedido);
			}
			catch (Exception ex)
			{
				_log.LogError(ex.Message, ex);
				return BadRequest(ex.Message);
			}

		}
    }
}