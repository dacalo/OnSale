using Bogus;
using Microsoft.EntityFrameworkCore;
using OnSale.Common.Entities;
using OnSale.Common.Enums;
using OnSale.Common.Helpers;
using OnSale.Common.Models;
using OnSale.Common.Services;
using OnSale.Web.Data.Entities;
using OnSale.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnSale.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IApiService _apiService;
        private readonly IFilesHelper _filesHelper;
        private readonly Random _random;

        public SeedDb(
            DataContext context,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IApiService apiService,
            IFilesHelper filesHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _apiService = apiService;
            _filesHelper = filesHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUsersAsync();
            //await CheckUserAsync("CALD7808244AA", "David", "Chávez", "divadchl@gmail.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);
            await CheckCategories();
            await CheckProductsAsync();
        }

        private async Task CheckCategories()
        {
            if (!_context.Categories.Any())
            {
                byte[] data;
                List<Category> list = new List<Category>
                {
                    new Category { Name = "Zapatería", UrlImage = GenerateImage() },
                    new Category { Name = "Ropa", UrlImage = GenerateImage() },
                    new Category { Name = "Tecnología", UrlImage = GenerateImage() },
                    new Category { Name = "Electrodomésticos", UrlImage = GenerateImage() },
                    new Category { Name = "Joyería", UrlImage = GenerateImage() }
                };

                await _context.Categories.AddRangeAsync(list);
                await _context.SaveChangesAsync();

                foreach (Category item in list)
                {
                    using (WebClient webClient = new WebClient())
                    {
                        data = webClient.DownloadData(item.UrlImage);
                    }
                    item.UrlImage = await _blobHelper.SaveFile(data, "categories");
                }
            }
        }

        private string GenerateImage()
        {
            Faker faker = new Faker();
            return faker.Image.LoremFlickrUrl();
        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                User user = await _userHelper.GetUserAsync("buyer1@yopmail.com");
                Category mascotas = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Mascotas");
                byte[] data;
                Faker<Product> product = new Faker<Product>("es_MX")
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                    .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(100, 10000)))
                    .RuleFor(p => p.IsActive, f => true)
                    .RuleFor(p => p.IsStarred, f => f.Random.Bool())
                    .RuleFor(p => p.Qualifications, f =>
                    {
                        return GetRandomQualifications(f.Commerce.ProductDescription(), user);
                    });

                List<Product> listProducts = product.Generate(10);
                List<Product> list = new List<Product>();
                Random rnd = new Random();
                foreach (Product item in listProducts)
                {
                    Faker<ProductImage> images = new Faker<ProductImage>()
                        .RuleFor(pi => pi.UrlImage, f => f.Image.PlaceholderUrl(100, 100));
                    List<ProductImage> listImages = images.Generate(2);
                    item.ProductImages = listImages;
                    foreach (ProductImage image in listImages)
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            data = webClient.DownloadData(image.UrlImage);
                        }
                        image.UrlImage = await _blobHelper.SaveFile(data, "products");
                    }
                    item.Category = await _context.Categories.FindAsync(rnd.Next(1, 5));
                    list.Add(item);
                }
                await _context.Products.AddRangeAsync(list);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        /*
        private async Task<User> CheckUserAsync(
            string rfc,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    RFC = rfc,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }*/

        private async Task CheckUsersAsync()
        {
            if (!_context.Users.Any())
            {
                await CheckAdminsAsync();
                await CheckBuyersAsync();
            }
        }

        private async Task CheckBuyersAsync()
        {
            for (int i = 1; i <= 20; i++)
            {
                await CheckUserAsync($"200{i}", $"buyer{i}@yopmail.com", UserType.User);
            }
        }

        private async Task CheckAdminsAsync()
        {
            await CheckUserAsync("1001", "admin1@yopmail.com", UserType.Admin);
        }

        private async Task<User> CheckUserAsync(
            string document,
            string email,
            UserType userType)
        {
            RandomUsers randomUsers;

            do
            {
                randomUsers = await _apiService.GetRandomUser("https://randomuser.me", "api");
            } while (randomUsers == null);

            string imageId = string.Empty;
            RandomUser randomUser = randomUsers.Results.FirstOrDefault();
            string imageUrl = randomUser.Picture.Large.ToString().Substring(22);
            Stream stream = await _apiService.GetPictureAsync("https://randomuser.me", imageUrl);

            if (stream != null)
            {
                imageId = await _blobHelper.SaveFile(_filesHelper.ReadFully(stream), "users");
            }

            int cityId = _random.Next(1, _context.Cities.Count());
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = randomUser.Name.First,
                    LastName = randomUser.Name.Last,
                    Email = email,
                    UserName = email,
                    PhoneNumber = randomUser.Cell,
                    Address = $"{randomUser.Location.Street.Number}, {randomUser.Location.Street.Name}",
                    RFC = document,
                    UserType = userType,
                    City = await _context.Cities.FindAsync(cityId),
                    ImageId = imageId,
                    Latitude = double.Parse(randomUser.Location.Coordinates.Latitude),
                    Logitude = double.Parse(randomUser.Location.Coordinates.Longitude)
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "México",
                    Departments = new List<Department>
                {
                    new Department
                    {
                        Name = "Ciudad de México",
                        Cities = new List<City>
                        {
                            new City { Name = "Cuauhtémoc" },
                            new City { Name = "Iztapalapa" },
                            new City { Name = "Milpa Alta" }
                        }
                    },
                    new Department
                    {
                        Name = "Monterrey",
                        Cities = new List<City>
                        {
                            new City { Name = "Apodaca" },
                            new City { Name = "Guadalupe" },
                            new City { Name = "Santiago" }
                        }
                    },
                    new Department
                    {
                        Name = "Guadalajara",
                        Cities = new List<City>
                        {
                            new City { Name = "Puerto Vallarta" },
                            new City { Name = "Tlaquepaque" },
                            new City { Name = "Tequila" }
                        }
                    }
                }
                });
                _context.Countries.Add(new Country
                {
                    Name = "USA",
                    Departments = new List<Department>
                {
                    new Department
                    {
                        Name = "California",
                        Cities = new List<City>
                        {
                            new City { Name = "Los Angeles" },
                            new City { Name = "San Diego" },
                            new City { Name = "San Francisco" }
                        }
                    },
                    new Department
                    {
                        Name = "Illinois",
                        Cities = new List<City>
                        {
                            new City { Name = "Chicago" },
                            new City { Name = "Springfield" }
                        }
                    }
                }
                });
                await _context.SaveChangesAsync();
            }
        }

        private ICollection<Qualification> GetRandomQualifications(string description, User user)
        {
            List<Qualification> qualifications = new List<Qualification>();
            for (int i = 0; i < 10; i++)
            {
                qualifications.Add(new Qualification
                {
                    Date = DateTime.UtcNow,
                    Remarks = description,
                    Score = _random.Next(1, 5),
                    User = user
                });
            }

            return qualifications;
        }

    }
}
