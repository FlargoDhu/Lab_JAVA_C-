using System;
using System.Collections.Generic;
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

            
        }
       private  Task LoadStock(string symbol)
        {
            return Task.Factory.StartNew(async () => {
                string data = await StockConnection.LoadDataAsync(symbol);
                Stock stock = StockReader.Read(data);
                await Dispatcher.BeginInvoke((Action)(() =>
                {
                    CompanyBox.Text = stock.CompanyName;
                    SymbolBox.Text = stock.Symbol;
                    PriceBox.Text = stock.priceToSales.ToString();
                }));

            });
        }
        private async void LoadPrice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await LoadStock(SymbolSelectBox.Text);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }
    }
}
