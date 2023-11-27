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
    public class Coml_Agmt_AuDtBS : BusinessServicesBase<Coml_Agmt_AuDtBE, Coml_Agmt_AuDtDA>
    {
        /// <summary>
        /// Function to get the row from Coml_agmt_audt Table based on ComlAgrAuditID
        /// </summary>
        /// <param name="CommAgrAuditID"></param>
        /// <returns></returns>
        public Coml_Agmt_AuDtBE getCommAgrRow(int CommAgrAuditID)
        {
            Coml_Agmt_AuDtBE Coml_Agmt_AuDtBE = new Coml_Agmt_AuDtBE();
            Coml_Agmt_AuDtBE = DA.Load(CommAgrAuditID);
            return Coml_Agmt_AuDtBE;
        }
        /// <summary>
        /// Function to get the records from Coml_agmt_audt Table based on ProgramPeriodID
        /// </summary>
        /// <param name="progPeriodID"></param>
        /// <returns></returns>
        public IList<Coml_Agmt_AuDtBE> getCommAgrAuditList(int progPeriodID)
        {
            IList<Coml_Agmt_AuDtBE> list = new List<Coml_Agmt_AuDtBE>();
            Coml_Agmt_AuDtDA Comml = new Coml_Agmt_AuDtDA();
            try
            {
                list = Comml.getCommAgrAuditList(progPeriodID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            //foreach (AccountBE acct in list)
            //    acct.PERSON_ID = (new CustomerContactBS()).getPrimaryContactData(acct.CUSTMR_ID).PERSON_ID;
            return list;
        }

        /// <summary>
        /// Function to get the records from Coml_agmt_audt Table based on SustomerID and ProgramPeriodID
        /// </summary>
        /// <param name="CustmrID"></param>
        /// <param name="progPeriodID"></param>
        /// <returns></returns>
        public IList<Coml_Agmt_AuDtBE> getCommAgrAuditList(int CustmrID, int progPeriodID)
        {
            IList<Coml_Agmt_AuDtBE> list = new List<Coml_Agmt_AuDtBE>();
            Coml_Agmt_AuDtDA Comml = new Coml_Agmt_AuDtDA();
            try
            {
                list = Comml.getCommAgrAuditList(CustmrID, progPeriodID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            //foreach (AccountBE acct in list)
            //    acct.PERSON_ID = (new CustomerContactBS()).getPrimaryContactData(acct.CUSTMR_ID).PERSON_ID;
            return list;
        }

        /// <summary>
        /// Function to Save or Update the Record in Coml_agmt_audt Table
        /// </summary>
        /// <param name="commlBE"></param>
        /// <returns></returns>
        public bool Update(Coml_Agmt_AuDtBE commlBE)
        {
            bool succeed = false;
            if (commlBE.Comm_Agr_Audit_ID > 0) //On Update
            {
                succeed = this.Save(commlBE);
            }
            else //On Insert
            {
                //commlBE.Comm_Agr_Audit_ID = DA.GetNextPkID().Value;
                succeed = DA.Add(commlBE);
            }
            return succeed;
        }
    }
}
