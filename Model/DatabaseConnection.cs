using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ÜbungsbeispielBibliothek.Model
{
    internal class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
        }

        private MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public async Task<List<Buch>> GetBooksAsync(string query)
        {
            List<Buch> books = new List<Buch>();
            Console.WriteLine($"DB: Executing query: {query}");
            using (MySqlConnection conn = CreateConnection())
            {
                try
                {
                    await conn.OpenAsync();
                    Console.WriteLine("Connection created successfully!");
                    

                    MySqlCommand mySqlCommand = new MySqlCommand(query, conn);
                    MySqlDataReader reader = await mySqlCommand.ExecuteReaderAsync() as MySqlDataReader;
                    Console.WriteLine("DB: Got reader");
                    while (await reader.ReadAsync())
                    {
                        books.Add(new Buch
                        {
                            BuchID = reader.GetInt32("BuchID"),
                            Titel = reader.GetString("Titel"),
                            Autor = reader.GetString("Autor"),
                            Jahr = reader.GetInt32("Jahr"),
                            Preis = reader.GetFloat("Preis"),
                            Genre = reader.GetString("Genre"),
                            VerlagID = reader.GetInt32("VerlagID"),

                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler: {ex.Message}");
                }
            } return books;
        }


        public async Task CreateBuchAsync(Buch buch)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd =null;

            try
            {
                conn = CreateConnection();
                await conn.OpenAsync();
                Console.WriteLine("Connection created successfully!");
                string query = $"INSERT INTO BUCH(Titel, Autor, Jahr, Preis, Genre, VerlagID) Values('{buch.Titel}', '{buch.Autor}', {buch.Jahr}, {buch.Preis}, '{buch.Genre}', {buch.VerlagID})";
                    
                cmd = new MySqlCommand(query, conn);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                Console.WriteLine($"Affected Rows: {rowsAffected}");

            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (conn != null) conn.Close();
            }

        }
        
        public async Task DeleteBuchAsync(int ID)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;

            try
            {
                conn = CreateConnection();
                await conn.OpenAsync();
                Console.WriteLine("Connection created successfully!");
                string query = $"DELETE FROM BUCH where Buch.BuchID = {ID}";

                cmd = new MySqlCommand(query, conn);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                Console.WriteLine($"Deleted from Buch {rowsAffected} row(s)");

            } catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (conn != null) conn.Close();
            }

        }

        public async Task UpdateBuchAsync(Buch buch)
        {
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            try
            {
                conn = CreateConnection();
                await conn.OpenAsync();
                Console.WriteLine("Connection created successfully!");
                string query = $"UPDATE BUCH SET Titel = '{buch.Titel}', Autor = '{buch.Autor}', Jahr = {buch.Jahr}, Preis={buch.Preis}, Genre='{buch.Genre}', VerlagID={buch.VerlagID} Where BuchID = {buch.BuchID}";


                cmd = new MySqlCommand(query, conn);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                Console.WriteLine($"Affected Rows: {rowsAffected}");
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (conn != null) conn.Close();
            }
        }

    }
}

