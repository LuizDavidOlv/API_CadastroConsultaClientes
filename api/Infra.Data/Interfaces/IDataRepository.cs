using Domain.API;
using Domain.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Interfaces
{
    public interface IDataRepository
    {
        Cadastro InserirCadastroTabelaTemp(requestAPI request);
    }
}
