using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Mail;

namespace PAZ.Model
{

    /**
    * In deze klassen worden e-mailberichten verzonden naar studenten en docenten 
    * via de mailserver van Gmail.
    * 
    * Auteur: Gökhan 
    */
    public class Emailer
    {
        public string From = string.Empty;
        public string To = string.Empty;
        public string User = string.Empty;
        public string Password = string.Empty;
        public string Subject = string.Empty;
        public string Body = string.Empty;
        public string Host = string.Empty;
        public int Port;
        public bool IsHtml = true;
        public int SendUsing = 0; //0 = Network, 1 = PickupDirectory, 2 = SpecifiedPickupDirectory
        public bool UseSSL = true;
        public int AuthenticationMode = 1; //0 = No authentication, 1 = Plain Text, 2 = NTLM authenticatio
        
        public Emailer()
        {

        }


        /**
        * Voert de opdracht in de achtergrond uit. Anders kan de applicatie overbelast worden!
        * 
        * Auteur: Gökhan 
        */
        public void SendEmail()
        {
            new Thread(new ThreadStart(SendMessage)).Start();
        }


        /**
        * Verstuurt het e-mailbericht
        * 
        * Auteur: Gökhan 
        */
        private void SendMessage()
        {
            try
            {
                MailMessage msg = new MailMessage();
                SmtpClient smtpClient = new SmtpClient(Host);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                msg.From = new MailAddress(From);
                msg.To.Add(To);
                msg.Subject = Subject;
                msg.IsBodyHtml = IsHtml;
                msg.Body = Body;


                if (AuthenticationMode > 0)
                {
                    smtpClient.Credentials = new NetworkCredential(User, Password);
                }

                smtpClient.Port = Port;
                smtpClient.EnableSsl = UseSSL;

                try
                {
                    // Stuur het bericht    
                    smtpClient.Send(msg);
                }

                catch (Exception ex)
                {
                    ex.ToString();

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}
