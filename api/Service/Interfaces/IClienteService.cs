using Domain.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IClienteService
    {
        responseAPI BuscarTodosClientes(requestAPI request);
        responseAPI BuscarCliente(requestAPI request);
        motivoResponse AdicionarCliente(requestAPI request);
    }
}
 