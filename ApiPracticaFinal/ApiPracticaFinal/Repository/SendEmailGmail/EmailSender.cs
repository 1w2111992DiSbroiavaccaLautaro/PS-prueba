using ApiPracticaFinal.Repository.SendGrid;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using ApiPracticaFinal.Models.DTO.UsuariosDTO;
using ApiPracticaFinal.Resultados;
using ApiPracticaFinal.Models;

namespace ApiPracticaFinal.Repository.SendEmailGmail
{
    public class EmailSender : IEmailSender
    {
        //private SmtpClient Cliente { get; }
        private readonly EmailSenderOptions _emailSenderOptions;
        private readonly dd4snj9pkf64vpContext _context;

        public EmailSender(IOptions<EmailSenderOptions> emailSenderOptions, dd4snj9pkf64vpContext context)
        {
            _emailSenderOptions = emailSenderOptions.Value;
            _context = context;
        }

        public async Task SendEmailAsync(string subject, string body, List<UsuarioListaDTO> usuarios)
        {
            var emailToSend = new MimeMessage();

            var personalizationIndex = 0;
            InternetAddressList list = new InternetAddressList();
            foreach (var person in usuarios)
            {
                list.Add(new MailboxAddress(person.Nombre, person.Email));               
                personalizationIndex++;
            }

            emailToSend.From.Add(new MailboxAddress(subject, "111992lautaro@gmail.com"));
            emailToSend.To.AddRange(list);
            emailToSend.Subject = subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = body;
            emailToSend.Body = builder.ToMessageBody();

            try
            {
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("111992lautaro@gmail.com", "Valentina3870");
                await smtp.SendAsync(emailToSend);
                smtp.Disconnect(true);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task SendSingleEmailAsync(string email, string subject, string body)
        {
            var emailToSend = new MimeMessage();

            emailToSend.Sender = MailboxAddress.Parse("111992lautaro@gmail.com");
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            var builder = new BodyBuilder();

            builder.HtmlBody = body;
            emailToSend.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();

            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("111992lautaro@gmail.com", "Nicolas3870");
            await smtp.SendAsync(emailToSend);
            smtp.Disconnect(true);
        }

        public async Task<ResultadosApi> ConfirmEmailPass(string email, string password)
        {
            ResultadosApi resultados = new ResultadosApi();

            var user = _context.Usuarios.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                resultados.Ok = false;
                resultados.Error = "Usuario no encontrado";
                return resultados;
            }
            else
            {
                if (password.Equals(user.Password))
                {
                    resultados.Ok = true;
                    user.Activo = true;
                    _context.Usuarios.Update(user);
                    await _context.SaveChangesAsync();
                    resultados.Respuesta = user;
                    resultados.InfoAdicional = "Email confirmado";
                    return resultados;
                }
                else
                {
                    resultados.Ok = false;
                    resultados.Error = "No se pudo registrar";
                    return resultados;
                }
            }
        }

        
    }
}
