using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using System.Collections.Generic;

    /// <summary>
    /// This Menu class is used to generate the menu 
    /// and its menu options
    /// </summary>
    public class AISMenu
    {
        /// <summary>
        /// Constructor for AISMenu
        /// </summary>
        static AISMenu()
        {

        }

        /// <summary>
        /// State Persisted Property to hold Menu Data
        /// </summary>
        public static DataSet MenuDataSet
        {
            get
            {
                if (HttpContext.Current.Session["MenuDataSet"] != null)
                    return ((DataSet)HttpContext.Current.Session["MenuDataSet"]);
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["MenuDataSet"] = value;
            }
        }

        /// <summary>
        /// This Method generates Menu
        /// </summary>
        /// <param name="roleName">Role Name</param>
        /// <param name="xdsSource">Menu Control Reference</param>
         public static void CreateMenu(string roleName, ref XmlDataSource xdsSource)
        {
            DataSet dsMenu = new DataSet();
            DataTable dtMenu = new DataTable();
            MenuBS objMnuBS = new MenuBS();

            //if MenuDataSet is null, assign value to menudataset
             if (MenuDataSet == null)
            {
                if (roleName.Length > 1)
                {
                    //Retrieves Menu Items
                    dtMenu = objMnuBS.RetrieveMenuitems(roleName);

                    dsMenu.DataSetName = "Menus";

                    dsMenu.Tables.Add(dtMenu);
                    dsMenu.Tables[0].TableName = "Menu";
                    DataRelation drLink = new DataRelation("MasterChild",
                    dsMenu.Tables["Menu"].Columns["menu_id"],
                    dsMenu.Tables["Menu"].Columns["parnt_id"], false);
                    drLink.Nested = true;
                    dsMenu.Relations.Add(drLink);
                }
                MenuDataSet = dsMenu;
            }
            else
                // Retrieves from MenuDataSet
                dsMenu = MenuDataSet;

            //Retrieves XML Document
            xdsSource.Data = dsMenu.GetXml();
            xdsSource.GetXmlDocument();

            //Disposing the objects
            dsMenu.Dispose();
            xdsSource.Dispose();
        }

        /// <summary>
        /// Enables Dependant Menu Options
        /// </summary>
        /// <param name="level">Dependant Menu Level</param>
        public static void EnableDependMenu(DependentMenuLevel level)
        {
            //Enables Dependency Menu Items
            DataView view = MenuDataSet.Tables[0].AsDataView();
            //Filters the data based on the Level
            view.RowFilter = "depnd_txt='" + level.ToString() + "'";
            view.AllowEdit = true;
            //Assigns true value actv_ind for all rows
            for (int rowLoop = 0; rowLoop < view.Count; rowLoop++)
            {
                view[rowLoop].BeginEdit();
                view[rowLoop]["actv_ind"] = true;
                view[rowLoop].EndEdit();
            }
        }
        /// <summary>
        /// Disable Dependant Menu Options
        /// </summary>
        /// <param name="level">Dependant Menu Level</param>
        public static void DisableDependMenu(string roleName, DependentMenuLevel level)
        {
            DataSet dsMenu = new DataSet();
            DataTable dtMenu;
            MenuBS objMnuBS = new MenuBS();
            //Retrieves Menu Items
            dtMenu = objMnuBS.RetrieveMenuitems(roleName);

            dsMenu.DataSetName = "Menus";

            dsMenu.Tables.Add(dtMenu);
            dsMenu.Tables[0].TableName = "Menu";
            DataRelation drLink = new DataRelation("MasterChild",
            dsMenu.Tables["Menu"].Columns["menu_id"],
            dsMenu.Tables["Menu"].Columns["parnt_id"], false);
            drLink.Nested = true;
            dsMenu.Relations.Add(drLink);

            MenuDataSet = dsMenu;
        }
        /// <summary>
        /// Enables AdjustmentReview Menu
        /// </summary>
        /// <param name="level"></param>
        public static void EnableAdjReviewMenu()
        {
            //Enables Dependency Menu Items
            DataView view = MenuDataSet.Tables[0].AsDataView();
            //Filters the data based on the Level
            view.RowFilter = "web_page_txt='AdjCalc/AdjustmentReview.aspx'";
            view.AllowEdit = true;
            //Assigns true value actv_ind for all rows
            for (int rowLoop = 0; rowLoop < view.Count; rowLoop++)
            {
                view[rowLoop].BeginEdit();
                view[rowLoop]["actv_ind"] = true;
                view[rowLoop].EndEdit();
            }
        }

    }

