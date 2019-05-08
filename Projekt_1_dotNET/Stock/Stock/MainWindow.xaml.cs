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
using System.Windows.Threading;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Data;

namespace StockApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlotModel dataploter { get; set; }
        public LinearAxis LAY;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            this.Model = Plot();
            dataploter = this.Model;
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
            //txtTicks.Text = "TIMER - " + newData;
        }

        public PlotModel Model { get; set; }

        /// <summary>
        /// Creates a model showing normal distributions.
        /// </summary>
        /// <returns>A PlotModel.</returns>
        /// 
        
        public PlotModel Plot()
        {
           
            var plot = new PlotModel
            {
                Title = "Symbol value",
                Subtitle = "From last month"
            };

            LAY = new OxyPlot.Axes.LinearAxis()
            {
                Key = "yaxis",
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 200,
                MajorStep = 10,
                MinorStep = 20,
                TickStyle = TickStyle.Inside
            };
            plot.Axes.Add(LAY);
            var startDate = DateTime.Now.AddDays(-30);
            var endDate = DateTime.Now;

            var minValue = DateTimeAxis.ToDouble(startDate);
            var maxValue = DateTimeAxis.ToDouble(endDate);

            plot.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Minimum =minValue , Maximum = maxValue, StringFormat = "MM/d" });
            
            

            return plot;
        }

        private void plot(List<StockHistory> history)
        {
            dataploter.Series.Clear();
            dataploter.Series.Add(new LineSeries());
            // Create Line
            Axis yAxis = dataploter.Axes.FirstOrDefault(s => s.Key == "yaxis");
            yAxis.Zoom(history[0].Close - 20, history[0].Close + 20);
            for (int i = 0; i < history.Count; i++)
            {
                (dataploter.Series[0] as LineSeries).Points.Add(new DataPoint(history[i].Date.ToOADate(), history[i].Close));
            }
            dataploter.InvalidatePlot(true);
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
                plot(history);
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
                
                }));

            });
        }
    }
}
