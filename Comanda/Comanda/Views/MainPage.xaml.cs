using Comanda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Comanda.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();

            BTentrar.Clicked += BTentrar_Clicked;

            usuarioEntry.Completed += (s, e) => {
                senhaEntry.Focus();
            };

            senhaEntry.Completed += (s, e) => {
                BTentrar.Focus();
            };

            BTconfigurar.Clicked += BTconfigurar_Clicked;
        }

        private void BTconfigurar_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushModalAsync(new Configuracao());
        }

        private void BTentrar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(usuarioEntry.Text))
            {
                DisplayAlert("Erro", "Digite um usuario", "Aceitar");
            }

            if (string.IsNullOrEmpty(senhaEntry.Text))
            {
                DisplayAlert("Erro", "Digite uma senha", "Aceitar");
            }

            Logar();
        }

        public void Logar()
        {
            ClasseUsuario login = new ClasseUsuario
            {
                login = usuarioEntry.Text,
                senha = senhaEntry.Text
            };


            bool certo = login.Logar();
            try
            {
                if (certo)
                {
                    login.CarregarUsuarioPorLogin(usuarioEntry.Text);
                    if (login.cargo.id_cargo == 4)
                    {
                        Navigation.PushModalAsync(new Principal(usuarioEntry.Text));
                    }
                    else
                    {
                        DisplayAlert("Negado", "Acesso Negado", "ok");
                    }

                }
                else
                {
                    DisplayAlert("Negado", "Usuario ou Senha Invalidos", "ok");
                }

            }

            catch (Exception erro)
            {
                DisplayAlert("erro", erro + "Erro Ocorrido", "erro");
            }
        }
    }
}