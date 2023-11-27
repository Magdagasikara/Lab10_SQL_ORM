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

            // allt buggar: om lång rad så bryts det i två och choice räkningen pajar
            // behöver själv styra i två rader och göra choice *2 ibland

            //•	[ ] Hämta alla kunder.Visa företagsnamn, land, region, telefonnummer och antal ordrar de har
            //Sortera på företagsnamn.Användaren ska kunna välja stigande eller fallande ordning.
            //•	[ ] Användaren ska kunna välja en kund i listan. Alla fält (utom IDn) för kunden ska då visas samt en lista på alla ordrar kunden har gjort.
            //•	[ ] Lägga till kund.Användaren ska kunna lägga till en kund och fylla i värden för alla kolumner utom IDn. iD behöver ni generera en slumpad sträng för (5 tecken lång). Om användaren inte fyller i ett värde ska null skickas till databasen, inte en tom sträng. </aside>

            NorthContext context = new NorthContext();
            CustomerRepo customerRepo = new CustomerRepo(context);


            // 1 Show list of customers + Press Enter to choose a customer to show more info about
            int choice = 0;
            List<Customer> custList = customerRepo.GetAllCustomers();
            List<string> custInfoList = customerRepo.GetInfoOnCustomers(custList);

            string menuTitle = "Use up and down arrows to choose a customer you want to know more about!";
            string optionAD = "Press A to sort the list ascending, press D to sort the list descending";
            string optionZ = "Press X to escape";

            // varför måste jag trycka Enter två gånger...

            while (true) {
                choice = Menu.ShowMenu(custInfoList, menuTitle, optionAD, optionZ);
                if (choice == custInfoList.Count) 
                {
                    //option A
                    Console.Clear();
                    custList = customerRepo.GetAllCustomers();
                    custInfoList = customerRepo.GetInfoOnCustomers(custList);
                    choice = Menu.ShowMenu(custInfoList, menuTitle, optionAD, optionZ);
                    continue;
                }
            
            else if (choice == custInfoList.Count + 1)
                {
                    //option D
                    Console.Clear();
                    custList = customerRepo.GetAllCustomers(false);
                    custInfoList = customerRepo.GetInfoOnCustomers(custList);
                    choice = Menu.ShowMenu(custInfoList, menuTitle, optionAD, optionZ);
                    continue;
                }
            else if (choice == custInfoList.Count + 2)
            {
                    //option X

                    Console.Clear();
                    Console.WriteLine("Bye byeee");
                    return;

            }
            else {
                customerRepo.PrintInfoByCustomerId(custList[choice].CustomerId);
                    Console.WriteLine("Press Enter to go back");
                    Console.ReadKey();
                                     
            }
            }

        }
      

        }

    }
        

       
    
