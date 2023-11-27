using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Web;
using System.IO;
using System.Configuration;

namespace ZurichNA.LSP.Framework.Business
{
    public abstract class BusinessServicesBase<TBusinessEntity, TDataAccessor>
        where TBusinessEntity : BusinessEntityBase
        where TDataAccessor : DataAccess.IDataAccess<TBusinessEntity>, new()
    {
        public BusinessServicesBase()
        {
            //common = new Common(this.GetType());
        }

        public BusinessServicesBase(TransactionWrapperBase AppTransWrapper)
        {
            //common = new Common(this.GetType());
            AppTransactionWrapper = AppTransWrapper;
        }

//        public Common common = null;
        private TDataAccessor da;
        protected TDataAccessor DA
        {
            get
            {
                if (da == null)
                    if (AppTransactionWrapper == null)
                        da = new TDataAccessor();
                    else
                    {
                        da = new TDataAccessor();
                        da.ApplicationTransactionWrapper = AppTransactionWrapper;
                    }
                return da;
            }
        }

        public TransactionWrapperBase AppTransactionWrapper { get; set; }


        public TBusinessEntity Retrieve(int BusinessEntity_ID)
        {
            return DA.Load(BusinessEntity_ID);
        }

        public bool Save(BusinessEntityBase BE)
        {
            bool retValue = false;
            if (BE.AutoValidate && !BE.Validate())
                return retValue;
            try
            {
                retValue = DA.Update(BE);
            }
            catch (Exception ex)
            {
                BE.SetError(ex.Message);
                retValue = false;
            }
            return retValue;
        }

        public bool Delete(TBusinessEntity BE)
        {
            bool retValue = false;
            if (BE.AutoValidate && !BE.Validate())
                return retValue;

            try
            {
                retValue = DA.Delete(BE);
            }
            catch (Exception ex)
            {
                BE.SetError(ex.Message);
                retValue = false;
            }
            return retValue;
        }

    }
}
