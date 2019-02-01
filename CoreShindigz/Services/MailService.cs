
using MailKit.Net.Smtp;
using MimeKit;


namespace CoreShindigz.Services
{
    // This is the generic class to collect the data we need
    public class MailMessage
    {
        public string[] To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set;}
        public bool IsHtml { get; set; } = false;
    }

    // Interface and implementation to use mailkit for an email send
    public interface IMailService
    {
        void Send(MailMessage message);
    }


    // IMailService is from mailkit. 
    public class SmtpMailService : IMailService    {
       
        public void Send(MailMessage message)
        {
            var mm = new MimeMessage();

            // can have multiple recipients
            foreach (var address in message.To)
            {
                mm.To.Add(new MailboxAddress(address));
            }

            //only one sender
            mm.From.Add(new MailboxAddress(message.From));

            mm.Subject = message.Subject;
            
            // see mailKit docs for other body types
            mm.Body = new TextPart("plain") 
            {
                Text = message.Body
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s,c,h,e) => true;

                //client.Connect("outlook.office365.com", 25, false);
                client.Connect("192.100.100.55", 25, false);

                client.Send(mm);

                client.Disconnect(true);
            }
        }
    }
}
