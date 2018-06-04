using Comanda;

using System;
using System.Data;
using System.Data.SqlClient;


namespace Comanda.Conexao
{
    class Conexao
    {

        public string stringConexao = @"Server=tcp:10.125.230.16,1433; Database=BD_RESTAURANTE; User Id = felipe; Password=040594;";

        public SqlConnection objCon = null;
        public SqlCommand cmd;
        public SqlDataReader leitor;

        public Conexao()
        {
            objCon = new SqlConnection(stringConexao);
        }


        #region "Métodos de conexão com o banco"
        public bool Conectar()
        {
            objCon = new SqlConnection(stringConexao);

            try
            {
                if (objCon.State != ConnectionState.Closed)
                {
                    objCon.Close();
                }

                    objCon.Open();
                    return true;
            }
            catch (Exception ex)
            {
                App.PaginaPrincipal.DisplayAlert("Aviso", "Não foi possível estabelecer uma conexão com o servidor!", "OK");
                var e = ex.ToString();
                return false;
            }
        }

        public bool Desconectar()
        {
            if (objCon.State != ConnectionState.Closed)
            {
                objCon.Close();
                return true;
            }
            else
            {
                objCon.Dispose();
                return false;
            }
        }
        #endregion
    }
}

