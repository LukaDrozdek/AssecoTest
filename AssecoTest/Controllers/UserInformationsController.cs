using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssecoTest.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using Newtonsoft.Json.Linq;

namespace AssecoTest.Controllers
{
    public class UserInformationsController : Controller
    {

        private readonly IConfiguration _configuration;
        public EmailController emailController = new EmailController();

        public UserInformationsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: UserInformations
       
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("AllUsers", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }


        // GET: UserInformations/AddOrEdit/5
        [HttpGet("Limit")]
        public IActionResult AddOrEdit(int? Id)
        {
            UserInformation userInformation = new UserInformation();
            if(Id > 0)
            {
                userInformation = FetchUserID(Id);
            }
            return View(userInformation);
        }

        // POST: UserInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEditPost(int Id, [Bind("Id,Name,Username,Email,Street,Suite,City,Zipcode,Lat,Lng,Phone,Website,CompanyName,CompanyCatchPhrase,Bs")] UserInformation userInformation)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("AddOrEdit", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;


                Json(sqlCmd);
                WebClient client = new WebClient();
                var strPageCode = client.DownloadString("http://jsonplaceholder.typicode.com/users");

                dynamic blogPosts = JArray.Parse(strPageCode);
                for (int i = 0; i < blogPosts.Count; i++)
                {
                    dynamic blogPost = blogPosts[i];


                    int id = blogPost.id;
                    string name = blogPost.name;
                    string username = blogPost.username;
                    string email = blogPost.email;

                    string street = blogPost.address.street;
                    string suite = blogPost.address.suite;
                    string city = blogPost.address.city;
                    string zipcode = blogPost.address.zipcode;
                    float lat = blogPost.address.geo.lat;
                    float lng = blogPost.address.geo.lng;

                    string phone = blogPost.phone;
                    string website = blogPost.website;

                    string conpanyName = blogPost.company.name;
                    string catchPhrase = blogPost.company.catchPhrase;
                    string bs = blogPost.company.bs;


                    if (userInformation.Username == username && userInformation.Email == email)
                    {
                        sqlCmd.Parameters.AddWithValue("Id", id);
                        sqlCmd.Parameters.AddWithValue("Name", name);
                        sqlCmd.Parameters.AddWithValue("Username", username);
                        sqlCmd.Parameters.AddWithValue("Email", email);
                        sqlCmd.Parameters.AddWithValue("City", city);
                        sqlCmd.Parameters.AddWithValue("Street", street);
                        sqlCmd.Parameters.AddWithValue("Suite", suite);
                        sqlCmd.Parameters.AddWithValue("Zipcode", zipcode);
                        sqlCmd.Parameters.AddWithValue("Lat", lat);
                        sqlCmd.Parameters.AddWithValue("Lng", lng);
                        sqlCmd.Parameters.AddWithValue("Phone", phone);
                        sqlCmd.Parameters.AddWithValue("Website", website);
                        sqlCmd.Parameters.AddWithValue("CompanyName", conpanyName);
                        sqlCmd.Parameters.AddWithValue("CompanyCatchPhrase", catchPhrase);
                        sqlCmd.Parameters.AddWithValue("Bs", bs);
                        emailController.Email(userInformation);
                        sqlCmd.ExecuteNonQuery();
                        return RedirectToAction(nameof(Index));

                    }

                }
                    sqlCmd.Parameters.AddWithValue("Id", userInformation.Id);
                    sqlCmd.Parameters.AddWithValue("Name", userInformation.Name);
                    sqlCmd.Parameters.AddWithValue("Username", userInformation.Username);
                    sqlCmd.Parameters.AddWithValue("Email", userInformation.Email);
                    sqlCmd.Parameters.AddWithValue("City", userInformation.City);
                    sqlCmd.Parameters.AddWithValue("Street", userInformation.Street);
                    sqlCmd.Parameters.AddWithValue("Suite", userInformation.Suite);
                    sqlCmd.Parameters.AddWithValue("Zipcode", userInformation.Zipcode);
                    sqlCmd.Parameters.AddWithValue("Lat", userInformation.Lat);
                    sqlCmd.Parameters.AddWithValue("Lng", userInformation.Lng);
                    sqlCmd.Parameters.AddWithValue("Phone", userInformation.Phone);
                    sqlCmd.Parameters.AddWithValue("Website", userInformation.Website);
                    sqlCmd.Parameters.AddWithValue("CompanyName", userInformation.CompanyName);
                    sqlCmd.Parameters.AddWithValue("CompanyCatchPhrase", userInformation.CompanyCatchPhrase);
                    sqlCmd.Parameters.AddWithValue("Bs", userInformation.Bs);
                    emailController.Email(userInformation);
                    sqlCmd.ExecuteNonQuery();
            }
                return RedirectToAction(nameof(Index));
        }

        // GET: UserInformations/Delete/5
        public IActionResult Delete(int? Id)
        {
            UserInformation userInformation = FetchUserID(Id);

            return View(userInformation);
        }

        // POST: UserInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("DeleteUsersByID", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("Id", Id);


                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public UserInformation FetchUserID(int? Id)
        {
            UserInformation userInformationModel = new UserInformation();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("UsersByID", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("Id", Id);
                sqlDa.Fill(dtbl);
                if(dtbl.Rows.Count == 1)
                {
                    userInformationModel.Id = Convert.ToInt32(dtbl.Rows[0]["Id"].ToString());
                    userInformationModel.Name = dtbl.Rows[0]["Name"].ToString();
                    userInformationModel.Username = dtbl.Rows[0]["Username"].ToString();
                    userInformationModel.Email = dtbl.Rows[0]["Email"].ToString();
                    userInformationModel.City = dtbl.Rows[0]["City"].ToString();
                    userInformationModel.Street = dtbl.Rows[0]["Street"].ToString();
                    userInformationModel.Suite = dtbl.Rows[0]["Suite"].ToString();
                    userInformationModel.Zipcode = dtbl.Rows[0]["ZipCode"].ToString();
                    userInformationModel.Lat = dtbl.Rows[0]["Lat"].ToString();
                    userInformationModel.Lng = dtbl.Rows[0]["Lng"].ToString();
                    userInformationModel.Phone = dtbl.Rows[0]["Phone"].ToString();
                    userInformationModel.Website = dtbl.Rows[0]["Website"].ToString();
                    userInformationModel.CompanyName = dtbl.Rows[0]["CompanyName"].ToString();
                    userInformationModel.CompanyCatchPhrase = dtbl.Rows[0]["CompanyCatchPhrase"].ToString();
                    userInformationModel.Bs = dtbl.Rows[0]["Bs"].ToString();
                }
                return userInformationModel;
            }
        }

        // .biz Mail

        public IActionResult BizEmail()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("AllBizEmail", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View("Index", dtbl);
        }
    }
}
