using ProjectMyShop.DTO;
using ProjectMyShop.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProjectMyShop.DAO
{
    internal class CategoryDAO : SqlDataAccess
    {
        public Category GetCategoryById(int id)
        {
            var sql = "select * from Category where ID=@CatId";
            var command = new SqlCommand(sql, _connection);

            command.Parameters.Add("CatId", SqlDbType.Int).Value = id;

            var reader = command.ExecuteReader();

            Category? result = null;

            if (reader.Read()) // ORM - Object relational mapping
            {
                var catId = (int)reader["ID"];
                var catName = (string)reader["category_name"];

                result = new Category()
                {
                    ID = catId,
                    CatName = catName,
                };
            }

            reader.Close();
            return result;
        }

        public List<Category> getCategoryList()
        {
            var sql = "select * from Category;";

            var command = new SqlCommand(sql, _connection);

            var reader = command.ExecuteReader();

            var resultList = new List<Category>();
            while (reader.Read())
            {
                Category category = new Category()
                {
                    ID = (int)reader["ID"],
                    CatName = (string)reader["CatName"],
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
                        category.Avatar = image;
                    }
                }

                resultList.Add(category);
            }
            reader.Close();
            return resultList;
        }
        public void AddCategory(Category cat)
        {
            var sql = "";
            if(cat.Avatar != null)
            { 
                sql = "insert into Category(CatName, Avatar) " +
                    "values (@CatName, @Avatar)"; //
            }
            else
            {
                sql = "insert into Category(CatName) " +
                    "values (@CatName)";
            }
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);

            sqlCommand.Parameters.AddWithValue("@CatName", cat.CatName);

            if (cat.Avatar != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(cat.Avatar));
                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    sqlCommand.Parameters.AddWithValue("@Avatar", stream.ToArray());
                }
            }
            
            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Inserted {cat.ID} OK");
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Inserted {cat.ID} Fail: " + ex.Message);
            }

            
        }
        public int GetLastestInsertID()
        {
            string sql = "select ident_current('Category')";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);
            var resutl = sqlCommand.ExecuteScalar();
            System.Diagnostics.Debug.WriteLine(resutl);
            return System.Convert.ToInt32(sqlCommand.ExecuteScalar());
        }
        public int isExisted(Category cat)
        {
            string sql = "select ID from Category where CatName = @CatName";
            SqlCommand command = new SqlCommand(sql, _connection);
            command.Parameters.AddWithValue("@CatName", cat.CatName);

            var reader = command.ExecuteReader();
            int ID = 0;
            while (reader.Read())
            {
                ID = (int)reader["ID"];
            }
            reader.Close();
            return ID;
        }

        public void updateCategory(int ID, Category category)
        {
            string sql;
            if (category.Avatar != null)
            {
                sql = "update Category set CatName = @CatName, Avatar = @Avatar where ID = @ID";
            }
            else
            {
                sql = "update Category set CatName = @CatName where ID = @ID";
            }
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@CatName", category.CatName);


            if (category.Avatar != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(category.Avatar));
                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    sqlCommand.Parameters.AddWithValue("@Avatar", stream.ToArray());
                }
            }

            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Updated {category.ID} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Updated {category.ID} Fail: " + ex.Message);
            }
        }

        public void removeCategory(int ID)
        {
            string sql = "delete from Category where ID = @ID";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            try
            {
                sqlCommand.ExecuteNonQuery();
                sql = "delete from Product where CatID = @ID";
                sqlCommand.Parameters.AddWithValue("@ID", ID);
                sqlCommand.ExecuteNonQuery();

                System.Diagnostics.Debug.WriteLine($"Deleted {ID} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Deleted {ID} Fail: " + ex.Message);
            }
        }
    }
}
