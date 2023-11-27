using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.DAL.Logic.Queries;

namespace ZurichNA.AIS.Business.Logic
{
    public class CombinedElementsBS:BusinessServicesBase<CombinedElementsBE,CombinedElementsDA>
    {
        /// <summary>
        /// This function is used to retrieve the combined elements 
        /// information based on the programperiodid.
        /// </summary>
        /// <param name="Programperiod">Programperiod</param>
        /// <returns>List</returns>
        
        public IList<CombinedElementsBE> GetCombinedElements(int Programperiod)
        {
            IList<CombinedElementsBE> lstCombinedElements;

            try
            {
              lstCombinedElements = new CombinedElementsDA().getCombinedElements(Programperiod);
               
               
             }
            catch (Exception ex)
            {
               
                    RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                    throw myException;
                
            }

            return lstCombinedElements;
            
        }

        /// <summary>
        /// This function is used to add or update the combined elements inforamation.
        /// The input parameter is the combinedelementsBE.
        /// </summary>
        /// <param name="combelemsBE">combelemsBE</param>
        /// <returns>Success or Failure.</returns>
        public bool Update(CombinedElementsBE  combelemsBE)
        {
            
            bool succeed = false;
            try
            {
                if (combelemsBE.COMB_ELEMTS_SETUP_ID > 0)
                {
                    succeed = this.DA.Update(combelemsBE);
                }
                else
                {
                    succeed = this.DA.Add(combelemsBE);
                }
                return succeed;
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                return succeed;
            }

        }

        /// <summary>
        /// This function is used to get the particular record based on the combinedelementid(primary key)
        /// </summary>
        /// <param name="CombElemID">CombElemID</param>
        /// <returns>combinedelementsBE</returns>
        public CombinedElementsBE getCombelemID(int CombElemID)
        {
           
                CombinedElementsBE CombElem = new CombinedElementsBE();
                CombElem = DA.Load(CombElemID);
                return CombElem;
           
        }


        public IList<CombinedElementsBE> GetCombinedElements(int intProgramperiodID,int intAccountID)
        {
            IList<CombinedElementsBE> lstCombinedElements;

            try
            {
                lstCombinedElements = new CombinedElementsDA().getCombinedElements(intProgramperiodID, intAccountID);


            }
            catch (Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;

            }

            return lstCombinedElements;

        }

        public IList<CombinedElementsBE> getCombelemsPolicylist(int ProgramPeriodID)
        {
            IList<CombinedElementsBE> list = new List<CombinedElementsBE>();
            CombinedElementsBE lstPolicy = new CombinedElementsBE();
            lstPolicy.COML_AGMT_ID = 0;
            lstPolicy.Policy = "(Select)";

            try
            {
                list  = new CombinedElementsDA().getCombelemsPolicylist(ProgramPeriodID);
                list.Insert(0, lstPolicy);
            }
            catch (Exception ex)
            {

                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;

            }
            return list;

        }      

        
    } 
}
