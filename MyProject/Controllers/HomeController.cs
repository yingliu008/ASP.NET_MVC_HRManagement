using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Globalization;
using Newtonsoft.Json;
using MyProject.Models;
using System.Collections.Specialized;
using MyProject.Helpers;
using MyProject.Repo;


namespace MyProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveData(string JsonData)
        {
            var employee = SerializeEmployee(JsonData);
            var success = new EmployeeRepo().Save(employee);

            return Content(JsonConvert.SerializeObject(success));
        }

        public ActionResult GetEmployeeList()
        {
            try
            {
                var list = GetEmployees();

                //var helper = new JqGridHelper();
                //var gridResult = helper.GenerateResults<Employee>(volunteerListModel.VolunteerList, "Id", "Id, FirstName, LastName, Address, City, State, PhoneNumber, Email", searchCriteria.GridParams.CurrentPage, 50);

                //var s = Serializer.ToJson(gridResult);

                return Content(JsonConvert.SerializeObject(list));


            }
            catch (Exception e)
            {
                return new JsonResult { Data = e.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult RemoveEmployee(string JsonData)
        {
            var employee = SerializeEmployee(JsonData);
            var success = new EmployeeRepo().DeleteEmployee(employee);

            return Content(JsonConvert.SerializeObject(success));
        }


        public Employee SerializeEmployee(string employeeString)
        {
            return JsonConvert.DeserializeObject<Employee>(employeeString);
        }

        public List<Employee> GetEmployees()
        {
            return new EmployeeRepo().EmployeeList();
        }
    }
}