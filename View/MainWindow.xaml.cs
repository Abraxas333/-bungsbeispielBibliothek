using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using ÜbungsbeispielBibliothek.Controller;
using ÜbungsbeispielBibliothek.Model;

namespace ÜbungsbeispielBibliothek.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly BuchController _controller;
        public MainWindow()
        {
            InitializeComponent();
            _controller = new BuchController();
            LoadBooks();

        }

        private async void LoadBooks()
        {
            try
            {
                string query = "SELECT * FROM BUCH";
                var books = await _controller.GetAllBooksAsync(query);
                Console.WriteLine($"Number of books: {books.Count}"); // Debug count
                foreach (var book in books)
                {
                    Console.WriteLine($"Book: {book.Titel}"); // Debug each book
                }
                
                booksDataGrid.ItemsSource = books;
            }
            catch (Exception ex) { Console.WriteLine($"Error loading books: {ex.Message}"); }

        }

        private void openDetailsWindow(object sender, RoutedEventArgs e)
        {
            var DetailsWindow = new DetailsWindow();
            DetailsWindow.Show();
            DetailsWindow.Closed += (s, args) => Show();
            Hide();
        }

        private async void applyFilter(object sender, RoutedEventArgs e)  // Name muss exakt wie im XAML sein
        {
            try
            {
                string selectedFilter = ((ComboBoxItem)Filter.SelectedItem).Content.ToString();
                var books = await _controller.GetFiltersAsync();
                string filter = TextKeyword.Text;

                List<Buch> filteredBooks = null;

                switch (selectedFilter)
                {
                    case "Books by Price Range":
                        if (float.TryParse(priceMinFilter.Text, out float minPrice) &&
                            float.TryParse(priceMaxFilter.Text, out float maxPrice))
                        {
                            filteredBooks = books.Where(p => p.Preis <= maxPrice && p.Preis >= minPrice).ToList();
                        }
                        else
                        {
                            MessageBox.Show("Please enter valid numbers for price range");
                        }
                        break;

                    case "Books by Genre":
                        filteredBooks = books.Where(g => g.Genre == filter).ToList();
                        break;

                    case "Books by Year":
                        if (int.TryParse(filter, out var year))
                        {
                            filteredBooks = books.Where(y => y.Jahr == year).ToList();
                        }
                        break;

                    case "Newest 5 Books":
                        filteredBooks = books.OrderByDescending(b => b.Jahr).Take(5).ToList();
                        break;
                }

                if (filteredBooks != null)
                {
                    booksDataGrid.ItemsSource = filteredBooks;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filter: {ex.Message}");
            }
        }



    }
}
