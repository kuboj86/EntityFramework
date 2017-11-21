using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using TestEntityFrameWork.Models;

namespace TestEntityFrameWork.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {

            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ListCustomer()
        {
            //1.creating an object for the ORM    
            NorthwindEntities ORM = new NorthwindEntities();

            //2. Load the data from the DBset intoa data structure
            List<Customer> CustomerList=ORM.Customers.ToList();
            //3. filter the data (Optional)
            ViewBag.CustomerList = CustomerList;

            return View("CustomersView");
        }

        public ActionResult ListCustomersByCountry(string Country)
        {
            NorthwindEntities ORM = new NorthwindEntities();

            List<Customer> OutputList = new List<Customer>();

            foreach (Customer CustomerRecord in ORM.Customers.ToList())
            {
                if (CustomerRecord.Country.ToLower() == Country.ToLower())
                {
                    OutputList.Add(CustomerRecord);
                }
            }
            ViewBag.CustomerList = OutputList;

            return View("CustomersView");
        }

        public ActionResult ListCustomersById(string CustomerIdNumber)
       {
           NorthwindEntities ORM = new NorthwindEntities();

           List<Customer> OutputList = new List<Customer>();

            foreach (Customer CustomerRecord in ORM.Customers.ToList())
           {
               if (CustomerRecord.CustomerID.ToLower().Contains(CustomerIdNumber.ToLower()))
               {
                   OutputList.Add(CustomerRecord);
               }
           }
           ViewBag.CustomerList = OutputList;
           return View("CustomersView");
       }
    }
}