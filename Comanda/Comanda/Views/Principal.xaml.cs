using Comanda.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Comanda.Models.EnumStatus;

namespace Comanda.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Principal : TabbedPage
    {
        String NomeGarcom;
        private ClasseUsuario usuario = new ClasseUsuario();
        private ClasseVenda venda = new ClasseVenda();
        private ClasseConsumo consumo = new ClasseConsumo();
        private ClasseCardapio cardapio = new ClasseCardapio();
        private ObservableCollection<ClasseItensListView> ListaItens { get; }
        List<ClasseCardapio> listaproduto = new List<ClasseCardapio>();
        List<ClasseConsumo> lista = new List<ClasseConsumo>();

        public Principal (String nomegarcom)
		{
			InitializeComponent ();

            BTabrir.Clicked += BTabrir_Clicked;
            ComandaBTok.Clicked += ComandaBTok_Clicked;
            ComandaBTenviar.Clicked += ComandaBTenviar_Clicked;
            this.NomeGarcom = nomegarcom;
            Comandagarcom.Text = nomegarcom;
            ListaItens = new ObservableCollection<ClasseItensListView>();
            ComandaCodigo.Completed += ComandaCodigo_Completed;
            BTConsultar.Clicked += BTConsultar_Clicked;
            BTFechar.Clicked += BTFechar_Clicked;

            NumeroMesa.Completed += (s, e) =>
            {
                NumeroPessoas.Focus();
            };

            NumeroPessoas.Completed += (s, e) =>
            {
                BTabrir.Focus();
            };
        }

        #region "Mesa"

        private void BTabrir_Clicked(object sender, EventArgs e)
        {
            ClasseMesa mesa = new ClasseMesa();
            mesa.CarregarMesaPorID(int.Parse(NumeroMesa.Text));
            if (mesa.status == StatusMesa.Disponivel)
            {
                ComandaNumeromesa.Text = NumeroMesa.Text;
                ComandaNumeropessoas.Text = NumeroPessoas.Text;
                Comandagarcom.Text = NomeGarcom;
                CurrentPage = paginaComanda;
                AbrirMesa();
                EfetuarVenda();
                NumeroMesa.Text = String.Empty;
                NumeroPessoas.Text = String.Empty;
            }
            else
            {
                DisplayAlert("Aviso", "Essa Mesa já está Aberta", "OK");
            }
        }

        public void AbrirMesa()
        {
            ClasseMesa mesa = new ClasseMesa();
            mesa.CarregarMesaPorID(int.Parse(ComandaNumeromesa.Text));
            mesa.MudarParaOcupado();
        }

        #endregion

        #region Comanda

        private void ComandaBTenviar_Clicked(object sender, EventArgs e)
        {
            CadastrarConsumo();
            ListaItens.Clear();
            ComandaCodigo.Text = string.Empty;
            Comandaproduto.Text = string.Empty;
        }

        private void ComandaCodigo_Completed(object sender, EventArgs e)
        {
            int codigo = int.Parse(ComandaCodigo.Text);
            cardapio.CarregarPorId(codigo);

            Comandaquantidade.Focus();
            Comandaproduto.Text = cardapio.nome_item;
        }

        private void ComandaBTok_Clicked(object sender, EventArgs e)
        {
            ListviewComanda.ItemsSource = ListaItens;
            AdicionarListView();
        }

        public void AdicionarListView()
        {
            ListaItens.Add(new ClasseItensListView
            {
                id_cardapio = int.Parse(ComandaCodigo.Text),
                nome_item = Comandaproduto.Text,
                quantidade = float.Parse(Comandaquantidade.Value.ToString())
            });

        }

        public void EfetuarVenda()
        {
            ClasseVenda venda = new ClasseVenda();
            ClasseMesa mesa = new ClasseMesa();


            venda.Numero_pessoa = int.Parse(ComandaNumeropessoas.Text);
            venda.Data_entrada = DateTime.Now;
            venda.Data_saida = DateTime.Now;
            usuario.CarregarUsuarioGarcomPorNome(Comandagarcom.Text);
            venda.usuario = usuario;
            mesa.CarregarMesaPorID(int.Parse(ComandaNumeromesa.Text));
            venda.mesa = mesa;
            venda.Status_Venda = StatusVenda.Ocupado;
            venda.Couvert_artistico = 0;
            venda.taxaservico = 0;

            venda.InserirVenda();
        }

        public void AtualizarVenda(int statusdamesa)
        {
            ClasseMesa mesa = new ClasseMesa();
            venda.CarregarVendaPorMesa(int.Parse(ConsultarNumeroMesa.Text));
            venda.id_venda = venda.id_venda;
            venda.Numero_pessoa = venda.Numero_pessoa;
            venda.Desconto = CalculaDesconto();
            venda.Data_saida = DateTime.Now;
            venda.Couvert_artistico = CalculaCouvert_Artistico();
            venda.taxaservico = CalculaTaxaServiço();
            mesa.CarregarMesaPorID(venda.mesa.id_mesa);
            venda.mesa = mesa;
            ClasseUsuario carregar = new ClasseUsuario();
            carregar.CarregarUsuarioPorLogin(NomeGarcom);
            venda.usuario = carregar;

            if (statusdamesa == 1)
            {
                venda.Status_Venda = StatusVenda.Ocupado;
            }
            else
            {
                venda.Status_Venda = StatusVenda.ReceberPagamento;
            }

            venda.AtualizarVenda();
        }

        public void CadastrarConsumo()
        {
            foreach (var item in ListaItens)
            {
                cardapio.CarregarPorId(item.id_cardapio);
                consumo.Cardapio = cardapio;
                consumo.quantidade = int.Parse(item.quantidade.ToString());
                consumo.Valor_total = cardapio.preco_item * item.quantidade;
                venda.CarregarVendaPorMesa(int.Parse(ComandaNumeromesa.Text));
                consumo.Venda = venda;
                consumo.apagado = false;
                consumo.CadastrarConsumo();
            }

        }

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var item = menuItem.CommandParameter as ClasseItensListView;
            ListaItens.Remove(item);
            ListviewComanda.ItemsSource = null;
            Carregar();
        }

        private void Carregar()
        {
            ListviewComanda.ItemsSource = ListaItens;
        }

        public float CalculaSubTotal()
        {
            venda.CarregarVendaPorMesa(int.Parse(ConsultarNumeroMesa.Text));
            return ClasseConsumo.CarregerConsumoPorVenda(venda.id_venda).Sum(x => x.Valor_total);
        }

        public float CalculaTaxaServiço()
        {

            float valor1 = 0, valortaxa = 0;

            valor1 = CalculaSubTotal();
            valortaxa = valor1 * 10 / 100;

            return valortaxa;
        }

        public float CalculaDesconto()
        {
            float valor1, valor2;
            venda.CarregarVendaPorMesa(int.Parse(ConsultarNumeroMesa.Text));
            valor1 = float.Parse(venda.Desconto.ToString());
            valor2 = CalculaSubTotal();

            return valor1 * valor2 / 100;
        }

        public float CalculaCouvert_Artistico()
        {
            venda.CarregarVendaPorMesa(int.Parse(ConsultarNumeroMesa.Text));
            return venda.Numero_pessoa * venda.Couvert_artistico;
        }

        public float CalcularValorTotalPagar()
        {
            float valor1, valor2, valor3, valor4, valortotal;

            valor1 = CalculaSubTotal();
            valor2 = CalculaCouvert_Artistico();
            valor3 = CalculaTaxaServiço();
            valor4 = CalculaDesconto();


            valortotal = (valor1 + valor2 + valor3) - valor4;

            return valortotal;
        }

        #endregion

        #region Consultar

        public void LimparLabel()
        {
            lbTotalProduto.Text = $"Sub-Total R$ 0,00";
            lbValorCouver.Text = $"Couvert Artistico R$ 0,00";
            lbTotalServico.Text = $"Taxa de Serviço R$ 0,00";
            lbValorDesconto.Text = $"Desconto R$ 0,00";

            lbTotal.Text = $"Valor Total R$ 0,00";
        }

        public void PreencherLabel()
        {
            lbTotalProduto.Text = $"Sub-Total R$ {CalculaSubTotal():n}";
            lbValorCouver.Text = $"Couvert Artistico R$ {CalculaCouvert_Artistico():n}";
            lbTotalServico.Text = $"Taxa de Serviço R$ {CalculaTaxaServiço():n}";
            lbValorDesconto.Text = $"Desconto R$ {CalculaDesconto():n}";

            lbTotal.Text = $"Valor Total R$ {CalcularValorTotalPagar():n}";
        }

        public void ConsultarMesa()
        {
            int numero = int.Parse(ConsultarNumeroMesa.Text);
            lista = ClasseConsumo.CarregarConsumoPorMesa(numero);
            ListViewConsultarMesa.ItemsSource = lista;
        }

        private void BTConsultar_Clicked(object sender, EventArgs e)
        {
            ClasseMesa mesa = new ClasseMesa();
            mesa.CarregarMesaPorID(int.Parse(ConsultarNumeroMesa.Text));
            if (mesa.status == StatusMesa.Disponivel)
            {
                DisplayAlert("Aviso", "Mesa não contem nenhum item", "OK");
                LimparLabel();
                ListViewConsultarMesa.ItemsSource = null;
                ConsultarNumeroMesa.Text = String.Empty;
            }
            else
            {
                ConsultarMesa();
                PreencherLabel();
            }

        }

        private void BTFechar_Clicked(object sender, EventArgs e)
        {
            ClasseMesa mesa = new ClasseMesa();
            mesa.CarregarMesaPorID(int.Parse(ConsultarNumeroMesa.Text));
            mesa.status = StatusMesa.Ausente;
            mesa.AtualizarMesa();
            AtualizarVenda(2);
            ListViewConsultarMesa.ItemsSource = null;
            ConsultarNumeroMesa.Text = String.Empty;

        }

        #endregion
    }
}