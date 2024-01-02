using ProjectMyShop.DTO;
using ProjectMyShop.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Media.Imaging;

namespace ProjectMyShop.DAO
{
    internal class ProductDAO : SqlDataAccess
    {
        public int getTotalProduct()
        {
            var sql = "select count(*) as total from Product";
            var command = new SqlCommand(sql, _connection);
            var reader = command.ExecuteReader();

            int result = 0;
            if (reader.Read())
            {
                result = (int)reader["total"];
            }
            reader.Close();
            return result;
        }

        public Product? getProductByID(int productID)
        {
            var sql = "select * from Product WHERE ID = @productID";
            var command = new SqlCommand(sql, _connection);

            command.Parameters.AddWithValue("@productID", productID);

            var reader = command.ExecuteReader();

            Product? product = null;
            if (reader.Read())
            {
                var ID = (int)reader["ID"];
                var ProductName = (String)reader["ProductName"];
                var Manufacturer = (String)reader["Manufacturer"];

                var SoldPrice = (int)(decimal)reader["SoldPrice"];
                //var SoldPrice = (int)reader["SoldPrice"];
                var Stock = (int)reader["Stock"];

                product = new Product()
                {
                    ID = ID,
                    ProductName = ProductName,
                    Manufacturer = Manufacturer,
                    SoldPrice = SoldPrice,
                    Stock = Stock,
                };

                byte[] byteAvatar = new byte[5];
                if (reader["Avatar"] != System.DBNull.Value)
                {

                    byteAvatar = (byte[])reader["Avatar"];

                    using (MemoryStream ms = new MemoryStream(byteAvatar))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = ms;
                        image.EndInit();
                        image.Freeze();
                        product.Avatar = image;
                    }
                }
            }
            reader.Close();
            return product;
        }

        public List<Product> GetTop5OutStock()
        {
            var sql = "select top(5) * from Product where stock < 5 order by stock ";
            var command = new SqlCommand(sql, _connection);
            var reader = command.ExecuteReader();

            List<Product> list = new List<Product>();
            while (reader.Read())
            {
                var ID = (int)reader["ID"];
                var ProductName = (String)reader["ProductName"];
                var Manufacturer = (String)reader["Manufacturer"];

                var SoldPrice = (int)(decimal)reader["SoldPrice"];
                //var SoldPrice = (int)reader["SoldPrice"];
                var Stock = (int)reader["Stock"];

                Product product = new Product()
                {
                    ID = ID,
                    ProductName = ProductName,
                    Manufacturer = Manufacturer,
                    SoldPrice = SoldPrice,
                    Stock = Stock,
                };
                if (product.ProductName != "")
                    list.Add(product);
            }
            reader.Close();
            return list;
        }

        public List<Product> getProductsAccordingToSpecificCategory(int srcCategoryID)
        {
            var sql = "select * from Product where CatID = @CategoryID";

            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@CategoryID";
            sqlParameter.Value = srcCategoryID;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            List<Product> list = new List<Product>();
            while (reader.Read())
            {
                var ID = (int)reader["ID"];
                var ProductName = (String)reader["ProductName"];
                var Manufacturer = (String)reader["Manufacturer"];

                var SoldPrice = (int)(decimal)reader["SoldPrice"];
                var BoughtPrice = (int)(decimal)reader["BoughtPrice"];
                var Description = (String)reader["Description"];
                //var SoldPrice = (int)reader["SoldPrice"];
                var Stock = (int)reader["Stock"];

                Product product = new Product()
                {
                    ID = ID,
                    ProductName = ProductName,
                    Manufacturer = Manufacturer,
                    SoldPrice = SoldPrice,
                    Stock = Stock,
                    BoughtPrice = BoughtPrice,
                    Description = Description
                };
                if(!reader["Avatar"].Equals(DBNull.Value))
                {
                    var byteAvatar = (byte[])reader["Avatar"];
                    using (MemoryStream ms = new MemoryStream(byteAvatar))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = ms;
                        image.EndInit();
                        image.Freeze();
                        product.Avatar = image;
                    }
                }
                if (product.ProductName != "")
                    list.Add(product);
            }
            reader.Close();
            return list;
        }

        public void addProduct(Product product)
        {
            // ID Auto Increment
            var sql = "";
            if (product.Avatar != null)
            {
                sql = "insert into Product(ProductName, Manufacturer, BoughtPrice, SoldPrice, Stock, UploadDate, Description, CatID, Avatar) " +
                    "values (@ProductName, @Manufacturer, @BoughtPrice, @SoldPrice, @Stock, @UploadDate, @Description, @CatID, @Avatar)"; //
            }
            else
            {
                sql = "insert into Product(ProductName, Manufacturer, BoughtPrice, SoldPrice, Stock, UploadDate, Description, CatID) " +
                    "values (@ProductName, @Manufacturer, @BoughtPrice, @SoldPrice, @Stock, @UploadDate, @Description, @CatID)"; //
            }
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);

            sqlCommand.Parameters.AddWithValue("@ProductName", product.ProductName);
            sqlCommand.Parameters.AddWithValue("@Manufacturer", product.Manufacturer);
            sqlCommand.Parameters.AddWithValue("@BoughtPrice", product.BoughtPrice);
            sqlCommand.Parameters.AddWithValue("@SoldPrice", product.SoldPrice);
            sqlCommand.Parameters.AddWithValue("@Stock", product.Stock);
            sqlCommand.Parameters.AddWithValue("@UploadDate", product.UploadDate);
            sqlCommand.Parameters.AddWithValue("@Description", product.Description);
            sqlCommand.Parameters.AddWithValue("@CatID", product.Category.ID);
            if(product.Avatar != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(product.Avatar));
                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    sqlCommand.Parameters.AddWithValue("@Avatar", stream.ToArray());
                }
            }

            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Inserted {product.ID} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Inserted {product.ID} Fail: " + ex.Message);
            }
        }

        public int GetLastestInsertID()
        {
            string sql = "select ident_current('Product')";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);
            var resutl = sqlCommand.ExecuteScalar();
            System.Diagnostics.Debug.WriteLine(resutl);
            return System.Convert.ToInt32(sqlCommand.ExecuteScalar());
        }
        public void deleteProduct(int productid)
        {
            string sql = "delete from Product where ID = @ID";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);
            sqlCommand.Parameters.AddWithValue("@ID", productid);
            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Deleted {productid} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Deleted {productid} Fail: " + ex.Message);
            }
        }
        public void updateProduct(int id, Product product)
        {
            string sql;
            if (product.Avatar != null)
            {
                sql = "update Product set ProductName = @ProductName, Manufacturer = @Manufacturer, Description = @Description, " +
                "BoughtPrice = @BoughtPrice, Stock = @Stock, SoldPrice = @SoldPrice, Avatar = @Avatar where ID = @ID";
            }
            else
            {
                sql = "update Product set ProductName = @ProductName, Manufacturer = @Manufacturer, Description = @Description, " +
                "BoughtPrice = @BoughtPrice, Stock = @Stock, SoldPrice = @SoldPrice where ID = @ID";
            }
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);
            sqlCommand.Parameters.AddWithValue("@ID", id);
            sqlCommand.Parameters.AddWithValue("@ProductName", product.ProductName);
            sqlCommand.Parameters.AddWithValue("@Manufacturer", product.Manufacturer);
            sqlCommand.Parameters.AddWithValue("@BoughtPrice", product.BoughtPrice);
            sqlCommand.Parameters.AddWithValue("@SoldPrice", product.SoldPrice);
            sqlCommand.Parameters.AddWithValue("@Stock", product.Stock);
            sqlCommand.Parameters.AddWithValue("@Description", product.Description);

            if (product.Avatar != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(product.Avatar));
                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    sqlCommand.Parameters.AddWithValue("@Avatar", stream.ToArray());
                }
            }

            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Updated {product.ID} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Updated {product.ID} Fail: " + ex.Message);
            }
        }

        public List<BestSellingProduct> getBestSellingProductsInWeek(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy-MM-dd");

            var sql = "select TOP(5) p.ID, p.ProductName, p.Manufacturer, p.Stock, p.Description, p.BoughtPrice, p.SoldPrice, p.CatID, p.UploadDate, p.Avatar, sum(do.Quantity) as Quantity from Orders o join DetailOrder do on o.ID = do.OrderID join Product p on p.ID = do.ProductID where OrderDate between DATEADD(DAY, -7, @SelectedDate) and DATEADD(DAY, 1, @SelectedDate) group by p.ID, p.ProductName, p.Manufacturer, p.Stock, p.Description, p.BoughtPrice, p.SoldPrice, p.CatID, p.UploadDate, p.Avatar order by sum(do.Quantity) desc;";

            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedDate";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            List<BestSellingProduct> list = new List<BestSellingProduct>();
            while (reader.Read())
            {
                var ID = (int)reader["ID"];
                var ProductName = (String)reader["ProductName"];
                var Manufacturer = (String)reader["Manufacturer"];

                var SoldPrice = (int)(decimal)reader["SoldPrice"];
                var BoughtPrice = (int)(decimal)reader["BoughtPrice"];
                var Description = (String)reader["Description"];
                var Stock = (int)reader["Stock"];
                var Quantity = (int)reader["Quantity"];

                BestSellingProduct product = new BestSellingProduct()
                {
                    ID = ID,
                    ProductName = ProductName,
                    Manufacturer = Manufacturer,
                    SoldPrice = SoldPrice,
                    Stock = Stock,
                    BoughtPrice = BoughtPrice,
                    Description = Description,
                    Quantity = Quantity
                };
                if (!reader["Avatar"].Equals(DBNull.Value))
                {
                    var byteAvatar = (byte[])reader["Avatar"];
                    using (MemoryStream ms = new MemoryStream(byteAvatar))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = ms;
                        image.EndInit();
                        image.Freeze();
                        product.Avatar = image;
                    }
                }
                if (product.ProductName != "")
                    list.Add(product);
            }
            reader.Close();
            return list;
        }

        public List<BestSellingProduct> getBestSellingProductsInMonth(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy-MM-dd");

            var sql = "select TOP(5) p.ID, p.ProductName, p.Manufacturer, p.Stock, p.Description, p.BoughtPrice, p.SoldPrice, p.CatID, p.UploadDate, p.Avatar, sum(do.Quantity) as Quantity from Orders o join DetailOrder do on o.ID = do.OrderID join Product p on p.ID = do.ProductID where datepart(year, OrderDate) = datepart(year, @SelectedDate) and datepart(month, OrderDate) = datepart(month, @SelectedDate) group by p.ID, p.ProductName, p.Manufacturer, p.Stock, p.Description, p.BoughtPrice, p.SoldPrice, p.CatID, p.UploadDate, p.Avatar order by sum(do.Quantity) desc;";

            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedDate";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            List<BestSellingProduct> list = new List<BestSellingProduct>();

            while (reader.Read())
            {
                var ID = (int)reader["ID"];
                var ProductName = (String)reader["ProductName"];
                var Manufacturer = (String)reader["Manufacturer"];
                var SoldPrice = (int)(decimal)reader["SoldPrice"];
                var BoughtPrice = (int)(decimal)reader["BoughtPrice"];
                var Description = (String)reader["Description"];
                var Stock = (int)reader["Stock"];
                var Quantity = (int)reader["Quantity"];

                BestSellingProduct product = new BestSellingProduct()
                {
                    ID = ID,
                    ProductName = ProductName,
                    Manufacturer = Manufacturer,
                    SoldPrice = SoldPrice,
                    Stock = Stock,
                    BoughtPrice = BoughtPrice,
                    Description = Description,
                    Quantity = Quantity
                };
                if (!reader["Avatar"].Equals(DBNull.Value))
                {
                    var byteAvatar = (byte[])reader["Avatar"];
                    using (MemoryStream ms = new MemoryStream(byteAvatar))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = ms;
                        image.EndInit();
                        image.Freeze();
                        product.Avatar = image;
                    }
                }
                if (product.ProductName != "")
                    list.Add(product);
            }
            reader.Close();
            return list;
        }

        public List<BestSellingProduct> getBestSellingProductsInYear(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy-MM-dd");

            var sql = "select TOP(5) p.ID, p.ProductName, p.Manufacturer, p.Stock, p.Description, p.BoughtPrice, p.SoldPrice, p.CatID, p.UploadDate, p.Avatar, sum(do.Quantity) as Quantity from Orders o join DetailOrder do on o.ID = do.OrderID join Product p on p.ID = do.ProductID where datepart(year, OrderDate) = datepart(year, @SelectedDate) group by p.ID, p.ProductName, p.Manufacturer, p.Stock, p.Description, p.BoughtPrice, p.SoldPrice, p.CatID, p.UploadDate, p.Avatar order by sum(do.Quantity) desc;";

            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedDate";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            List<BestSellingProduct> list = new List<BestSellingProduct>();
            while (reader.Read())
            {
                var ID = (int)reader["ID"];
                var ProductName = (String)reader["ProductName"];
                var Manufacturer = (String)reader["Manufacturer"];
                var SoldPrice = (int)(decimal)reader["SoldPrice"];
                var BoughtPrice = (int)(decimal)reader["BoughtPrice"];
                var Description = (String)reader["Description"];
                var Stock = (int)reader["Stock"];
                var Quantity = (int)reader["Quantity"];

                BestSellingProduct product = new BestSellingProduct()
                {
                    ID = ID,
                    ProductName = ProductName,
                    Manufacturer = Manufacturer,
                    SoldPrice = SoldPrice,
                    Stock = Stock,
                    BoughtPrice = BoughtPrice,
                    Description = Description,
                    Quantity = Quantity
                };
                if (!reader["Avatar"].Equals(DBNull.Value))
                {
                    var byteAvatar = (byte[])reader["Avatar"];
                    using (MemoryStream ms = new MemoryStream(byteAvatar))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = null;
                        image.StreamSource = ms;
                        image.EndInit();
                        image.Freeze();
                        product.Avatar = image;
                    }
                }
                if (product.ProductName != "")
                    list.Add(product);
            }
            reader.Close();
            return list;
        }
    }
}
