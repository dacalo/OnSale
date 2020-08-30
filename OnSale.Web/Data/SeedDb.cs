using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using OnSale.Common.Entities;
using OnSale.Common.Enums;
using OnSale.Web.Data.Entities;
using OnSale.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly Random _random;

        public SeedDb(
            DataContext context,
            IUserHelper userHelper,
            IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("CALD7808244AA", "David", "Chávez", "divadchl@gmail.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);
            await CheckCategories();
            await CheckProducts();
        }

        private async Task CheckCategories()
        {
            if(!_context.Categories.Any())
            {
                byte[] data;
                List<Category> list = new List<Category>();
                list.Add(new Category { Name = "Zapatería", UrlImage = GenerateImage() });
                list.Add(new Category { Name = "Ropa", UrlImage = GenerateImage() });
                list.Add(new Category { Name = "Tecnología", UrlImage = GenerateImage() });
                list.Add(new Category { Name = "Electrodomésticos", UrlImage = GenerateImage() });
                list.Add(new Category { Name = "Joyería", UrlImage = GenerateImage() });
                
                await _context.Categories.AddRangeAsync(list);
                await _context.SaveChangesAsync();

                foreach (var item in list)
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
            var faker = new Faker();
            return faker.Image.LoremPixelUrl();
        }

        private async Task CheckProducts()
        {
            if(!_context.Products.Any())
            {
                User user = await _userHelper.GetUserAsync("divadchl@gmail.com");
                byte[] data;
                var product = new Faker<ProductEntity>("es_MX")
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                    .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(100, 10000))) 
                    .RuleFor(p => p.IsActive, f => true)
                    .RuleFor(p => p.IsStarred, f => f.Random.Bool())
                    .RuleFor(p => p.Qualifications, f =>
                    {
                        return GetRandomQualifications(f.Commerce.ProductDescription(), user);
                    });

                var listProducts = product.Generate(15);
                List<ProductEntity> list = new List<ProductEntity>();
                Random rnd = new Random();
                foreach (var item in listProducts)
                {
                    var images = new Faker<ProductImage>()
                        .RuleFor(pi => pi.UrlImage, f => f.Image.LoremPixelUrl());
                    var listImages = images.Generate(2);
                    item.ProductImages = listImages;
                    foreach (var image in listImages)
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            data = webClient.DownloadData(image.UrlImage);
                        }
                        image.UrlImage = await _blobHelper.SaveFile(data, "products");
                    }
                    item.Category = await _context.Categories.FindAsync(rnd.Next(1,5));
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
        }
        
        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    Departments = new List<Department>
                {
                    new Department
                    {
                        Name = "Antioquia",
                        Cities = new List<City>
                        {
                            new City { Name = "Medellín" },
                            new City { Name = "Envigado" },
                            new City { Name = "Itagüí" }
                        }
                    },
                    new Department
                    {
                        Name = "Bogotá",
                        Cities = new List<City>
                        {
                            new City { Name = "Bogotá" }
                        }
                    },
                    new Department
                    {
                        Name = "Valle del Cauca",
                        Cities = new List<City>
                        {
                            new City { Name = "Calí" },
                            new City { Name = "Buenaventura" },
                            new City { Name = "Palmira" }
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
