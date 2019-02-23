﻿using Controle.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace Controle.Controllers
{
    
    public class HomeController : Controller
    {
        [Authorize(Roles = "admin, user")]
        public ActionResult Index()
        {

            if (Session["Nome"] != null)
            {
                return View();
            }
            else
            {
                
                return RedirectToAction("ErrorLogin");
            }
        }

        public ActionResult ErrorLogin()
        {
            ViewBag.Message = "Você precisa estar logado para executar esta ação";

            return View();
        }



        /// <param name="returnURL"></param>
        /// <returns></returns>
        public ActionResult Login(string returnURL)
        {

            if (Session["Nome"] == null)
            {
                /*Recebe a url que o usuário tentou acessar*/
                ViewBag.ReturnUrl = returnURL;
                return View(new Usuario());
            }
            else
            {
                return RedirectToAction("ErrorLogin", "Home");
            }
            
            
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Usuario login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                using (ControleEntities db = new ControleEntities())
                {
                    var vLogin = db.Usuario.Where(p => p.Email.Equals(login.Email)).FirstOrDefault();
                    /*Verificar se a variavel vLogin está vazia. 
                    Isso pode ocorrer caso o usuário não existe. 
              Caso não exista ele vai cair na condição else.*/
                    if (vLogin != null)
                    {
                        /*Código abaixo verifica se o usuário que retornou na variavel tem está 
                          ativo. Caso não esteja cai direto no else*/
                        if (Equals(vLogin.Bl_Ativo, "s"))
                        {
                            /*Código abaixo verifica se a senha digitada no site é igual a 
                            senha que está sendo retornada 
                             do banco. Caso não cai direto no else*/
                            if (Equals(vLogin.Senha, login.Senha))
                            {
                                FormsAuthentication.SetAuthCookie(vLogin.Email, false);
                                if (Url.IsLocalUrl(returnUrl)
                                && returnUrl.Length > 1
                                && returnUrl.StartsWith("/")
                                && !returnUrl.StartsWith("//")
                                && returnUrl.StartsWith("/\\"))
                                {
                                    return Redirect(returnUrl);
                                }
                                /*código abaixo cria uma session para armazenar o nome do usuário*/
                                Session["Nome"] = vLogin.Nome;
                                /*código abaixo cria uma session para armazenar o sobrenome do usuário*/
                                Session["Perfil"] = vLogin.Perfil;
                                Session["Email"] = vLogin.Email;
                                Session["Registro"] = vLogin.RegData;
                                Session["Id"] = vLogin.Id;
                                /*retorna para a tela inicial do Home*/
                                return RedirectToAction("Index", "Home");
                            }
                            /*Else responsável da validação da senha*/
                            else
                            {
                                /*Escreve na tela a mensagem de erro informada*/
                                ModelState.AddModelError("", "Senha informada Inválida!!!");
                                /*Retorna a tela de login*/
                                return View(new Usuario());
                            }
                        }
                        /*Else responsável por verificar se o usuário está ativo*/
                        else
                        {
                            /*Escreve na tela a mensagem de erro informada*/
                            ModelState.AddModelError("", "Usuário sem acesso para usar o sistema!!!");
                            /*Retorna a tela de login*/
                            return View(new Usuario());
                        }
                    }
                    /*Else responsável por verificar se o usuário existe*/
                    else
                    {
                        /*Escreve na tela a mensagem de erro informada*/
                        ModelState.AddModelError("", "E-mail informado inválido!!!");
                        /*Retorna a tela de login*/
                        return View(new Usuario());
                    }
                }
            }
            /*Caso os campos não esteja de acordo com a solicitação retorna a tela de login 
            com as mensagem dos campos*/
            return View(login);
        }

        
        public ActionResult LogOut()
        {
            
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Login", "Home");
        }


    }
}