using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data;
using System.Configuration;
using System.Text;
using ZurichNA.LSP.Framework.DataAccess;
using System.Transactions;

namespace ZurichNA.LSP.Framework
{
    public class TransactionWrapperBase
    {

    }

    public abstract class TransactionWrapper<TLinqContext> : TransactionWrapperBase, ILinqContextAccessor
        where TLinqContext : LinqContext, new()
    {
        protected LinqContext context;

        public TransactionWrapper()
        {
            this.context = (LinqContext)Activator.CreateInstance(typeof(TLinqContext), ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ConnectionString);
            context.Log = new DebuggerWriter();
        }

        LinqContext ILinqContextAccessor.Context
        {
            get { return context; }
            set { context = value; }
        }

        public virtual bool SubmitTransactionChanges()
        {
            bool success = true;
            //This is where we could set the timeout period
            //using (TransactionScope ts = new TransactionScope())
            //{
            try
            {
                context.SubmitChanges(ConflictMode.FailOnFirstConflict);
            }
            //catch //(ChangeConflictException e)
            //{
            //    //Console.WriteLine("Optimistic concurrency error");
            //    // raise optimistic concurrency error...
            //    //e = null;
            //    success = false;
            //}
            catch (Exception e)
            {
                success = false;
                Console.WriteLine("General Exception: {0}", e);
                errorMessage = e.Message;
            }
            //}
            return success;
        }
        public virtual void RollbackChanges()
        {
            context.Transaction.Rollback();
        }

        private string errorMessage = String.Empty;
        public string ErrorMessage
        {
            get { return errorMessage; }
        }
    }
}
