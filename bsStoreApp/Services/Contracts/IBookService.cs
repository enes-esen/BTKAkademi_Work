using System;
using Entities.Models;

namespace Services.Contracts
{
	public interface IBookService
	{
		IEnumerable<Book> GetAllBooks(bool trackChanges);

		//Bir kitaba ihtiyacımız var
		Book GetOneBookById(int id, bool trackChanges);

		//Kitap oluşturulduğunda geriye bir kitap döndürmek için
		Book CreateOneBook(Book book);

		//Güncellemede bir şey dönmemesi için void
		void UpdateOneBook(int id, Book book, bool trackChanges);

		//Silme işlemi
		void DeleteOneBook(int id, bool trackChanges);
	}
}

