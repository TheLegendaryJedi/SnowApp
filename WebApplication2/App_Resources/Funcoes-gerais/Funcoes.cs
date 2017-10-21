using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
// Para o email
using System.Net;
using System.Net.Mail;
using System.IO;
using NManif.App_Logic.Helpers.Active_Directory;
using System.Configuration;
using NManif.Helpers;
using NManif.Web.App_Resources.Acesso_DB;

namespace NManif.Web.App_Resources.Funcoes_gerais
{

    // Estruturas usada para envio de email //
    public struct Destinatario
    {
        public string _nim, _mail;

        public Destinatario(string nim, string mail)
        {
            _nim = nim;
            _mail = mail;
        }
    }

    // Estruturas Usadas para estatisticas - Pedidos //
    public struct PedTotal // Total de Pedidos / Ano
    {
        public string _Ano, _Total;

        public PedTotal(string Ano, string Total)
        {
            _Ano = Ano;
            _Total = Total;
        }
    }

    public struct PedTotalMes // Total de Pedidos / Mes
    {
        public string _Ano, _Mes, _Total;

        public PedTotalMes(string Ano, string Mes, string Total)
        {
            _Ano = Ano;
            _Mes = Mes;
            _Total = Total;
        }
    }

    public struct PedTotalDim // Total de Pedidos / Ano e outra dimensão
    {
        public string _Ano, _Dim, _Total;

        public PedTotalDim(string Ano, string Dim, string Total)
        {
            _Ano = Ano;
            _Dim = Dim;
            _Total = Total;
        }
    }

    public struct PedTotalDimMes // Total de Pedidos / Mes e outra dimensão
    {
        public string _Ano, _Mes, _Dim, _Total;

        public PedTotalDimMes(string Ano, string Mes, string Dim, string Total)
        {
            _Ano = Ano;
            _Mes = Mes;
            _Dim = Dim;
            _Total = Total;
        }
    }

    // Estruturas Usadas para estatisticas - Recursos //
    public struct PedTotalRecurso // Total de Recursos / Ano
    {
        public string _Ano, _Recurso, _Total;

        public PedTotalRecurso(string Ano, string Recurso, string Total)
        {
            _Ano = Ano;
            _Recurso = Recurso;
            _Total = Total;
        }
    }

    public struct PedTotalRecursoMes // Total de Recursos / Mes
    {
        public string _Ano, _Mes, _Recurso, _Total;

        public PedTotalRecursoMes(string Ano, string Mes, string Recurso, string Total)
        {
            _Ano = Ano;
            _Mes = Mes;
            _Recurso = Recurso;
            _Total = Total;
        }
    }

    public struct PedTotalRecursoDim // Total de Recursos / Ano e outra dimensão
    {
        public string _Ano, _TipoRecurso, _Recurso, _Dim, _Total;

        public PedTotalRecursoDim(string Ano, string TipoRecurso, string Recurso, string Dim, string Total)
        {
            _Ano = Ano;
            _TipoRecurso = TipoRecurso;
            _Recurso = Recurso;
            _Dim = Dim;
            _Total = Total;
        }
    }

    public struct PedTotalRecursoMesDim // Total de Recursos / Ano, mes e outra dimensão
    {
        public string _Ano, _Mes, _TipoRecurso, _Recurso, _Dim, _Total;

        public PedTotalRecursoMesDim(string Ano, string Mes, string TipoRecurso, string Recurso, string Dim, string Total)
        {
            _Ano = Ano;
            _Mes = Mes;
            _TipoRecurso = TipoRecurso;
            _Recurso = Recurso;
            _Dim = Dim;
            _Total = Total;
        }
    }

    // Outras estruturas usadas
    public struct LogUser // Utilizador logado
    {
        public string _perfilID, _estabelecimentoID, _OCAD, _GU, _UEO;

        public LogUser(string PerfilID, string EstabelecimentoID, string OCAD, string GU, string UEO)
        {
            _perfilID = PerfilID;
            _estabelecimentoID = EstabelecimentoID;
            _OCAD = OCAD;
            _GU = GU;
            _UEO = UEO;
        }
    }


    public class Funcoes
    {

        // Get TempoID a partir da data actual
        public static string getDTempoID(DateTime data)
        {
            string DTempoID;
            string Ano, Mes;
            string Semestre = "";
            string Trimestre = "";
            string Dia = "00";

            // o DTempoID é constituido por Ano(4), Semestre(1), Trimestre(1), mes(2), dia(2 -> dois 00)

            Ano = data.Year.ToString();
            if (data.Month >= 10)
            {
                Mes = data.Month.ToString();
            }
            else
            {
                Mes = "0" + data.Month.ToString();
            }

            if (data.Month <= 3)
            {
                Semestre = "1";
                Trimestre = "1";
            }
            if (data.Month > 3 && data.Month <= 6)
            {
                Semestre = "1";
                Trimestre = "2";
            }
            if (data.Month > 6 && data.Month <= 9)
            {
                Semestre = "2";
                Trimestre = "3";
            }
            if (data.Month > 9)
            {
                Semestre = "2";
                Trimestre = "4";
            }

            DTempoID = Ano + Semestre + Trimestre + Mes + Dia;
            return DTempoID;
        }

        // Get dados do user logado
        public static LogUser getLogUser(string nomeNim)
        {
            /* dados do utilizador logado  */
            string nimUser;
            string estabelecimentoId;
            string perfil, perfilID;
            //string nomeNim = HttpContext.Current.User.Identity.Name.ToString();
            string[] aux = nomeNim.Split('|');
            nimUser = aux[1];
            string[] utilizador = DB.getUserByNim(aux[1]);
            estabelecimentoId = utilizador[0] + "-" + utilizador[1] + "-" + utilizador[2];
            perfil = utilizador[3];
            //martelada para converter o perfil em perfil ID e passar o perfilID no pedido
            if (perfil == "Administrador")
            {
                perfilID = "ADMIN";
            }
            else
            {
                if (perfil == "Exercito")
                {
                    perfilID = "EXE";
                }
                else
                {
                    perfilID = perfil;
                }
            }

            return new LogUser(perfilID, estabelecimentoId, utilizador[0], utilizador[1], utilizador[2]);
        }

        // Get GDH a partir da data e hora inseridas (DDhhmmMMMAA - ex:010700NOV11)

        public static string getGDH(DateTime data, string horaMinuto)
        {
            string GDH;
            string Ano, Mes, Dia;

            // o GDH é constituido por Dia (2), Hora(2), Minuto (2), Mes (3 - Letras iniciais), Ano(2)

            Ano = data.Year.ToString();
            Mes = "";
            Dia = "";
            switch (data.Month.ToString())
            {
                case "1":
                    Mes = "JAN";
                    break;
                case "2":
                    Mes = "FEV";
                    break;
                case "3":
                    Mes = "MAR";
                    break;
                case "4":
                    Mes = "ABR";
                    break;
                case "5":
                    Mes = "MAI";
                    break;
                case "6":
                    Mes = "JUN";
                    break;
                case "7":
                    Mes = "JUL";
                    break;
                case "8":
                    Mes = "AGO";
                    break;
                case "9":
                    Mes = "SET";
                    break;
                case "10":
                    Mes = "OUT";
                    break;
                case "11":
                    Mes = "NOV";
                    break;
                case "12":
                    Mes = "DEC";
                    break;
                default:
                    break;
            }
            if (data.Day >= 10)
            {
                Dia = data.Day.ToString();
            }
            else
            {
                Dia = "0" + data.Day.ToString();
            }

            GDH = Dia + horaMinuto + Ano + Mes + Dia;
            return GDH;
        }


        // - - - -   Enviar os emails ------ //

        public static string getMailByNim(string nim)
        {
            return Ligacao.GetProperty("mail", nim);
        }

        // # Inserção de novo pedido # Nota: aqui já não estou a usar o perfil. esse é usado no DB.getAllDestinatarios
        public static void enviarEmail(ArrayList destinatario, string dataPretendida, string resumo, string nimEmissor, string ueoEmissor)
        {
            MailMessage mMailMessage = new MailMessage();


            // address of sender
            mMailMessage.From = new MailAddress("no_reply@mail.exercito.pt");

            // recipient address (s)
            foreach (Destinatario aux_dest in destinatario)
            {
                mMailMessage.To.Add(new MailAddress(aux_dest._mail));
            }
            mMailMessage.CC.Add(new MailAddress(getMailByNim(nimEmissor)));

            // Set the subject of the mail message - ASSUNTO
            mMailMessage.Subject = "--- Aviso de novo Pedido de Apoio (NManif) ---";

            // Set the body of the mail message
            string connectionInfo = ConfigurationManager.AppSettings["HTMLTemplate"];
            string caminhoHTML = HttpContext.Current.Server.MapPath("../../") + connectionInfo;
            StreamReader reader = File.OpenText(caminhoHTML);
            mMailMessage.Body = reader.ReadToEnd();
            reader.Close();

            // Alterar itens de Template no Body do email
            mMailMessage.Body = mMailMessage.Body.Replace("{DATAPRETENDIDA}", dataPretendida);
            mMailMessage.Body = mMailMessage.Body.Replace("{UEO}", ueoEmissor);
            mMailMessage.Body = mMailMessage.Body.Replace("{RESUMO}", resumo);


            /*
             * Colocar imagem embebida
             * 
             * */

            //Add Image
            string caminhoIMAGEM = HttpContext.Current.Server.MapPath("../../") + ConfigurationManager.AppSettings["HTMLImage"];
            LinkedResource theEmailImage = new LinkedResource(caminhoIMAGEM);
            //LinkedResource theEmailImage = new LinkedResource("D:\\atraso\\Reciclagem.png");
            theEmailImage.ContentId = "myImageID"; // este nome está no template html no src da imagem.

            //create Alternative HTML view
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mMailMessage.Body, null, "text/html");

            //Add the Image to the Alternate view
            htmlView.LinkedResources.Add(theEmailImage);

            //Add view to the Email Message
            mMailMessage.AlternateViews.Add(htmlView);


            // Set the format of the mail message body as HTML
            mMailMessage.IsBodyHtml = true;
            // Set the priority of the mail message to normal
            mMailMessage.Priority = MailPriority.Normal;

            // Instantiate a new instance of SmtpClient indicando o servidor smpt
            //SmtpClient mSmtpClient = new SmtpClient("10.105.0.221"); // este é o correcto (não tem credenciais) 10.105.0.221
            SmtpClient mSmtpClient = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);

            mSmtpClient.EnableSsl = false; // isto para o gmail
            // Criando as credenciais
            //NetworkCredential credenciais = new NetworkCredential("username", "password", "");
            // Associando as credenciais ao cliente smtp
            //mSmtpClient.Credentials = credenciais;

            // Send the mail message
            try
            {
                mSmtpClient.Send(mMailMessage);
                /* lblMessage.Text = MessageFormatter.GetFormattedSuccessMessage(
                "A sua mensagem foi enviada com sucesso");*/
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                //log error - Escrever para ficheiro de log
                /* lblMessage.Text = MessageFormatter.GetFormattedErrorMessage(
                "A sua mensagem não foi enviada!!! " + ex.InnerException + "");*/
            }
            finally
            {
                mMailMessage.Dispose();
            }
        }

        // ------ Manipulação de ArrayList --------//
        public static ArrayList getUniqueDataFromArrayListRecursos(ArrayList recursos, string dataItem, string categoria_grafico_pai = "")
        {
            ArrayList aux = new ArrayList();
            string auxDim = "";
            string auxRecurso = "";
            string auxMes = "";
            if (dataItem == "Dim")
            {
                foreach (PedTotalRecursoDim auxRecursos in recursos)
                {
                    if (auxRecursos._Dim != auxDim && !(aux.Contains(auxRecursos._Dim)))
                    {
                        aux.Add(auxRecursos._Dim);
                        //auxDim = auxRecursos._Dim;
                    }
                    auxDim = auxRecursos._Dim;
                }
            }
            if (dataItem == "Recurso")
            {
                foreach (PedTotalRecursoDim auxRecursos in recursos)
                {
                    if (auxRecursos._Recurso != auxRecurso && !(aux.Contains(auxRecursos._Recurso)))
                    {
                        aux.Add(auxRecursos._Recurso);
                        //auxDim = auxRecursos._Dim;
                    }
                    auxRecurso = auxRecursos._Recurso;
                }
            }
            if (dataItem == "Mes")
            {
                foreach (PedTotalRecursoMesDim auxRecursos in recursos)
                {
                    if (auxRecursos._Mes != auxMes && !(aux.Contains(auxRecursos._Mes)) && auxRecursos._Dim == categoria_grafico_pai)
                    {
                        aux.Add(auxRecursos._Mes);
                        //auxDim = auxRecursos._Dim;
                    }
                    if (auxRecursos._Dim == categoria_grafico_pai)
                    {
                        auxMes = auxRecursos._Mes;
                    }
                }
            }
            if (dataItem == "RecursoMes")
            {
                foreach (PedTotalRecursoMesDim auxRecursos in recursos)
                {
                    if (auxRecursos._Recurso != auxRecurso && !(aux.Contains(auxRecursos._Recurso)) && auxRecursos._Dim == categoria_grafico_pai)
                    {
                        aux.Add(auxRecursos._Recurso);
                        //auxDim = auxRecursos._Dim;
                    }
                    if (auxRecursos._Dim == categoria_grafico_pai)
                    {
                        auxRecurso = auxRecursos._Recurso;
                    }
                }
            }
            return aux;
        }


    }
}