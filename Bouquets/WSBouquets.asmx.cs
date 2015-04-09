using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Data;

using Natuflora.BD.Bouquets;
using NatufloraDistribuidoraApl.Old_App_Code.Natuflora.BD.Layers.BLL;
using NatufloraDistribuidoraApl.Old_App_Code.Natuflora.BD.Layers.BEL;

namespace Natuflora.WebService.Bouquets
{
    /// <summary>
    /// Descripción breve de WebService2
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class WSBouquets : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ConsultarDetallePO(Int32 idDetallePO)
        {
            HttpContext.Current.Session["SessionPO"] = "216";
            HttpContext.Current.Session["id_usuario"] = "44";

            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.DetallePO(Int32.Parse(HttpContext.Current.Session["SessionPO"].ToString()), Int32.Parse(HttpContext.Current.Session["id_usuario"].ToString()));
            var query = new BLL_Bouquets().ConstruirDetallePO(ds.Tables[0]).Where(bouquet => bouquet.id_detalle_po == idDetallePO);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ConsultarRecetas(Int32 id_version_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultarRecetas(id_version_bouquet);
            var query = new BLL_Bouquets().ConstruirRecetas(ds.Tables[0]);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadDetailRecipe(Int32 id_version_bouquet, Int32 id_formula_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultarRecetas(id_version_bouquet);
            DataTable dt = ds.Tables[1].Clone();
            ds.Tables[1].Select(String.Format("id_formula_bouquet = {0}", id_formula_bouquet)).Take(1).CopyToDataTable(dt, LoadOption.Upsert);
            var query = new BLL_Bouquets().BuildDetailRecipe(dt);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadSleeve(String nombre_capuchon)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultarSleeve();
            DataTable dt = ds.Tables[0].Clone();
            ds.Tables[0].Select(String.Format("nombre_capuchon LIKE '%{0}%'", nombre_capuchon)).Take(100).CopyToDataTable(dt, LoadOption.Upsert);
            var query = new BLL_Bouquets().BuildSleeve(dt);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadSleeveBouquet(Int32 id_version_bouquet, Int32 id_detalle_version_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultarRecetas(id_version_bouquet);
            DataTable dt = ds.Tables[2].Clone();
            ds.Tables[2].Select(String.Format("id_detalle_version_bouquet = {0}", id_detalle_version_bouquet)).CopyToDataTable(dt, LoadOption.Upsert);
            var query = new BLL_Bouquets().BuildSleeve(dt);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadFood(String nombre_comida_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultarComida();
            DataTable dt = ds.Tables[0].Clone();
            ds.Tables[0].Select(String.Format("nombre_comida_bouquet LIKE '%{0}%'", nombre_comida_bouquet)).Take(100).CopyToDataTable(dt, LoadOption.Upsert);
            var query = new BLL_Bouquets().BuildFood(dt);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String InsertSleeve(Int32 id_capuchon_cultivo, Int32 id_detalle_version_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.InsertarSleeve(id_capuchon_cultivo, id_detalle_version_bouquet);
            var query = new BLL_Bouquets().BuildGenericResponse(ds.Tables[0]);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String RemoveSleeve(Int32 id_capuchon_cultivo, Int32 id_detalle_version_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.EliminarSleeve(id_capuchon_cultivo, id_detalle_version_bouquet);
            var query = new BLL_Bouquets().BuildGenericResponse("_removed");
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadDetailFormulaOP3(Int32 id_detalle_version_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultarDetalleFormula_3(id_detalle_version_bouquet);
            var query = new BLL_Bouquets().BuildDetailFormulaOP3(ds.Tables[0]);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }
    }
}
