using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.DynamicData;
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

            ViewBag.CountryList = ORM.Customers.Select(x => x.Country).Distinct().ToList(); 


            return View("CustomersView");
        }

        public ActionResult ListCustomersByCountry(string Country)
        {

            NorthwindEntities ORM = new NorthwindEntities();

            List<Customer> OutputList = new List<Customer>();

            ViewBag.CountryList = ORM.Customers.Select(x => x.Country).Distinct().ToList();


            //foreach (Customer CustomerRecord in ORM.Customers.ToList())
            //{
            //    if (CustomerRecord.Country.ToLower() == Country.ToLower())
            //    {
            //        OutputList.Add(CustomerRecord);
            //    }
            //}

            //LINQ Query Syntax
            //OutputList = (from CustomerRecord in ORM.Customers
            //    where CustomerRecord.Country == Country
            //    select CustomerRecord).ToList();

            //LINQ Method Syntax                Lambda expression

            //OutputList = ORM.Customers.Where(x => x.Country == Country).ToList();

            OutputList = ORM.Customers.SqlQuery($"select * from customers where country=@param1" , new SqlParameter("@param1",
                Country)).ToList();


            ViewBag.CustomerList = OutputList;

            return View("CustomersView");
        }

        // public ActionResult ListCustomersById(string CustomerIdNumber)
        //{
        //    NorthwindEntities ORM = new NorthwindEntities();

        //    List<Customer> OutputList = new List<Customer>();

        //     foreach (Customer CustomerRecord in ORM.Customers.ToList())
        //    {
        //        if (CustomerRecord.CustomerID.ToLower().Contains(CustomerIdNumber.ToLower()))
        //        {
        //            OutputList.Add(CustomerRecord);
        //        }
        //    }
        //ViewBag.CustomerList = OutputList;
        //return View("CustomersView");
        //}
        public ActionResult DeleteCustomer(string CustomerId)
        {   //TODO: Add exception handling for database exception
            //1 find the record
            NorthwindEntities ORM = new NorthwindEntities();

            //Find looks for a record based on the primary key

            Customer RecordToBeDeleted = ORM.Customers.Find(CustomerId);

            //2 Delete the recourd using the ORM
            if (RecordToBeDeleted != null)
            {
                ORM.Customers.Remove(RecordToBeDeleted);
                ORM.SaveChanges();
            }
            //3 Reload the list

            //3 Reload the list
            return RedirectToAction("ListCustomer");
        }

        public ActionResult NewCustomerForm()
        {
            return View();
        }

        public ActionResult SaveCustomer(Customer NewCustomerRecord)
        {
            //1 validation
            if (ModelState.IsValid)
            {
                // 2. add the new record to ORM, save changes to database
                NorthwindEntities ORM= new NorthwindEntities();

                ORM.Customers.Add(NewCustomerRecord);
                ORM.SaveChanges();


                //3. showing the list of all customers

                return RedirectToAction("ListCustomer");
            }
            else
            {
                //if validation fails
                //go back to the form and show the list of errors
                return View("NewCustomerForm");
            }
        }

        public ActionResult UpdateCustomer(string CustomerID)
        {
            //1 find the customer by using the CUstomerID
            NorthwindEntities ORM = new NorthwindEntities();

            Customer RecordToBeUpdated = ORM.Customers.Find(CustomerID);
            //2 Load the record into a Viewbag
            if (RecordToBeUpdated != null)
            {
                ViewBag.RecordToBeUpdated = RecordToBeUpdated;
                //3 Go to the view that has the update form

                return View("UpdateCustomerForm");
            }
            else
            {
                return RedirectToAction("ListCustomer");
            }
        }

        public ActionResult SaveUpdatedCustomer(Customer RecordToBeUpdated)
        {    //TODO: Validation and exception handling
            //1 find the original record
            NorthwindEntities ORM = new NorthwindEntities();

            Customer temp =ORM.Customers.Find(RecordToBeUpdated.CustomerID);

            //2 do the update on the record, then save to the database

            temp.CompanyName = RecordToBeUpdated.CompanyName;
            temp.City = RecordToBeUpdated.City;
            temp.Country = RecordToBeUpdated.Country;

            ORM.Entry(temp).State = System.Data.Entity.EntityState.Modified;
            ORM.SaveChanges();

            //3 load all the customer records

            return RedirectToAction("ListCustomer");
        }
    }
}