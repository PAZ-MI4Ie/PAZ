using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;

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
        public int AuthenticationMode = 1; //0 = No authentication, 1 = Plain Text, 2 = NTLM authentication

        private Queue<Email> _emailQueue;

        public Emailer()
        {
            _emailQueue = new Queue<Email>();
        }


        /**
        * Voert de opdracht in de achtergrond uit. Anders kan de applicatie overbelast worden!
        * 
        * Auteur: Gökhan en Yorg
        */
        public void SendEmail()
        {
            _emailQueue.Enqueue(new Email(To, Subject, Body));
            Body = string.Empty;

            new Thread(new ThreadStart(SendMessage)).Start();
        }


        /**
        * Verstuurt het e-mailbericht
        * 
        * Auteur: Gökhan en Yorg
        */
        private void SendMessage()
        {
            try
            {
                MailMessage msg = new MailMessage();
                SmtpClient smtpClient = new SmtpClient(Host);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                Email emailMessage = _emailQueue.Dequeue();

                msg.From = new MailAddress(From);
                msg.To.Add(emailMessage.To);
                msg.Subject = emailMessage.Subject;
                msg.IsBodyHtml = IsHtml;
                msg.Body = emailMessage.Body;

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
