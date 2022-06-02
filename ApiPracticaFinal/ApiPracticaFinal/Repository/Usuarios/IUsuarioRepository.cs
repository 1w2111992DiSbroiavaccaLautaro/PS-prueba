using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.UsuariosDTO;
using ApiPracticaFinal.Models.DTO.Validadores;
using ApiPracticaFinal.Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Repository.Usuarios
{
    public interface IUsuarioRepository
    {
        
        //string Authenticate(string email, string password);
        Task<List<UsuarioListaDTO>> GetUsuariosAsync();
        List<UsuarioListaDTO> GetListaUsuarios();
        List<UsuarioListaDTO> GetUsuariosMail();
        Task<ResultadosApi> UpdatePassword(UsuarioUpdatePass usu);
        Task<ResultadosApi> Signup(UsuarioSignUp oUser);
        Task<bool> Delete(string email);
        Task<List<UsuarioListaDTO>> GetUsuarioEmail(string email);
        //ResultadosApi Login(UsuarioLogin usu);
        Task<ResultadosApi> Login(UsuarioLogin login);
        Task<Usuario> UpdateRol(UsuarioRolDTO usu);
        List<ValidadorDTO> ListaValidadoresMail();
        Task<ResultadosApi> UpdateCredenciales(UsuarioCredencialDTO usu);

    }
}
