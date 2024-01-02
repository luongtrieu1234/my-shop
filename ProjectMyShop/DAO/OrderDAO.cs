using ProjectMyShop.BUS;
using ProjectMyShop.DTO;
using ProjectMyShop.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMyShop.DAO
{
    internal class OrderDAO: SqlDataAccess
    {
        List<DetailOrder> GetDetailOrder(int orderID)
        {
            string sql = "select * from DetailOrder WHERE OrderID = @orderID";

            var command = new SqlCommand(sql, _connection);
            command.Parameters.AddWithValue("@orderID", orderID);
            
            var reader = command.ExecuteReader();

            var result = new List<DetailOrder>();

            var _productBUS = new ProductBUS();
            while (reader.Read())
            {
                var OrderID = reader.GetInt32("OrderID");
                var ProductID = reader.GetInt32("ProductID");
                var Quantity = reader.GetInt32("Quantity");

                var Product = _productBUS.getProductByID(ProductID);

                DetailOrder _order = new DetailOrder()
                {
                    OrderID = OrderID,
                    Product = Product,
                    Quantity = Quantity
                };

                result.Add(_order);
            }

            reader.Close();
            return result;
        }

        Order ORMapping(SqlDataReader reader)
        {
            var ID = (int)reader["ID"];
            var CustomerName = (String)reader["CustomerName"];
            var OrderDate = DateOnly.Parse(DateTime.Parse(reader["OrderDate"].ToString()).Date.ToShortDateString());
            var Status = (System.Int16)reader["Status"];
            var Address = (String)reader["Address"];
            // var VoucherID = (Voucher)reader["VoucherID"];


            Order order = new Order()
            {
                ID = ID,
                CustomerName = CustomerName,
                OrderDate = OrderDate,
                Status = (Order.OrderStatusEnum)Status,
                Address = Address,
            };
            return order;
        }

        List<Order> Select(SqlCommand command)
        {
            var reader = command.ExecuteReader();

            var result = new List<Order>();

            while (reader.Read())
            {
                result.Add(ORMapping(reader));
            }
            reader.Close();

            foreach (var order in result)
            {
                List<DetailOrder> DetailOrderList = GetDetailOrder(order.ID);
                order.DetailOrderList = DetailOrderList;
            }

            return result;
        } 

        public List<Order> GetOrders(int offset, int size)
        {
            string sql = "select * from Orders " +
                "Order by OrderDate DESC, ID ASC " +
                "offset @Off rows " +
                "fetch first @Size rows only";

            var command = new SqlCommand(sql, _connection);

            command.Parameters.AddWithValue("@Off", offset);
            command.Parameters.AddWithValue("@Size", size);

            return Select(command);
        }

        public List<Order> GetAllOrders()
        {
            string sql = "select * from Orders" +
                "Order by OrderDate DESC, ID ASC ";

            var command = new SqlCommand(sql, _connection);
            return Select(command);
        }

        internal List<Order> GetAllOrdersByDate(DateTime FromDate, DateTime ToDate)
        {
            string sql = "select * from Orders " +
                "Where OrderDate >= @fromDate AND OrderDate <= @toDate";
            var command = new SqlCommand(sql, _connection);

            command.Parameters.AddWithValue("@fromDate", DateTime.Parse(FromDate.ToString()));
            command.Parameters.AddWithValue("@toDate", DateTime.Parse(ToDate.ToString()));

            return Select(command);
        }


        public List<Order> GetOrdersByDate(int offset, int size, DateTime fromDate, DateTime toDate)
        {
            string sql = "select * from Orders " +
                "Where OrderDate >= @fromDate AND OrderDate <= @toDate" +
                "Order by OrderDate DESC, ID ASC " +
                "offset @Off rows " +
                "fetch first @Size rows only";

            var command = new SqlCommand(sql, _connection);

            command.Parameters.AddWithValue("@fromDate", DateTime.Parse(fromDate.ToString()));
            command.Parameters.AddWithValue("@toDate", DateTime.Parse(toDate.ToString()));
            command.Parameters.AddWithValue("@Off", offset);
            command.Parameters.AddWithValue("@Size", size);

            return Select(command);
        }

        public void AddDetailOrder(DetailOrder detail)
        {
            var sql = "insert into DetailOrder(OrderID, ProductID, Quantity) " +
                "values (@OrderID, @ProductID, @Quantity)";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);

            sqlCommand.Parameters.AddWithValue("@OrderID", detail.OrderID);
            sqlCommand.Parameters.AddWithValue("@ProductID", detail.Product.ID);
            sqlCommand.Parameters.AddWithValue("@Quantity", detail.Quantity);

            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Inserted {detail.OrderID} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Inserted {detail.OrderID} Fail: " + ex.Message);
            }
        }
        public void UpdateDetailOrder(int oldProductID, DetailOrder detail)
        {
            var sql = "update DetailOrder set Quantity = @Quantity, ProductID = @ProductID where OrderID = @OrderID and ProductID = @oldProductID";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);

            sqlCommand.Parameters.AddWithValue("@OrderID", detail.OrderID);
            sqlCommand.Parameters.AddWithValue("@ProductID", detail.Product.ID);
            sqlCommand.Parameters.AddWithValue("@oldProductID", oldProductID);
            sqlCommand.Parameters.AddWithValue("@Quantity", detail.Quantity);

            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Updated {detail.OrderID} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Updated {detail.OrderID} Fail: " + ex.Message);
            }
        }
        public void DeleteDetailOrder(DetailOrder detail)
        {
            var sql = "delete from DetailOrder where OrderID = @OrderID and ProductID = @ProductID and Quantity = @Quantity";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);

            sqlCommand.Parameters.AddWithValue("@OrderID", detail.OrderID);
            sqlCommand.Parameters.AddWithValue("@ProductID", detail.Product.ID);
            sqlCommand.Parameters.AddWithValue("@Quantity", detail.Quantity);

            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Deleted {detail.OrderID} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Deleted {detail.OrderID} Fail: " + ex.Message);
            }
        }
        
        public void AddOrder(Order order)
        {
            // ID Auto Increment
            var sql = "insert into Orders(CustomerName, OrderDate, Status, Address) " +
                "values (@CustomerName, @OrderDate, @Status, @Address)"; // Them VoucherID sau
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);

            sqlCommand.Parameters.AddWithValue("@CustomerName", order.CustomerName);
            sqlCommand.Parameters.AddWithValue("@OrderDate", DateTime.Parse(order.OrderDate.ToString()));
            sqlCommand.Parameters.AddWithValue("@Status", order.Status);
            sqlCommand.Parameters.AddWithValue("@Address", order.Address);
            
            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Inserted {order.ID} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Inserted {order.ID} Fail: " + ex.Message);
            }
        }

        public int GetLastestInsertID()
        {
            string sql = "select ident_current('Orders')";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);
            var resutl = sqlCommand.ExecuteScalar();
            System.Diagnostics.Debug.WriteLine(resutl);
            return System.Convert.ToInt32(sqlCommand.ExecuteScalar());
        }
        public void UpdateOrder(int orderID,Order order)
        {
            var sql = "update Orders " +
                "SET CustomerName = @CustomerName, OrderDate = @OrderDate, Status =  @Status, Address = @Address " +
                "where ID = @OrderID";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);

            sqlCommand.Parameters.AddWithValue("@OrderID", orderID);
            sqlCommand.Parameters.AddWithValue("@CustomerName", order.CustomerName);
            sqlCommand.Parameters.AddWithValue("@OrderDate", DateTime.Parse(order.OrderDate.ToString()));
            sqlCommand.Parameters.AddWithValue("@Status", order.Status);
            sqlCommand.Parameters.AddWithValue("@Address", order.Address);


            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Updated {orderID} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Updated {orderID} Fail: " + ex.Message);
            }
        }

        public void DeleteOrder(int orderID)
        {
            var sql = "delete from Orders where ID = @OrderID";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);

            sqlCommand.Parameters.AddWithValue("@OrderID", orderID);

            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"Deleted {orderID} OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Deleted {orderID} Fail: " + ex.Message);
            }
        }

        public int CountOrders()
        {
            string sql = "select count(*) as c from Orders";
            var command = new SqlCommand(sql, _connection);

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.Read())
            {
                result = (int)reader["c"];
            }
            reader.Close();
            return result;
        }

        public int CountOrderByWeek()
        {
            string sql = "select count(*) as week from Orders where datediff(day, OrderDate, GETDATE()) < 7";
            var command = new SqlCommand(sql, _connection);

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.Read())
            {
                result = (int)reader["week"];
            }
            reader.Close();
            return result;
        }
        public int CountOrderByMonth()
        {
            string sql = "select count(*) as month from Orders where datediff(day, OrderDate, GETDATE()) < 30";
            var command = new SqlCommand(sql, _connection);

            var reader = command.ExecuteReader();

            int result = 0;

            if (reader.Read())
            {
                result = (int)reader["month"];
            }
            reader.Close();
            return result;
        }
    }
}
