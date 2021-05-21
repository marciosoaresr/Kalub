using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Hql.Ast.ANTLR.Tree;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Fabrica.Enum;
using ProfitManager.Negocio.Repositorio;

namespace ProfitManager.Negocio.Implementacao
{
    public static class FuncoesUsuario
    {
        public static Usuario AutenticarAdm(string usuario, string senha)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("login", usuario));
                    parameters.Add(new HqlParameter("senha", senha));

                    string hql = "FROM Usuario u WHERE (u.Login = :login or u.Email = :login) AND u.Senha = :senha ";

                    var objUsuario = repositorio.Buscar<Usuario>(hql, parameters).FirstOrDefault();

                    if (objUsuario != null)
                    {
                        objUsuario.Administrador = EnumSimNao.Sim;
                    }

                    return objUsuario;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static Usuario Autenticar(string usuario, string senha)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("login", usuario));
                    parameters.Add(new HqlParameter("senha", senha));

                    Usuario objUsuario = null;

                    string hql =
                        "FROM EmpresaUsuario eu WHERE (eu.Login = :login or eu.Email = :login) AND eu.Senha = :senha ";
                    var empresaUsuario = repositorio.Buscar<EmpresaUsuario>(hql, parameters).FirstOrDefault();

                    if (empresaUsuario != null)
                    {
                        objUsuario = new Usuario
                        {
                            Id = empresaUsuario.Id,
                            Login = empresaUsuario.Login,
                            Administrador = EnumSimNao.Nao,
                            Empresa = empresaUsuario.Empresa
                        };
                    }

                    return objUsuario;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static Usuario AutenticarCnpj(string cnpj)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("cnpj", cnpj));

                    Usuario objUsuario = null;

                    string hql =
                        "FROM EmpresaUsuario eu WHERE eu.Cnpj = :cnpj ";
                    var empresaUsuario = repositorio.Buscar<EmpresaUsuario>(hql, parameters).FirstOrDefault();

                    if (empresaUsuario != null)
                    {
                        objUsuario = new Usuario
                        {
                            Id = empresaUsuario.Id,
                            Login = empresaUsuario.Login,
                            Administrador = EnumSimNao.Nao
                        };
                    }

                    return objUsuario;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        public static Usuario BuscarPorId(int idUsuario, EnumSimNao administrador)
        {
            using (var repositorio = SessionFactory.CreateRepositorio())
            {
                try
                {
                    var parameters = new List<HqlParameter>();
                    parameters.Add(new HqlParameter("idUsuario", idUsuario));

                    Usuario objUsuario = null;

                    if (administrador == EnumSimNao.Sim)
                    {
                        objUsuario = repositorio.Buscar<Usuario>("From Usuario u Where u.Id = :idUsuario", parameters)
                            .SingleOrDefault();

                        if (objUsuario != null)
                        {
                            objUsuario.Administrador = EnumSimNao.Sim;
                        }
                    }
                    else
                    {
                        var empresaUsuario =
                            repositorio.Buscar<EmpresaUsuario>("From EmpresaUsuario eu Where eu.Id = :idUsuario",
                                parameters)
                                .SingleOrDefault();

                        if (empresaUsuario != null)
                        {
                            objUsuario = new Usuario
                            {
                                Id = empresaUsuario.Id,
                                Administrador = EnumSimNao.Nao
                            };
                        }

                    }

                    return objUsuario;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }


    }
}
