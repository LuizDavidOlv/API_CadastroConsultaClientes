using Helper.Comons;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Data
{
    public class AcessoDados
    {
        #region Propriedades

        private OracleConnection conn;
        private OracleCommand cmd;
        #endregion

        #region Construtor

        /// <summary>
        /// Método construtor da classe Corporativa de Acesso a Dados.
        /// </summary>
        public AcessoDados()
        {
            try
            {
                this.conn = new OracleConnection(GetConnectionString());
                this.cmd = new OracleCommand();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Parâmetros

        public void AddParametro(string nomeParametro, object valor)
        {
            try
            {
                OracleParameter parameter = new OracleParameter(":" + nomeParametro, valor);
                parameter.Size = 20000;
                parameter.Direction = ParameterDirection.InputOutput;

                cmd.Parameters.Add(parameter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar parametro '{nomeParametro}' => {ex.Message}");
            }
        }

        public void AddParametro(string nomeParametro, OracleDbType tipoParametro, object valor)
        {
            try
            {
                OracleParameter parameter = new OracleParameter(":" + nomeParametro, tipoParametro);
                parameter.Size = 20000;
                parameter.Value = valor;

                cmd.Parameters.Add(parameter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar parametro '{nomeParametro}' => {ex.Message}");
            }
        }

        public void AddParametro(string nomeParametro, OracleDbType tipoParametro, ParameterDirection direcao)
        {
            try
            {
                OracleParameter parameter = new OracleParameter(":" + nomeParametro, tipoParametro);
                parameter.Size = 20000;
                parameter.Direction = direcao;

                cmd.Parameters.Add(parameter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar parametro '{nomeParametro}' => {ex.Message}");
            }
        }

        public void AddParametro(string nomeParametro, OracleDbType tipoParametro, object valor, ParameterDirection direcao)
        {
            try
            {
                OracleParameter parameter = new OracleParameter(":" + nomeParametro, tipoParametro);
                parameter.Value = valor;
                parameter.Direction = direcao;

                cmd.Parameters.Add(parameter);

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar parametro '{nomeParametro}' => {ex.Message}");
            }
        }
        #endregion

        #region CallProcedure

        /// <summary>
        /// Executa Stored Procedure e RETORNA JSON tipado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nomeProcedure"></param>
        /// <returns></returns>
        public T CallProcedure<T>(string nomeProcedure)
        {
            try
            {
                string resultJson = CallProcedure(nomeProcedure);

                if (string.IsNullOrEmpty(resultJson))
                {
                    return default;
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(resultJson);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Executa Stored Procedure e retorna JSON não-tipado.
        /// </summary>
        /// <param name="nomeProcedure"></param>
        /// <returns></returns>
        public string CallProcedure(string nomeProcedure)
        {
            try
            {
                this.cmd.CommandText = nomeProcedure;
                this.cmd.Connection = this.conn;
                this.cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                OracleDataReader reader = cmd.ExecuteReader();

                List<IEnumerable<Dictionary<string, object>>> list = new List<IEnumerable<Dictionary<string, object>>>();

                list.Add(Serialize(reader));

                while (reader.NextResult())
                {
                    list.Add(Serialize(reader));
                }

                if (list.Count == 0)
                {
                    return null;
                }
                else if (list.Count == 1)
                {
                    return JsonConvert.SerializeObject(list[0], Newtonsoft.Json.Formatting.Indented);
                }
                else
                {
                    return JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region ExecuteCommand

        /// <summary>
        /// Executa um comando SQL sem esperar quaisquer retornos.
        /// </summary>
        /// <param name="comando"></param>
        public void ExecuteCommand(string comando)
        {
            try
            {
                this.cmd.CommandText = comando.Trim();
                this.cmd.Connection = this.conn;
                this.cmd.CommandType = CommandType.Text;

                this.cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region tipoDado
        public string tipoDado(object dado)
        {
            try
            {
                return Enum.Parse(typeof(DbType), dado.GetType().ToString().Replace("System.", "")).ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Serialize
        public IEnumerable<Dictionary<string, object>> Serialize(OracleDataReader reader)
        {
            try
            {
                var results = new List<Dictionary<string, object>>();
                var cols = new List<string>();
                for (var i = 0; i < reader.FieldCount; i++)
                    cols.Add(reader.GetName(i));

                while (reader.Read())
                    results.Add(SerializeRow(cols, reader));

                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region SerializeRow
        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols, OracleDataReader reader)
        {
            try
            {
                var result = new Dictionary<string, object>();
                foreach (var col in cols)
                    result.Add(col, reader[col]);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetConnectionString


        /// <summary>
        /// Recupera a ConnectionString via API [ URI descrita na Key 'URIAPIConnString' do App/Web.config].
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            try
            {
                string _keyrotaapi = Utils.GetKey("RotaAPIConnString");

                if (string.IsNullOrEmpty(_keyrotaapi.Trim()))
                {
                    throw new Exception("'Key' com a Rota da API de ConnectionString não foi encontrada no App.config/Web.config.");
                }

                HttpClient client = new HttpClient();

                HttpResponseMessage response = client.GetAsync(_keyrotaapi.Trim()).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Não foi possível obter a ConnectionString.");
                }

                string resultado = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(resultado))
                {
                    return Cripto.Decrypt(resultado);
                }
                else
                {
                    throw new Exception("Não foi possível obter a ConnectionString: Vazia.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
