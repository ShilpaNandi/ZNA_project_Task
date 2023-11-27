using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class InvoiceExhibitDA : DataAccessor<INVC_EXHIBIT_SETUP, InvoiceExhibitBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Returns all Invoice Exhibit Data
        /// </summary>
        /// <returns></returns>
        public IList<InvoiceExhibitBE> getInvoiceExhibitData()
        {
            IList<InvoiceExhibitBE> result = new List<InvoiceExhibitBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Invoice Exhibit
            /// and project it into Invoice Exhibit Business Entity
            IQueryable<InvoiceExhibitBE> query =
            (from invc in this.Context.INVC_EXHIBIT_SETUPs
             orderby invc.seq_nbr
             select new InvoiceExhibitBE()
             {
                 INVC_EXHIBIT_SETUP_ID=invc.invc_exhibit_setup_id,
                 ATCH_CD=invc.atch_cd,
                 ATCH_NM=invc.atch_nm,
                 STS_IND=invc.sts_ind,
                 SEQ_NBR=invc.seq_nbr,
                 INTRNL_FLAG_IND=invc.intrnl_flag_cd,
                 CESAR_CD_IND=invc.cesar_cd_ind,
                 UPDATEDDATE=invc.updt_dt,
                 UPDATEDUSER=invc.updt_user_id,
                 CREATEDATE=invc.crte_dt,
                 CREATEUSER=invc.crte_user_id
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;

        }
        /// <summary>
        /// Returns all Invoice Exhibit Data
        /// </summary>
        /// <returns></returns>
        ///intFlag is three types
        ///1 - Internal
        ///2 - External
        ///3 - Cesar
        public IList<InvoiceExhibitBE> getInvoiceExhibitData(int intFlag)
        {
            IList<InvoiceExhibitBE> result = new List<InvoiceExhibitBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Invoice Exhibit
            /// and project it into Invoice Exhibit Business Entity
            IQueryable<InvoiceExhibitBE> query =
            (from invc in this.Context.INVC_EXHIBIT_SETUPs
             orderby invc.seq_nbr
             select new InvoiceExhibitBE()
             {
                 INVC_EXHIBIT_SETUP_ID = invc.invc_exhibit_setup_id,
                 ATCH_CD = invc.atch_cd,
                 ATCH_NM = invc.atch_nm,
                 STS_IND = invc.sts_ind,
                 SEQ_NBR = invc.seq_nbr,
                 INTRNL_FLAG_IND = invc.intrnl_flag_cd,
                 CESAR_CD_IND = invc.cesar_cd_ind,
                 
             });
            /// Force an enumeration so that the SQL is only
            /// executed in this method
            /// Commented by Suneel 30-Jan-2009
            //if (intFlag == 1)
            //{
            //    query = query.Where(invc=>(invc.INTRNL_FLAG_IND=='I' || invc.INTRNL_FLAG_IND=='B'));
            //}
            //if (intFlag == 2)
            //{
            //    query = query.Where(invc => (invc.INTRNL_FLAG_IND == 'E' || invc.INTRNL_FLAG_IND == 'B'));
            //}
            if (intFlag == 3)
            {
                query = query.Where(invc => (invc.CESAR_CD_IND ==true ));
            }
            result = query.ToList();
            return result;

        }
    }
}
