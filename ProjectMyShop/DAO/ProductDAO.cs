using ProjectMyShop.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMyShop.DTO;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.IO;

namespace ProjectMyShop.DAO
{
    internal class ProductDAO : SqlDataAccess
    {
        public int getTotalProduct()
        {
            var sql = "select count(*) as total from Phone";
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

        public Product? getProductByID(int phoneID)
        {
            var sql = "select * from Phone WHERE ID = @phoneID";
            var command = new SqlCommand(sql, _connection);

            command.Parameters.AddWithValue("@phoneID", phoneID);

            var reader = command.ExecuteReader();

            Product? product = null;
            if (reader.Read())
            {
                var ID = (int)reader["ID"];
                var PhoneName = (String)reader["PhoneName"];
                var Manufacturer = (String)reader["Manufacturer"];

                var SoldPrice = (int)(decimal)reader["SoldPrice"];
                //var SoldPrice = (int)reader["SoldPrice"];
                var Stock = (int)reader["Stock"];

                product = new Product()
                {
                    ID = ID,
                    ProductName = PhoneName,
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
            var sql = "select top(5) * from Phone where stock < 5 order by stock ";
            var command = new SqlCommand(sql, _connection);
            var reader = command.ExecuteReader();

            List<Product> list = new List<Product>();
            while (reader.Read())
            {
                var ID = (int)reader["ID"];
                var ProductName = (String)reader["PhoneName"];
                var Manufacturer = (String)reader["Manufacturer"];

                var SoldPrice = (int)(decimal)reader["SoldPrice"];
                //var SoldPrice = (int)reader["SoldPrice"];
                var Stock = (int)reader["Stock"];

                Product phone = new Product()
                {
                    ID = ID,
                    ProductName = ProductName,
                    Manufacturer = Manufacturer,
                    SoldPrice = SoldPrice,
                    Stock = Stock,
                };
                if (phone.ProductName != "")
                    list.Add(phone);
            }
            reader.Close();
            return list;
        }

        public List<Product> getProductsAccordingToSpecificCategory(int srcCategoryID)
        {
            var sql = "select * from Phone where CatID = @CategoryID";

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
                var ProductName = (String)reader["PhoneName"];
                var Manufacturer = (String)reader["Manufacturer"];

                var SoldPrice = (int)(decimal)reader["SoldPrice"];
                var BoughtPrice = (int)(decimal)reader["BoughtPrice"];
                var Description = (String)reader["Description"];
                //var SoldPrice = (int)reader["SoldPrice"];
                var Stock = (int)reader["Stock"];

                Product phone = new Product()
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
                        phone.Avatar = image;
                    }
                }
                if (phone.ProductName != "")
                    list.Add(phone);
            }
            reader.Close();
            return list;
        }

        public void addPhone(Product product)
        {
            // ID Auto Increment
            var sql = "";
            if (product.Avatar != null)
            {
                sql = "insert into Phone(PhoneName, Manufacturer, BoughtPrice, SoldPrice, Stock, UploadDate, Description, CatID, Avatar) " +
                    "values (@PhoneName, @Manufacturer, @BoughtPrice, @SoldPrice, @Stock, @UploadDate, @Description, @CatID, @Avatar)"; //
            }
            else
            {
                sql = "insert into Phone(PhoneName, Manufacturer, BoughtPrice, SoldPrice, Stock, UploadDate, Description, CatID) " +
                    "values (@PhoneName, @Manufacturer, @BoughtPrice, @SoldPrice, @Stock, @UploadDate, @Description, @CatID)"; //
            }
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);

            sqlCommand.Parameters.AddWithValue("@PhoneName", product.ProductName);
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
            string sql = "select ident_current('Phone')";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);
            var resutl = sqlCommand.ExecuteScalar();
            System.Diagnostics.Debug.WriteLine(resutl);
            return System.Convert.ToInt32(sqlCommand.ExecuteScalar());
        }
        public void deleteProduct(int phoneid)
        {
            string sql = "delete from Phone where ID = @ID";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);
            sqlCommand.Parameters.AddWithValue("@ID", phoneid);
            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Deleted {phoneid} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Deleted {phoneid} Fail: " + ex.Message);
            }
        }
        public void updateProduct(int id, Product phone)
        {
            string sql;
            if (phone.Avatar != null)
            {
                sql = "update Phone set PhoneName = @PhoneName, Manufacturer = @Manufacturer, Description = @Description, " +
                "BoughtPrice = @BoughtPrice, Stock = @Stock, SoldPrice = @SoldPrice, Avatar = @Avatar where ID = @ID";
            }
            else
            {
                sql = "update Phone set PhoneName = @PhoneName, Manufacturer = @Manufacturer, Description = @Description, " +
                "BoughtPrice = @BoughtPrice, Stock = @Stock, SoldPrice = @SoldPrice where ID = @ID";
            }
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);
            sqlCommand.Parameters.AddWithValue("@ID", id);
            sqlCommand.Parameters.AddWithValue("@PhoneName", phone.ProductName);
            sqlCommand.Parameters.AddWithValue("@Manufacturer", phone.Manufacturer);
            sqlCommand.Parameters.AddWithValue("@BoughtPrice", phone.BoughtPrice);
            sqlCommand.Parameters.AddWithValue("@SoldPrice", phone.SoldPrice);
            sqlCommand.Parameters.AddWithValue("@Stock", phone.Stock);
            sqlCommand.Parameters.AddWithValue("@Description", phone.Description);

            if (phone.Avatar != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(phone.Avatar));
                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    sqlCommand.Parameters.AddWithValue("@Avatar", stream.ToArray());
                }
            }

            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Updated {phone.ID} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Updated {phone.ID} Fail: " + ex.Message);
            }
        }

        public List<BestSellingPhone> getBestSellingProductsInWeek(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy-MM-dd");

            var sql = "select TOP(5) p.ID, p.PhoneName, p.Manufacturer, p.Stock, p.Description, p.BoughtPrice, p.SoldPrice, p.CatID, p.UploadDate, p.Avatar, sum(do.Quantity) as Quantity from Orders o join DetailOrder do on o.ID = do.OrderID join Phone p on p.ID = do.PhoneID where OrderDate between DATEADD(DAY, -7, @SelectedDate) and DATEADD(DAY, 1, @SelectedDate) group by p.ID, p.PhoneName, p.Manufacturer, p.Stock, p.Description, p.BoughtPrice, p.SoldPrice, p.CatID, p.UploadDate, p.Avatar order by sum(do.Quantity) desc;";

            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedDate";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            List<BestSellingPhone> list = new List<BestSellingPhone>();
            while (reader.Read())
            {
                var ID = (int)reader["ID"];
                var ProductName = (String)reader["PhoneName"];
                var Manufacturer = (String)reader["Manufacturer"];

                var SoldPrice = (int)(decimal)reader["SoldPrice"];
                var BoughtPrice = (int)(decimal)reader["BoughtPrice"];
                var Description = (String)reader["Description"];
                var Stock = (int)reader["Stock"];
                var Quantity = (int)reader["Quantity"];

                BestSellingPhone product = new BestSellingPhone()
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

        public List<BestSellingPhone> getBestSellingProductsInMonth(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy-MM-dd");

            var sql = "select TOP(5) p.ID, p.PhoneName, p.Manufacturer, p.Stock, p.Description, p.BoughtPrice, p.SoldPrice, p.CatID, p.UploadDate, p.Avatar, sum(do.Quantity) as Quantity from Orders o join DetailOrder do on o.ID = do.OrderID join Phone p on p.ID = do.PhoneID where datepart(year, OrderDate) = datepart(year, @SelectedDate) and datepart(month, OrderDate) = datepart(month, @SelectedDate) group by p.ID, p.PhoneName, p.Manufacturer, p.Stock, p.Description, p.BoughtPrice, p.SoldPrice, p.CatID, p.UploadDate, p.Avatar order by sum(do.Quantity) desc;";

            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedDate";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            List<BestSellingPhone> list = new List<BestSellingPhone>();

            while (reader.Read())
            {
                var ID = (int)reader["ID"];
                var PhoneName = (String)reader["PhoneName"];
                var Manufacturer = (String)reader["Manufacturer"];
                var SoldPrice = (int)(decimal)reader["SoldPrice"];
                var BoughtPrice = (int)(decimal)reader["BoughtPrice"];
                var Description = (String)reader["Description"];
                var Stock = (int)reader["Stock"];
                var Quantity = (int)reader["Quantity"];

                BestSellingPhone phone = new BestSellingPhone()
                {
                    ID = ID,
                    ProductName = PhoneName,
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
                        phone.Avatar = image;
                    }
                }
                if (phone.ProductName != "")
                    list.Add(phone);
            }
            reader.Close();
            return list;
        }

        public List<BestSellingPhone> getBestSellingProductsInYear(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy-MM-dd");

            var sql = "select TOP(5) p.ID, p.PhoneName, p.Manufacturer, p.Stock, p.Description, p.BoughtPrice, p.SoldPrice, p.CatID, p.UploadDate, p.Avatar, sum(do.Quantity) as Quantity from Orders o join DetailOrder do on o.ID = do.OrderID join Phone p on p.ID = do.PhoneID where datepart(year, OrderDate) = datepart(year, @SelectedDate) group by p.ID, p.PhoneName, p.Manufacturer, p.Stock, p.Description, p.BoughtPrice, p.SoldPrice, p.CatID, p.UploadDate, p.Avatar order by sum(do.Quantity) desc;";

            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedDate";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            List<BestSellingPhone> list = new List<BestSellingPhone>();
            while (reader.Read())
            {
                var ID = (int)reader["ID"];
                var PhoneName = (String)reader["PhoneName"];
                var Manufacturer = (String)reader["Manufacturer"];
                var SoldPrice = (int)(decimal)reader["SoldPrice"];
                var BoughtPrice = (int)(decimal)reader["BoughtPrice"];
                var Description = (String)reader["Description"];
                var Stock = (int)reader["Stock"];
                var Quantity = (int)reader["Quantity"];

                BestSellingPhone phone = new BestSellingPhone()
                {
                    ID = ID,
                    ProductName = PhoneName,
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
                        phone.Avatar = image;
                    }
                }
                if (phone.ProductName != "")
                    list.Add(phone);
            }
            reader.Close();
            return list;
        }
    }
}
