using Comanda.Conexao;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Comanda.Models
{
    class ClasseCargo
    {
        public int id_cargo { get; set; }
        public string descricao { get; set; }

        public void CarregarCargo()
        {
            Conexao.Conexao obj = new Conexao.Conexao();
            try
            {
                obj.Conectar();
                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT id_cargo, descricao FROM CARGO", obj.objCon);

                Leitor = cmd.ExecuteReader();

                if (Leitor.Read())
                {
                    id_cargo = int.Parse((Leitor["id_cargo"]).ToString());
                    descricao = Leitor["descricao"].ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { obj.Desconectar(); }
        }

        public void CarregarCargoPorID(int ID)
        {
            Conexao.Conexao obj = new Conexao.Conexao();
            try
            {
                obj.Conectar();
                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT id_cargo, descricao FROM CARGO WHERE id_cargo=@ID", obj.objCon);
                cmd.Parameters.AddWithValue("@ID", ID);
                Leitor = cmd.ExecuteReader();

                if (Leitor.Read())
                {
                    id_cargo = int.Parse((Leitor["id_cargo"]).ToString());
                    descricao = Leitor["descricao"].ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { obj.Desconectar(); }
        }

        public static List<ClasseCargo> ListaCarregarCargo()
        {

            {
                Conexao.Conexao obj = new Conexao.Conexao();
                List<ClasseCargo> lista = new List<ClasseCargo>();

                try
                {
                    obj.Conectar();
                    SqlDataReader Leitor = null;
                    SqlCommand cmd = new SqlCommand("SELECT id_cargo, descricao FROM CARGO", obj.objCon);

                    Leitor = cmd.ExecuteReader();

                    while (Leitor.Read())
                    {
                        ClasseCargo cargo = new ClasseCargo();
                        cargo.id_cargo = int.Parse((Leitor["id_cargo"]).ToString());
                        cargo.descricao = Leitor["descricao"].ToString();


                        lista.Add(cargo);
                    }

                }
                catch (Exception)
                {

                    throw;
                }
                finally { obj.Desconectar(); }
                return lista;
            }
        }
    }
}
