using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace KatalogProduktow
{
    public partial class MainWindow : Window
    {
        private const double RabatPromocji = 0.9; // 10% zniżki
        private static readonly CultureInfo Kultura = new CultureInfo("pl-PL");

        public ObservableCollection<Produkt> produkty { get; set; } = new ObservableCollection<Produkt>();

        public MainWindow()
        {
            InitializeComponent();
            KategorieComboBox.ItemsSource = new string[] { "Elektronika", "Odzież", "Książki", "Meble" };
            KategorieComboBox.SelectedIndex = 0;
            produkty.Add(new Produkt { Nazwa = "Laptop", Cena = 3500, Ilosc = 5, Kategoria = "Elektronika" });
            produkty.Add(new Produkt { Nazwa = "Koszula", Cena = 120, Ilosc = 10, Kategoria = "Odzież" });
            produkty.Add(new Produkt { Nazwa = "Biurko", Cena = 450, Ilosc = 3, Kategoria = "Meble" });

            DataContext = this;
        }

        // cena przecinek lub kropka nie ujemna bez NaN
        private static bool TryParseCena(string tekst, out double cena)
        {
            tekst = tekst.Trim().Replace('.', ',');
            return double.TryParse(tekst, NumberStyles.Float, Kultura, out cena)
                && !double.IsNaN(cena) && !double.IsInfinity(cena) && cena >= 0;
        }

        private static bool TryParseIlosc(string tekst, out int ilosc)
        {
            return int.TryParse(tekst.Trim(), NumberStyles.Integer, Kultura, out ilosc) && ilosc >= 0;
        }

        // wspulna walidacja dla dodaj i zapisz komunikat trafia do okna
        private bool Waliduj(out string nazwa, out double cena, out int ilosc)
        {
            nazwa = NazwaTextBox.Text.Trim();
            cena = 0;
            ilosc = 0;

            if (string.IsNullOrEmpty(nazwa))
            {
                WynikTextBlock.Text = "Podaj nazwę produktu";
                return false;
            }
            if (!TryParseCena(CenaTextBox.Text, out cena))
            {
                WynikTextBlock.Text = "Niepoprawna wartość ceny";
                return false;
            }
            if (!TryParseIlosc(IloscTextBox.Text, out ilosc))
            {
                WynikTextBlock.Text = "Niepoprawna wartość ilości";
                return false;
            }
            return true;
        }

        private void Oblicz(object sender, RoutedEventArgs e)
        {
            if (!TryParseCena(CenaTextBox.Text, out double cena))
            {
                WynikTextBlock.Text = "Niepoprawna wartość ceny";
                return;
            }
            if (!TryParseIlosc(IloscTextBox.Text, out int ilosc))
            {
                WynikTextBlock.Text = "Niepoprawna wartość ilości";
                return;
            }

            string kategoria = KategorieComboBox.SelectedItem as string ?? "brak kategorii";

            double wartoscZamowienia = cena * ilosc;

            if (PromocjaCheckBox.IsChecked == true)
            {
                wartoscZamowienia *= RabatPromocji;
            }

            WynikTextBlock.Text = $"{NazwaTextBox.Text} ({kategoria}): {wartoscZamowienia:F2} zł";
        }

        private void DodajProdukt(object sender, RoutedEventArgs e)
        {
            if (!Waliduj(out string nazwa, out double cena, out int ilosc)) return;

            string kategoria = KategorieComboBox.SelectedItem as string ?? "brak kategorii";

            produkty.Add(new Produkt { Nazwa = nazwa, Cena = cena, Ilosc = ilosc, Kategoria = kategoria });

            WynikTextBlock.Text = $"Dodano produkt: {nazwa}";

            // czyszczenie pól po dodaniu
            NazwaTextBox.Clear();
            CenaTextBox.Clear();
            IloscTextBox.Clear();
            PromocjaCheckBox.IsChecked = false;
        }

        private void UsunProdukt(object sender, RoutedEventArgs e)
        {
            if (ListaProduktow.SelectedItem is Produkt wybranyProdukt)
            {
                MessageBoxResult wynik =
                    MessageBox.Show($"Czy na pewno chcesz usunąć produkt: {wybranyProdukt.Nazwa}?",
                    "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (wynik == MessageBoxResult.Yes)
                {
                    produkty.Remove(wybranyProdukt);
                    WynikTextBlock.Text = $"Usunięto produkt: {wybranyProdukt.Nazwa}";
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano produktu do usunięcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PokazZaznaczony(object sender, SelectionChangedEventArgs e)
        {
            if (ListaProduktow.SelectedItem is not Produkt p) return;

            NazwaTextBox.Text = p.Nazwa;
            CenaTextBox.Text = p.Cena.ToString("F2", Kultura);
            IloscTextBox.Text = p.Ilosc.ToString();
            KategorieComboBox.SelectedItem = p.Kategoria;
        }

        private void Zapisz(object sender, RoutedEventArgs e)
        {
            if (ListaProduktow.SelectedItem is not Produkt p)
            {
                WynikTextBlock.Text = "Najpierw wybierz produkt z listy";
                return;
            }

            if (!Waliduj(out string nazwa, out double cena, out int ilosc)) return;

            p.Nazwa = nazwa;
            p.Cena = cena;
            p.Ilosc = ilosc;
            p.Kategoria = KategorieComboBox.SelectedItem as string ?? p.Kategoria;

            WynikTextBlock.Text = $"Zapisano zmiany dla produktu: {p.Nazwa}";
        }
    }
}