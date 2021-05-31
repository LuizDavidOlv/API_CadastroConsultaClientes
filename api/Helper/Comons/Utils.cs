using System.Configuration;

namespace Helper.Comons
{
    public class Utils
    {
        #region GetKey WEB
        /// <summary>
        /// Obtém o valor da chave do webconfig
        /// </summary>
        /// <param name="value">Chave de configuração</param>
        /// <returns>Valor da chave</returns>
        public static string GetKey(string value)
        {
            //return String.Format(System.Configuration.ConfigurationManager.AppSettings[value]);
            return ConfigurationManager.AppSettings[value];
        }
        #endregion

    }
}
