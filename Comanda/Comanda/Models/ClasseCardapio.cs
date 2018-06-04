using Comanda.Conexao;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Comanda.Models
{
    class ClasseCardapio
    {
        public int id_cardapio { get; set; }
        public float preco_item { get; set; }
        public string nome_item { get; set; }
        public ClasseCategoria_Cardapio categoria { get; set; }
        public bool apagado { get; set; } = false;

        public void CarregarPorId(int ID)
        {
            Conexao.Conexao obj = new Conexao.Conexao();
            try
            {
                obj.Conectar();

                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT  nome_item, preco_item, id_categoriacardapio  FROM CARDAPIO WHERE id_cardapio = @CODIGO", obj.objCon);
                cmd.Parameters.AddWithValue("@CODIGO", ID);

                Leitor = cmd.ExecuteReader();

                if (Leitor.Read())
                {
                    this.id_cardapio = ID;
                    nome_item = (Leitor["nome_item"].ToString());
                    preco_item = float.Parse(Leitor["preco_item"].ToString());
                    categoria = new ClasseCategoria_Cardapio();
                    categoria.CarregarCardapioID(int.Parse(Leitor["id_categoriacardapio"].ToString()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                obj.Desconectar();
            }

        }

        public static List<ClasseCardapio> CarregarTodoCardapio()
        {
            List<ClasseCardapio> listadecardapio = new List<ClasseCardapio>();

            Conexao.Conexao obj = new Conexao.Conexao();
            try
            {
                obj.Conectar();

                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT id_cardapio, nome_item, preco_item, id_categoriacardapio  FROM CARDAPIO ", obj.objCon);
                Leitor = cmd.ExecuteReader();

                while (Leitor.Read())
                {
                    ClasseCardapio cardapioitem = new ClasseCardapio();

                    cardapioitem.id_cardapio = int.Parse((Leitor["id_cardapio"].ToString()));
                    cardapioitem.nome_item = (Leitor["nome_item"].ToString());
                    cardapioitem.preco_item = float.Parse(Leitor["preco_item"].ToString());
                    cardapioitem.categoria = new ClasseCategoria_Cardapio();
                    cardapioitem.categoria.CarregarCardapioID(int.Parse(Leitor["id_categoriacardapio"].ToString()));

                    listadecardapio.Add(cardapioitem);
                }

            }

            catch (Exception)
            {
                throw;
            }
            finally
            {
                obj.Desconectar();
            }
            return listadecardapio;
        }

        public override string ToString()
        {
            return nome_item;
        }
    }
}
