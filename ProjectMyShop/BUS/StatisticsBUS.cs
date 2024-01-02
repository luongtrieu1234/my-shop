using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectMyShop.DAO;

namespace ProjectMyShop.BUS
{
    internal class StatisticsBUS
    {
        private StatisticsDAO _statisticsDAO;

        public StatisticsBUS()
        {
            _statisticsDAO = new StatisticsDAO();
            if (_statisticsDAO.CanConnect())
            {
                _statisticsDAO.Connect();
            }
        }

        public string getTotalRevenueUntilDate(DateTime src)
        {
            return _statisticsDAO.getTotalRevenueUntilDate(src);
        }

        public string getTotalProfitUntilDate(DateTime src)
        {
            return _statisticsDAO.getTotalProfitUntilDate(src);
        }

        public int getTotalOrdersUntilDate(DateTime src)
        {
            return _statisticsDAO.getTotalOrdersUntilDate(src);
        }

        public List<Tuple<string, decimal>> getDailyRevenue(DateTime src)
        {
            return _statisticsDAO.getDailyRevenue(src);
        }

        public List<Tuple<string, decimal>> getWeeklyRevenue(DateTime src)
        {
            return _statisticsDAO.getWeeklyRevenue(src);
        }

        public List<Tuple<string, decimal>> getMonthlyRevenue(DateTime src)
        {
            return _statisticsDAO.getMonthlyRevenue(src);
        }

        public List<Tuple<string, decimal>> getYearlyRevenue()
        {
            return _statisticsDAO.getYearlyRevenue();
        }

        public List<Tuple<string, decimal>> getDailyProfit(DateTime src)
        {
            return _statisticsDAO.getDailyProfit(src);
        }

        public List<Tuple<string, decimal>> getWeeklyProfit(DateTime src)
        {
            return _statisticsDAO.getWeeklyProfit(src);
        }

        public List<Tuple<string, decimal>> getMonthlyProfit(DateTime src)
        {
            return _statisticsDAO.getMonthlyProfit(src);
        }

        public List<Tuple<string, decimal>> getYearlyProfit()
        {
            return _statisticsDAO.getYearlyProfit();
        }

        public List<Tuple<string, int>> getDailyQuantityOfSpecificProduct(int srcPhoneID, int srcCategoryID, DateTime srcDate)
        {
            return _statisticsDAO.getDailyQuantityOfSpecificProduct(srcPhoneID, srcCategoryID, srcDate);
        }

        public List<Tuple<string, int>> getWeeklyQuantityOfSpecificProduct(int srcPhoneID, int srcCategoryID, DateTime srcDate)
        {
            return _statisticsDAO.getWeeklyQuantityOfSpecificProduct(srcPhoneID, srcCategoryID, srcDate);
        }

        public List<Tuple<string, int>> getMonthlyQuantityOfSpecificProduct(int srcPhoneID, int srcCategoryID, DateTime srcDate)
        {
            return _statisticsDAO.getMonthlyQuantityOfSpecificProduct(srcPhoneID, srcCategoryID, srcDate);
        }

        public List<Tuple<string, int>> getYearlyQuantityOfSpecificProduct(int srcPhoneID, int srcCategoryID)
        {
            return _statisticsDAO.getYearlyQuantityOfSpecificProduct(srcPhoneID, srcCategoryID);
        }

        public List<Tuple<string, int>> getProductQuantityInCategory(int srcCategoryID)
        {
            return _statisticsDAO.getProductQuantityInCategory(srcCategoryID);
        }
    }
}
