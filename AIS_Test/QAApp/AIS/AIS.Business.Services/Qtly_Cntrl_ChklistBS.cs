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
    public class Qtly_Cntrl_ChklistBS : BusinessServicesBase<Qtly_Cntrl_ChklistBE, Qtly_Cntrl_ChklistDA>
    {
        public Qtly_Cntrl_ChklistBE getQualityControlRow(int QualityControlChklstID)
        {
            Qtly_Cntrl_ChklistBE list = new Qtly_Cntrl_ChklistBE();
            list = DA.Load(QualityControlChklstID);
            return list;
        }
        public IList<Qtly_Cntrl_ChklistBE> getQualityControlList()
        {
            IList<Qtly_Cntrl_ChklistBE> list = new List<Qtly_Cntrl_ChklistBE>();
            Qtly_Cntrl_ChklistDA Qtlychklist = new Qtly_Cntrl_ChklistDA();

            try
            {
                list = Qtlychklist.getQtlychklistList();
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            //foreach (AccountBE acct in list)
            //    acct.Qtlychklist_ID = (new CustomerContactBS()).getPrimaryContactData(acct.CUSTMR_ID).Qtlychklist_ID;
            return list;
        }
        public bool Update(Qtly_Cntrl_ChklistBE qltyBE)
        {
            bool succeed = false;
            if (qltyBE.QualityControlChklst_ID > 0) //On Update
            {
                succeed = this.Save(qltyBE);
            }
            else //On Insert
            {
               // PremBE.PremumAdj_sts_ID = DA.GetNextPkID().Value;
                succeed = DA.Add(qltyBE);
            }
            return succeed;
        }
       
        public IList<Qtly_Cntrl_ChklistBE> getAccQtlychklistList(int QualityCntlTypID, int PremAdjPgmID, int customerID)
        {
            IList<Qtly_Cntrl_ChklistBE> list = new List<Qtly_Cntrl_ChklistBE>();
            Qtly_Cntrl_ChklistDA Qtlychklist = new Qtly_Cntrl_ChklistDA();
            try
            {
                list = Qtlychklist.getAccQtlychklistList(QualityCntlTypID, PremAdjPgmID, customerID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }
    
        public IList<Qtly_Cntrl_ChklistBE> getQtlychklistList(int QualityCntlTypID, int PremAdjStsID,int customerID)
        {
            IList<Qtly_Cntrl_ChklistBE> list = new List<Qtly_Cntrl_ChklistBE>();
            Qtly_Cntrl_ChklistDA Qtlychklist = new Qtly_Cntrl_ChklistDA();
            try
            {
                list = Qtlychklist.getQtlychklistList(QualityCntlTypID, PremAdjStsID,customerID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }
        public IList<Qtly_Cntrl_ChklistBE> getArieschecklist(int QualityCntlTypID, int PremAdjstClrgId, int customerID)
 
     {
            IList<Qtly_Cntrl_ChklistBE> list = new List<Qtly_Cntrl_ChklistBE>();
            Qtly_Cntrl_ChklistDA Qtlychklist = new Qtly_Cntrl_ChklistDA();
            try
            {
                list = Qtlychklist.getAriesdetailslist(QualityCntlTypID, PremAdjstClrgId,customerID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }

        public bool IsExistsAriesIssue(int? ariesClrngid, int ChkListItemID, int? CustomerID, int QltyID)
        {
            Qtly_Cntrl_ChklistDA Qlty = new Qtly_Cntrl_ChklistDA();
            return Qlty.IsExistsAriesQCIssue(ariesClrngid, ChkListItemID, CustomerID, QltyID);
        }
        public bool IsExistsIssue(int? premAdjStsID, int ChkListItemID, int? CustomerID, int QltyID)
        {
            Qtly_Cntrl_ChklistDA Qlty = new Qtly_Cntrl_ChklistDA();
            return Qlty.IsExistsIssue(premAdjStsID, ChkListItemID, CustomerID,QltyID);
        }
        public bool IsExistsIssueQCDetails(int? premAdjStsID, int? ProgramPerioDID, int ChkListItemID, int? CustomerID, int QltyID)
        {
            Qtly_Cntrl_ChklistDA Qlty = new Qtly_Cntrl_ChklistDA();
            return Qlty.IsExistsIssueQCDetails(premAdjStsID, ProgramPerioDID, ChkListItemID, CustomerID, QltyID);
        }

        public bool IsExistsAcctQCIssue(int? premAdjPgmID, int ChkListItemID, int? CustomerID, int QltyID)
        {
            Qtly_Cntrl_ChklistDA Qlty = new Qtly_Cntrl_ChklistDA();
            return Qlty.IsExistsAcctQCIssue(premAdjPgmID, ChkListItemID, CustomerID, QltyID);
        }
    }
}
