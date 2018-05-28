using Comanda.Conexao;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Comanda.Models
{
    class ClasseCategoria_Cardapio
    {
        public int id_categoriacardapio { get; set; }
        public string descricao { get; set; }
        public bool apagado { get; set; }

        public void CarregarCardapioID(int ID)
        {
            conexao obj = new conexao();
            try
            {
                obj.Conectar();
                int Codigo = ID;
                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT descricao FROM CATEGORIACARDAPIO WHERE id_categoriacardapio = @CODIGO", obj.objCon);
                cmd.Parameters.AddWithValue("@CODIGO", ID);
                Leitor = cmd.ExecuteReader();

                if (Leitor.Read())
                {
                    this.id_categoriacardapio = ID;
                    descricao = (Leitor["descricao"].ToString());

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { obj.desconectar(); }
        }

        public static List<ClasseCategoria_Cardapio> CarregarCategoriaCardapio()
        {


            conexao obj = new conexao();
            List<ClasseCategoria_Cardapio> lista = new List<ClasseCategoria_Cardapio>();

            try
            {
                obj.Conectar();

                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT  id_categoriacardapio, descricao  FROM CATEGORIACARDAPIO WHERE apagado = 0", obj.objCon);
                Leitor = cmd.ExecuteReader();

                while (Leitor.Read())
                {
                    ClasseCategoria_Cardapio c = new ClasseCategoria_Cardapio();
                    c.id_categoriacardapio = int.Parse(Leitor["id_categoriacardapio"].ToString());
                    c.descricao = (Leitor["descricao"].ToString());

                    lista.Add(c);
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally { obj.desconectar(); }
            return lista;
        }
    }
}
