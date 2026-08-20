namespace Products.Data
{

  public static class DbSeeder
  {
    public static void Seed(AppDbContext db)
    {
    
      db.Products.AddRange(Products);
      db.SaveChanges();
    }

    private static List<Product> Products =>
    [
        new Product { Id = 1, Title = "Fjallraven - Foldsack No. 1 Backpack", Price = 109.95m, Description = "Your perfect pack for everyday use and walks in the forest.", Category = "men's clothing", Image = "https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_.jpg" },
        new Product { Id = 2, Title = "Mens Casual Premium Slim Fit T-Shirts", Price = 22.30m, Description = "Slim-fitting style, contrast raglan long sleeve.", Category = "men's clothing", Image = "https://fakestoreapi.com/img/71-3HjGNDUL._AC_SY879._SX._UX._SY._UY_.jpg" },
        new Product { Id = 3, Title = "Mens Cotton Jacket", Price = 55.99m, Description = "Great outerwear jackets for Spring/Autumn/Winter.", Category = "men's clothing", Image = "https://fakestoreapi.com/img/71li-ujtlUL._AC_UX679_.jpg" },
        new Product { Id = 4, Title = "Mens Casual Slim Fit", Price = 15.99m, Description = "The color could be slightly different between on the screen and in practice.", Category = "men's clothing", Image = "https://fakestoreapi.com/img/71YXzeOuslL._AC_UY879_.jpg" },
        new Product { Id = 5, Title = "John Hardy Women's Legends Naga Gold & Silver Bracelet", Price = 695.00m, Description = "From our Legends Collection, the Naga was inspired by the mythical water dragon.", Category = "jewelery", Image = "https://fakestoreapi.com/img/71pWzhdJNwL._AC_UL640_QL65_ML3_.jpg" },
        new Product { Id = 6, Title = "Solid Gold Petite Micropave", Price = 168.00m, Description = "Satisfaction Guaranteed. Return or exchange any order within 30 days.", Category = "jewelery", Image = "https://fakestoreapi.com/img/61sbMiUnoGL._AC_UL640_QL65_ML3_.jpg" },
        new Product { Id = 7, Title = "White Gold Plated Princess", Price = 9.99m, Description = "Classic Created Wedding Engagement Solitaire Diamond Promise Ring.", Category = "jewelery", Image = "https://fakestoreapi.com/img/71YAIFU48IL._AC_UL640_QL65_ML3_.jpg" },
        new Product { Id = 8, Title = "WD 2TB Elements Portable External Hard Drive - USB 3.0", Price = 64.00m, Description = "USB 3.0 and USB 2.0 Compatibility. Fast data transfers.", Category = "electronics", Image = "https://fakestoreapi.com/img/61IBBVJvSDL._AC_SY879_.jpg" },
        new Product { Id = 9, Title = "SanDisk SSD PLUS 1TB Internal SSD", Price = 109.00m, Description = "Easy upgrade for faster boot up, shutdown, application load and response.", Category = "electronics", Image = "https://fakestoreapi.com/img/61U7T1koQqL._AC_SX679_.jpg" },
        new Product { Id = 10, Title = "Silicon Power 256GB SSD 3D NAND A55", Price = 109.00m, Description = "3D NAND flash provides better performance and enhanced reliability.", Category = "electronics", Image = "https://fakestoreapi.com/img/71kWymZ+c+L._AC_SX679_.jpg" },
    ];
  }
}
