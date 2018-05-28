using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Aplicativo.Conexao
{
    class Conexao
    {
                   
            public string stringConexao = @"Server = tcp:192.168.0.104,1433; Database=BD_RESTAURANTE; User Id = felipe; Password=040594; Trusted_Connection=true ";
            


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
                        objCon.Close();

                    objCon.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    var e = ex.ToString();
                    return false;
                }
            }

            public bool desconectar()
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
}
