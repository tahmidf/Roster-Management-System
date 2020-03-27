using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;


using System.Data;
using MySql.Data.MySqlClient;
using Try.Models;
using System.Globalization;

namespace Try.Controllers
{
    public class SearchModelShift
    {
        public string date { get; set; }

    }
    public class HomeController : Controller
    {
        [HttpGet]
        public JsonResult getEmployee()
        {
            List<EmployeeModel> employee = new List<EmployeeModel>();
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            try
            {
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    string query = "SELECT ID, Name FROM roster.employee";
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                employee.Add(new EmployeeModel
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Name = reader["Name"].ToString()

                                });
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = "There is something wrong with your Query";
                Add_New_Roster();
            }

            return Json(employee, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add_New_Roster()
        {
            ViewBag.Message = "Set roster for particular employee.";

            return View();
        }

        [HttpPost]
        public JsonResult saveRoster(Shift[] dataShift)
        {
         
            for (int j = 0; j < dataShift.Length; j++)
            {
                string start_date = dataShift[j].date;
                DateTime roster_begins;
                CultureInfo provider = CultureInfo.InvariantCulture;
                // Parse date-only value with invariant culture.
                string format = "dd/MM/yyyy";
                try
                {
                    roster_begins = DateTime.ParseExact(start_date, format, provider);
                }
                catch (FormatException ex)
                {
                    roster_begins = DateTime.Now;
                    ViewBag.Message = "Problem found! ";
                    Add_New_Roster();
                }
                DateTime roster_ends = roster_begins.AddMonths(1);

                int total_days = (roster_ends - roster_begins).Days;

                string empshift = "";
                string empday = "";
                int counter = 0;

                for (int i = 0; i < total_days; i++)
                {
                    if (i == 0);
                    else
                        roster_begins = roster_begins.AddDays(1);
                    string ex_date = roster_begins.ToString();
                    string[] date = ex_date.Split(' ');

                    if (counter == 0)
                    {
                        empshift = dataShift[j].sunday;
                        empday = "sunday";
                        counter++;
                    }
                    else if (counter == 1)
                    {
                        empshift = dataShift[j].monday;
                        empday = "monday";
                        counter++;
                    }
                    else if (counter == 2)
                    {
                        empshift = dataShift[j].tuesday;
                        empday = "tuesday";
                        counter++;
                    }

                    else if (counter == 3)
                    {
                        empshift = dataShift[j].wednesday;
                        empday = "wednesday";
                        counter++;
                    }
                    else if (counter == 4)
                    {
                        empshift = dataShift[j].thursday;
                        empday = "thursday";
                        counter++;
                    }
                    else if (counter == 5)
                    {
                        empshift = dataShift[j].friday;
                        empday = "friday";
                        counter++;
                    }
                    else if (counter == 6)
                    {
                        empshift = dataShift[j].saturday;
                        empday = "saturday";
                        counter = 0;
                    }
                    string id = dataShift[j].id.ToString();
                    string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                    try
                    {
                        using (MySqlConnection con = new MySqlConnection(constr))
                        {
                            string query = "INSERT INTO roster.shifting (empid,date,shift,day) values ('" + id + "','" + date[0] + "','" + empshift + "','" + empday + "')";

                            using (MySqlCommand cmd = new MySqlCommand(query))
                            {
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();

                                con.Close();
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        ViewBag.Message = "There is something wrong with the query";
                        Add_New_Roster();
                    }
                }
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult getshift(SearchModelShift starting_date)
        {
            DateTime month_begins;
            CultureInfo provider = CultureInfo.InvariantCulture;
            // Parse date-only value with invariant culture.
            string format = "dd/MM/yyyy";
            try
            {
                month_begins = DateTime.ParseExact(starting_date.date, format, provider);
            }
            catch (FormatException ex)
            {
                month_begins = DateTime.Now;
                ViewBag.Message = "Problem found! ";
                Add_New_Roster();
            }
            DateTime month_ends = month_begins.AddDays(6);

            string temp_search_b_date = month_begins.ToString();
            string[] search_date_begin = temp_search_b_date.Split(' ');

            string temp_search_e_date = month_ends.ToString();
            string[] search_date_ends = temp_search_e_date.Split(' ');

            List<EmployeeShift> empshifts = new List<EmployeeShift>();
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            
            try
            {
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    string query = "SELECT employee.ID,employee.Name, shifting.shift,shifting.day,shifting.date FROM roster.employee,roster.shifting where employee.ID=shifting.empid and shifting.date >= '" + search_date_begin[0] + "' and shifting.date <= '" + search_date_ends[0] + "'";
                        
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                empshifts.Add(new EmployeeShift
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Name = reader["Name"].ToString(),
                                    date = reader["date"].ToString(),
                                    shift = reader["shift"].ToString(),
                                    day = reader["day"].ToString()
                                });
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "There is something wrong with your Query";
                Add_New_Roster();
            }
            return Json(empshifts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Previous_Roster()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}