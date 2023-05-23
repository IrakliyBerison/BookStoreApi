using BookStoreApi.Models;

namespace BookStoreApi.Repositories
{
    public interface IBookRepository
    {
        /// <summary>
        /// 2. Получение книг по названию (при вводе неполного имени также должен находиться результат)
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Task<List<Book>> GetBookByTitleAsync(string title);

        /// <summary>
        /// 3. Получение списка всех книг с полным именем автора.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public Task<object> GetAllBooksWithAuthorFullNameAsync();


        /// <summary>
        /// 4. Создание книги (название книги и автор обязательные поля)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="surname"></param>
        /// <returns></returns>
        public Task<Book> CreateBooksWithAuthorFullNameAsync(string title, string firstName, string lastName, string? surname);


        /// <summary>
        /// 7. метод удаления книги (Если не найден автор, выводить текст ошибки "не найдена книга")
        /// ** Если не найден автор - предлагаю эту ошибку отсеивать на шаге поиска книги
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public Task DeleteBookAsync(Book book);

        /// <summary>
        /// 8. Поиск книг по названию или Имени, или Фамилии или Отчеству автора
        /// </summary>
        /// <returns></returns>
        public Task<List<Book>> SearchBookByKeyValuesAsync(string value);


        /// <summary>
        /// 9. Метод изменения статуса книги с любого значения на "в наличии" по идентификатору книги (если книга не найдена выводит ошибку). 
        /// При смене статуса в поле логин пользователя должен быть записан логин пользователя, отправившего запрос.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public Task<Book> SetBookStateToExistAsync(Book book, User user);

        /// <summary>
        /// 10. Метод изменения статуса книги с "в наличии" на "продана" по идентификатору книги(если книга не найдена выводит ошибку).  
        /// При смене статуса в поле логин пользователя должен быть записан логин пользователя, отправившего запрос.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public Task<Book> SetBookStateToSoldAsync(Book book, User user);


        public Task<Book> GetBookByIdAsync(Guid Id);
    }

}
