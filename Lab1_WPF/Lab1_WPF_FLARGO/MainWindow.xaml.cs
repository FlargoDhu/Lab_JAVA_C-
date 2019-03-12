using Microsoft.Win32;
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

namespace Lab01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Person> people = new ObservableCollection<Person>
        {
            /*new Person { Name = "P1", Age = 1 },
            new Person { Name = "P2", Age = 2 }
        */};

        public ObservableCollection<Person> Items
        {
            get => people;
        }
        private Person m_SelectedPerson;
        public Person SelectedPerson
        {
            get
            {
                return m_SelectedPerson;
            }
            set
            {
                m_SelectedPerson = value;
                BoxWithPicture.Source = new BitmapImage(
                new Uri(m_SelectedPerson.Picture));
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        
        private void AddNewPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(ageTextBox.Text) == false && String.IsNullOrWhiteSpace(nameTextBox.Text) == false && String.IsNullOrWhiteSpace(PictureBox.Text) == false)
            {
                people.Add(new Person { Age = int.Parse(ageTextBox.Text), Name = nameTextBox.Text, Picture = (PictureBox.Text) });
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you a simpleson?",
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    Application.Current.Shutdown();
                }
            }
        }
        private void LoadPicture_Click(object sender, RoutedEventArgs e)
        { 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {

                string filename = dlg.FileName;
                PictureBox.Text = filename;
            }
        }
    }
}