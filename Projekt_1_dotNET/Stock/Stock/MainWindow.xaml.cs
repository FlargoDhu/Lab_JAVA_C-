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
using System.Windows.Threading;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

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

            this.Model = CreateNormalDistributionModel();
            this.DataContext = this;
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

        private Task LoadLogo(string name)
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
        private async void Complain_Click(object sender, RoutedEventArgs e)
        {
   
           await DoWorkTimer();
        }


        System.Windows.Threading.DispatcherTimer _timer = new DispatcherTimer();
        private async Task DoWorkTimer()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(200);
            _timer.Tick += _timer_Tick;
            _timer.IsEnabled = true;
            await Task.Delay(200);
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            // do the work in the loop
            string newData = DateTime.Now.ToLongTimeString();

            // update the UI on the UI thread
            txtTicks.Text = "TIMER - " + newData;
        }

        public PlotModel Model { get; set; }

        /// <summary>
        /// Creates a model showing normal distributions.
        /// </summary>
        /// <returns>A PlotModel.</returns>
        private static PlotModel CreateNormalDistributionModel()
        {
            // http://en.wikipedia.org/wiki/Normal_distribution
            var plot = new PlotModel
            {
                Title = "Normal distribution",
                Subtitle = "Probability density function"
            };

            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = -0.05,
                Maximum = 1.05,
                MajorStep = 0.2,
                MinorStep = 0.05,
                TickStyle = TickStyle.Inside
            });
            plot.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = -5.25,
                Maximum = 5.25,
                MajorStep = 1,
                MinorStep = 0.25,
                TickStyle = TickStyle.Inside
            });
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 0.2));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 1));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 5));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, -2, 0.5));
            return plot;
        }

        private static LineSeries CreateNormalDistributionSeries(double x0, double x1, double mean, double variance, int n = 1000)
        {
            var ls = new LineSeries
            {
                Title = string.Format("μ={0}, σ²={1}", mean, variance)
            };

            for (int i = 0; i < n; i++)
            {
                double x = x0 + ((x1 - x0) * i / (n - 1));
                double f = 1.0 / Math.Sqrt(2 * Math.PI * variance) * Math.Exp(-(x - mean) * (x - mean) / 2 / variance);
                ls.Points.Add(new DataPoint(x, f));
            }

            return ls;
        }
    }
}
