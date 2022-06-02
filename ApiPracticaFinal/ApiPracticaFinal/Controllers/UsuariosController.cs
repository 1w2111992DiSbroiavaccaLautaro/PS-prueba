using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.UsuariosDTO;
using ApiPracticaFinal.Repository.SendEmailGmail;
using ApiPracticaFinal.Repository.SendGrid;
using ApiPracticaFinal.Repository.Usuarios;
using ApiPracticaFinal.Resultados;
using Aspose.Words;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiPracticaFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Prog3")]

    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository usuarioRepository;
        private readonly IMailService mailService;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;
        public UsuariosController(IUsuarioRepository usuarioRepository, IMailService mailService, IConfiguration configuration, IEmailSender emailSender)
        {
            this.usuarioRepository = usuarioRepository;
            this.mailService = mailService;
            this.configuration = configuration;
            this.emailSender = emailSender;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<UsuarioListaDTO>> Get()
        {
            return await usuarioRepository.GetUsuariosAsync();
        }

        [HttpGet("sendMail")]
        public async Task<IActionResult> SendEmail()
        {
            //await emailSender.SendEmailAsync(email, tema, mensaje);
            var usuarios = usuarioRepository.GetUsuariosMail();
            await mailService.ExecuteMailMultiple(usuarios, "Envio Masivo", "Envio de Mail Masivo");
            //await mailService.ExecuteMail("lautarodisbro@gmail.com", "Login", "Holaaaa desde Swagger");
            return Ok();
        }

        // GET api/<UsuariosController>/5
        [HttpGet("{email}")]
        public async Task<List<UsuarioListaDTO>> GetPorEmail(string email)
        {
            return await usuarioRepository.GetUsuarioEmail(email);
        }

        [HttpGet("envioGmail")]
        public async Task<IActionResult> SendGmail()
        {
            try
            {
                await emailSender.SendSingleEmailAsync("lautarodisbro@gmail.com", "Mail individual", "Cuerpo del mail");
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception("Excepcion: " + ex);
            }
        }

        //Este es el metodo que va para registrar usuarios
        [HttpPost]
        public async Task<ResultadosApi> Post(UsuarioSignUp usu)
        {
            var usuario = await usuarioRepository.Signup(usu);
            //validar si el usuario se logro insertar correctamente en la base de datos antes de enviar el mail de confirmacion

            if (usuario.Ok == true)
            {
                string url = $"{configuration["AppUrl"]}/api/usuarios/confirmarUsuxValidador?email={usu.Email}&nombre={usu.Nombre}&pass={usu.Password}";
                string url2 = $"{configuration["AppUrl"]}/api/usuarios/denegarUsuxValidador?email={usu.Email}&nombre={usu.Nombre}";

                //string url = $"{configuration["AppUrl"]}/api/usuarios/confirmemail?email={usu.Email}&pass={usu.Password}";

                //await emailSender.SendSingleEmailAsync(usu.Email, "Confirmar el mail", "<h1>Bienvenido a la confirmacion de mail</h1>" +
                //    $"<p>Porfavor, confirme su mail hacienco <a href='{url}'>Click aquí</a></p>");

                var validadores = usuarioRepository.ListaValidadoresMail();

                foreach (var i in validadores)
                {
                    if (i.Id == 6)
                    {
                        await emailSender.SendSingleEmailAsync(i.Email, "Solicitud de confirmacion de mail", 
                            "<h1>Confirmación de mail de un nuevo registro</h1>" +
                            $"<ul><li>Nombre: {usu.Nombre} </li><li>Email: {usu.Email} </li></ul><br>" +
                            $"<p>Porfavor, confirme el usuario o deniegue su acceso: </p>" +
                            $"<button><a href='{url}' style='color: green;' 'text-decoration: none;'>Aceptar</a></button>" +
                            $"<button><a href='{url2}' style='color: red;' 'text-decoration: none;'>Rechazar</a></button>");
                    }
                }
            }

            return usuario;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ResultadosApi>> Login(UsuarioLogin usu)
        {
            var usuario = await usuarioRepository.Login(usu);

            return usuario;
        }

        [Authorize]
        // PUT api/<UsuariosController>/5
        [HttpPut("UpdatePassword")]
        public async Task<ResultadosApi> UpdatePassword(UsuarioUpdatePass usuario)
        {
            return await usuarioRepository.UpdatePassword(usuario);
        }

        [Authorize]
        [HttpPut("UpdateRol")]
        public async Task<Usuario> UpdateRol(UsuarioRolDTO usu)
        {
            return await usuarioRepository.UpdateRol(usu);
        }

        [Authorize]
        [HttpPut("UpdateCredenciales")]
        public async Task<ResultadosApi> UpdateCredenciales(UsuarioCredencialDTO usu)
        {
            return await usuarioRepository.UpdateCredenciales(usu);
        }

        [Authorize]
        // DELETE api/<UsuariosController>/5
        [HttpDelete("{email}")]
        public async Task<bool> Delete(string email)
        {
            return await usuarioRepository.Delete(email);
        }

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string email, string pass)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
            {
                return NotFound();
            }

            var result = await emailSender.ConfirmEmailPass(email, pass);
            //var result = await mailService.ConfirmEmailPassAync(userId, pass);

            if (result.Ok)
            {
                return Redirect($"{configuration["AppUrl"]}/confirmEmail.html");
            }
            return BadRequest(result);
        }

        //validacion de mail por un validador (nicolas)
        [HttpGet("confirmarUsuxValidador")]
        public async Task<RedirectResult> ConfirmEmailxValidador(string email, string pass)
        {
            var result = await emailSender.ConfirmEmailPass(email, pass);

            if (result.Ok)
            {
                await emailSender.SendSingleEmailAsync(email, "Email confirmado", "<h1>Su usuario ha sido confirmado exitosamente</h1>" +
                    $"<p>Puede iniciar sesión</p>");
                return Redirect($"{configuration["AppUrl"]}/confirmarUsuario.html");
            }

            return null;

        }

        [HttpGet("denegarUsuxValidador")]
        public async Task<RedirectResult> DenegarUsuxValidador(string email)
        {
            await emailSender.SendSingleEmailAsync(email, "Email no confirmado", "<h1>Su usuario no ha sido confirmado</h1>");
            return Redirect($"{configuration["AppUrl"]}/denegarUsuario.html");
        }
    }
}
