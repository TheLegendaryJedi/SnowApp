using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using NManif.Web.App_Resources.Funcoes_gerais;
using NManif.Helpers;

namespace NManif.Web.App_Resources.Acesso_DB
{
    public class DB
    {
        /* método de exemplo com sqldatareader (quey em string) */
        public static bool XPTO1(string classeEstacaoID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoID from Pedido p " +
                    "where p.ClEstID = '" + classeEstacaoID + "'";
                System.Data.SqlClient.SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                System.Data.SqlClient.SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }


        /* método de exemplo com chamada a store procedure */
        public static ArrayList XPTO2(string DEstabelecimentoID, string perfil)
        {
            ArrayList Destinatarios = new ArrayList();

            string[] OCAD_GU_UEO = DEstabelecimentoID.Split('-');
            SqlConnection mConn = new SqlConnection(
                         ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);

            /* SqlConnection conn = new SqlConnection(
                "server=rt-dboracle;Database=SGAE;User ID=SGAE;Password=pataniscas;Trusted_Connection=False;"); */
            try
            {
                //Criando o SqlCommand
                SqlCommand cmd = new SqlCommand();
                //Associando a conexão para o SqlCommand
                cmd.Connection = mConn;
                //Nome da stored procedure
                cmd.CommandText = "sp_GetDestinatarios";
                //Definindo o tipo de comando como StoredProcedure
                cmd.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros (OCAD, GU e Perfil do emissor)
                cmd.Parameters.AddWithValue("@OCAD", OCAD_GU_UEO[0]);
                cmd.Parameters.AddWithValue("@GU", OCAD_GU_UEO[1]);
                cmd.Parameters.AddWithValue("@PerfilID", perfil);
                /*cmd.Parameters.Add("@nomeparametro", SqlDbType.VarChar);
                cmd.Parameters["@nomeparametro"] = "valor do parametro";*/

                //Abrir a conexão
                mConn.Open();
                //Executar a stored procedure
                int intReturn = cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();

                //Verifica se tem registos no select.
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Destinatario aux = new Destinatario(
                            dr["Nim"].ToString(), dr["Email_Func"].ToString());
                        Destinatarios.Add(aux);
                    }
                }
                else
                {
                    // Não Há atrasos - Escrever no log
                    Console.WriteLine("Não obtive qualquer registo \n");
                }

            }
            catch (Exception ex)
            {
                // erro - Escrever no log
                Console.WriteLine("Erros -> " + ex.Message);
            }
            finally
            {
                //Fechar a conexão
                mConn.Close();
                // Console.ReadLine();
            }
            return Destinatarios;
        }

        /*############################## métodos em uso ##########################################*/

        /*********************************************************************
        * Get Iva (taxa Normal) actual
        ********************************************************************/
        public static string getIVA()
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["NMANIFConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select p.TaxaN from IVA p where p.DTFim IS NULL";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    return dr["TaxaN"].ToString();
                }
                else
                {
                    return "1"; // primeiro pedido do ano
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return "0";
            }
            finally
            {
                mConn.Close();
            }
        }


        /*********************************************************************
         * Get Numero da próxima Manif com base no ano
         ********************************************************************/

        public static string getNumeroProximaManif(string anoManif)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["NMANIFConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select p.Numero from manif p where p.ManifID = (" +
                    "Select Max(p.ManifID) as ManifID from manif p where p.Ano ='" + anoManif + "')";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    int auxUltimaManif = Convert.ToInt32(dr["Numero"].ToString());
                    return (auxUltimaManif + 1).ToString();
                }
                else
                {
                    return "1"; // primeiro pedido do ano
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return "0";
            }
            finally
            {
                mConn.Close();
            }
        }

        /*********************************************************************
        * Get Numero do próximo Parecer com base no ano
        ********************************************************************/

        public static string getNumeroProximoParecer(string anoParecer)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["NMANIFConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select p.Numero from parecer p where p.ParecerID = (" +
                    "Select Max(p.ParecerID) as ParecerID from parecer p where p.Ano ='" + anoParecer + "')";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    int auxUltimoParecer = Convert.ToInt32(dr["Numero"].ToString());
                    return (auxUltimoParecer + 1).ToString();
                }
                else
                {
                    return "1"; // primeiro pedido do ano
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return "0";
            }
            finally
            {
                mConn.Close();
            }
        }










        public static string getNumeroProximoPedido(string anoPedido)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["NMANIFConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select p.Numero from manif p where p.manifID = (" +
                    "Select Max(p.ManifID) as ManifID from manif p where p.Ano ='" + anoPedido + "')";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    int auxUltimoPedido = Convert.ToInt32(dr["Numero"].ToString());
                    return (auxUltimoPedido + 1).ToString();
                }
                else
                {
                    return "1"; // primeiro pedido do ano
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return "0";
            }
            finally
            {
                mConn.Close();
            }
        }



















        /*********************************************************************
            * Insere novo pedido
        ********************************************************************/
        public static int inserePedido(string numero, string ano, string DEstabelecimentoID, string codEstadoID,
            string codApoioID, string codEntidadeID, string entidade, string descrApoio, string local, DateTime dtInicio,
            DateTime dtFim, string DTempoID, string obs, string userCri, DateTime dtCri, string userModif, DateTime dtModif)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            SqlTransaction transaction;
            mConn.Open();
            transaction = mConn.BeginTransaction();//Boqueia qualquer operação nas tabelas envolvidas
            //transaction = mConn.BeginTransaction(IsolationLevel.ReadCommitted); // não bloqueia as leituras nas tabelas
            try
            {
                //Inserção do pedido
                SqlCommand mComand = mConn.CreateCommand();
                mComand.Transaction = transaction;
                mComand.CommandText = "sp_InserirPedido";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Numero", numero);
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@DEstabelecimentoID", DEstabelecimentoID);
                mComand.Parameters.AddWithValue("@CodEstadoID", codEstadoID);
                mComand.Parameters.AddWithValue("@CodApoioID", codApoioID);
                mComand.Parameters.AddWithValue("@CodEntidadeID", codEntidadeID);
                mComand.Parameters.AddWithValue("@Entidade", entidade);
                mComand.Parameters.AddWithValue("@DescrApoio", descrApoio);
                mComand.Parameters.AddWithValue("@Local", local);
                mComand.Parameters.AddWithValue("@DTInicio", dtInicio);
                mComand.Parameters.AddWithValue("@DTFim", dtFim);
                mComand.Parameters.AddWithValue("@DTempoID", DTempoID);
                mComand.Parameters.AddWithValue("@Obs", obs);
                mComand.Parameters.AddWithValue("@DTCri", dtCri);
                mComand.Parameters.AddWithValue("@UserCri", userCri);
                mComand.Parameters.AddWithValue("@DTModif", dtModif);
                mComand.Parameters.AddWithValue("@UserModif", userModif);
                //Adicionar os parâmetros de output       
                mComand.Parameters.Add("@PedidoID", SqlDbType.Int).Direction = ParameterDirection.Output;

                // Executar o comando para inserir o pedido e ir buscar o ID gerado
                int linhasAfectadas = mComand.ExecuteNonQuery();
                int pedidoID = Convert.ToInt32(mComand.Parameters["@PedidoID"].Value);

                ////Inserção das localizaçoes do pedido
                ////Terá um ciclo para a inserção das localizações do pedido
                //mComand.CommandText = "sp_InserirLocPedido";

                //foreach (Localizacao local in Locais)
                //{
                //    //Adicionar os parâmetros
                //    mComand.Parameters.Clear(); // Limpeza do parametros anteriores
                //    mComand.Parameters.AddWithValue("@PedidoID", pedidoID);
                //    mComand.Parameters.AddWithValue("@TipoLocal", local.TipoLocal);
                //    if (local.Pais.Equals("PT"))
                //    {
                //        mComand.Parameters.AddWithValue("@CodLocID", local.CodLocID);
                //    }
                //    else
                //    {
                //        mComand.Parameters.AddWithValue("@CodLocID", local.Pais);
                //    }
                //    mComand.Parameters.AddWithValue("@Latitude", local.Latitude);
                //    mComand.Parameters.AddWithValue("@Lat_NS", local.Lat_NS);
                //    mComand.Parameters.AddWithValue("@Longitude", local.Longitude);
                //    mComand.Parameters.AddWithValue("@Long_EO", local.Long_EO);
                //    // Executar o comando para inserir as localizações do pedido
                //    linhasAfectadas = mComand.ExecuteNonQuery();
                //}
                //Depois de inserir o pedido e as localizações do pedido faço o commit
                transaction.Commit();
                return pedidoID;
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return 0;
            }
            finally
            {
                mConn.Close();
            }

        }

        /**************************************************************************
           * Insere novo recurso
       ***************************************************************************/

        public static int insereRecPedido(string pedidoID, string codRecEmpenhadoID, string codViaturaID,
            string codClasseID, string codMaterialID, string QTD)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);

            try
            {
                mConn.Open();
                //Inserção do recurso
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_InserirRecPedido";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@PedidoID", pedidoID);
                mComand.Parameters.AddWithValue("@CodRecEmpenhadoID", codRecEmpenhadoID);

                mComand.Parameters.AddWithValue("@CodViaturaID", codViaturaID);
                mComand.Parameters.AddWithValue("@CodClasseID", codClasseID);
                mComand.Parameters.AddWithValue("@CodMaterialID", codMaterialID);


                mComand.Parameters.AddWithValue("@QTD", QTD);

                // Executar o comando para inserir o recurso
                int linhasAfectadas = mComand.ExecuteNonQuery();
                return linhasAfectadas;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return 0;
            }
            finally
            {
                mConn.Close();
            }
        }

        /**************************************************************************
            * Selecionar dados do utilizador da tabela UTILIZADOR, com base no NIM
        ***************************************************************************/

        public static string[] getUserByNim(string nim)
        {
            SqlConnection mConn = new SqlConnection(
                            ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);

            try
            {
                mConn.Open();
                string sSQL = "select u.OCAD, u.GU, u.UEO, p.Descricao as dPerfil " +
                    "from utilizador u, perfil p where u.PerfilID = p.PerfilID " +
                    "and u.Nim = '" + nim + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                dr.Read();
                string[] user = { dr["OCAD"].ToString(), dr["GU"].ToString(), dr["UEO"].ToString(), dr["dPerfil"].ToString() };
                return user;
            }
            catch (SqlException ex)
            {

                string[] user = { "", "", "", "" };
                Logger.LogError(ex);
                return user;
            }
            finally
            {
                mConn.Close();
            }
        }

        /**************************************************************************************
            * Update dos metadados de modificação do pedido com base no NIM e data de alteração
        ****************************************************************************************/

        public static int setUserAndDTModifByPedidoID(string userModif, string dtModif, string pedidoID)
        {
            SqlConnection mConn = new SqlConnection(
                            ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            int linhasAfectadas = 0;
            try
            {
                mConn.Open();
                string sSQL = "update pedido set UserModif = '" + userModif + "', DTModif ='" + dtModif + "' " +
                    "where PedidoID = '" + pedidoID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                // Executar o comando para fazer o update
                linhasAfectadas = mComand.ExecuteNonQuery();
                return linhasAfectadas;
            }
            catch (SqlException ex)
            {

                Logger.LogError(ex);
                return linhasAfectadas;
            }
            finally
            {
                mConn.Close();
            }
        }

        /**************************************************************************************
            * Apagar todos os recursos com base no PedidoID
        ****************************************************************************************/

        public static int delRecursosByPedidoID(string pedidoID)
        {
            SqlConnection mConn = new SqlConnection(
                            ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            int linhasAfectadas = 0;
            try
            {
                mConn.Open();
                string sSQL = "delete Recursos_Pedido " +
                    "where Recursos_Pedido.PedidoID = '" + pedidoID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                // Executar o comando para fazer o update
                linhasAfectadas = mComand.ExecuteNonQuery();
                return linhasAfectadas;
            }
            catch (SqlException ex)
            {

                Logger.LogError(ex);
                return linhasAfectadas;
            }
            finally
            {
                mConn.Close();
            }
        } // esta é do SIRCAPE_GCEME

        /**************************************************************************************
            * Testes às tabelas auxiliares antes de apagar códigos
         * 
         * 
        ****************************************************************************************/

        // Codigos usados na tabela Pedido

        public static bool testaCodApoio(string codApoioID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoID from Pedido p " +
                    "where p.CodApoioID = '" + codApoioID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static bool testaCodEntidade(string codEntidadeID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoID from Pedido p " +
                    "where p.CodEntidadeID = '" + codEntidadeID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }



        // Codigos usados na tabela Recursos_Pedido

        public static bool testaCodMaterial(string codMaterialID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoID from Recursos_Pedido rp " +
                    "where rp.CodMaterialID = '" + codMaterialID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static bool testaCodRecEmpenhado(string codRecEmpenhadoID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoID from Recursos_Pedido rp " +
                    "where rp.CodRecEmpenhadoID = '" + codRecEmpenhadoID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static bool testaCodViatura(string codViaturaID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoID from Recursos_Pedido rp " +
                    "where rp.CodViaturaID = '" + codViaturaID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static bool testaCodClasse(string codClasseID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoID from Recursos_Pedido rp " +
                    "where rp.CodClasseID = '" + codClasseID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }


        /**************************************************************************************
            * Chamadas aos store procedures referentes às estatísticas e pedidos e 
         * recursos de pedidos
         * 
        ****************************************************************************************/
        /* ##################   Métodos para os PEDIDOS ###################################*/

        public static ArrayList statsTotalPedidoAnual(string ano, string perfilID, string siglaOCAD,
          string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuais";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotal pTotal_aux = new PedTotal(dr["Ano"].ToString(), dr["totalAnual"].ToString());
                    aux.Add(pTotal_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }


        public static ArrayList statsTotalPedidoAnual_Mensal(string ano, string perfilID, string siglaOCAD,
         string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuais_Mes";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotalMes pTotalMes_aux = new PedTotalMes(dr["Ano"].ToString(), dr["DescricaoMes"].ToString(), dr["totalMensal"].ToString());
                    aux.Add(pTotalMes_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }


        public static ArrayList statsTotalPedidoAnual_Estado(string ano, string perfilID, string siglaOCAD,
         string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuais_Estado";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    //PedTotalDim pTotal_aux = new PedTotalDim(dr["Ano"].ToString(), dr["CodEstadoDescr"].ToString(), dr["totalEstado"].ToString());
                    PedTotalDim pTotal_aux = new PedTotalDim("", dr["CodEstadoDescr"].ToString(), dr["totalEstado"].ToString());
                    aux.Add(pTotal_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalPedidoAnual_Mensal_Estado(string ano, string perfilID, string siglaOCAD,
        string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuais_Mes_Estado";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotalDimMes pTotalMes_aux = new PedTotalDimMes("", dr["DescricaoMes"].ToString(), dr["CodEstadoDescr"].ToString(), dr["totalMensalEstado"].ToString());
                    aux.Add(pTotalMes_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalPedidoAnual_UEO(string ano, string perfilID, string siglaOCAD,
         string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuais_UEO";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    //PedTotalDim pTotal_aux = new PedTotalDim(dr["Ano"].ToString(), dr["CodEstadoDescr"].ToString(), dr["totalEstado"].ToString());
                    PedTotalDim pTotal_aux = new PedTotalDim("", dr["SiglaUnidade"].ToString(), dr["totalUEO"].ToString());
                    aux.Add(pTotal_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalPedidoAnual_Mensal_UEO(string ano, string perfilID, string siglaOCAD,
        string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuais_Mes_UEO";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotalDimMes pTotalMes_aux = new PedTotalDimMes("", dr["DescricaoMes"].ToString(), dr["SiglaUnidade"].ToString(), dr["totalMensalUEO"].ToString());
                    aux.Add(pTotalMes_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalPedidoAnual_Entidade(string ano, string perfilID, string siglaOCAD,
         string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuais_Entidade";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    //PedTotalDim pTotal_aux = new PedTotalDim(dr["Ano"].ToString(), dr["CodEstadoDescr"].ToString(), dr["totalEstado"].ToString());
                    PedTotalDim pTotal_aux = new PedTotalDim("", dr["CodEntidadeDescr"].ToString(), dr["totalEntidade"].ToString());
                    aux.Add(pTotal_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalPedidoAnual_Mensal_Entidade(string ano, string perfilID, string siglaOCAD,
        string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuais_Mes_Entidade";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotalDimMes pTotalMes_aux = new PedTotalDimMes("", dr["DescricaoMes"].ToString(), dr["CodEntidadeDescr"].ToString(), dr["totalMensalEntidade"].ToString());
                    aux.Add(pTotalMes_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalPedidoAnual_TipoApoio(string ano, string perfilID, string siglaOCAD,
         string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuais_TipoApoio";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    //PedTotalDim pTotal_aux = new PedTotalDim(dr["Ano"].ToString(), dr["CodEstadoDescr"].ToString(), dr["totalEstado"].ToString());
                    PedTotalDim pTotal_aux = new PedTotalDim("", dr["CodApoioDescr"].ToString(), dr["totalTipoApoio"].ToString());
                    aux.Add(pTotal_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalPedidoAnual_Mensal_TipoApoio(string ano, string perfilID, string siglaOCAD,
        string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuais_Mes_TipoApoio";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotalDimMes pTotalMes_aux = new PedTotalDimMes("", dr["DescricaoMes"].ToString(), dr["CodApoioDescr"].ToString(), dr["totalMensalTipoApoio"].ToString());
                    aux.Add(pTotalMes_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        /* ##################   Métodos para os Recursos ###################################*/

        public static ArrayList statsTotalRecursoAnual(string ano, string perfilID, string siglaOCAD,
          string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuaisRecursos";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotalRecurso pTotal_aux = new PedTotalRecurso("", dr["RecDescricao"].ToString(), dr["QTD"].ToString());
                    aux.Add(pTotal_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalRecursoAnual_Mensal(string ano, string perfilID, string siglaOCAD,
         string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuaisRecursos_Mes";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotalRecursoMes pTotalMes_aux = new PedTotalRecursoMes("", dr["DescricaoMes"].ToString(), dr["RecDescricao"].ToString(), dr["QTD"].ToString());
                    aux.Add(pTotalMes_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalRecursoAnual_UEO(string ano, string perfilID, string siglaOCAD,
         string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuaisRecursos_UEO";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotalRecursoDim pTotal_aux = new PedTotalRecursoDim("", dr["CodRecEmpenhadoDescr"].ToString(), dr["RecDescricao"].ToString(), dr["SiglaUnidade"].ToString(), dr["QTD"].ToString());
                    aux.Add(pTotal_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalRecursoAnual_Mensal_UEO(string ano, string perfilID, string siglaOCAD,
        string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuaisRecursos_Mes_UEO";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotalRecursoMesDim pTotal_aux = new PedTotalRecursoMesDim("", dr["DescricaoMes"].ToString(), dr["CodRecEmpenhadoDescr"].ToString(), dr["RecDescricao"].ToString(), dr["SiglaUnidade"].ToString(), dr["QTD"].ToString());
                    aux.Add(pTotal_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalRecursoAnual_Entidade(string ano, string perfilID, string siglaOCAD,
         string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuaisRecursos_Entidade";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotalRecursoDim pTotal_aux = new PedTotalRecursoDim("", dr["CodRecEmpenhadoDescr"].ToString(), dr["RecDescricao"].ToString(), dr["CodEntidadeDescr"].ToString(), dr["QTD"].ToString());
                    aux.Add(pTotal_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalRecursoAnual_Mensal_Entidade(string ano, string perfilID, string siglaOCAD,
        string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuaisRecursos_Mes_Entidade";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotalRecursoMesDim pTotal_aux = new PedTotalRecursoMesDim("", dr["DescricaoMes"].ToString(), dr["CodRecEmpenhadoDescr"].ToString(), dr["RecDescricao"].ToString(), dr["CodEntidadeDescr"].ToString(), dr["QTD"].ToString());
                    aux.Add(pTotal_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalRecursoAnual_TipoApoio(string ano, string perfilID, string siglaOCAD,
         string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuaisRecursos_TipoApoio";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotalRecursoDim pTotal_aux = new PedTotalRecursoDim("", dr["CodRecEmpenhadoDescr"].ToString(), dr["RecDescricao"].ToString(), dr["CodApoioDescr"].ToString(), dr["QTD"].ToString());
                    aux.Add(pTotal_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }

        public static ArrayList statsTotalRecursoAnual_Mensal_TipoApoio(string ano, string perfilID, string siglaOCAD,
         string siglaGU, string siglaUnidade)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            ArrayList aux = new ArrayList();
            try
            {
                mConn.Open();
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = "sp_StatsTotaisAnuaisRecursos_Mes_TipoApoio";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@PerfilID", perfilID);
                mComand.Parameters.AddWithValue("@SiglaOCAD", siglaOCAD);
                mComand.Parameters.AddWithValue("@SiglaGU", siglaGU);
                mComand.Parameters.AddWithValue("@SiglaUnidade", siglaUnidade);

                // Executar o comando
                SqlDataReader dr = mComand.ExecuteReader();
                while (dr.Read())
                {
                    PedTotalRecursoMesDim pTotal_aux = new PedTotalRecursoMesDim("", dr["DescricaoMes"].ToString(), dr["CodRecEmpenhadoDescr"].ToString(), dr["RecDescricao"].ToString(), dr["CodApoioDescr"].ToString(), dr["QTD"].ToString());
                    aux.Add(pTotal_aux);
                }
                return aux;
            }
            catch (SqlException ex)
            {

                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return aux;
            }
            finally
            {
                mConn.Close();
            }
        }


        /*############################## métodos em uso para a parte de Planos e Protocolos ##########################################*/


        /*************************************************************************
         * Get Numero do próximo pedido/registo com base no ano e plano/protocolo
         *************************************************************************/

        public static string getNumeroProximoPedidoPlano(string anoPedido, string codPlanoID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select p.NumeroCFT from pedido_plano p where p.PedidoPlanoID = (" +
                    "Select Max(p.PedidoPlanoID) as PedidoPlanoID from pedido_plano p where p.Ano ='" + anoPedido + "' and p.CodPlanoID ='" + codPlanoID + "')";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    int auxUltimoPedido = Convert.ToInt32(dr["NumeroCFT"].ToString());
                    return (auxUltimoPedido + 1).ToString();
                }
                else
                {
                    return "1"; // primeiro pedido do ano
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return "0";
            }
            finally
            {
                mConn.Close();
            }
        }


        /*********************************************************************
            * Insere novo pedido de plano e protocolo
        ********************************************************************/
        public static int inserePedidoPlano(string codPlanoID, string numeroCFT, string ano, string numeroRegistoEnt, string codEntApoiadaID,
            string DEstabelecimentoID, DateTime dtInicio, DateTime dtFim, string DTempoID, string hInicio, string hFim, string concelhoID,
            string localidade, string coordenadas, string obs, string userCri, DateTime dtCri, string userModif, DateTime dtModif)
        {
            SqlConnection mConn = new SqlConnection(
                           ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            SqlTransaction transaction;
            mConn.Open();
            transaction = mConn.BeginTransaction();//Boqueia qualquer operação nas tabelas envolvidas
            //transaction = mConn.BeginTransaction(IsolationLevel.ReadCommitted); // não bloqueia as leituras nas tabelas
            try
            {
                //Inserção do pedido
                SqlCommand mComand = mConn.CreateCommand();
                mComand.Transaction = transaction;
                mComand.CommandText = "sp_InserirPedidoPlano";
                mComand.CommandType = CommandType.StoredProcedure;

                //Adicionar os parâmetros de input
                mComand.Parameters.AddWithValue("@CodPlanoID", codPlanoID);
                mComand.Parameters.AddWithValue("@NumeroCFT", numeroCFT);
                mComand.Parameters.AddWithValue("@Ano", ano);
                mComand.Parameters.AddWithValue("@NumeroRegistoEnt", numeroRegistoEnt);
                mComand.Parameters.AddWithValue("@CodEntApoiadaID", codEntApoiadaID);
                mComand.Parameters.AddWithValue("@DEstabelecimentoID", DEstabelecimentoID);
                mComand.Parameters.AddWithValue("@DTInicio", dtInicio);
                mComand.Parameters.AddWithValue("@DTFim", dtFim);
                mComand.Parameters.AddWithValue("@DTempoID", DTempoID);
                mComand.Parameters.AddWithValue("@HInicio", hInicio);
                mComand.Parameters.AddWithValue("@HFim", hFim);
                mComand.Parameters.AddWithValue("@ConcelhoID", concelhoID);
                mComand.Parameters.AddWithValue("@Localidade", localidade);
                mComand.Parameters.AddWithValue("@Coordenadas", coordenadas);
                mComand.Parameters.AddWithValue("@Obs", obs);
                mComand.Parameters.AddWithValue("@DTCri", dtCri);
                mComand.Parameters.AddWithValue("@UserCri", userCri);
                mComand.Parameters.AddWithValue("@DTModif", dtModif);
                mComand.Parameters.AddWithValue("@UserModif", userModif);
                //Adicionar os parâmetros de output       
                mComand.Parameters.Add("@PedidoPlanoID", SqlDbType.Int).Direction = ParameterDirection.Output;

                // Executar o comando para inserir o pedido e ir buscar o ID gerado
                int linhasAfectadas = mComand.ExecuteNonQuery();
                int pedidoPlanoID = Convert.ToInt32(mComand.Parameters["@PedidoPlanoID"].Value);


                //Depois de inserir o pedido  faço o commit
                transaction.Commit();
                return pedidoPlanoID;
            }
            catch (SqlException ex)
            {
                transaction.Rollback();
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return 0;
            }
            finally
            {
                mConn.Close();
            }

        }

        /*********************************************************************
           * Insere actividades de um novo pedido de plano e protocolo
       ********************************************************************/
        public static int insereActividadePedidoPlano(string pedidoPlanoID, string codActividadeID)
        {
            SqlConnection mConn = new SqlConnection(
                            ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            int linhasAfectadas = 0;
            try
            {
                mConn.Open();
                string sSQL = "insert into PedidoPlano_Actividade (PedidoPlanoID, CodActividadeID) values (" + pedidoPlanoID + "," + codActividadeID + ")";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                // Executar o comando para fazer o update
                linhasAfectadas = mComand.ExecuteNonQuery();
                return linhasAfectadas;
            }
            catch (SqlException ex)
            {

                Logger.LogError(ex);
                return linhasAfectadas;
            }
            finally
            {
                mConn.Close();
            }
        }



        /*************************************************************************
        * Apagar associações Plano->Actividade com base no CodPlanoID
        *************************************************************************/

        public static int delAssPlanoActividadeByCodPlanoID(string codPlanoID)
        {
            SqlConnection mConn = new SqlConnection(
                            ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            int linhasAfectadas = 0;
            try
            {
                mConn.Open();
                string sSQL = "delete Plano_Actividade " +
                    "where Plano_Actividade.CodPlanoID = '" + codPlanoID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                // Executar o comando para fazer o update
                linhasAfectadas = mComand.ExecuteNonQuery();
                return linhasAfectadas;
            }
            catch (SqlException ex)
            {

                Logger.LogError(ex);
                return linhasAfectadas;
            }
            finally
            {
                mConn.Close();
            }
        }

        /******************************************************************************************
        * Inserir novas associações Plano->Actividade com base no CodPLanoID e CodActividadeID
        *******************************************************************************************/
        public static int insereAssPlanoActividade(string codPlanoID, string codActividadeID)
        {
            SqlConnection mConn = new SqlConnection(
                            ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            int linhasAfectadas = 0;
            try
            {
                mConn.Open();
                string sSQL = "insert into Plano_Actividade (CodPlanoID, CodActividadeID) values ('" + codPlanoID + "'," + codActividadeID + ")";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                // Executar o comando para fazer o update
                linhasAfectadas = mComand.ExecuteNonQuery();
                return linhasAfectadas;
            }
            catch (SqlException ex)
            {

                Logger.LogError(ex);
                return linhasAfectadas;
            }
            finally
            {
                mConn.Close();
            }
        }



        /*************************************************************************
        * Apagar associações Plano->EntApoiante com base no CodPLanoID
        *************************************************************************/

        public static int delAssPlanoEntApointeByCodPlanoID(string codPlanoID)
        {
            SqlConnection mConn = new SqlConnection(
                            ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            int linhasAfectadas = 0;
            try
            {
                mConn.Open();
                string sSQL = "delete Plano_EntApoiante " +
                    "where Plano_EntApoiante.CodPlanoID = '" + codPlanoID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                // Executar o comando para fazer o update
                linhasAfectadas = mComand.ExecuteNonQuery();
                return linhasAfectadas;
            }
            catch (SqlException ex)
            {

                Logger.LogError(ex);
                return linhasAfectadas;
            }
            finally
            {
                mConn.Close();
            }
        }

        /******************************************************************************************
        * Inserir novas associações Plano->EntApoiante com base no CodPLanoID e DEstabelecimentoID
        *******************************************************************************************/
        public static int insereAssPlanoEntApointe(string codPlanoID, string dEstabelecimentoID)
        {
            SqlConnection mConn = new SqlConnection(
                            ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            int linhasAfectadas = 0;
            try
            {
                mConn.Open();
                string sSQL = "insert into Plano_EntApoiante (CodPlanoID, DEstabelecimentoID) values ('" + codPlanoID + "','" + dEstabelecimentoID + "')";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                // Executar o comando para fazer o update
                linhasAfectadas = mComand.ExecuteNonQuery();
                return linhasAfectadas;
            }
            catch (SqlException ex)
            {

                Logger.LogError(ex);
                return linhasAfectadas;
            }
            finally
            {
                mConn.Close();
            }
        }


        /*************************************************************************
        * Apagar associações Plano->EntApoiada com base no CodPLanoID
        *************************************************************************/

        public static int delAssPlanoEntApoiadaByCodPlanoID(string codPlanoID)
        {
            SqlConnection mConn = new SqlConnection(
                            ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            int linhasAfectadas = 0;
            try
            {
                mConn.Open();
                string sSQL = "delete Plano_EntApoiada " +
                    "where Plano_EntApoiada.CodPlanoID = '" + codPlanoID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                // Executar o comando para fazer o update
                linhasAfectadas = mComand.ExecuteNonQuery();
                return linhasAfectadas;
            }
            catch (SqlException ex)
            {

                Logger.LogError(ex);
                return linhasAfectadas;
            }
            finally
            {
                mConn.Close();
            }
        }

        /******************************************************************************************
        * Inserir novas associações Plano->EntApoiada com base no CodPLanoID e CodEntApoiadaID
        *******************************************************************************************/
        public static int insereAssPlanoEntApoiada(string codPlanoID, string codEntApoiadaID)
        {
            SqlConnection mConn = new SqlConnection(
                            ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            int linhasAfectadas = 0;
            try
            {
                mConn.Open();
                string sSQL = "insert into Plano_EntApoiada (CodPlanoID, CodEntApoiadaID) values ('" + codPlanoID + "','" + codEntApoiadaID + "')";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                // Executar o comando para fazer o update
                linhasAfectadas = mComand.ExecuteNonQuery();
                return linhasAfectadas;
            }
            catch (SqlException ex)
            {

                Logger.LogError(ex);
                return linhasAfectadas;
            }
            finally
            {
                mConn.Close();
            }
        }


        /**************************************************************************************
         * Testes às tabelas recursos  (dos planos e protocolos)
         * 
         * 
       ****************************************************************************************/

        // Tabela de Recursos Consumidos por PedidoPlanoID

        public static bool testaRecConsumidos(string pedidoPlanoID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_RecConsumido rc " +
                    "where rc.PedidoPlanoID = '" + pedidoPlanoID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }

        // Tabela de Trabalho Realizado por PedidoPlanoID

        public static bool testaTrabRealizado(string pedidoPlanoID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_TrabRealizado tr " +
                    "where tr.PedidoPlanoID = '" + pedidoPlanoID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }

        // Tabela de material de sapador por PedidoPlanoID, e codigoSapadorID

        public static string getQuantidadeMatSapador(string pedidoPlanoID, string codigoSapadorID)
        {
            string aux = "";
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select s.Quantidade from PedidoPlano_RecEmp_MatSapador s " +
                    "where s.PedidoPlanoID = " + pedidoPlanoID + " and s.CodSapadorID = " + codigoSapadorID + "";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    aux = dr["Quantidade"].ToString();
                }
                return aux;

            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return string.Empty;
            }
            finally
            {
                mConn.Close();
            }
        }



        /**************************************************************************************
         * Testes às tabelas Danos  (dos planos e protocolos)
         * 
         * 
       ****************************************************************************************/

        // Tabela de Danos (Viaturas) por PedidoPlanoID e Matricula

        public static bool testaDanoViatura(string pedidoPlanoID, string matricula)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_Dano_Viatura dv " +
                    "where dv.PedidoPlanoID = '" + pedidoPlanoID + "' and dv.Matricula = '" + matricula + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }

        // Tabela de Danos (Uniforme) por PedidoPlanoID e NIM

        public static bool testaDanoUniforme(string pedidoPlanoID, string nim)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_Dano_Uniforme dh " +
                    "where dh.PedidoPlanoID = '" + pedidoPlanoID + "' and dh.NIM = '" + nim + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }

        // Tabela de Danos (MatSapador) por PedidoPlanoID e CodSapadorID

        public static bool testaDanoMatSapador(string pedidoPlanoID, string codSapadorID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_Dano_MatSapador ds " +
                    "where ds.PedidoPlanoID = '" + pedidoPlanoID + "' and ds.CodSapadorID = '" + codSapadorID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }




        /**************************************************************************************
           * Testes às tabelas auxiliares antes de apagar códigos (dos planos e protocolos)
        * 
        * 
       ****************************************************************************************/

        // Codigos usados na tabela PedidoPlano_Actividade

        public static bool testaCodActividade(string codActividadeID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_Actividade p " +
                    "where p.CodActividadeID = " + codActividadeID + "";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }

        // Codigos usados na tabela PedidoPlano_Dano_Uniforme

        public static bool testaCodDanoUniforme(string codDanoUniformeID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_Dano_Uniforme d " +
                    "where d.CodDanoUniformeID = " + codDanoUniformeID + "";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }

        // Codigos usados na tabela PedidoPlano_Dano_Viatura

        public static bool testaCodDanoViatura(string codDanoViaturaID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_Dano_Viatura d " +
                    "where d.CodDanoViaturaID = " + codDanoViaturaID + "";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }


        // Codigos usados na tabela Pedido_Plano

        public static bool testaCodEntApoiada(string codEntApoiadaID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from Pedido_Plano p " +
                    "where p.CodEntApoiadaID = '" + codEntApoiadaID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }


        // Codigos usados na tabela PedidoPlano_RecEmp_EqApoio

        public static bool testaCodEqApoio(string codEqApoioID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_RecEmp_EqApoio eq " +
                    "where eq.CodEqApoioID = " + codEqApoioID + "";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }


        // Codigos usados na tabela PedidoPlano_RecEmp_Gerador

        public static bool testaCodGerador(string codGeradorID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_RecEmp_Gerador g " +
                    "where g.CodGeradorID = " + codGeradorID + "";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }

        // Codigos usados na tabela PedidoPlano_RecEmp_Infraestrutura

        public static bool testaCodInfraestrutura(string codInfraestruturaID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_RecEmp_Infraestrutura i " +
                    "where i.CodInfraestruturaID = " + codInfraestruturaID + "";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }


        // Codigos usados na tabela PedidoPlano_RecEmp_MatSapador

        public static bool testaCodMatSapador(string codMatSapadorID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_RecEmp_MatSapador ms " +
                    "where ms.CodSapadorID = " + codMatSapadorID + "";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }


        // Codigos usados na tabela de Codigo de Actividades

        public static bool testaCodMIFA(string codMIFAID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select CodMIFAID from Cod_Actividade ca " +
                    "where ca.CodMIFAID = '" + codMIFAID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }


        // Codigos usados na tabela Pedido_Plano

        public static bool testaCodPlano(string codPlanoID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from Pedido_Plano p " +
                    "where p.CodPlanoID = '" + codPlanoID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }


        // Codigos usados na tabela PedidoPlano_RecEmp_RHumanos

        public static bool testaCodPosto(string codPostoID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_RecEmp_RHumano rh " +
                    "where rh.CodPostoID = '" + codPostoID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }


        // Codigos usados na tabela PedidoPlano_EncFinanceiro

        public static bool testaCodRubrica(string codRubricaID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_EncFinanceiro p " +
                    "where p.CodRubricaID = '" + codRubricaID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }


        // Codigos usados na tabela Cod_Viatura_CFT

        public static bool testaCodTipoViatura(string codTipoViaturaID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select CodViaturaID from Cod_Viatura_CFT v " +
                    "where v.CodTipoViaturaID = '" + codTipoViaturaID + "'";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }

        // Codigos usados na PedidoPlano_RecEmp_Viatura

        public static bool testaCodViaturaCFT(string codViaturaID)
        {
            SqlConnection mConn = new SqlConnection(
                          ConfigurationManager.ConnectionStrings["SIRCAPEConnectionString"].ConnectionString);
            try
            {
                mConn.Open();
                string sSQL = "Select PedidoPlanoID from PedidoPlano_RecEmp_Viatura v " +
                    "where v.CodViaturaID = " + codViaturaID + "";
                SqlCommand mComand = mConn.CreateCommand();
                mComand.CommandText = sSQL;
                SqlDataReader dr = mComand.ExecuteReader();
                return dr.HasRows;
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Response.Redirect("~/App_Resources/messages/error-page.aspx");
                Logger.LogError(ex);
                return true;
            }
            finally
            {
                mConn.Close();
            }
        }



    }
}