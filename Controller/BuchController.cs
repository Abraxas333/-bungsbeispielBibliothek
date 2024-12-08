using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ÜbungsbeispielBibliothek.Model;

namespace ÜbungsbeispielBibliothek.Controller
{
    internal class BuchController
    {
        private readonly DatabaseConnection _connection;


        public BuchController()
        {
            _connection = new DatabaseConnection();
        }

        public async Task<List<Buch>>GetAllBooksAsync(string query)
        {
            List<Buch> books = new List<Buch>();
            Console.WriteLine("Controller: Getting books");
            
            books = await _connection.GetBooksAsync(query);
            Console.WriteLine($"Controller: Got {books?.Count ?? 0} books");
            return books;

        }

        public async Task InsertBookAsync(Buch buch)
        {
            await _connection.CreateBuchAsync(buch);
        }

        public async Task DeleteBookAsync(int ID)
        {
            await _connection.DeleteBuchAsync(ID);
        }
        public async Task UpdateAsync(Buch buch)
        {
            await _connection.UpdateBuchAsync(buch);
        }

        public async Task<List<Buch>> GetFiltersAsync()
        {
            List<Buch> books = new List<Buch>();
            string query = "Select * from Buch;";
            books = await _connection.GetBooksAsync(query);

            return books;

        }
    }
}
