using System;
using System.Diagnostics;
using System.Net.Mail;
using API.Models.Evento;

namespace API.Models
{
    public class SMTPClient
    {
        public static bool ResetPasswordEmail(string name,string email,string token)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(Properties.Settings.Default.SMTPServer, Properties.Settings.Default.SMTPPort);

                smtpClient.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.FromEmail, Properties.Settings.Default.FromPassword);
                // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                MailMessage mail = new MailMessage();

                //Setting From , To and CC
                mail.From = new MailAddress(Properties.Settings.Default.FromEmail, Properties.Settings.Default.FromName);
                mail.To.Add(new MailAddress(email));
                mail.Subject = "ATEMPHAR Plataforma de dados - Reset de Password";
                mail.IsBodyHtml = true;
                mail.Body = @"<head>
                   <title>ATEMPHAR - Pedido de Mudança de Password</title> 
                   </head>
                   <body>
                   <div>
                       <h3>Caro/a " + name + @",</h3> 
                       <p>Recebeu este email porque requisitou uma alteração de password na plataforma de dados da ATEMPHAR. 
                       <br> 
                           Para alterar a password, siga o seguinte link ou cole-o na barra de endereço do seu browser: 
                       <br> 
                       http://localhost:8080/reset/" + token +
                           @"<br> 
                           Se não realizou este pedido, ignore este email. A sua password não sofrerá alterações.
                       </p> 
                       <br> 
                       <p>Obrigado.</p>
                     </div> 
                   </body>";


                smtpClient.Send(mail);
                return true;
            }catch(Exception e)
            {
                Debug.WriteLine("Error sending password reset email-> "+ e.Message);
                return false;
            }
        }

        public static bool PasswordChangedEmail(string email)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(Properties.Settings.Default.SMTPServer, Properties.Settings.Default.SMTPPort);

                smtpClient.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.FromEmail, Properties.Settings.Default.FromPassword);
                // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                MailMessage mail = new MailMessage();

                //Setting From , To and CC
                mail.From = new MailAddress(Properties.Settings.Default.FromEmail, Properties.Settings.Default.FromName);
                mail.To.Add(new MailAddress(email));
                mail.Subject = "ATEMPHAR Plataforma de dados - Alteração de Password efetuada";
                mail.IsBodyHtml = true;
                mail.Body = @"<head>
                        <title>Alteração de Password</title>
                        </head>
                        <body>
                        <div>
                            <p>
                             Informamos que a alteração de password na plataforma de dados da ATEMPHAR foi realizada com sucesso.</p> 
                            <br>
                                Se não realizou esta alteração, sugerimos que entre em contacto com o administrador da plataforma.
                            <div> Obrigado. </div>
                        </div>
                        </body>";

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error sending password changed email-> " + e.Message);
                return false;
            }
        }

        public static bool RegistoEventoEmail(string email,Evento.Evento evento)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(Properties.Settings.Default.SMTPServer, Properties.Settings.Default.SMTPPort);

                smtpClient.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.FromEmail, Properties.Settings.Default.FromPassword);
                // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;
                MailMessage mail = new MailMessage();

                //Setting From , To and CC
                mail.From = new MailAddress(Properties.Settings.Default.FromEmail, Properties.Settings.Default.FromName);
                mail.To.Add(new MailAddress(email));
                mail.Subject = "ATEMPHAR Plataforma de dados - Inscrição em Evento efetuada";
                mail.IsBodyHtml = true;
                mail.Body = @"<head>
                        <title>Inscrição em Evento</title>
                        </head>
                        <body>
                        <div>
                            <p>
                             Informamos que a sua inscrição no evento " + evento.nomeEvento + @" a realizar-se no dia "+ evento.dataEvento + @" foi realizada com sucesso.</p> 
                            <br>
                                Se não realizou esta inscrição, sugerimos que entre em contacto com o administrador da plataforma.
                            <div> Obrigado. </div>
                        </div>
                        </body>";

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error sending inscrição evento email-> " + e.Message);
                return false;
            }
        }

    }
}