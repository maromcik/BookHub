using BookHub.Models;

namespace DataAccessLayer.Entities;

public class Publisher : BaseEntity, IModelRelated
{
    public string Name { get; set; }

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}