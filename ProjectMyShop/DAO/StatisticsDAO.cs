using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMyShop.Helpers;
using System.Data.SqlClient;

namespace ProjectMyShop.DAO
{
    internal class StatisticsDAO : SqlDataAccess
    {
        public string getTotalRevenueUntilDate(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy-MM-dd");

            var sql = "select convert(varchar,cast(SUM(do.Quantity * p.SoldPrice) as money), 1) as Revenue from DetailOrder do join Phone p on do.PhoneID = p.ID join Orders o on do.OrderID = o.ID where OrderDate <= @SelectedDate;";
            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedDate";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            string result = "0";
            if (reader.Read())
            {
                if (reader["Revenue"].GetType() != typeof(DBNull))
                    result = (string)reader["Revenue"];
            }
            reader.Close();
            return result.ToString();
        }

        public string getTotalProfitUntilDate(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy-MM-dd");

            var sql = "select convert(varchar,cast(SUM(do.Quantity *(p.SoldPrice - p.BoughtPrice)) as money), 1) as Profit from DetailOrder do join Phone p on do.PhoneID = p.ID join Orders o on do.OrderID = o.ID where OrderDate <= @SelectedDate;";
            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedDate";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            string result = "0";
            if (reader.Read())
            {
                if (reader["Profit"].GetType() != typeof(DBNull))
                    result = (string)reader["Profit"];
            }
            reader.Close();
            return result.ToString();
        }

        public int getTotalOrdersUntilDate(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy-MM-dd");

            var sql = "select COUNT(ID) as Orders from Orders where OrderDate <= @SelectedDate;";
            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedDate";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            int result = 0;
            if (reader.Read())
            {
                result = (int)reader["Orders"];
            }
            reader.Close();
            return result;
        }

        public List<Tuple<string, decimal>> getDailyRevenue(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy-MM-dd");

            //var sql = "select convert(varchar, o.OrderDate) as OrderDate, convert(varchar,cast(SUM(do.Quantity * p.SoldPrice) as money), 1) as Revenue from DetailOrder do join Phone p on do.PhoneID = p.ID join Orders o on do.OrderID = o.ID where OrderDate < @SelectedDate group by o.OrderDate order by o.OrderDate asc;";
            var sql = "select convert(varchar, o.OrderDate) as OrderDate, cast(SUM(do.Quantity * p.SoldPrice) AS decimal(13,4)) as Revenue from DetailOrder do join Phone p on do.PhoneID = p.ID join Orders o on do.OrderID = o.ID where OrderDate <= @SelectedDate group by o.OrderDate order by o.OrderDate asc;";
            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedDate";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            var resultList = new List<Tuple<string, decimal>>();
            while (reader.Read())
            {
                var tuple = Tuple.Create((string)reader["OrderDate"], (decimal)reader["Revenue"]);
                resultList.Add(tuple);
            }
            reader.Close();
            return resultList;
        }

        public List<Tuple<string, decimal>> getWeeklyRevenue(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy");

            var sql = "SELECT convert(varchar, DATEPART(iso_week, o.OrderDate)) AS Week, cast(SUM(do.Quantity * p.SoldPrice) as decimal(13,4)) as Revenue FROM Phone p join DetailOrder do on p.ID = do.PhoneID join Orders o on o.ID = do.OrderID WHERE DATEPART(year, o.OrderDate) = @SelectedYear GROUP BY DATEPART(iso_week, o.OrderDate) ORDER BY DATEPART(iso_week, o.OrderDate);";
            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedYear";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            var resultList = new List<Tuple<string, decimal>>();
            while (reader.Read())
            {
                var tuple = Tuple.Create((string)reader["Week"], (decimal)reader["Revenue"]);
                resultList.Add(tuple);
            }
            reader.Close();
            return resultList;
        }

        public List<Tuple<string, decimal>> getMonthlyRevenue(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy");

            var sql = "WITH Months as (select month(GETDATE()) as Monthnumber, datename(month, GETDATE()) as NameOfMonth, 1 as number union all select month(dateadd(month, number, (GETDATE()))) Monthnumber, datename(month, dateadd(month, number, (GETDATE()))) as NameOfMonth, number + 1  from Months  where number < 12) select NameOfMonth, Revenue from Months left join (select datepart(month, o.OrderDate) as OrderMonth, cast(SUM(do.Quantity * p.SoldPrice) AS decimal(13, 4)) as Revenue from DetailOrder do join Phone p on do.PhoneID = p.ID join Orders o on do.OrderID = o.ID where datepart(year, o.OrderDate) = @SelectedYear group by datepart(month, o.OrderDate)) MonthlyRevenue on Months.Monthnumber = MonthlyRevenue.OrderMonth order by Monthnumber;";
            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedYear";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            var resultList = new List<Tuple<string, decimal>>();
            while (reader.Read())
            {
                string monthName = (string)reader["NameOfMonth"];
                decimal monthValue = 0;
                
                if (reader["Revenue"].GetType() != typeof(DBNull))
                {
                    monthValue = (decimal)reader["Revenue"];
                }

                resultList.Add(Tuple.Create(monthName, (decimal)monthValue));
            }
            reader.Close();
            return resultList;
        }

        public List<Tuple<string, decimal>> getYearlyRevenue()
        {
            var sql = "select convert(varchar, datepart(year, o.OrderDate)) as OrderYear, cast(SUM(do.Quantity * SoldPrice) AS decimal(13,4)) as Revenue from DetailOrder do join Phone p on do.PhoneID = p.ID join Orders o on do.OrderID = o.ID group by datepart(year, o.OrderDate) order by datepart(year, o.OrderDate) asc;";

            var command = new SqlCommand(sql, _connection);

            var reader = command.ExecuteReader();

            var resultList = new List<Tuple<string, decimal>>();
            while (reader.Read())
            {
                var tuple = Tuple.Create((string)reader["OrderYear"], (decimal)reader["Revenue"]);
                resultList.Add(tuple);
            }
            reader.Close();
            return resultList;
        }

        public List<Tuple<string, decimal>> getDailyProfit(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy-MM-dd");

            var sql = "select convert(varchar, o.OrderDate) as OrderDate, cast(SUM(do.Quantity * (p.SoldPrice - p.BoughtPrice)) AS decimal(13,4)) as Profit from DetailOrder do join Phone p on do.PhoneID = p.ID join Orders o on do.OrderID = o.ID where OrderDate <= @SelectedDate group by o.OrderDate order by o.OrderDate asc;";
            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedDate";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            var resultList = new List<Tuple<string, decimal>>();
            while (reader.Read())
            {
                var tuple = Tuple.Create((string)reader["OrderDate"], (decimal)reader["Profit"]);
                resultList.Add(tuple);
            }
            reader.Close();
            return resultList;
        }

        public List<Tuple<string, decimal>> getWeeklyProfit(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy");

            var sql = "SELECT convert(varchar, DATEPART(iso_week, o.OrderDate)) AS Week, cast(SUM(do.Quantity * (p.SoldPrice - p.BoughtPrice)) as decimal(13,4)) as Profit FROM Phone p join DetailOrder do on p.ID = do.PhoneID join Orders o on o.ID = do.OrderID WHERE DATEPART(year, o.OrderDate) = @SelectedYear GROUP BY DATEPART(iso_week, o.OrderDate) ORDER BY DATEPART(iso_week, o.OrderDate);";
            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedYear";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            var resultList = new List<Tuple<string, decimal>>();
            while (reader.Read())
            {
                var tuple = Tuple.Create((string)reader["Week"], (decimal)reader["Profit"]);
                resultList.Add(tuple);
            }
            reader.Close();
            return resultList;
        }

        public List<Tuple<string, decimal>> getMonthlyProfit(DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy");

            var sql = "WITH Months as (select month(GETDATE()) as Monthnumber, datename(month, GETDATE()) as NameOfMonth, 1 as number union all select month(dateadd(month, number, (GETDATE()))) Monthnumber, datename(month, dateadd(month, number, (GETDATE()))) as NameOfMonth, number + 1  from Months  where number < 12) select NameOfMonth, Profit from Months left join (select datepart(month, o.OrderDate) as OrderMonth, cast(SUM(do.Quantity * (p.SoldPrice - p.BoughtPrice)) AS decimal(13, 4)) as Profit from DetailOrder do join Phone p on do.PhoneID = p.ID join Orders o on do.OrderID = o.ID where datepart(year, o.OrderDate) = @SelectedYear group by datepart(month, o.OrderDate)) MonthlyProfit on Months.Monthnumber = MonthlyProfit.OrderMonth order by Monthnumber;";
            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedYear";
            sqlParameter.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            var resultList = new List<Tuple<string, decimal>>();
            while (reader.Read())
            {
                string monthName = (string)reader["NameOfMonth"];
                decimal monthValue = 0;

                if (reader["Profit"].GetType() != typeof(DBNull))
                {
                    monthValue = (decimal)reader["Profit"];
                }

                resultList.Add(Tuple.Create(monthName, (decimal)monthValue));
            }
            reader.Close();
            return resultList;
        }

        public List<Tuple<string, decimal>> getYearlyProfit()
        {
            var sql = "select convert(varchar, datepart(year, o.OrderDate)) as OrderYear, cast(SUM(do.Quantity * (p.SoldPrice - p.BoughtPrice)) AS decimal(13,4)) as Profit from DetailOrder do join Phone p on do.PhoneID = p.ID join Orders o on do.OrderID = o.ID group by datepart(year, o.OrderDate) order by datepart(year, o.OrderDate) asc;";

            var command = new SqlCommand(sql, _connection);

            var reader = command.ExecuteReader();

            var resultList = new List<Tuple<string, decimal>>();
            while (reader.Read())
            {
                var tuple = Tuple.Create((string)reader["OrderYear"], (decimal)reader["Profit"]);
                resultList.Add(tuple);
            }
            reader.Close();
            return resultList;
        }

        public List<Tuple<string, int>> getDailyQuantityOfSpecificProduct(int srcPhoneID, int srcCategoryID, DateTime srcDate)
        {
            var sql = "select convert(varchar, o.OrderDate) as OrderDate, SUM(do.Quantity) as Quantity from Phone p left join DetailOrder do on p.ID = do.PhoneID join Orders o on do.OrderID = o.ID where p.ID = @PhoneID  and p.CatID = @CategoryID and o.OrderDate <= @SelectedDate group by o.OrderDate order by o.OrderDate asc";

            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@PhoneID";
            sqlParameter.Value = srcPhoneID;

            var sqlParameter1 = new SqlParameter();
            sqlParameter1.ParameterName = "@CategoryID";
            sqlParameter1.Value = srcCategoryID;

            string sqlFormattedDate = srcDate.ToString("yyyy-MM-dd");
            var sqlParameter2 = new SqlParameter();
            sqlParameter2.ParameterName = "@SelectedDate";
            sqlParameter2.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);

            command.Parameters.Add(sqlParameter);
            command.Parameters.Add(sqlParameter1);
            command.Parameters.Add(sqlParameter2);

            var reader = command.ExecuteReader();

            var resultList = new List<Tuple<string, int>>();
            while (reader.Read())
            {
                var tuple = Tuple.Create((string)reader["OrderDate"], (int)reader["Quantity"]);
                resultList.Add(tuple);
            }
            reader.Close();
            return resultList;
        }

        public List<Tuple<string, int>> getWeeklyQuantityOfSpecificProduct(int srcPhoneID, int srcCategoryID, DateTime src)
        {
            string sqlFormattedDate = src.ToString("yyyy");

            var sql = "SELECT DATEPART(iso_week, o.OrderDate) AS Week, convert(varchar,cast(SUM(do.Quantity * (p.SoldPrice - p.BoughtPrice)) as money), 1) as Profit FROM Phone p join DetailOrder do on p.ID = do.PhoneID join Orders o on o.ID = do.OrderID WHERE DATEPART(year, o.OrderDate) = @SelectedYear and p.ID = @PhoneID  and p.CatID = @CategoryID GROUP BY DATEPART(iso_week, o.OrderDate) ORDER BY DATEPART(iso_week, o.OrderDate);";
            
            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedYear";
            sqlParameter.Value = sqlFormattedDate;
            
            var sqlParameter1 = new SqlParameter();
            sqlParameter1.ParameterName = "@PhoneID";
            sqlParameter1.Value = srcPhoneID;

            var sqlParameter2 = new SqlParameter();
            sqlParameter2.ParameterName = "@CategoryID";
            sqlParameter2.Value = srcCategoryID;

            var command = new SqlCommand(sql, _connection);
            command.Parameters.Add(sqlParameter);
            command.Parameters.Add(sqlParameter1);
            command.Parameters.Add(sqlParameter2);

            var reader = command.ExecuteReader();

            var resultList = new List<Tuple<string, int>>();
            while (reader.Read())
            {
                var tuple = Tuple.Create((string)reader["Week"], (int)reader["Quantity"]);
                resultList.Add(tuple);
            }
            reader.Close();
            return resultList;
        }

        public List<Tuple<string, int>> getMonthlyQuantityOfSpecificProduct(int srcPhoneID, int srcCategoryID, DateTime srcDate)
        {
            var sql = "WITH Months as (select month(GETDATE()) as Monthnumber, datename(month, GETDATE()) as NameOfMonth, 1 as number union all select month(dateadd(month, number, (GETDATE()))) Monthnumber, datename(month, dateadd(month, number, (GETDATE()))) as NameOfMonth, number + 1  from Months  where number < 12) select NameOfMonth, Quantity from Months left join (select datepart(month, o.OrderDate) as OrderMonth, SUM(do.Quantity) as Quantity from DetailOrder do join Phone p on do.PhoneID = p.ID join Orders o on do.OrderID = o.ID where datepart(year, o.OrderDate) = @SelectedYear and p.ID = @PhoneID and p.CatID = @CategoryID group by datepart(month, o.OrderDate)) MonthlyQuantity on Months.Monthnumber = MonthlyQuantity .OrderMonth order by Monthnumber;";

            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@PhoneID";
            sqlParameter.Value = srcPhoneID;

            var sqlParameter1 = new SqlParameter();
            sqlParameter1.ParameterName = "@CategoryID";
            sqlParameter1.Value = srcCategoryID;

            string sqlFormattedDate = srcDate.ToString("yyyy");
            var sqlParameter2 = new SqlParameter();
            sqlParameter2.ParameterName = "@SelectedYear";
            sqlParameter2.Value = sqlFormattedDate;

            var command = new SqlCommand(sql, _connection);

            command.Parameters.Add(sqlParameter2);
            command.Parameters.Add(sqlParameter);
            command.Parameters.Add(sqlParameter1);

            var reader = command.ExecuteReader();

            var resultList = new List<Tuple<string, int>>();
            while (reader.Read())
            {
                string monthName = (string)reader["NameOfMonth"];
                int quantity = 0;

                if (reader["Quantity"].GetType() != typeof(DBNull))
                {
                    quantity = (int)reader["Quantity"];
                }
                resultList.Add(Tuple.Create(monthName, quantity));
            }
            reader.Close();
            return resultList;
        }

        public List<Tuple<string, int>> getYearlyQuantityOfSpecificProduct(int srcPhoneID, int srcCategoryID)
        {
            var sql = "select convert(varchar, datepart(year, o.OrderDate)) as OrderYear, SUM(do.Quantity) as Quantity from DetailOrder do join Phone p on do.PhoneID = p.ID join Orders o on do.OrderID = o.ID join Category cat on p.CatID = cat.ID where p.ID = @PhoneID and cat.ID = @CategoryID group by convert(varchar, datepart(year, o.OrderDate)) order by convert(varchar, datepart(year, o.OrderDate)) asc;";

            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@PhoneID";
            sqlParameter.Value = srcPhoneID;

            var sqlParameter1 = new SqlParameter();
            sqlParameter1.ParameterName = "@CategoryID";
            sqlParameter1.Value = srcCategoryID;

            var command = new SqlCommand(sql, _connection);

            command.Parameters.Add(sqlParameter);
            command.Parameters.Add(sqlParameter1);

            var reader = command.ExecuteReader();

            var resultList = new List<Tuple<string, int>>();
            while (reader.Read())
            {
                resultList.Add(Tuple.Create((string)reader["OrderYear"], (int)reader["Quantity"]));
            }
            reader.Close();
            return resultList;
        }

        public List<Tuple<string, int>> getPhoneQuantityInCategory(int srcCategoryID)
        {
            var sql = "select p.PhoneName, sum(do.Quantity) as Quantity from Phone p join DetailOrder do on p.ID = do.PhoneID join Orders o on o.ID = do.OrderID where p.CatID = @SelectedCategory group by p.PhoneName;";

            var sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@SelectedCategory";
            sqlParameter.Value = srcCategoryID;

            var command = new SqlCommand(sql, _connection);

            command.Parameters.Add(sqlParameter);

            var reader = command.ExecuteReader();

            var resultList = new List<Tuple<string, int>>();
            while (reader.Read())
            {
                string phoneName = (string)reader["PhoneName"];
                int quantity = 0;

                if (reader["Quantity"].GetType() != typeof(DBNull))
                {
                    quantity = (int)reader["Quantity"];
                }
                resultList.Add(Tuple.Create(phoneName, quantity));
            }
            reader.Close();
            return resultList;
        }
    }
}
