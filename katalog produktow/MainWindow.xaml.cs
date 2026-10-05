using System.Text;
using System.Windows;
using System.Globalization;
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
        }

        private void ObliczButton_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdzenie ceny
            if (!double.TryParse(CenaTextBox.Text, out double cena))
            {
                WynikTextBlock.Text = "Błąd: cena musi być liczbą.";
                return;
            }

            // Sprawdzenie ilości
            if (!int.TryParse(IloscTextBox.Text, out int ilosc))
            {
                WynikTextBlock.Text = "Błąd: ilość musi być liczbą całkowitą.";
                return;
            }

            // Sprawdzenie wartości ujemnych
            if (cena < 0 || ilosc < 0)
            {
                WynikTextBlock.Text = "Błąd: cena i ilość nie mogą być ujemne.";
                return;
            }

            // Obliczenie wartości
            double wartosc = cena * ilosc;

            // Wynik
            WynikTextBlock.Text = $"Wartość: {wartosc:C}";
        }


        private void CenaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}