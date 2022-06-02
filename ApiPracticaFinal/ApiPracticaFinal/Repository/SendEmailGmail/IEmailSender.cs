using ApiPracticaFinal.Models.DTO.UsuariosDTO;
using ApiPracticaFinal.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Repository.SendEmailGmail
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string subject, string body, List<UsuarioListaDTO> usuarios);
        Task SendSingleEmailAsync(string email, string subject, string body);
        Task<ResultadosApi> ConfirmEmailPass(string email, string password);
        
    }
}
