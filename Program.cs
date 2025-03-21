using OnlineLibrary.Models;
using OnlineLibrary.Models.Book;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Models.Repositories.Book;
using OnlineLibrary.Models.Repositories.User;
using OnlineLibrary.Models.Repositories.Role;
using OnlineLibrary.Models.Repositories.Category;
using OnlineLibrary.Models.Repositories.Wishlist;
using OnlineLibrary.Models.Repositories.ReadBooks;
using OnlineLibrary.Models.Repositories.UnitOfWork;
using OnlineLibrary.Models.Repositories.ReadingHistory;
using OnlineLibrary.Storage;
using OnlineLibrary.Models.ReadingHistory;
using OnlineLibrary.Models.Wishlist;
using OnlineLibrary.Models.ReadBooks;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                         
                      });
});

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IReadingHistoryRepository, ReadingHistoryRepository>();
builder.Services.AddScoped<IReadingHistoryService, ReadingHistoryService>();

builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
builder.Services.AddScoped<IWishlistService, WishlistService>();

builder.Services.AddScoped<IReadBooksRepository, ReadBooksRepository>();
builder.Services.AddScoped<IReadBooksService, ReadBooksService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBookStorageService, BookStorageService>();
builder.Services.AddScoped<IFileStorageService, BlobStorageService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
