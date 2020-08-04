using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfLibrary.Models;
using WpfLibrary.Services;

namespace WpfLibrary.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Book> Books { get; set; }
        public int Count { get; set; } = 10;

        private Book _book;
        public Book Book
        {
            get => _book;
            set
            {
                _book = value;
                OnPropertyChanged("Book");
            }
        }
        private readonly ApiService _api;

        public MainViewModel()
        {
            _api = new ApiService(new Uri(ConfigurationManager.ConnectionStrings["API"].ConnectionString));
            Books = new ObservableCollection<Book>(_api.GetBooks().Result);

            Book = Books.FirstOrDefault();
        }

        private void UpdateBooks(IEnumerable<Book> books)
        {
            Books.Clear();
            foreach (var book in books)
                Books.Add(book);

            Book = Books.FirstOrDefault();
        }

        public ICommand GetBooks
        {
            get
            {
                return new DelegateCommand(_ =>
                {
                    UpdateBooks(_api.GetBooks(Count < 0 ? 0 : Count).Result);
                });
            }
        }

        public ICommand AddBook
        {
            get
            {
                return new DelegateCommand(async _ =>
                {
                    _book.Id = 0;
                    await _api.CreateBook(_book);
                    GetBooks.Execute(null);
                }, _ => !string.IsNullOrEmpty(Book.Title.Trim(' ')));
            }
        }

        public ICommand EditBook
        {
            get
            {
                return new DelegateCommand(async _ =>
                {
                    await _api.EditBook(_book);
                    GetBooks.Execute(null);
                }, _ => !string.IsNullOrEmpty(Book.Title.Trim(' ')));
            }
        }

        public ICommand DeleteBook
        {
            get
            {
                return new DelegateCommand(async _ =>
                {
                    await _api.DeleteBook(_book);
                    GetBooks.Execute(null);
                }, _ => Books.Count != 0);

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
