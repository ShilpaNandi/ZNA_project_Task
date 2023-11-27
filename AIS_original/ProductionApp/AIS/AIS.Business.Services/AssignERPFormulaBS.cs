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
{    //BusinessService For ERP Formula
    public class AssignERPFormulaBS:BusinessServicesBase<ProgramPeriodBE, AssignERPFormulaDA>
    {
       
        /// <summary>
        /// Function for retrieving  ERP Formula corresponding to a particular ProgramPeriodID
        /// </summary>
        /// <param name="ProgPrdID"></param>
        /// <returns></returns>
        public ProgramPeriodBE getAssignERPRow(int ProgPrdID)
        {
            ProgramPeriodBE assignERP = new ProgramPeriodBE();
            assignERP = DA.Load(ProgPrdID);
            return assignERP;
        }
        /// <summary>
        /// Function for Updating the PremiumAdjustmentProgram Business Entity
        /// </summary>
        /// <param name="papBE">PremiumAdjustmentProgramBE</param>
        /// <returns>PremiumAdjustmentProgramBE</returns>
        public bool Update(ProgramPeriodBE papBE)
        {
            bool succeed = false;
            if (papBE.PREM_ADJ_PGM_ID > 0)
            {
                succeed = this.Save(papBE);
            }
            else //On Insert
            {
                //PerBE.PERSON_ID = this.DA.GetNextPkID().Value;
                succeed = DA.Add(papBE);
            }
            return succeed;
        }
    }
}
