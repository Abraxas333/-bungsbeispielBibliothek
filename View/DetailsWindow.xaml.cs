using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using ÜbungsbeispielBibliothek.Controller;
using ÜbungsbeispielBibliothek.Model;

namespace ÜbungsbeispielBibliothek.View
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        private Buch book;
        private readonly BuchController _controller;
        public DetailsWindow()
        {
            InitializeComponent();
            _controller = new BuchController();

        }

       

        private async void SaveButton(object sender, RoutedEventArgs e)
        {
            string titel = TextTitel.Text;
            string autor = TextAutor.Text;
            string jahr = TextJahr.Text;
            string preis = TextPreis.Text;
            string genre = TextGenre.Text;
            string verlagID = TextVerlagID.Text;

            Buch book = new Buch();
            book.Titel = titel;
            TextTitel.Clear();

            book.Autor = autor;
            TextAutor.Clear();
            
            if (Int32.TryParse(jahr, out int j) == false)
            {
                MessageBox.Show("Please input an integer for Jahr");
                TextJahr.Clear();
            }
            else book.Jahr = j;
            TextJahr.Clear();

            if (float.TryParse(preis, out float p) == false)
            {
                MessageBox.Show("Please input a number for Preis");
                TextPreis.Clear();
            }
            else book.Preis = p;
            TextPreis.Clear();

            book.Genre = genre;
            TextGenre.Clear();

            if (int.TryParse(verlagID, out int v) == false)
            {
                MessageBox.Show("Please input a number for VerlagID");
                TextVerlagID.Clear();
            }
            else book.VerlagID = v;
            TextVerlagID.Clear();

            await _controller.InsertBookAsync(book);

        }

        private async void DeleteButton(object sender, RoutedEventArgs e)
        {
            int id = 0;
            string ID = TextID.Text;
            if (string.IsNullOrEmpty(ID) == true)
            {
                MessageBox.Show("Need ID of Buch to delete");
            }
            else if (int.TryParse(ID, out int i) == false)
            {
                MessageBox.Show("Please enter a number for ID");
                TextID.Clear();
            }
            else id = i;

            if (id != 0)
            {
                await _controller.DeleteBookAsync(id);

            }
        }

        
        private async void UpdateButton(object sender, RoutedEventArgs e)
        {
            // First, validate ID and get existing book
            if (string.IsNullOrEmpty(TextID.Text) || !int.TryParse(TextID.Text, out int bookId))
            {
                MessageBox.Show("Please enter a valid ID");
                return;
            }

            // Get existing book
            string query = $"SELECT * FROM Buch WHERE BuchID = {bookId}";
            var existingBook = (await _controller.GetAllBooksAsync(query)).FirstOrDefault();

            if (existingBook == null)
            {
                MessageBox.Show("Book not found!");
                return;
            }

            // Validate all inputs before updating
            if (!int.TryParse(TextJahr.Text, out int jahr))
            {
                MessageBox.Show("Please input a valid year");
                return;
            }

            if (!float.TryParse(TextPreis.Text, out float preis))
            {
                MessageBox.Show("Please input a valid price");
                return;
            }

            if (!int.TryParse(TextVerlagID.Text, out int verlagId))
            {
                MessageBox.Show("Please input a valid publisher ID");
                return;
            }

            // Create updated book object
            var updatedBook = new Buch
            {
                BuchID = bookId,
                Titel = TextTitel.Text,
                Autor = TextAutor.Text,
                Jahr = jahr,
                Preis = preis,
                Genre = TextGenre.Text,
                VerlagID = verlagId
            };

            try
            {
                // Call controller to update book
                await _controller.UpdateAsync(updatedBook);
                MessageBox.Show("Book updated successfully!");

                // Clear all fields after successful update
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating book: {ex.Message}");
            }
        }

        private void ClearFields()
        {
            TextID.Clear();
            TextTitel.Clear();
            TextAutor.Clear();
            TextJahr.Clear();
            TextPreis.Clear();
            TextGenre.Clear();
            TextVerlagID.Clear();
        }

        private async void RetrieveButton(object sender, RoutedEventArgs e)
        {
            Buch book = new Buch();
            List<Buch> list = new List<Buch>();
            int id = 0;
            string ID = TextID.Text;
            if (string.IsNullOrEmpty(ID) == true)
            {
                MessageBox.Show("Need ID of Buch to delete");
            }
            else if (int.TryParse(ID, out int i) == false)
            {
                MessageBox.Show("Please enter a number for ID");
                TextID.Clear();
            }
            else id = i;

            if (id != 0)
            {
                string query = $"SELECT * FROM Buch WHERE Buch.BuchID = '{id}'";

                list = await _controller.GetAllBooksAsync(query);
                book = list.FirstOrDefault();
                if (book != null)
                {
                    TextTitel.Text = book.Titel;
                    TextAutor.Text = book.Autor;
                    TextJahr.Text = book.Jahr.ToString();
                    TextPreis.Text = book.Preis.ToString();
                    TextGenre.Text = book.Genre;
                    TextVerlagID.Text = book.VerlagID.ToString();

                }
                else
                {
                    Console.WriteLine("Book not found!");
                }
            }
        }
    }
}
