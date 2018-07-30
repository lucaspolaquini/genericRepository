# genericRepository
        Entity Framework Core Generic Repository
        
        
# Sample

## Set up the DbContext
I will create a new MVC template with ASP.NET Core 2, without auth.

Let’s set up DbContext and some sample DB Entity class. Here is the Category Database Entity:

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}


And here is the DbContext:

public class CodingBlastDbContext : DbContext
{
    public CodingBlastDbContext(DbContextOptions<CodingBlastDbContext> options)
        : base(options)
    {
    }
    public DbSet<Category> Categories { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
        
     
You can see we have Categories DbSet which represents a matching table in the database.

We are creating a constructor that accepts DbContextOptions as a parameter. This will enable us to pass in options from ConfigureServices in Startup class:


public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<CodingBlastDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
 
    services.AddMvc();
}

We will add a connection string to appsettings.json so I can actually interact with the database:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=GenericRepository;Trusted_Connection=True;MultipleActiveResultSets=true"
},


## Building generic repository
Just like with Entity Framework 6, in EF Core we use DbContext to query a database and group together changes that will be written back to the store as a unit.

The great thing about DbContext class is that it’s generic and therefore supports generics on methods that we will use to interact with the database.

IGenericRepository interface
The base interface that we will use looks like this:


public interface IGenericRepository<TEntity>
 where TEntity : class, <span class="pl-en">IEntity</span>
{
    IQueryable<TEntity> GetAll();
 
    Task<TEntity> GetById(int id);
 
    Task Create(TEntity entity);
 
    Task Update(int id, TEntity entity);
 
    Task Delete(int id);
}



The first thing you will notice is generic TEntity type, that will be the type of our entity in Database (Category, User, Role, etc.).

We also set a constraint that TEntity needs to be class. We can also use some interface to mark an entity and use that interface in each of entity classes. But we will talk about that later.

GetAll returns IQueryable because we don’t want to return full list. However, we want to return something that caller will be able to use to further process the query. Maybe filter it by something, do paging, etc. That’s no interest of us for now.

Other method signatures are probably just like you would expect them. However, you might notice Task in front of the methods. That is because these methods will be async because we will be making use of Entity Framework’s async support.

Let’s start implementing generic repository:


public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, <span class="pl-en">IEntity</span>
{
    private readonly CodingBlastDbContext _dbContext;
 
    public GenericRepository(CodingBlastDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}


We will have ASP.NET Core inject DbContext for us that’s why we are passing it in as a constructor parameter.

Now you will see that compiler is complaining because we are not implementing IGenericRepository. Time to do that!


public IQueryable<TEntity> GetAll()
{
    return _dbContext.Set<TEntity>().AsNoTracking();
}
        
public async Task<TEntity> GetById(int id)
{
    return await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
}
 
public async Task Create(TEntity entity)
{
    await _dbContext.Set<TEntity>().AddAsync(entity);
    await _dbContext.SaveChangesAsync();
}
 
public async Task Update(int id, TEntity entity)
{
    _dbContext.Set<TEntity>().Update(entity);
    await _dbContext.SaveChangesAsync();
}
 
public async Task Delete(int id)
{
    var entity = await GetById(id);
    _dbContext.Set<TEntity>().Remove(entity);
    await _dbContext.SaveChangesAsync();
}
