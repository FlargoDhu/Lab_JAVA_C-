using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    textBlock.Text = text;
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

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
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
    }
}