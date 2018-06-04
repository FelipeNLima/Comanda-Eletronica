using Comanda.Conexao;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Comanda.Models
{
    class ClasseConsumo
    {
        public int id_consumo { get; set; }
        public int quantidade { get; set; }
        public float Valor_total { get; set; }
        public bool apagado { get; set; } = false;
        public ClasseCardapio Cardapio { get; set; }
        public ClasseVenda Venda { get; set; }


        public bool CadastrarConsumo()
        {
            Conexao.Conexao obj = new Conexao.Conexao();

            bool correto = false;

            try
            {
                obj.Conectar();

                string sql = "INSERT INTO CONSUMO (quantidade, Valor_total, apagado, id_cardapio, id_venda)  VALUES ( @QUANTIDADE, @VALORTOTAL, @APAGADO, @IDCARDAPIO, @IDVENDA)";

                obj.cmd = new SqlCommand(sql, obj.objCon);


                obj.cmd.Parameters.AddWithValue("@QUANTIDADE", quantidade);
                obj.cmd.Parameters.AddWithValue("@VALORTOTAL", Valor_total);
                obj.cmd.Parameters.AddWithValue("@APAGADO", apagado);
                obj.cmd.Parameters.AddWithValue("@IDCARDAPIO", Cardapio.id_cardapio);
                obj.cmd.Parameters.AddWithValue("@IDVENDA", Venda.id_venda);


                obj.cmd.ExecuteNonQuery();

                correto = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally { obj.Desconectar(); }
            return correto;
        }

        public bool AtualizarConsumo()
        {
            Conexao.Conexao obj = new Conexao.Conexao();

            bool correto = false;

            try
            {
                obj.Conectar();

                string sql = "UPDATE CONSUMO SET  quantidade=@QUANTIDADE, Valor_total=@VALORTOTAL, apagado=@APAGADO, id_cardapio=@IDCARDAPIO, id_venda=@IDVENDA WHERE id_consumo = @IDCONSUMO";

                obj.cmd = new System.Data.SqlClient.SqlCommand(sql, obj.objCon);


                obj.cmd.Parameters.AddWithValue("@QUANTIDADE", quantidade);
                obj.cmd.Parameters.AddWithValue("@VALORTOTAL", Valor_total);
                obj.cmd.Parameters.AddWithValue("@APAGADO", apagado);
                obj.cmd.Parameters.AddWithValue("@IDCARDAPIO", Cardapio.id_cardapio);
                obj.cmd.Parameters.AddWithValue("@IDVENDA", Venda.id_venda);
                obj.cmd.Parameters.AddWithValue("@IDCONSUMO", id_consumo);

                obj.cmd.ExecuteNonQuery();
                correto = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally { obj.Desconectar(); }
            return correto;
        }

        public void CarregarPorID(int id)
        {
            Conexao.Conexao obj = new Conexao.Conexao();

            try
            {
                obj.Conectar();
                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT	quantidade, Valor_total, apagado, id_cardapio, id_venda FROM CONSUMO WHERE id_consumo = @ID", obj.objCon);

                cmd.Parameters.AddWithValue("@ID", id);

                Leitor = cmd.ExecuteReader();

                if (Leitor.Read())
                {
                    id_consumo = id;
                    quantidade = int.Parse(Leitor["quantidade"].ToString());
                    Valor_total = float.Parse(Leitor["Valor_total"].ToString());
                    apagado = bool.Parse((Leitor["apagado"].ToString()));
                    Cardapio = new ClasseCardapio();
                    Cardapio.id_cardapio = int.Parse(Leitor["id_cardapio"].ToString());
                    Venda = new ClasseVenda();
                    Venda.id_venda = int.Parse(Leitor["id_venda"].ToString());
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally { obj.Desconectar(); }
        }

        public static List<ClasseConsumo> CarregerConsumoPorVenda(int idVenda)
        {
            List<ClasseConsumo> listaconsumo = new List<ClasseConsumo>();
            Conexao.Conexao obj = new Conexao.Conexao();

            try
            {
                obj.Conectar();
                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT	id_consumo, CARDAPIO.id_cardapio, CARDAPIO.nome_item, CARDAPIO.preco_item, quantidade, valor_total " +
                "FROM CONSUMO " +
                "INNER JOIN CARDAPIO ON CARDAPIO.id_cardapio = CONSUMO.id_cardapio " +
                "INNER JOIN VENDA  ON VENDA.id_venda = CONSUMO.id_venda " +
                "WHERE VENDA.id_venda = @ID AND CONSUMO.apagado =  0", obj.objCon);

                cmd.Parameters.AddWithValue("@ID", idVenda);

                Leitor = cmd.ExecuteReader();

                while (Leitor.Read())
                {
                    ClasseConsumo consumo = new ClasseConsumo();
                    consumo.Cardapio = new ClasseCardapio();

                    consumo.id_consumo = int.Parse(Leitor["id_consumo"].ToString());
                    consumo.Cardapio.id_cardapio = int.Parse(Leitor["id_cardapio"].ToString());
                    consumo.Cardapio.nome_item = (Leitor["nome_item"].ToString());
                    consumo.Cardapio.preco_item = float.Parse(Leitor["preco_item"].ToString());
                    consumo.quantidade = int.Parse(Leitor["quantidade"].ToString());
                    consumo.Valor_total = float.Parse(Leitor["valor_total"].ToString());

                    listaconsumo.Add(consumo);
                }


            }
            catch (Exception)
            {
                throw;
            }
            finally { obj.Desconectar(); }
            return listaconsumo;
        }

        public static List<ClasseConsumo> CarregarConsumoPorMesa(int numeromesa)
        {
            List<ClasseConsumo> listaconsumo = new List<ClasseConsumo>();
            Conexao.Conexao obj = new Conexao.Conexao();

            try
            {
                obj.Conectar();
                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT	id_consumo, CARDAPIO.id_cardapio, CARDAPIO.nome_item, CARDAPIO.preco_item, quantidade, valor_total " +
                "FROM CONSUMO " +
                "INNER JOIN CARDAPIO ON CARDAPIO.id_cardapio = CONSUMO.id_cardapio " +
                "INNER JOIN VENDA  ON VENDA.id_venda = CONSUMO.id_venda " +
                "WHERE VENDA.id_mesa = @ID AND CONSUMO.apagado =  0", obj.objCon);

                cmd.Parameters.AddWithValue("@ID", numeromesa);

                Leitor = cmd.ExecuteReader();

                while (Leitor.Read())
                {
                    ClasseConsumo consumo = new ClasseConsumo();
                    consumo.Cardapio = new ClasseCardapio();

                    consumo.id_consumo = int.Parse(Leitor["id_consumo"].ToString());
                    consumo.Cardapio.id_cardapio = int.Parse(Leitor["id_cardapio"].ToString());
                    consumo.Cardapio.nome_item = (Leitor["nome_item"].ToString());
                    consumo.Cardapio.preco_item = float.Parse(Leitor["preco_item"].ToString());
                    consumo.quantidade = int.Parse(Leitor["quantidade"].ToString());
                    consumo.Valor_total = float.Parse(Leitor["valor_total"].ToString());
                    listaconsumo.Add(consumo);
                }


            }
            catch (Exception)
            {
                throw;
            }
            finally { obj.Desconectar(); }
            return listaconsumo;
        }
    }
}
