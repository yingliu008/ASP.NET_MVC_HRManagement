using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyProject.Repo
{
    public class EmployeeRepo: BaseRepo
    {
        public Employee Find(int id)
        {
            try
            {
                return Db.Employees.Find(id);
            }
            catch(DbException)
            {
                return null;
            }
        }

        public bool Save(Employee employee)
        {
            if (employee.Id <= 0)
            {
                Db.Employees.Add(employee);
            }
            else
            {
                Db.Employees.Attach(employee);
                Db.Entry(employee).State = EntityState.Modified;
            }
            return SaveDbChanges();
        }

        public List<Employee> EmployeeList()
        {
            List<Employee> allEmployees = Db.Employees.ToList();
            return allEmployees;
        }

        public bool DeleteEmployee(Employee employee)
        {
            var selectedEmployee = Db.Employees.Find(employee.Id);
            Db.Entry(selectedEmployee).State = System.Data.Entity.EntityState.Deleted;

            return SaveDbChanges();
        }

    }
}