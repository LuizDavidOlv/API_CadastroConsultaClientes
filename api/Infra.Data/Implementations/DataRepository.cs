using Domain.API;
using Domain.Cliente;
using Helper.Data;
using Infra.Data.Interfaces;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Infra.Data.Implementations
{
    public class DataRepository : IDataRepository
    {
        #region Consultar Fatura na Base Fria
        public Cadastro InserirCadastroTabelaTemp(requestAPI request)
        {
            try
            {
                AcessoDados db = new AcessoDados();

                db.AddParametro("p_Motivo", OracleDbType.RefCursor, ParameterDirection.Output);
                db.AddParametro("p_Nome", request.Nome);
                db.AddParametro("p_CNPJ", request.CNPJ);

                dynamic json = JsonConvert.DeserializeObject(db.CallProcedure(""));

                string dadosFatura = JsonConvert.SerializeObject(json[0], Formatting.Indented);

                string detalhesFatura = JsonConvert.SerializeObject(json[1], Formatting.Indented);

                List<Cadastro> dadosFat = JsonConvert.DeserializeObject<List<Cadastro>>(dadosFatura);


                if (dadosFat != null && dadosFat.Count > 0 )
                {
                    Cadastro fat = new Cadastro()
                    {
                        
                    };

                    return fat;
                }

                return null;
            }
            catch (Exception ex)
            {
                string erro = "Erro ao consultar ao inserir cliente da base temporária: " + ex.Message;

                throw new Exception(erro);
            }
        }

     
        #endregion
    }
}
