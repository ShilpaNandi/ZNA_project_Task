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
    public class AdjProcChklstBS : BusinessServicesBase<Qtly_Cntrl_ChklistBE, AdjProcChklstDA>
    {
        public AdjProcChklstBS()
        {
        }
        public Qtly_Cntrl_ChklistBE getQltyCntrlRow(int qltyCntrlID)
        {
            Qtly_Cntrl_ChklistBE adjChklst = new Qtly_Cntrl_ChklistBE();
            adjChklst = DA.Load(qltyCntrlID);
            return adjChklst;
        }
        public IList<Qtly_Cntrl_ChklistBE> getAllAdjChklstItems()
        {
            IList<Qtly_Cntrl_ChklistBE> list = new List<Qtly_Cntrl_ChklistBE>();
            AdjProcChklstDA adjchklst = new AdjProcChklstDA();

            try
            {
                list = adjchklst.getAdjProcItemInfo();
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (list);
        }
        public IList<Qtly_Cntrl_ChklistBE> getAllApprovedInvChklstItems()
        {
            IList<Qtly_Cntrl_ChklistBE> list = new List<Qtly_Cntrl_ChklistBE>();
            AdjProcChklstDA adjchklst = new AdjProcChklstDA();

            try
            {
                list = adjchklst.getApprovedInvItemInfo();
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (list);
        }
        public IList<Qtly_Cntrl_ChklistBE> getRelatedChklstItems(int ChklstID, int prmAdjID, int custID, int prgPrdID)
        {
            IList<Qtly_Cntrl_ChklistBE> list = new List<Qtly_Cntrl_ChklistBE>();
            AdjProcChklstDA chklst = new AdjProcChklstDA();

            try
            {
                list = chklst.getRelatedChklstItems(ChklstID, prmAdjID, custID, prgPrdID);
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
    }
}

