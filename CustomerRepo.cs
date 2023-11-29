using Lab10_SQL_ORM.Data;
using Lab10_SQL_ORM.Models;
using Microsoft.EntityFrameworkCore;

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

            return customersWithOrders.ToList(); // Take(15). when testing
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
                //info.Add($"Company: {customer.CompanyName} | Country: {customer.Country} | Region: {customer.Region} | Phone: {customer.Phone} | Number of orders: {customer.OrderAmount}");
                info.Add($"Company: {customer.CompanyName}\nCountry: {customer.Country} | Region: {customer.Region} | Phone: {customer.Phone}\nNumber of orders: {customer.OrderAmount}");
            }

            return info;

        }

        public void PrintInfoByCustomerId(string customerId, string title)
        {
            Console.Clear();

            var chosenCustomer = _context.Customers
                .Where(c => c.CustomerId == customerId)
                .Include(c => c.Orders)
                //.ThenInclude(o => o.OrderDetails)
                .SingleOrDefault();

            Console.WriteLine($"{title}: {chosenCustomer.CompanyName}" +
                $"\n\n\tContactName: {chosenCustomer.ContactName}" +
                $"\n\tContactTitle: {chosenCustomer.ContactTitle}" +
                $"\n\tAddress: {chosenCustomer.Address}" +
                $"\n\tCity: {chosenCustomer.City}" +
                $"\n\tRegion: {chosenCustomer.Region}" +
                $"\n\tPostalCode: {chosenCustomer.PostalCode}" +
                $"\n\tCountry: {chosenCustomer.Country}" +
                $"\n\tPhone: {chosenCustomer.Phone}" +
                $"\n\tFax: {chosenCustomer.Fax}");

            if (chosenCustomer.Orders.Count != 0)
            {
                Console.WriteLine($"\nPlaced orders:");
                foreach (var order in chosenCustomer.Orders)
                {
                    Console.WriteLine($"\tOrder nr {order.OrderId} placed on {order.OrderDate?.ToShortDateString() ?? "(No date!)"}");
                    //lyckades formatera om OrderDate först med "null-coalescing operator"
                    // "The null-coalescing operator ?? is used to provide a default value for a nullable type
                    // or a value that might be null. It's a concise way to handle scenarios where a variable
                    // might be null, allowing you to specify a default value if the variable is null."
                }
            }
            else
            {
                Console.WriteLine($"\n\t{chosenCustomer.CompanyName} hasn't placed any orders yet.");
            }

            Console.WriteLine("");

        }

        public bool AddCustomer()
        {
            // massa frågor inga svar 
            // hur ordnar man sin kod, ska man bara ha anrop till DB här och inte lulllull för att user ska ange saker?
            // men om det senare, hur ska jag hitta på bra namn, AddCustomer här och FillInInfoAboutNewCustomer eller vadå...
            // även: för att validera, ha en separat klass för det?
            Console.Clear();
            Console.WriteLine("To add a customer please fill in following customer information.\n");

            Console.Write("Company name: ");
            string companyName;
            while (true)
            {
                companyName = Console.ReadLine();
                if (companyName is not null && companyName.Length > 2)
                {
                    break;
                }
                Console.Write("Company name has to be at least 3 characters long. Try again: ");
            }
            Console.Write("Contact name: ");
            string contactName = Console.ReadLine();
            Console.Write("Contact title: ");
            string contactTitle = Console.ReadLine();
            Console.Write("Address: ");
            string address = Console.ReadLine();
            Console.Write("City: ");
            string city = Console.ReadLine();
            Console.Write("Region: ");
            string region = Console.ReadLine();
            Console.Write("Postal code: ");
            string postalCode = Console.ReadLine();
            Console.Write("Country: ");
            string country = Console.ReadLine();
            Console.Write("Phone: ");
            string phone = Console.ReadLine();
            Console.Write("Fax: ");
            string fax = Console.ReadLine();

            //random customerID, 5 letters and unique
            string customerId = "";
            while (true)
            {
                char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                Random random = new Random();
                for (int i = 0; i < 5; i++)
                {
                    int randomNumber = random.Next(chars.Length);
                    customerId += chars[randomNumber].ToString();
                }
                if (!_context.Customers.Any(c => c.CustomerId == customerId))
                {
                    break;
                }
            }
            Customer customer = new Customer()
            {
                CustomerId = customerId,
                CompanyName = companyName,
                ContactName = string.IsNullOrEmpty(contactName) ? null : contactName,
                ContactTitle = string.IsNullOrEmpty(contactTitle) ? null : contactTitle,
                Address = string.IsNullOrEmpty(address) ? null : address,
                City = string.IsNullOrEmpty(city) ? null : city,
                Region = string.IsNullOrEmpty(region) ? null : region,
                PostalCode = string.IsNullOrEmpty(postalCode) ? null : postalCode,
                Country = string.IsNullOrEmpty(country) ? null : country,
                Phone = string.IsNullOrEmpty(phone) ? null : phone,
                Fax = string.IsNullOrEmpty(fax) ? null : fax,
            };
            _context.Customers.Add(customer);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error adding user: {e}");
                Console.ReadKey();
                return false;
            }


            PrintInfoByCustomerId(customerId, "Successfully added a new customer");
            Console.SetCursorPosition(0, 0);
            Console.ReadKey();
            return true;
        }
        public void ShowCustomers()
        {

            // Shows list of customers + press Enter to choose a customer to show more info about

            int choice = 0;
            List<Customer> custList = GetAllCustomers();
            List<string> custInfoList = GetInfoOnCustomers(custList);

            string menuTitle = "Use up and down arrows to choose a customer you want to know more about!";
            string optionsADX = "Press A to sort the list ascending, press D to sort the list descending\nPress X to escape";

            while (true)
            {
                choice = Menu.ShowMenu(custInfoList, 3, menuTitle, optionsADX);
                if (choice == custInfoList.Count)
                {
                    //option A
                    Console.Clear();
                    custList = GetAllCustomers();
                    custInfoList = GetInfoOnCustomers(custList);
                }

                else if (choice == custInfoList.Count + 1)
                {
                    //option D
                    Console.Clear();
                    custList = GetAllCustomers(false);
                    custInfoList = GetInfoOnCustomers(custList);
                }
                else if (choice == custInfoList.Count + 2)
                {
                    //option X
                    Console.Clear();
                    return;

                }
                else
                {
                    PrintInfoByCustomerId(custList[choice].CustomerId, "Chosen customer");
                    Console.WriteLine("Press any key to go back to the list");
                    Console.SetCursorPosition(0, 0);
                    Console.ReadKey();

                }
            }

        }

    }
}
