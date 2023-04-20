
using Clean.Infrastructure.CleanDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.Email
{
    public static class EmailBuilder
    {
        internal static EmailMessage NewUserAssigned(Employee employee, string username, string password)
        {
            EmailMessage message = new EmailMessage();
            StringBuilder sb = new StringBuilder();
            message.Subject = "New User";
            sb.Append($"Hello {employee.FirstName} {employee.LastName}");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine("You were designated to use the Clean Application");
            sb.Append("Your credentials are as follow:");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine($"Username :{username}");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine($"Password : {password}");
            message.Body = sb.ToString();
            return message;
        }

        internal static EmailMessage DisableUser(string userName)
        {
            EmailMessage message = new EmailMessage();
            StringBuilder sb = new StringBuilder();
            message.Subject = "Compte désactivé";
            sb.Append($"Bonjour");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine($"Votre compte {userName} à été désactiver par l'administrateur ");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine($" Contactez le pour plus d'informations,merci de votre coopération");
            message.Body = sb.ToString();
            return message;

        }

        internal static EmailMessage EnabledUser(Employee employee, string username)
        {
            EmailMessage message = new EmailMessage();
            StringBuilder sb = new StringBuilder();
            message.Subject = "Account Activated";
            sb.Append($"Hello {employee.FirstName} {employee.LastName}");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine($"Your account {username} has been activated");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine("use your last credentials to login or contact the administrator if you don't remember them");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            message.Body = sb.ToString();
            return message;
        }

        internal static EmailMessage PasswordChanged(string username, string password)
        {
            EmailMessage message = new EmailMessage();
            StringBuilder sb = new StringBuilder();
            message.Subject = "Your password has been reset";
            sb.Append($"Hello");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine("An Administrator has reset your password,");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.Append("To Log in use the following credentials :");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine($"User :{username}");
            sb.AppendLine("<br/>");
            sb.AppendLine("<br/>");
            sb.AppendLine($"Password : {password}");
            message.Body = sb.ToString();
            return message;
        }
    }
}
