using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.Business.Logic
{
    /// <summary>
    /// This Class is used to interact with Lookup Table
    /// </summary>
    public class LookupBS : BusinessServicesBase<LookupBE, LookupDA>
    {
        public Dictionary<int, string> States
        {
            get
            {
                IList<LookupBE> result = this.getLookupData("States");
                Dictionary<int, string> temp = new Dictionary<int, string>();
                temp.Add(0, "(Select)");
                foreach (LookupBE val in result)
                {
                    temp.Add(val.LookUpID, val.LookUpName);
                }

                return temp;
            }
        }

        public Dictionary<int, string> BktcyBuyout
        {
            get
            {
                IList<LookupBE> result = this.getLookupData("BKTCY/BUYOUT");
                Dictionary<int, string> temp = new Dictionary<int, string>();
                temp.Add(0, "(Select)");
                foreach (LookupBE val in result)
                {
                    temp.Add(val.LookUpID, val.LookUpName);
                }

                return temp;
            }
        }
        public Dictionary<int, string> AdjustmentType
        {
            get
            {
                IList<LookupBE> result = this.getLookupData("AdjustmentType");
                Dictionary<int, string> temp = new Dictionary<int, string>();
                temp.Add(0, "(Select)");
                foreach (LookupBE val in result)
                {
                    temp.Add(val.LookUpID, val.LookUpName);
                }

                return temp;
            }
        }
        public Dictionary<int, string> LBAAdjustmentType
        {
            get
            {
                IList<LookupBE> result = this.getLookupData("LBAADjustmentType");
                Dictionary<int, string> temp = new Dictionary<int, string>();
                temp.Add(0, "(Select)");
                foreach (LookupBE val in result)
                {
                    temp.Add(val.LookUpID, val.LookUpName);
                }
                return temp;
            }
        }

        public IList<LookupBE> PrimaryContact
        {
            get
            {
                IList<LookupBE> result = this.getLookupData("PRIMARY CONTACT");
                return result;
            }
        }

        public Dictionary<int, string> ALAEType
        {
            get
            {
                IList<LookupBE> result = this.getLookupData("ALAEType");
                Dictionary<int, string> temp = new Dictionary<int, string>();
                temp.Add(0, "(Select)");
                foreach (LookupBE val in result)
                {
                    temp.Add(val.LookUpID, val.LookUpName);
                }

                return temp;
            }
        }

        public Dictionary<int, string> CoverageType
        {
            get
            {
                IList<LookupBE> result = this.getLookupData("CoverageType");
                Dictionary<int, string> temp = new Dictionary<int, string>();
                temp.Add(0, "(Select)");
                foreach (LookupBE val in result)
                {
                    temp.Add(val.LookUpID, val.LookUpName);
                }

                return temp;
            }
        }
        public Dictionary<int, string> LossSource
        {
            get
            {
                IList<LookupBE> result = this.getLookupData("LossSource");
                Dictionary<int, string> temp = new Dictionary<int, string>();
                temp.Add(0, "(Select)");
                foreach (LookupBE val in result)
                {
                    temp.Add(val.LookUpID, val.LookUpName);
                }

                return temp;
            }
        }

        public Dictionary<int, string> ProgramType
        {
            get
            {
                IList<LookupBE> result = this.getLookupData("ProgramType");
                Dictionary<int, string> temp = new Dictionary<int, string>();
                temp.Add(0, "(Select)");
                foreach (LookupBE val in result)
                {
                    temp.Add(val.LookUpID, val.LookUpName);
                }

                return temp;
            }
        }
        /// <summary>
        /// Retrieves LookupData for a LookUpTypeName
        /// </summary>
        /// <param name="LookupTypeName"></param>
        /// <returns>List of LookupBE</returns>
        public IList<LookupBE> getLookupData(string LookupTypeName)
        {
            LookupDA lookup = new LookupDA();
            IList<LookupBE> result = lookup.getLookUpData(LookupTypeName);
            return result;
        }
        /// <summary>
        /// Retrieves all LookupData 
        /// </summary>
        /// <returns>List of LookupBE</returns>
        public IList<LookupBE> getLookupData()
        {
            LookupDA lookup = new LookupDA();
            IList<LookupBE> result = lookup.getLookUpData();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string getLookupName(object Key, Dictionary<int, string> Table)
        {
            string name = String.Empty;
            if (Key != null)
                if (Convert.ToInt32(Key) != -1)
                    name = (from c in Table
                            where c.Key == Convert.ToInt32(Key)
                            select c.Value).First();
            return name;
        }
        /// <summary>
        /// Saves/updates LookupData 
        /// </summary>
        /// <param name="LookupBusinessEntity"></param>
        /// <returns>True/False</returns>
        public bool Update(LookupBE lkupBE)
        {
            bool succeed = false;
            try
            {
                if (lkupBE.LookUpID > 0) //On Update
                {
                    succeed = this.Save(lkupBE);

                }
                else //On Insert
                {
                    lkupBE.LookUpID = DA.GetNextPkID().Value;
                    lkupBE.Effective_Date = DateTime.Now;
                    succeed = DA.Add(lkupBE);
                }
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return succeed;

        }
        /// <summary>
        /// Retrieves LookupData for a LookUpID
        /// </summary>
        /// <param name="LookupTypeID"></param>
        /// <returns>Single Row of LookupBE</returns>
        public LookupBE getLkupRow(int LookupID)
        {
            LookupBE lkupBE = new LookupBE();
            lkupBE = DA.Load(LookupID);
            return lkupBE;
        }
        /// <summary>
        /// checks for Duplicate Record of LookupData
        /// </summary>
        /// <param name="LookupID"></param>
        /// <param name="LookupTypeID"></param>
        /// <param name="lookupName"></param>
        /// <returns>bool True/False</returns>
        public bool IsExistsLookupName(int LookupID, int LookupTypeID, string lookupName)
        {
            LookupDA lookDA = new LookupDA();
            return lookDA.IsExistsLookupName(LookupID, LookupTypeID, lookupName);
        }
    }
}
