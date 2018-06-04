using System;
using Xamarin.Forms;
using Comanda.Views;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Comanda
{
	public partial class App : Application
	{
		
        public static Page PaginaPrincipal { get; set; }

		public App ()
		{
			InitializeComponent();


			MainPage = new MainPage();
            PaginaPrincipal = MainPage;
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
