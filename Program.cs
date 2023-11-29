using Lab10_SQL_ORM.Data;
using Lab10_SQL_ORM.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;
using System.Diagnostics.Metrics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Primitives;

namespace Lab10_SQL_ORM
{
    internal class Program
    {
        static void Main(string[] args)
        {

            NorthContext context = new NorthContext();
            CustomerRepo customerRepo = new CustomerRepo(context);

            // next time I will move this intro out of Main

          
            while (true)
            {
                int choice = 0;
                string[] menuOptions = ["Show all customers", "Add a customer", "Hejdå!"];
                choice = Menu.ShowMenu(menuOptions, 1, "Hi user, use arrows and press Enter to choose your option", "");
                switch (choice)
                {
                    case 0:
                        customerRepo.ShowCustomers();
                        break;
                    case 1:
                        customerRepo.AddCustomer();
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Time to say goodbye!");
                        return;
                }
            }
        }



    }

}




