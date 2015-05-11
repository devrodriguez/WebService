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
using System.Collections;

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
            HttpContext.Current.Session["SessionPO"] = "2425";
            HttpContext.Current.Session["id_usuario"] = "44";

            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.DetallePO(Int32.Parse(HttpContext.Current.Session["SessionPO"].ToString()), Int32.Parse(HttpContext.Current.Session["id_usuario"].ToString()));
            var query = new BLL_Bouquets().BuildRecipePO(ds.Tables[0]).Where(bouquet => bouquet.id_detalle_po == idDetallePO);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadBouquets(Int32 id_version_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultarRecetas(id_version_bouquet);
            var query = new BLL_Bouquets().BuildBouquet(ds.Tables[0]);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadDetailBouquet(Int32 id_version_bouquet, Int32 id_formula_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultarRecetas(id_version_bouquet);
            DataTable dt = ds.Tables[1].Clone();
            ds.Tables[1].Select(String.Format("id_formula_bouquet = {0}", id_formula_bouquet)).Take(1).CopyToDataTable(dt, LoadOption.Upsert);
            var query = new BLL_Bouquets().BuildDetailBouquet(dt);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadDetailBouquetSelected(Int32 id_detalle_version_bouquet)
        {
            List<Object> liBouquet = new List<object>();

            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultarFormula(id_detalle_version_bouquet);
            var query = new BLL_Bouquets().BuildBouquetSelected(ds.Tables[0]);
            liBouquet.Add(query);
            DataTable dt = ds.Tables[1];
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            var queryDetail = new BLL_Bouquets().BuildDetailBouquetSelected(dt);
            liBouquet.Add(queryDetail);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(liBouquet);
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

        // Utiliza id_version_bouquet
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadTablaSleeve(Int32 id_detalle_version_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultargvSleeve(id_detalle_version_bouquet);
            var query = new BLL_Bouquets().BuildSleeve(ds.Tables[0]);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String InsertSleeve(String id_capuchon_cultivo, Int32 id_detalle_version_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = new DataSet();
            foreach (var id_capuchon in id_capuchon_cultivo.Split(','))
            {
                ds = dbQuery.InsertarSleeve(Convert.ToInt32(id_capuchon), id_detalle_version_bouquet);    
            }
            
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
        public String ReadSticker(String nombre_sticker)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultarSticker();
            DataTable dt = ds.Tables[0].Clone();
            ds.Tables[0].Select(String.Format("nombre_sticker LIKE '%{0}%'", nombre_sticker)).Take(100).CopyToDataTable(dt, LoadOption.Upsert);
            var query = new BLL_Bouquets().BuildSticker(dt);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        // Utilizar id_detalle_version_bouquet
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadTablaSticker(Int32 id_detalle_version_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultargvSticker(id_detalle_version_bouquet);
            var query = new BLL_Bouquets().BuildSticker(ds.Tables[0]);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String InsertSticker(String id_sticker, Int32 id_detalle_version_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = new DataSet();
            foreach (var id_sticker_s in id_sticker.Split(','))
            {
                ds = dbQuery.InsertarSticker(id_detalle_version_bouquet, Convert.ToInt32(id_sticker_s));
            }
             
            var query = new BLL_Bouquets().BuildGenericResponse(ds.Tables[0]);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String RemoveSticker(Int32 id_sticker, Int32 id_detalle_version_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.EliminarSticker(id_sticker, id_detalle_version_bouquet);
            var query = new BLL_Bouquets().BuildGenericResponse("_removed");
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
        public String InsertUPC(Int32 id_detalle_version_bouquet, List<BEL_UPC> upc)
        {
            BDBouquets dbQuery = new BDBouquets();
            foreach (var item in upc)
            {
                DataSet ds = dbQuery.ActualizarUPC(id_detalle_version_bouquet, item.descripcion_upc, Convert.ToInt16(item.orden_upc), item.nombre_informacion_upc);
            }
 
            return "[]";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadFormatoUPC(String nombre_formato)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultarFormatoUpc();
            DataTable dt = ds.Tables[0].Clone();
            ds.Tables[0].Select(String.Format("nombre_formato LIKE '%{0}%'", nombre_formato)).Take(100).CopyToDataTable(dt, LoadOption.Upsert);
            var query = new BLL_Bouquets().BuildFormatoUPC(dt);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadFlowerType(String nombre_tipo_flor)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.BouquetConsulatarFlor("consultar_tipo_flor", 0);
            DataTable dt = ds.Tables[0].Clone();
            ds.Tables[0].Select(String.Format("disponible_comercializadora = True AND nombre_tipo_flor LIKE '%{0}%'", nombre_tipo_flor)).Take(100).CopyToDataTable(dt, LoadOption.Upsert);
            var query = new BLL_Bouquets().BuildFlowerType(dt);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadFlowerVariety(Int32 id_tipo_flor_cultivo, String nombre_variedad_flor)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.BouquetConsulatarFlor("consultar_variedad_flor", id_tipo_flor_cultivo);
            DataTable dt = ds.Tables[0].Clone();
            ds.Tables[0].Select(String.Format("disponible_comercializadora = True AND nombre_variedad_flor LIKE '%{0}%'", nombre_variedad_flor)).Take(100).CopyToDataTable(dt, LoadOption.Upsert);
            var query = new BLL_Bouquets().BuildFlowerVariety(dt);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadFlowerGrade(Int32 id_tipo_flor_cultivo, String nombre_grado_flor)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.BouquetConsulatarFlor("consultar_grado_flor", id_tipo_flor_cultivo);
            DataTable dt = ds.Tables[0].Clone();
            ds.Tables[0].Select(String.Format("disponible_comercializadora = True AND nombre_grado_flor LIKE '%{0}%'", nombre_grado_flor)).CopyToDataTable(dt, LoadOption.Upsert);
            var query = new BLL_Bouquets().BuildFlowerGrade(dt);
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

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String RemoveRecipe(Int32 id_detalle_version_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.EliminarReceta(id_detalle_version_bouquet);
            var query = new BLL_Bouquets().BuildGenericResponse("_removed");
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadBouquetRecipe(Int32 id_detalle_version_bouquet, Boolean getModel, Boolean emptyRow)
        {
            if (getModel)
            {
                return new JavaScriptSerializer().Serialize(new BEL_BouquetRecipe());
            }
            else
            {
                BDBouquets dbQuery = new BDBouquets();
                DataSet ds = dbQuery.ConsultarFormula(id_detalle_version_bouquet);
                DataTable dt = ds.Tables[1];
                if (emptyRow)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
                var query = new BLL_Bouquets().BuildBouquetRecipe(dt);
                dbQuery.Cerrar();
                return new JavaScriptSerializer().Serialize(query);
            }
            
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String InsertBouquetRecipe(String nombre_formula, Int32 id_version_bouquet, String especificacion, String construccion, String cadena_formula, Int32 opcion_menu, Int32 unidades, Decimal precio, Int32 id_comida, Int32 id_detalle_version_bouquet, Int32 id_formato_upc)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.InsertarFormula(nombre_formula, 50, id_version_bouquet, especificacion, construccion, cadena_formula, opcion_menu, unidades, precio, id_comida, id_detalle_version_bouquet, id_formato_upc);
            var query = new BLL_Bouquets().GenericBuilder(ds.Tables[0]);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadSearchRecipe(String texto, String items_a_buscar, String texto2, String items_a_buscar2)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultarRecetas(texto, items_a_buscar, texto2, items_a_buscar2);
            DataTable dt = ds.Tables[0].Clone();
            ds.Tables[0].Select().Take(10).CopyToDataTable(dt, LoadOption.Upsert);
            var query = new BLL_Bouquets().GenericBuilder(dt);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String InsertObservation(List<BEL_BouquetRecipe> liRecipes, Int32 id_formula_bouquet, Int32 id_detalle_version_bouquet)
        {
            BDBouquets dbQuery = new BDBouquets();
            foreach (var recipe in liRecipes)
            {
                DataSet ds = dbQuery.InsertarObservacion(recipe.id_variedad_flor_cultivo, recipe.id_grado_flor_cultivo, recipe.cantidad_tallos, id_formula_bouquet, recipe.observacion, recipe.id_variedad_flor_cultivo_sustitucion, recipe.id_grado_flor_cultivo_sustitucion, id_detalle_version_bouquet);    
            }
            return "[]";
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String ReadSearchBouquet(String texto, String columnas)
        {
            BDBouquets dbQuery = new BDBouquets();
            DataSet ds = dbQuery.ConsultarTodoFlor();
            DataTable dt = ds.Tables[0].Clone();
            ds.Tables[0].Select(String.Format("{0} LIKE '%{1}%'", columnas, texto)).Take(100).CopyToDataTable(dt, LoadOption.Upsert);
            var query = new BLL_Bouquets().BuildSearchBouquet(dt);
            dbQuery.Cerrar();
            return new JavaScriptSerializer().Serialize(query);
        }
    }
}
