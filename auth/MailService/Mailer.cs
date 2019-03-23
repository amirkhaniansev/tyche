/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Mailer
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * Full notice : https://github.com/amirkhaniansev/tyche/tree/master/LICENSE
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
**/

using System.Net;
using System.Net.Mail;

namespace MailSevice
{
    /// <summary>
    /// Class for sending mails
    /// </summary>
    public class Mailer
    {
        /// <summary>
        /// Network credentials
        /// </summary>
        private NetworkCredential _networkCredential;

        /// <summary>
        /// SMTP client
        /// </summary>
        private SmtpClient _smtpClient;

        /// <summary>
        /// Creates new instance of MailService
        /// </summary>
        /// <param name="networkCredential">Network credentials</param>
        public Mailer(NetworkCredential networkCredential)
        {
            // constructing 
            this._networkCredential = networkCredential;

            this._smtpClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = networkCredential,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        /// <summary>
        /// Sends verification key to the given mail address,
        /// </summary>
        /// <param name="to">Mail address</param>
        /// <param name="verifyKey">Verifiaction key</param>
        public void Send(string to, string verifyKey)
        {
            // constructing message
            var mail = new MailMessage
            {
                From = new MailAddress("no-reply.tyche@gmail.com"),
                Subject = "Tyche Mail Service",
                Body = verifyKey  
            };

            mail.To.Add(to);

            // sending message
            this._smtpClient.Send(mail);
        }
    }
}