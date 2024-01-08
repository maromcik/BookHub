using BusinessLayer.Errors;
using BusinessLayer.Models;
using BusinessLayer.Services;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Facades;

public class BookFacade
{
    private readonly ILogger<BookFacade> _logger;
    private readonly IBookService _bookService;
    private readonly IRatingService _ratingService;
    private readonly IUserService _userService;

    public BookFacade(ILogger<BookFacade> logger, IBookService bookService, IRatingService ratingService, IUserService userService)
    {
        _logger = logger;
        _bookService = bookService;
        _ratingService = ratingService;
        _userService = userService;
    }

    public async Task<IEnumerable<BookDetail>> GetAllBooks()
    {
        var books = await _bookService.GetBooksAsync(null, null, null, null, null, null, null);
        return books;
    }
    
    public async Task<IEnumerable<BookDetail>> GetSearchBooks(string? query)
    {
        var books = await _bookService.GetSearchBooksAsync(query);
        return books;
    }
    
    public async Task<Result<BookDetail, (Error err, string message)>> AddNewBook(BookCreate model)
    {
        return await _bookService.CreateBookAsync(model);
    }
}