using StockApp.DataServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

          
            foreach (var item in new DataServices.SymbolList().GetList())
            {
                symbols.Add(item);
            }
        }

        /// <summary>
        /// List Symboli wyswietlonych na ekranie
        /// </summary>
        private ObservableCollection<Symbol> symbols = new ObservableCollection<Symbol>();

        
     


        private Task LoadStock(string symbol)
        {
            return Task.Factory.StartNew(async () => {
                string data = await StockConnection.LoadDataAsync(symbol);
                Stock stock = StockReader.Read(data);
                await Dispatcher.BeginInvoke((Action)(() =>
                {
                    SelectedName.Text = stock.CompanyName;
                    SelectedSymbol.Text = stock.Symbol;
                    SelectedPrice.Text = stock.priceToSales.ToString();
                    AddSymbol(stock);
                    LoadLogo(stock.CompanyName);
                }));
                
            });
        }
        private void AddSymbol(Stock stock)
        {
            
                var service = new DataServices.AddPrice(stock);
                if (service.New())
                    symbols.Add(service.item);
           
        }
        private async void LoadPrice_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                await LoadStock(SymbolSelectBox.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private  Task LoadLogo(string name)
        {
            return Task.Factory.StartNew(() => {
                var finder = new Logo() { Topic = name };
                finder.FindLogo();
                Dispatcher.BeginInvoke((Action)(() => LogoImage.Source = finder.GetLogo()));
            });
            
        }

        private Task DisplaySymbols()
        {
            return Task.Factory.StartNew(() => {
                

                
                Dispatcher.BeginInvoke((Action)(() => { DataBox.ItemsSource = symbols; }));

            });
            
        }

        private void DataBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void SymbolsButton_Click(object sender, RoutedEventArgs e)
        {
            await DisplaySymbols();
        }

        private async void SymbolHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await AddHistory(SymbolSelectBox.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Task AddHistory(string symbol)
        {
            return Task.Factory.StartNew(async () => {
                string data = await StockHistoryConnection.LoadDataAsync(symbol);
                List<StockHistory> history = StockHistoryReader.Read(data);
                var service = new AddHistory(symbol);
                service.Add(history);
               
                

            });
        }

        private async void ShowSymbolHistory_Click(object sender, RoutedEventArgs e)
        {
            await ShowHistory(SymbolSelectBox.Text);

        }
        private Task ShowHistory(string symbol)
        {
            return Task.Factory.StartNew(async () => {
                var finder = new HistoryList(symbol);
                var list = finder.GetList();

                await Dispatcher.BeginInvoke((Action)(() =>
                {
                    //SelectedName.Text = stock.CompanyName;
                    // SelectedSymbol.Text = stock.Symbol;
                    //SelectedPrice.Text = stock.priceToSales.ToString();
                    //AddSymbol(stock);
                    //LoadLogo(stock.CompanyName);
                }));

            });
        }
    }
}
