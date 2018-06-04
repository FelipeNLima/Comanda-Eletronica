using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using static Comanda.Models.EnumStatus;
using Comanda.Conexao;

namespace Comanda.Models
{
    class ClasseVenda
    {
        public int id_venda { get; set; }
        public int Numero_pessoa { get; set; }
        public float Desconto { get; set; }
        public StatusVenda Status_Venda { get; set; }
        public DateTime Data_entrada { get; set; }
        public DateTime Data_saida { get; set; }
        public float Couvert_artistico { get; set; }
        public float taxaservico { get; set; }
        public ClasseUsuario usuario { get; set; }
        public ClasseMesa mesa { get; set; }


        public bool InserirVenda()
        {
            Conexao.Conexao obj = new Conexao.Conexao();

            bool correto = false;

            try
            {
                obj.Conectar();

                string sql = "INSERT INTO VENDA (Numero_pessoa, Data_entrada, Data_saida, Status_venda, Desconto, couvert, taxa_servico, id_usuario, id_mesa)  VALUES ( @NUMEROPESSOAS, @DATAENTRADA, @DATASAIDA, @STATUSVENDA, @DESCONTO,@COUVERT,@TAXA, @IDUSUARIO, @IDMESA)";

                obj.cmd = new System.Data.SqlClient.SqlCommand(sql, obj.objCon);


                obj.cmd.Parameters.AddWithValue("@NUMEROPESSOAS", Numero_pessoa);
                obj.cmd.Parameters.AddWithValue("@DATAENTRADA", Data_entrada);
                obj.cmd.Parameters.AddWithValue("@DATASAIDA", Data_entrada);
                obj.cmd.Parameters.AddWithValue("@STATUSVENDA", Status_Venda);
                obj.cmd.Parameters.AddWithValue("@DESCONTO", Desconto);
                obj.cmd.Parameters.AddWithValue("@COUVERT", Couvert_artistico);
                obj.cmd.Parameters.AddWithValue("@TAXA", taxaservico);
                obj.cmd.Parameters.AddWithValue("@IDUSUARIO", usuario.id_usuario);
                obj.cmd.Parameters.AddWithValue("@IDMESA", mesa.id_mesa);


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

        public bool AtualizarVenda()
        {
            Conexao.Conexao obj = new Conexao.Conexao();

            bool correto = false;

            try
            {
                obj.Conectar();

                string sql = "UPDATE VENDA SET Desconto=@DESCONTO, Numero_pessoa=@NUMERO, Status_venda=@STATUSVENDA, couvert=@COUVERT, taxa_servico=@TAXA, id_mesa=@IDMESA, id_usuario=@IDUSUARIO where id_venda = @IDVENDA";

                obj.cmd = new System.Data.SqlClient.SqlCommand(sql, obj.objCon);

                obj.cmd.Parameters.AddWithValue("@DESCONTO", Desconto);
                obj.cmd.Parameters.AddWithValue("@NUMERO", Numero_pessoa);
                obj.cmd.Parameters.AddWithValue("@COUVERT", Couvert_artistico);
                obj.cmd.Parameters.AddWithValue("@TAXA", taxaservico);
                obj.cmd.Parameters.AddWithValue("@STATUSVENDA", Status_Venda);
                obj.cmd.Parameters.AddWithValue("@IDVENDA", id_venda);
                obj.cmd.Parameters.AddWithValue("@IDMESA", mesa.id_mesa);
                obj.cmd.Parameters.AddWithValue("@IDUSUARIO", usuario.id_usuario);


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

        public void CarregarVendaPorID(int id)
        {
            Conexao.Conexao obj = new Conexao.Conexao();

            try
            {
                obj.Conectar();
                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT	Numero_pessoa,Data_entrada,Status_venda,couvert, taxa_servico, id_usuario, id_mesa FROM VENDA  WHERE id_venda = @ID AND Status_venda != 3", obj.objCon);

                cmd.Parameters.AddWithValue("@ID", id);

                Leitor = cmd.ExecuteReader();

                if (Leitor.Read())
                {
                    this.id_venda = id;
                    Numero_pessoa = int.Parse((Leitor["Numero_pessoa"].ToString()));
                    Status_Venda = (StatusVenda)Enum.Parse(typeof(StatusVenda), Leitor["Status_venda"].ToString());
                    Couvert_artistico = float.Parse(Leitor["couvert"].ToString());
                    taxaservico = float.Parse(Leitor["taxa_servico"].ToString());
                    Data_entrada = DateTime.Parse(Leitor["Data_entrada"].ToString());
                    usuario = new ClasseUsuario();
                    usuario.id_usuario = int.Parse(Leitor["id_usuario"].ToString());
                    mesa = new ClasseMesa();
                    mesa.id_mesa = int.Parse(Leitor["id_mesa"].ToString());
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally { obj.Desconectar(); }
        }

        public void CarregarVendaPorMesa(int idMesa)
        {
            Conexao.Conexao obj = new Conexao.Conexao();

            try
            {
                obj.Conectar();
                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT	id_venda,Numero_pessoa,Desconto,couvert, taxa_servico, Data_entrada,Status_venda, id_usuario, id_mesa FROM VENDA  WHERE id_mesa = @ID AND Status_venda != 3", obj.objCon);

                cmd.Parameters.AddWithValue("@ID", idMesa);

                Leitor = cmd.ExecuteReader();

                if (Leitor.Read())
                {
                    id_venda = int.Parse(Leitor["id_venda"].ToString());
                    Numero_pessoa = int.Parse((Leitor["Numero_pessoa"].ToString()));
                    Desconto = float.Parse(Leitor["Desconto"].ToString());
                    Couvert_artistico = float.Parse(Leitor["couvert"].ToString());
                    taxaservico = float.Parse(Leitor["taxa_servico"].ToString());
                    Status_Venda = (StatusVenda)Enum.Parse(typeof(StatusVenda), Leitor["Status_venda"].ToString());
                    Data_entrada = DateTime.Parse(Leitor["Data_entrada"].ToString());
                    usuario = new ClasseUsuario();
                    usuario.id_usuario = int.Parse(Leitor["id_usuario"].ToString());
                    mesa = new ClasseMesa();
                    mesa.id_mesa = int.Parse(Leitor["id_mesa"].ToString());
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally { obj.Desconectar(); }
        }
    }
}
