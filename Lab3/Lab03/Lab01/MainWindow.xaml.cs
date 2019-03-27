using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Lab01
{
    public partial class MainWindow : Window
    {
        BackgroundWorker worker = new BackgroundWorker();

        async Task<int> GetNumberAsync(int number)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException("number", number, "The number must be greater or equal zero");
            int result = 0;
            while (result < number)
            {
                result++;
                await Task.Delay(100);
            }
            return number;
        }

        protected void UpdateProgressBlock(string text, TextBlock textBlock)
        {
            int a = 2;
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    textBlock.Text = text + a.ToString();
                });
            }
            catch { } 
        }

        class WaitingAnimation
        {
            private int maxNumberOfDots;
            private int currentDots;
            private MainWindow sender;
            

            public WaitingAnimation(int maxNumberOfDots, MainWindow sender)
            {
                this.maxNumberOfDots = maxNumberOfDots;
                this.sender = sender;
                currentDots = 0;
            }

            public void CheckStatus(Object stateInfo)
            {
                sender.UpdateProgressBlock(
                    "Processing" + 
                    new Func<string>(() => {
                        StringBuilder strBuilder = new StringBuilder(string.Empty);
                        for (int i = 0; i < currentDots; i++)
                            strBuilder.Append(".");
                        return strBuilder.ToString();
                    })(), sender.progressTextBlock
                );
                if (currentDots == maxNumberOfDots)
                    currentDots = 0;
                else
                    currentDots++;
            }
        }

        ObservableCollection<Person> people = new ObservableCollection<Person>
        {
            new Person { Name = "P1", Age = 1 },
            new Person { Name = "P2", Age = 2 }
        };

        public ObservableCollection<Person> Items
        {
            get => people;
        }

        public void AddPerson(Person person)
        {
            Application.Current.Dispatcher.Invoke(() => { Items.Add(person); });
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
        }
        
        private void AddNewPersonButton_Click(object sender, RoutedEventArgs e)
        {
            people.Add(new Person { Age = int.Parse(ageTextBox.Text), Name = nameTextBox.Text });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int finalNumber = int.Parse(this.finalNumberTextBox.Text);
                var getResultTask = GetNumberAsync(finalNumber);
                var waitingAnimationTask = 
                    new System.Threading.Timer(
                        new WaitingAnimation(10, this).CheckStatus, 
                        null,
                        TimeSpan.FromMilliseconds(0), 
                        TimeSpan.FromMilliseconds(500)
                    );
                var waitingAnimationTask2 = new System.Timers.Timer(100);
                waitingAnimationTask2.Elapsed += 
                    (innerSender, innerE) => {
                        this.UpdateProgressBlock(
                            innerE.SignalTime.ToLongTimeString(),
                            this.progressTextBlock2);
                    };
                waitingAnimationTask2.Disposed +=
                    (innerSender, innerE) => {
                        this.progressTextBlock2.Text = "Koniec świata";
                    };
                waitingAnimationTask2.Start();
                int result = await getResultTask;
                waitingAnimationTask.Dispose();
                waitingAnimationTask2.Dispose();
                this.progressTextBlock.Text = "Obtained result: " + result;
            }
            catch(Exception ex)
            {
                this.progressTextBlock.Text = "Error! " + ex.Message;
            }
            
        }

        private async void LoadWeatherData(object sender, RoutedEventArgs e)
        {
            string responseXML = await WeatherConnection.LoadDataAsync("London");
            WeatherDataEntry result;

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(responseXML)))
            {
                result = ParseWeather_XmlReader.Parse(stream);
                Items.Add(new Person() {
                    Name = "StreamParser: " + result.City,
                    Age = (int)Math.Round(result.Temperature)
                });
            }

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(responseXML)))
            {
                result = ParseWeather_LINQ.Parse(stream);
                Items.Add(new Person() {
                    Name = "Linq: " + result.City,
                    Age = (int)Math.Round(result.Temperature)
                });
            }

            if (worker.IsBusy != true)
                worker.RunWorkerAsync();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            weatherDataProgressBar.Value = e.ProgressPercentage;
            weatherDataTextBlock.Text = e.UserState as string;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            List<string> cities = new List<string> {
                "London", "Warsaw", "Paris", "London", "Warsaw" };
            for (int i = 1; i <= cities.Count; i++)
            {
                string city = cities[i - 1];

                if (worker.CancellationPending == true)
                {
                    worker.ReportProgress(0, "Cancelled");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    worker.ReportProgress(
                        (int)Math.Round((float)i * 100.0 / (float)cities.Count), 
                        "Loading " + city + "...");
                    string responseXML = WeatherConnection.LoadDataAsync(city).Result;
                    WeatherDataEntry result;

                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(responseXML)))
                    {
                        result = ParseWeather_XmlReader.Parse(stream);
                        AddPerson(
                            new Person() {
                                Name = "StreamParser: " + result.City,
                                Age = (int)Math.Round(result.Temperature)
                            });
                    }
                    Thread.Sleep(2000);
                }
            }
            worker.ReportProgress(100, "Done");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (worker.WorkerSupportsCancellation == true)
            {
                weatherDataTextBlock.Text = "Cancelling...";
                worker.CancelAsync();
            }
        }
    }
}