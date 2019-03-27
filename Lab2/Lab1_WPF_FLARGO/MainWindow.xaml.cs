using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
                    BoxWithPicture.Source = m_SelectedPerson.PicturePath;          
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

        private BitmapSource Bitmap2BitmapImage(Bitmap bitmap)
        {
            BitmapSource i = Imaging.CreateBitmapSourceFromHBitmap(
                           bitmap.GetHbitmap(),
                           IntPtr.Zero,
                           Int32Rect.Empty,
                           BitmapSizeOptions.FromEmptyOptions());
            return (BitmapSource)i;
        }



        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        private async void AddNewPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(ageTextBox.Text) == false && String.IsNullOrWhiteSpace(nameTextBox.Text) == false && String.IsNullOrWhiteSpace(PictureBox.Text) == false)
            {
                Person person = new Person { Age = int.Parse(ageTextBox.Text), Name = nameTextBox.Text, ImageName = PictureBox.Text };
                if (PictureBox.Text == "random")
                {
                    
                    person.image = randomImage;
                    person.PicturePath = Bitmap2BitmapImage(randomImage);
          
                    
                }
                else
                {
                    person.PicturePath = (BitmapSource)(new BitmapImage(new Uri(PictureBox.Text)));
                }
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

        private  Task FindSomeImage(string rand)
        {
            return Task.Factory.StartNew(()=>{
                var _img = new RandomImage(rand).ToBitmap();
                randomImage = _img;
                Dispatcher.BeginInvoke((Action)(() => BoxWithPicture.Source = ImageSourceForBitmap(randomImage)));
            });
        }
        private readonly List<string> _topics = new List<string> { "dog", "car", "truck", "cat", "florida" };
        private async void ButtonImageRandom_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {

                var rnd = new Random();
                int topic = rnd.Next(0, _topics.Count - 1);
                await FindSomeImage(_topics[topic]);

                PictureBox.Text = "random";
                ageTextBox.Text = randomImage.Width.ToString();
                nameTextBox.Text = _topics[topic];
                AddNewPersonButton_Click(sender, e);

            }
            catch (Exception ex)
            {
                MessageBox.Show("RandomImage asyns error:"+ex.Message);
            }

            
            
        }

    }
}