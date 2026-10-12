using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KatalogProduktow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            KategorieComboBox.ItemsSource = new string[] { "Elektronika", "Odzież", "Książki", "Meble"};
            KategorieComboBox.SelectedIndex = 0;
        }

        private void Oblicz(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(CenaTextBox.Text, out double cena))
            {
                WynikTextBlock.Text = "Niepoprawna wartość ceny";
                return;
            }

            if (!int.TryParse(IloscTextBox.Text, out int ilosc))
            {
                WynikTextBlock.Text = "Niepoprawna wartość ilości";
                return;
            }

            if (cena < 0 || ilosc < 0)
            {
                WynikTextBlock.Text = "Cena i ilość nie mogą być ujemne";
                return;
            }

            double wartoscZamowienia = cena * ilosc;

            if (PromocjaCheckBox.IsChecked == true)
            {
                wartoscZamowienia *= 0.9;
            }

            WynikTextBlock.Text =
                $"Nazwa produktu: {NazwaTextBox.Text}\n" +
                $"Kategoria: {KategorieComboBox.Text}\n" +
                $"Cena: {cena:F2} zł\n" +
                $"Ilość: {ilosc}\n" +
                $"Wartość zamówienia: {wartoscZamowienia:F2} zł";
        }
    }
}