using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.UsuariosDTO;
using ApiPracticaFinal.Models.DTO.Validadores;
using ApiPracticaFinal.Repository.SendGrid;
using ApiPracticaFinal.Resultados;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Repository.Usuarios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        
        private readonly dd4snj9pkf64vpContext context;
        private readonly IConfiguration _configuration;

        //public UsuarioRepository(string key, dd4snj9pkf64vpContext context)
        //{
        //    this.key = key;
        //    this.context = context;
        //}

        public UsuarioRepository(IConfiguration configuration, dd4snj9pkf64vpContext context)
        {
            _configuration = configuration;
            this.context = context;
        }

        //public string Authenticate(string email, string password)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenKey = Encoding.ASCII.GetBytes(key);
        //    //var tokenKey = Encoding.UTF8.GetBytes(key);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //            {
        //                new Claim(ClaimTypes.Name, email)
        //            }),
        //        Expires = DateTime.UtcNow.AddHours(2),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}

        //metodo accesible solo para usuarios con rol 1
        public async Task<List<UsuarioListaDTO>> GetUsuariosAsync()
        {
            var rolesBD = await context.Roles.ToListAsync();
            var usuarios = await context.Usuarios.Where(x => x.Activo == true).ToListAsync();
            var listaDTO = new List<UsuarioListaDTO>();

            foreach (var i in usuarios)
            {
                var rolxusuario = rolesBD.FirstOrDefault(x => x.Idrol == i.Rol);

                var usuarioDTO = new UsuarioListaDTO
                {
                    Id = i.Idusuario,
                    Email = i.Email,
                    Nombre = i.Nombre,
                    Rol = rolxusuario.Rol
                };
                listaDTO.Add(usuarioDTO);
            }

            return listaDTO;
        }

        public async Task<ResultadosApi> Signup(UsuarioSignUp oUser)
        {
            oUser.Password = BCrypt.Net.BCrypt.HashPassword(oUser.Password);

            var r = new ResultadosApi();

            Usuario u = new Usuario
            {            
                Nombre = oUser.Nombre,
                Email = oUser.Email,
                Password = oUser.Password,
                Rol = 2,
                //cambiar a falso cuando se implemente la funcioalidad del mail
                Activo = false
            };

            if (u != null)
            {
                var repetido = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == oUser.Email);
                if (repetido == null)
                {
                    r.Ok = true;
                    r.Respuesta = u;
                    context.Usuarios.Add(u);
                    await context.SaveChangesAsync();
                    
                    return r;
                }
                else
                {
                    r.Ok = false;
                    r.Error = "Usuario ya registrado con ese mail";
                    r.Respuesta = null;
                    return r;
                }                
            }
            else
            {
                throw new Exception("No se pudo registrar el usuario");
            }

        }


        private string CrearToken(Usuario u)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, u.Idusuario.ToString()),
                new Claim(ClaimTypes.Name, u.Nombre)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Settings:key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<ResultadosApi> Login(UsuarioLogin login)
        {
            var r = new ResultadosApi();
            var contra = BCrypt.Net.BCrypt.HashPassword(login.Password);
            var resultado = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == login.Email && x.Activo == true);

            if (resultado != null)
            {
                var rol = await context.Roles.SingleOrDefaultAsync(x => x.Idrol == resultado.Rol);
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(login.Password, resultado.Password);
                if (isValidPassword)
                {
                    var usuarioSesion = new UsuarioSesionDto
                    {
                        IdUsuario = resultado.Idusuario,
                        Email = resultado.Email,
                        Password = resultado.Password,
                        Token = CrearToken(resultado),
                        Rol = rol.Idrol
                    };
                    r.Ok = true;
                    r.Respuesta = usuarioSesion;
                    return r;
                }
                else
                {
                    r.Ok = false;
                    r.Error = "Usuario y/o contraseña incorrectos";
                    r.Respuesta = null;
                    return r;
                }

            }
            else
            {
                r.Ok = false;
                r.Error = "El Usuario Ingresado no se encuentra Registrado";
                r.Respuesta = null;
                return r;
            }
        }


        //public ResultadosApi Login(UsuarioLogin usu)
        //{
        //    var token = Authenticate(usu.Email, usu.Password);

        //    var result = context.Usuarios.FirstOrDefault(x => x.Email == usu.Email && x.Password == usu.Password);

        //    ResultadosApi resultado = new ResultadosApi();
            
        //    var u = context.Usuarios.SingleOrDefault(x => x.Email == usu.Email && x.Activo == true);
        //    if (u != null)
        //    {
        //        bool isValidPassword = BCrypt.Net.BCrypt.Verify(usu.Password, u.Password);

        //        if (isValidPassword)
        //        {
        //            if (token == null)
        //            {
        //                resultado.Error = "No autorizado";
        //                resultado.Ok = false;
        //                return resultado;
        //            }
        //            else
        //            {
        //                resultado.Ok = true;
        //                resultado.InfoAdicional = "Login exitoso";
        //                Login lg = new Login(u.Idusuario, u.Email, token, u.Rol);
        //                resultado.Respuesta = lg;
        //                return resultado;
        //            }
        //        }
        //        else
        //        {
        //            resultado.Error = "Usuario y/o contraseña incorrectos";
        //            return resultado;
        //        }
        //    }
        //    else
        //    {
        //        resultado.Error = "Usuario y/o contraseña incorrectos";
        //        return resultado;
        //    }   
        //}

        //metodo accesible para todos los usuarios
        public async Task<ResultadosApi> UpdatePassword(UsuarioUpdatePass usu)
        {
            var r = new ResultadosApi();
            UsuarioUpdatePass u = new UsuarioUpdatePass
            {
                Email = usu.Email,
                PasswordNueva = usu.PasswordNueva,
                PasswordVieja = usu.PasswordVieja
            };

            var usuario = context.Usuarios.SingleOrDefault(x => x.Email == usu.Email);
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(usu.PasswordVieja, usuario.Password);
            if (isValidPassword)
            {
                usuario.Password = BCrypt.Net.BCrypt.HashPassword(usu.PasswordNueva);
                context.Usuarios.Update(usuario);
                await context.SaveChangesAsync();
                r.Ok = true;
                r.Respuesta = usuario;

                return r;
            }
            else
            {
                r.Ok = false;
                r.Error = "Las contraseñas son incorrectas";
                r.Respuesta = null;
                return r;
            }
        }

        //metodo accesible solo para usuarios con rol 1
        public async Task<bool> Delete(string email)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
            if(usuario != null)
            {
                usuario.Activo = false;
                context.Usuarios.Update(usuario);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new Exception("No se encontro el usuario");
            }
        }

        //metodo accesible solo para usuarios con rol 1
        public async Task<Usuario> UpdateRol(UsuarioRolDTO usu)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Idusuario == usu.Id);
            if (usuario != null)
            {
                usuario.Rol = usu.Rol;
                context.Usuarios.Update(usuario);
                await context.SaveChangesAsync();
                return usuario;
            }
            else
            {
                throw new Exception("Usuario no encontrado");
            }
        }

        //validar desde el front que se envie un mail correcto
        //metodo accesible para todos los usuarios
        public async Task<ResultadosApi> UpdateCredenciales(UsuarioCredencialDTO usu)
        {
            var resultados = new ResultadosApi();

            var repetido = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == usu.Email);

            if (repetido == null)
            {
                var usuario = await context.Usuarios.FirstOrDefaultAsync(x => x.Idusuario == usu.Id);
                if (usuario != null)
                {
                    if (usu.Email == "" || usu.Email == "string")
                    {
                        usuario.Email = usuario.Email;
                    }
                    else
                    {
                        usuario.Email = usu.Email ?? usuario.Email;
                    }
                    if (usu.Nombre == "" || usu.Nombre == "string")
                    {
                        usuario.Nombre = usuario.Nombre;
                    }
                    else
                    {
                        usuario.Nombre = usu.Nombre ?? usuario.Nombre;
                    }
                    context.Usuarios.Update(usuario);
                    await context.SaveChangesAsync();

                    resultados.Ok = true;
                    resultados.Respuesta = usuario;
                    resultados.InfoAdicional = "Usuario modificado correctamente";
                    return resultados;
                }
                else
                {
                    resultados.Ok = false;
                    resultados.Error = "No se modifico el Usuario correctamente";
                    return resultados;
                }
            }
            else
            {
                resultados.Ok = false;
                resultados.Error = "Error al registar";
                return resultados;
            }
            
        }

        public async Task<List<UsuarioListaDTO>> GetUsuarioEmail(string email)
        {
            var rolesBD = await context.Roles.ToListAsync();
            var usuario = await context.Usuarios.Where(x => x.Activo == true && x.Email == email).ToListAsync();
            var listaDto = new List<UsuarioListaDTO>();

            foreach (var i in usuario)
            {
                var rolxusuario = rolesBD.FirstOrDefault(x => x.Idrol == i.Rol);

                var usuarioDTO = new UsuarioListaDTO
                {
                    Id = i.Idusuario,
                    Email = i.Email,
                    Nombre = i.Nombre,
                    IdRol = rolxusuario.Idrol,
                    Rol = rolxusuario.Rol
                };
                listaDto.Add(usuarioDTO);
            }
            
            return listaDto;
        }

        public List<UsuarioListaDTO> GetUsuariosMail()
        {
            var rolesBD = context.Roles.ToList();
            var usuarios = context.Usuarios.Where(x => x.Activo == true).ToList();
            var listaDTO = new List<UsuarioListaDTO>();

            foreach (var i in usuarios)
            {
                var rolxusuario = rolesBD.FirstOrDefault(x => x.Idrol == i.Rol);

                var usuarioDTO = new UsuarioListaDTO
                {
                    Id = i.Idusuario,
                    Email = i.Email,
                    Nombre = i.Nombre,
                    Rol = rolxusuario.Rol
                };
                listaDTO.Add(usuarioDTO);
            }

            return listaDTO;
        }

        public List<UsuarioListaDTO> GetListaUsuarios()
        {
            var rolesBD = context.Roles.ToList();
            var usuarios = context.Usuarios.Where(x => x.Activo == true).ToList();
            List<UsuarioListaDTO> listaDTO = new List<UsuarioListaDTO>();

            foreach (var i in usuarios)
            {
                var rolxusuario = rolesBD.FirstOrDefault(x => x.Idrol == i.Rol);

                var usuarioDTO = new UsuarioListaDTO
                {
                    Id = i.Idusuario,
                    Email = i.Email,
                    Nombre = i.Nombre,
                    Rol = rolxusuario.Rol
                };
                listaDTO.Add(usuarioDTO);
            }

            return listaDTO;
        }

        public List<ValidadorDTO> ListaValidadoresMail()
        {
            var validadores = context.Validadores.Where(x => x.Activo == true).ToList();
            var lista = new List<ValidadorDTO>();
            foreach (var i in validadores)
            {
                var validadorDto = new ValidadorDTO
                {
                    Id = i.Id,
                    Nombre = i.Nombre,
                    Email = i.Sector
                };
                lista.Add(validadorDto);
            }
            return lista;
        }
    }
}
