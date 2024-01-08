namespace BusinessLayer.Models;

public class SearchType
{
    public IEnumerable<AuthorDetail> Authors { get; }
    public IEnumerable<BookDetail> Books { get; }
    public IEnumerable<GenreDetail> Genres { get; }
    public IEnumerable<PublisherDetail> Publishers { get; }
    public IEnumerable<RatingDetail> Ratings { get; }

    public SearchType(IEnumerable<AuthorDetail> authors, IEnumerable<BookDetail> books, IEnumerable<GenreDetail> genres,
        IEnumerable<PublisherDetail> publishers, IEnumerable<RatingDetail> ratings)
    {
        Authors = authors;
        Books = books;
        Genres = genres;
        Publishers = publishers;
        Ratings = ratings;
    }
}