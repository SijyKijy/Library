using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WpfLibrary.Models;

namespace WpfLibrary.Services
{
    public class ApiService
    {
        private string _json;
        private readonly string _apiPath = "api/books/";

        private readonly HttpClient _client;

        public ApiService(Uri apiUri)
        {
            ServicePointManager.DefaultConnectionLimit = 4;

            _client = new HttpClient()
            {
                BaseAddress = apiUri
            };
        }

        ~ApiService() => _client.Dispose();

        private void DbError()
        {
            MessageBox.Show("Проблемы с подключением к БД", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            Environment.Exit(0);
        }

        public async Task<IEnumerable<Book>> GetBooks(int count = 10)
        {
            try
            {
                _json = await _client.GetAsync($"{_client.BaseAddress}{_apiPath}?count={count}").Result.Content.ReadAsStringAsync();
            }
            catch { DbError(); }

            return JsonSerializer.Deserialize<IEnumerable<Book>>(_json);
        }

        public async Task CreateBook(Book book)
        {
            _json = JsonSerializer.Serialize(book);
            StringContent content = new StringContent(_json, Encoding.UTF8, "application/json");

            try
            {
                await _client.PostAsync($"{_client.BaseAddress}{_apiPath}", content);
            }
            catch { DbError(); }
        }

        public async Task EditBook(Book book)
        {
            _json = JsonSerializer.Serialize(book);
            StringContent content = new StringContent(_json, Encoding.UTF8, "application/json");

            try
            {
                await _client.PutAsync($"{_client.BaseAddress}{_apiPath}", content);
            }
            catch { DbError(); }
        }

        public async Task DeleteBook(Book book)
        {
            try
            {
                await _client.DeleteAsync($"{_client.BaseAddress}{_apiPath}{book.Id}");
            }
            catch { DbError(); }
        }
    }
}
