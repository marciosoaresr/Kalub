using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using iugu.net.Entity;
using iugu.net.Lib;
using iugu.net.Request;
using Newtonsoft.Json.Linq;
using ProfitManager.Fabrica.Entidade;
using ProfitManager.Negocio.Implementacao;
using ProfitManager.Web.RepositorioWeb;

namespace ProfitManager.Web.Controllers
{
    public class ContratarController : Controller
    {

        public ActionResult Index(string secureurl,string validado)
        {
            EmpresaUsuario empresaUsuario =
            FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
            new EmpresaUsuario();

            bool acessoMobile = VerificaMobile();
            ViewBag.Mobile = acessoMobile;

            var empresa = FuncoesEmpresa.Load((int)empresaUsuario.Empresa.Id);
            ViewBag.ListaCidade = ListCidade(empresa);
            ViewBag.ListaCategoriaEmpresa = ListCategoriaEmpresa(empresaUsuario.Empresa).OrderBy(x => x.Text);

            ViewBag.Empresa = empresaUsuario.Empresa.NomeFantasia;
            ViewBag.EmpresaId = empresaUsuario.Empresa.Id;
            ViewBag.EmpresaEmail = empresaUsuario.Empresa.Email;
            if (empresaUsuario.Empresa.Cep != null)
            {
                ViewBag.Cep = empresaUsuario.Empresa.Cep.Replace(".", "").Replace("-", "");
            }
            ViewBag.Endereco = empresaUsuario.Empresa.Logradouro;
            ViewBag.Bairro = empresaUsuario.Empresa.Bairro;
            if (empresaUsuario.Empresa.Cidade != null)
            {
                ViewBag.Cidade = empresaUsuario.Empresa.Cidade.Nome;
                ViewBag.Estado = empresaUsuario.Empresa.Cidade.Estado.Uf;
            }
            ViewBag.Telefone = empresaUsuario.Empresa.Telefone1.Substring(4).Replace(" ","");

            ViewBag.status = empresaUsuario.Empresa.Status;

            var msgPeriodoGratis = FuncoesEmpresa.MensagemPeriodoGratis(empresaUsuario.Empresa.Id);
            ViewBag.diasGratis = msgPeriodoGratis;
            ViewBag.msg = validado;
            ViewBag.url = secureurl;

            var dataLancamento = DateTime.Now;
            var listaSaldoInicialDiaAnterior =
            FuncoesEventoLancamento.PegaSaldoInicialContaContabil(empresaUsuario.Empresa.Id, 1, dataLancamento, true);
            ViewBag.TotalSaldoinicialDiaAnteiorContaCaixa = listaSaldoInicialDiaAnterior;

            return View("Index", empresa);
        }

        private static IEnumerable<SelectListItem> ListCategoriaEmpresa(Empresa empresa)
        {
            var listCategoriaEmpresa = FuncoesCategoriaEmpresa.Load();
            var listItemsCategoriaEmpresa = new List<SelectListItem>();

            foreach (var categoriaempresa in listCategoriaEmpresa)
            {
                var item = new SelectListItem
                {
                    Text = categoriaempresa.CampoNome.Nome,
                    Value = categoriaempresa.Id.ToString()
                };

                if (empresa.CategoriaEmpresa != null && categoriaempresa.Id == empresa.CategoriaEmpresa.Id)
                {
                    item.Selected = true;
                }

                listItemsCategoriaEmpresa.Add(item);
            }

            return listItemsCategoriaEmpresa;
        }


        private static List<SelectListItem> ListCidade(Empresa empresa)
        {
            var listCidade = FuncoesCidade.Load();
            var listItemsCidade = new List<SelectListItem>();

            foreach (var cidade in listCidade)
            {
                var item = new SelectListItem
                {
                    Text = cidade.NomeNormalizado,
                    Value = cidade.Id.ToString()
                };

                if (empresa.Cidade != null && cidade.Id == empresa.Cidade.Id)
                {
                    item.Selected = true;
                }

                listItemsCidade.Add(item);
            }

            return listItemsCidade;
        }


        [HttpPost]
        public ActionResult CadastrarEditarEmpresa(Empresa empresa, FormCollection form)
        {
            EmpresaUsuario empresaUsuario =
            FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
            new EmpresaUsuario();

            int idEmpresa = Convert.ToInt32(form["Id"]);
            string cnpj = form["Cnpj"];

            ModelState["Cidade.Id"].Errors.Clear();
            ModelState["CategoriaEmpresa.Codigo"].Errors.Clear();
            ModelState["CategoriaEmpresa.CampoNome"].Errors.Clear();
            ModelState["Telefone1"].Errors.Clear();
            ModelState["Telefone2"].Errors.Clear();
            ModelState["Email"].Errors.Clear();
            ModelState["Complemento"].Errors.Clear();

            string msg = "";
            bool existeCnpj = false;
            var cnpjExistente = FuncoesEmpresa.CnpjExistente(cnpj);

            if (cnpjExistente != null)
            {
                if (cnpjExistente.Id != idEmpresa)
                {
                    msg = "erro"; // cnpj existente em outra empresa
                    existeCnpj = true;
                }
                else
                {
                    cnpjExistente.Cnpj = cnpj;
                    cnpjExistente.DataAbertura = Convert.ToDateTime(form["DataAbertura"]);
                    cnpjExistente.Nome = form["Nome"];
                    cnpjExistente.NomeFantasia = form["NomeFantasia"];
                    cnpjExistente.CategoriaEmpresa.Id = Convert.ToInt32(form["CategoriaEmpresa.Id"]);
                    cnpjExistente.Logradouro = form["Logradouro"];
                    cnpjExistente.Numero = form["Numero"];
                    cnpjExistente.Complemento = form["Complemento"];
                    cnpjExistente.Cep = form["Cep"];
                    cnpjExistente.Bairro = form["Bairro"];
                    cnpjExistente.Cidade = empresa.Cidade;
                    cnpjExistente.Email = form["Email"];
                    cnpjExistente.Telefone1 = form["Telefone1"];
                    cnpjExistente.Telefone2 = form["Telefone2"];
                }
            }


            if (!existeCnpj)
            {
                if (ModelState.IsValid)
                {
                    if (cnpjExistente != null)
                    {
                        FuncoesEmpresa.Salvar(cnpjExistente);
                        empresaUsuario.Cnpj = cnpjExistente.Cnpj;
                        FuncoesEmpresaUsuario.Salvar(empresaUsuario);
                    }
                    else
                    {
                        FuncoesEmpresa.Salvar(empresa);
                        empresaUsuario.Cnpj = empresa.Cnpj;
                        FuncoesEmpresaUsuario.Salvar(empresaUsuario);
                    }
                    msg = "ok";
                    return RedirectToAction("GravaFatura", new { validado=msg });
                }
            }

            return RedirectToAction("Index", new { validado = msg });
        }

        public async Task<ActionResult> GravaFatura(string msg)
        {
            EmpresaUsuario empresaUsuario =
            FuncoesEmpresaUsuario.Load(RepositorioWebUsuario.VerificaSeOUsuarioEstaLogado(true).Id) ??
            new EmpresaUsuario();

            var invoice = await GerarFatura(empresaUsuario.Empresa.Nome, empresaUsuario.Empresa.Email, empresaUsuario.Empresa.Cnpj, empresaUsuario.Empresa.CupomPromocional);
            var assinatura = (JArray)invoice.recent_invoices;
            var empresaTransacaoIugu = new EmpresaTransacaoIugu();
            var secureUrl = "";
            foreach (var itens in assinatura)
            {
                var item = (JObject)itens;
                secureUrl = Convert.ToString(item["secure_url"]);
                var idFatura = item["id"];
                var idAssinatura = invoice.id;
                var dataVencimento = item["due_date"];
                var email = empresaUsuario.Empresa.Email;
                var status = item["status"];
                var updated = item["updated_at"];
                var secureId = item["secure_id"];
                var valor = Convert.ToString(item["total"]);
                var total = valor.Replace("R$ ", "");

                empresaTransacaoIugu.IdAssinatura = Convert.ToString(idAssinatura);
                empresaTransacaoIugu.IdFatura = Convert.ToString(idFatura);
                empresaTransacaoIugu.DataVencimento = Convert.ToDateTime(dataVencimento);
                empresaTransacaoIugu.Email = Convert.ToString(email);
                empresaTransacaoIugu.Status = Convert.ToString(status);
                empresaTransacaoIugu.Valor = Convert.ToDecimal(total);
                empresaTransacaoIugu.UpdatedAt = updated != null ? Convert.ToDateTime(updated) : DateTime.Now;
                empresaTransacaoIugu.SecureId = Convert.ToString(secureId);
                empresaTransacaoIugu.SecureUrl = Convert.ToString(secureUrl);
                empresaTransacaoIugu.Empresa = empresaUsuario.Empresa;
                FuncoesEmpresaTransacaoIugu.Salvar(empresaTransacaoIugu);
            }
            var empresaAssinaturaIugu = new EmpresaAssinaturaIugu
            {
                IdAssinatura = invoice.id,
                Empresa = empresaUsuario.Empresa
            };
            FuncoesEmpresaAssinaturaIugu.Salvar(empresaAssinaturaIugu);

            var empresa = FuncoesEmpresaUsuario.BuscarEmpresa(empresaUsuario.Empresa.Id);
            empresa.IdAssinatura = invoice.id;
            FuncoesEmpresa.Salvar(empresa);

            return RedirectToAction("Index", new { secureurl= secureUrl, validado = msg });
        }

        public async Task<SubscriptionModel> GerarFatura(string cliente, string email, string cnpj, int cupom)
        {
            var customer = new CustomerRequestMessage
            {
                Email = email,
                Name = cliente,
                CpfOrCnpj = cnpj
            };

            using (var apiCustomer = new Customer())
            using (var apiSubscription = new Subscription())
            {
                var customerResponse = await apiCustomer.CreateAsync(customer, null).ConfigureAwait(false);

                // Se existir cupom promocional gravado para a empresa concede desconto de 10,00
                int valor = 3990;
                if (cupom == 8726001)
                {
                    valor = 2990;
                }

                var subscriptionItems = new List<SubscriptionSubitem> { new SubscriptionSubitem { description = "Assinatura KALUB", price_cents = valor, quantity = 1, recurrent = true } };

                var subscription = await apiSubscription.CreateAsync(new SubscriptionRequestMessage(customerResponse.id)
                {
                    PlanId = "kalub_plano_assinatura",
                    IsCreditBased = false,
                    Subitems = subscriptionItems
                }).ConfigureAwait(false);

                return subscription;
            };
        }

        public bool VerificaMobile()
        {
            if (this.Request.Browser.IsMobileDevice || Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null ||
              (Request.ServerVariables["HTTP_ACCEPT"] != null && Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap")))
            {
                return true;
            }
            else if (Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                string[] mobiles = new[]
                {
                    "midp", "j2me", "avant", "docomo",
                    "novarra", "palmos", "palmsource",
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/",
                    "blackberry", "mib/", "symbian",
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio",
                    "SIE-", "SEC-", "samsung", "HTC",
                    "mot-", "mitsu", "sagem", "sony"
                    , "alcatel", "lg", "eric", "vx",
                    "NEC", "philips", "mmm", "xx",
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java",
                    "pt", "pg", "vox", "amoi",
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo",
                    "sgh", "gradi", "jb", "dddi",
                    "moto", "iphone", "ipad", "Macintosh", "windows phone"
                };

                foreach (string s in mobiles)
                {
                    if (Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower()))
                    {
                        return true;
                    }
                }

            }

            return false;
        }
    }
}
