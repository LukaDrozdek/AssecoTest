using AssecoTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;


namespace AssecoTest.Controllers
{
    public class EmailController : Controller
    {
        public void Email(UserInformation obj)
        {
            try
            {


                var fromAddress = new MailAddress("luka04111993@gmail.com", "From Name");
                var toAddress = new MailAddress("luka04111993@gmail.com", "To adrres");
                const string fromPassword = "pwqtrvdfawbwncnf";
                const string subject = "Asseco";

                //Mail Content

                string body = "Name: " + obj.Name +
                       " /Lastname: " + obj.Username +
                       " /Email: " + obj.Email +
                       " /City: " + obj.City +
                       " /Street: " + obj.Street +
                       " /Suite: " + obj.Suite +
                       " /Zipcode: " + obj.Zipcode +
                       " /Lat: " + obj.Lat +
                       " /Lng: " + obj.Lng +
                       " /Phone: " + obj.Phone +
                       " /Website: " + obj.Website +
                       " /CompanyName: " + obj.CompanyName +
                       " /CompanyCatchPhrase: " + obj.CompanyCatchPhrase +
                       " /Bs: " + obj.Bs;


                //Server details
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                //Mail message
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    //SendEmail
                    smtp.Send(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
