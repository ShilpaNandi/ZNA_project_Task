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
    public class AcctSetupProcChklstBS : BusinessServicesBase<Qtly_Cntrl_ChklistBE, AcctSetupProcChklstDA>
    {
        public AcctSetupProcChklstBS()
        {
            // AccountComments = new CustomerCommentsBS();
        }
        public Qtly_Cntrl_ChklistBE getQltyCntrlRow(int qltyCntrlID)
        {
            Qtly_Cntrl_ChklistBE acctChklst = new Qtly_Cntrl_ChklistBE();
            acctChklst = DA.Load(qltyCntrlID);
            return acctChklst;
        }
        public IList<Qtly_Cntrl_ChklistBE> getAllChklstItems()
        {
            IList<Qtly_Cntrl_ChklistBE> list = new List<Qtly_Cntrl_ChklistBE>();
            AcctSetupProcChklstDA chklst = new AcctSetupProcChklstDA();

            try
            {
                list = chklst.getAllChklstItems();
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (list);
        }
        public IList<Qtly_Cntrl_ChklistBE> getRelatedChklstItems(int ChklstID, int prmPrdID, int custID)
        {
            IList<Qtly_Cntrl_ChklistBE> list = new List<Qtly_Cntrl_ChklistBE>();
            AcctSetupProcChklstDA chklst = new AcctSetupProcChklstDA();

            try
            {
                list = chklst.getRelatedChklstItems(ChklstID, prmPrdID, custID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (list);
        }


        public bool Update(Qtly_Cntrl_ChklistBE ChklstBE)
        {
            bool succeed = false;

            if (ChklstBE.QualityControlChklst_ID > 0)
            {
                succeed = this.Save(ChklstBE);
            }
            else //On Insert
            {
                ChklstBE.QualityControlChklst_ID = DA.GetNextPkID().Value;
                succeed = DA.Add(ChklstBE);
            }
            return succeed;
        }

        public void UpdateQltycntrlchklst(Qtly_Cntrl_ChklistBE qltycntrlchklstBE)
        {
            (new AcctSetupProcChklstDA()).UpdateQltycntrlchklst(qltycntrlchklstBE);
        }
    }
}
