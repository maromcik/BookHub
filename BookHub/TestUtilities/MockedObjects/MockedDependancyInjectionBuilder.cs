using BusinessLayer.Services;
using DataAccessLayer;
using Microsoft.Extensions.DependencyInjection;

namespace TestUtilities.MockedObjects
{
    public class MockedDependencyInjectionBuilder
    {
        private IServiceCollection _serviceCollection = new ServiceCollection();

        public MockedDependencyInjectionBuilder AddMockedDbContext()
        {
            _serviceCollection = _serviceCollection
                .AddDbContext<BookHubDbContext>(options => options
                    .UseInMemoryDatabase(MockedDbContext.RandomDbName));

            return this;
        }

        public MockedDependencyInjectionBuilder AddScoped<T>(T objectToRegister)
            where T : class
        {
            _serviceCollection = _serviceCollection
                .AddScoped<T>(_ => objectToRegister);

            return this;
        }
        
        public MockedDependencyInjectionBuilder AddServices()
        {
            _serviceCollection = _serviceCollection
                .AddScoped<IUserService, UserService>()
                .AddScoped<IAuthorService, AuthorService>()
                .AddScoped<IGenreService, GenreService>()
                .AddScoped<IBookService, BookService>()
                .AddScoped<IPublisherService, PublisherService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<IRatingService, RatingService>();
            return this;
        }
        
        public ServiceProvider Create()
        {
            return _serviceCollection.BuildServiceProvider();
        }
    }
}