using Lab10_SQL_ORM.Data;
using Lab10_SQL_ORM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10_SQL_ORM
{
    internal class CustomerRepo
    {
        private readonly NorthContext _context;

        public CustomerRepo(NorthContext context)
        {
            _context = context;
        }


        public List<Customer> GetAllCustomers(bool ascendingByCompany = true)
        {
            List<Customer> customersWithOrders = _context.Customers
              .Include(c => c.Orders)
              .OrderBy(c => c.CompanyName)
              .ToList();

            if (!ascendingByCompany)
            {
                customersWithOrders = customersWithOrders
                    .OrderByDescending(g => g.CompanyName)
                    .ToList();
            }

            return customersWithOrders.Take(15).ToList(); // OBS ta bort nån gång!
        }

        public List<string> GetInfoOnCustomers(List<Customer> customerList)
        {
            var customersWithOrders = customerList
                .Select(c => new
                     {
                    c.CustomerId,
                    c.CompanyName,
                    c.Country,
                    c.Region,
                    c.Phone,
                    OrderAmount = c.Orders.Count
                })
                .ToList();

            List<string> info = new List<string>();
            foreach (var customer in customersWithOrders)
            {
                info.Add($"Company: {customer.CompanyName} | Country: {customer.Country} | Region: {customer.Region} | Phone: {customer.Phone} | Number of orders: {customer.OrderAmount}");
            }

            return info;

        }

        public void PrintInfoByCustomerId(string customerId)
        {
            Console.Clear();

            var chosenCustomer = _context.Customers
                .Where(c => c.CustomerId == customerId)
                .Include(c => c.Orders)
                //.ThenInclude(o => o.OrderDetails)
                .SingleOrDefault();

            Console.WriteLine($"Chosen customer: {chosenCustomer.CompanyName}" +
                $"\n\n\tContactName: {chosenCustomer.ContactName}" +
                $"\n\tContactTitle: {chosenCustomer.ContactTitle}" +
                $"\n\tAddress: {chosenCustomer.Address}" +
                $"\n\tCity: {chosenCustomer.City}" +
                $"\n\tRegion: {chosenCustomer.Region}" +
                $"\n\tPostalCode: {chosenCustomer.PostalCode}" +
                $"\n\tCountry: {chosenCustomer.Country}" +
                $"\n\tPhone: {chosenCustomer.Phone}" +
                $"\n\tFax: {chosenCustomer.Fax}" +
                $"\n\nPlaced orders:");

            foreach (var order in chosenCustomer.Orders)
            {
                Console.WriteLine($"\tOrder nr {order.OrderId} placed on {order.OrderDate}" );
                //lyckas inte formatera om OrderDate här
            }

            Console.WriteLine("");

        }

    }
}
