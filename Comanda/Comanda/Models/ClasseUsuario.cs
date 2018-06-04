using Comanda.Conexao;
using Comanda.Verificacoes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Comanda.Models
{
    class ClasseUsuario
    {
        public int id_usuario { get; set; }
        public string nome { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public bool apagado { get; set; } = false;
        public ClasseCargo cargo { get; set; }

        public bool CadastrarUsuario()
        {
            Conexao.Conexao obj = new Conexao.Conexao();

            bool correto = false;

            try
            {
                obj.Conectar();

                string sql = "INSERT INTO USUARIO (nome, login, senha, apagado, id_cargo)  VALUES (@NOME, @LOGIN, @SENHA, @APAGADO, @IDCARGO)";

                obj.cmd = new System.Data.SqlClient.SqlCommand(sql, obj.objCon);

                obj.cmd.Parameters.AddWithValue("@NOME", nome);
                obj.cmd.Parameters.AddWithValue("@LOGIN", login);
                obj.cmd.Parameters.AddWithValue("@SENHA", Criptografia.GerarHashMd5(senha));
                obj.cmd.Parameters.AddWithValue("@APAGADO", apagado);
                obj.cmd.Parameters.AddWithValue("@IDCARGO", cargo.id_cargo);

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

        public bool AtualizarUsuario()
        {
            Conexao.Conexao obj = new Conexao.Conexao();

            bool correto = false;

            try
            {
                obj.Conectar();

                string sql = "UPDATE USUARIO SET nome=@NOME, login=@LOGIN, senha=@SENHA, apagado=@APAGADO, id_cargo=@IDCARGO  WHERE id_usuario = @ID_USUARIO";

                obj.cmd = new SqlCommand(sql, obj.objCon);

                obj.cmd.Parameters.AddWithValue("@NOME", nome);
                obj.cmd.Parameters.AddWithValue("@LOGIN", login);
                obj.cmd.Parameters.AddWithValue("@SENHA", Criptografia.GerarHashMd5(senha));
                obj.cmd.Parameters.AddWithValue("@APAGADO", apagado);
                obj.cmd.Parameters.AddWithValue("@IDCARGO", cargo.id_cargo);
                obj.cmd.Parameters.AddWithValue("@ID_USUARIO", id_usuario);


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

        public bool DeletarUsuario(int id)
        {
            Conexao.Conexao obj = new Conexao.Conexao();

            bool correto = false;

            try
            {
                obj.Conectar();

                string sql = "DELETE FROM USUARIO WHERE ID_USUARIO = @ID_USUARIO";

                obj.cmd = new SqlCommand(sql, obj.objCon);

                obj.cmd.Parameters.AddWithValue("@ID_USUARIO", id_usuario);
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

        public void CarregarUsuarioPorId(int ID)
        {
            Conexao.Conexao obj = new Conexao.Conexao();
            try
            {
                obj.Conectar();

                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT nome, login, apagado, id_cargo FROM USUARIO WHERE id_usuario = @CODIGO", obj.objCon);
                cmd.Parameters.AddWithValue("@CODIGO", ID);

                Leitor = cmd.ExecuteReader();

                if (Leitor.Read())
                {
                    this.id_usuario = ID;
                    nome = Leitor["nome"].ToString();
                    login = Leitor["login"].ToString();
                    apagado = bool.Parse(Leitor["apagado"].ToString());
                    ClasseCargo cargo = new ClasseCargo();
                    cargo.CarregarCargoPorID(int.Parse(Leitor["id_cargo"].ToString()));
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { obj.Desconectar(); }
        }

        public bool TemUsuario(string Login, int id_usuario)
        {
            bool existe = false;
            Conexao.Conexao obj = new Conexao.Conexao();
            try
            {
                obj.Conectar();

                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT  COUNT (*) FROM USUARIO WHERE id_usuario != '" + id_usuario + "' AND login = '" + Login + "' AND apagado = 0", obj.objCon);
                Leitor = cmd.ExecuteReader();

                if (Leitor.Read())
                {
                    existe = int.Parse(Leitor[0].ToString()) > 0;

                }

            }
            catch (Exception)
            {

                throw;
            }
            finally { obj.Desconectar(); }
            return existe;

        }

        public void CarregarUsuarioGarcom()
        {
            Conexao.Conexao obj = new Conexao.Conexao();
            try
            {
                obj.Conectar();

                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT nome FROM USUARIO WHERE id_cargo = 4", obj.objCon);

                Leitor = cmd.ExecuteReader();

                if (Leitor.Read())
                {
                    nome = Leitor["nome"].ToString();

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { obj.Desconectar(); }
        }

        public static List<ClasseUsuario> CarregarGarcom()
        {

            {
                Conexao.Conexao obj = new Conexao.Conexao();
                List<ClasseUsuario> lista = new List<ClasseUsuario>();

                try
                {
                    obj.Conectar();

                    SqlDataReader Leitor = null;
                    SqlCommand cmd = new SqlCommand("SELECT nome FROM USUARIO WHERE id_cargo = 4", obj.objCon);
                    Leitor = cmd.ExecuteReader();

                    while (Leitor.Read())
                    {
                        ClasseUsuario usuario = new ClasseUsuario
                        {
                            nome = Leitor["nome"].ToString()
                        };

                        lista.Add(usuario);
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

        public void CarregarUsuarioGarcomPorNome(string nome)
        {
            Conexao.Conexao obj = new Conexao.Conexao();
            try
            {
                obj.Conectar();

                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT id_usuario, nome, login FROM USUARIO WHERE id_cargo = 4 AND nome = @NOME", obj.objCon);
                cmd.Parameters.AddWithValue("@NOME", nome);
                Leitor = cmd.ExecuteReader();

                if (Leitor.Read())
                {
                    id_usuario = int.Parse(Leitor["id_usuario"].ToString());
                    nome = Leitor["nome"].ToString();

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally { obj.Desconectar(); }
        }

        public void CarregarUsuarioPorLogin(string login)
        {
            Conexao.Conexao obj = new Conexao.Conexao();
            try
            {
                obj.Conectar();

                SqlDataReader Leitor = null;
                SqlCommand cmd = new SqlCommand("SELECT id_usuario, nome, login, senha, id_cargo FROM USUARIO WHERE login = @LOGIN", obj.objCon);
                cmd.Parameters.AddWithValue("@LOGIN", login);
                Leitor = cmd.ExecuteReader();

                if (Leitor.Read())
                {
                    id_usuario = int.Parse(Leitor["id_usuario"].ToString());
                    nome = Leitor["nome"].ToString();
                    login = Leitor["login"].ToString();
                    senha = Leitor["senha"].ToString();
                    cargo = new ClasseCargo();
                    cargo.CarregarCargoPorID(int.Parse(Leitor["id_cargo"].ToString()));
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally { obj.Desconectar(); }
        }

        public bool Logar()
        {
            Conexao.Conexao obj = new Conexao.Conexao();

            bool correto = false;

            try
            {
                obj.Conectar();

                string sql = "SELECT id_usuario, login, senha, id_cargo FROM USUARIO WHERE login = @LOGIN AND senha = @SENHA";

                obj.cmd = new SqlCommand(sql, obj.objCon);

                obj.cmd.Parameters.AddWithValue("@LOGIN", login);
                obj.cmd.Parameters.AddWithValue("@SENHA", Criptografia.GerarHashMd5(senha));



                obj.leitor = obj.cmd.ExecuteReader();

                correto = obj.leitor.Read();
            }
            catch (Exception)
            {

                throw;
            }
            finally { obj.Desconectar(); }
            return correto;
        }
    }
}
