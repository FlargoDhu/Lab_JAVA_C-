using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2
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



        private System.Drawing.Bitmap randomImage;

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
                Selected_Age.Text = m_SelectedPerson.Age.ToString();
                Selected_name.Text = m_SelectedPerson.Name;
                if (m_SelectedPerson.image == null)
                {
                    BoxWithPicture.Source = new BitmapImage(new Uri(m_SelectedPerson.PicturePath));          
                }
                else
                {
                    
                    BoxWithPicture.Source = ImageSourceForBitmap(m_SelectedPerson.image);
                }
                
            }
        }

        
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceForBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
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
                Person person = new Person { Age = int.Parse(ageTextBox.Text), Name = nameTextBox.Text, PicturePath = (PictureBox.Text) };
                if (PictureBox.Text == "random")
                    person.image =  randomImage;
                people.Add(person);
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
                BoxWithPicture.Source = new BitmapImage(
                new Uri(filename));
            }
        }

        public async Task<Bitmap> FindSomeImage()
        {
            var img = new RandomImage().ToBitmap();
            BoxWithPicture.Source = ImageSourceForBitmap(img);

            return img;      
        }
      
        private async void ButtonImageRandom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                randomImage = await FindSomeImage();
                PictureBox.Text = "random";
            }
            catch (Exception)
            {
                MessageBox.Show("RandomImage asyns error");
            }

            
            
        }

    }
}