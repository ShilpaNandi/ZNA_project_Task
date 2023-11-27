using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.LSP.Framework.Business
{

    public abstract class BusinessEntityBase : ILinqEntityAccessor, ILinqContextAccessor
    {

        protected LinqEntity BaseEntity;
        protected LinqContext Context;
        public BusinessEntityBase()
        {
            this.AutoValidate = true;
        }

        // ILM_LinqInfoAccess Explicit Interface Implmentations
        LinqEntity ILinqEntityAccessor.Entity
        {
            get { return BaseEntity; }
            set { BaseEntity = value; }
        }

        LinqContext ILinqContextAccessor.Context
        {
            get { return Context; }
            set { Context = value; }
        }

        public abstract ValidationErrorCollection ValidationErrors { get; }
        public virtual bool Validate() { return true; }
        public virtual bool Validate(ValidationErrorCollection ErrorCollection) { return true; }
        public virtual bool Validate(ValidationErrorCollection ErrorCollection, string BusinessEntityName) { return true; }
        public abstract bool AutoValidate { get; set; }
        public abstract void SetError(string Message);
        public bool InTransaction { get; set; }
        public bool IsNull(){return (BaseEntity==null);}

        public void SetContext(TransactionWrapperBase Transaction)
        {
            this.Context = ((ILinqContextAccessor)Transaction).Context;
        }

    }

    public class BusinessEntityList<TBEntity, TLEntity> : BusinessEntityBase
        where TBEntity : BusinessEntityBase, new()
        where TLEntity : LinqEntity, new()
    {
        public BusinessEntityList()
        {
            AutoValidate = true;
            this._BEList = new List<TBEntity>();
        }
        //public BusinessEntityList(TransactionWrapperBase Transaction)
        //{
        //    AutoValidate = true;
        //    this.Context = ((ILinqContextAccessor)Transaction).Context;
        //    this._BEList = new List<TBEntity>();
        //}

        public string BusinessEntityName { get; set; }
        public void SetContext(TransactionWrapperBase Transaction)
        {
            this.Context = ((ILinqContextAccessor)Transaction).Context;
        }


        #region PrivateVariables

        private List<TBEntity> _BEList;
        private ValidationErrorCollection _ValidationErrors;

        #endregion
        #region Public Properties
        /// <summary>
        /// Determines whether or not the Save operation causes automatic
        /// validation
        /// </summary>        
        public override bool AutoValidate { get; set; }

        /// <summary>
        /// Instance of an exception object that caused the last error
        /// </summary>                
        public Exception ErrorException { get; set; }

        /// <summary>
        /// Provides access to the list of Business Entities
        /// </summary>
        public List<TBEntity> BEList { get { return _BEList; } set { _BEList = value; } }
        #endregion

        #region ValidationMethods

        /// <summary>
        /// Validate() is used to validate business rules on the business object. 
        /// Generally this method consists of a bunch of if statements that validate 
        /// the data of the business object and adds any errors to the 
        /// <see>wwBusiness.ValidationErrors</see> collection.
        /// 
        /// If the <see>wwBusiness.AutoValidate</see> flag is set to true causes Save()
        ///  to automatically call this method. Must be overridden to perform any 
        /// validation.
        /// <seealso>Class wwBusiness Class ValidationErrorCollection</seealso>
        /// </summary>
        /// <returns>True or False.</returns>
        public override bool Validate()
        {
            this.ValidationErrors.Clear();
            ValidateList();
            return (this.ValidationErrors.Count > 0);
        }

        public override bool Validate(ValidationErrorCollection ErrorCollection)
        {
            this.ValidationErrors.Clear();
            ValidateList();
            ErrorCollection.CopyValidationErrors(this.ValidationErrors, BusinessEntityName);
            return (this.ValidationErrors.Count <= 0);
        }

        private bool ValidateList()
        {
            // Check individual Entry Rules first
            bool eachEntityIsValid = true;
            foreach (TBEntity BE in BEList)
            {
                if (BE.Validate(this.ValidationErrors) == false)
                {
                    eachEntityIsValid = false;
                }
            }
            if (eachEntityIsValid)
            {
                // Now check cross list validation rules.
                this.CheckCrossListValidationRules();

                if (this.ValidationErrors.Count > 0)
                    return false;

                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void CheckCrossListValidationRules()
        {
        }

        /// <summary>
        /// Sets an internal error message.
        /// </summary>
        /// <param name="Message"></param>
        public override void SetError(string Message)
        {
            if (string.IsNullOrEmpty(Message))
            {
                this.ErrorException = null;
                return;
            }

            this.ErrorException = new ApplicationException(Message);
            this.ValidationErrors.Add(Message);

            //if (this.Options.ThrowExceptions)
            //    throw this.ErrorException;
        }

        /// <summary>
        /// Sets an internal error exception
        /// </summary>
        /// <param name="ex"></param>
        protected void SetError(Exception ex)
        {
            this.ErrorException = ex;

            //if (ex != null && this.Options.ThrowExceptions)
            throw ex;
        }

        /// <summary>
        /// Clear out errors
        /// </summary>
        public void SetError()
        {
            this.ErrorException = null;
        }
        /// <summary>
        /// A collection that can be used to hold errors. This collection
        /// is set by the AddValidationError method.
        /// </summary>
        public override ValidationErrorCollection ValidationErrors
        {
            get
            {
                if (this._ValidationErrors == null)
                    this._ValidationErrors = new ValidationErrorCollection();
                return _ValidationErrors;
            }
        }
        #endregion

        public bool AddBusinessEntityToList(TBEntity Entity)
        {
            ILinqEntityAccessor myEntity = (ILinqEntityAccessor)Entity;
            Table<TLEntity> table = this.Context.GetTable(typeof(TLEntity)) as Table<TLEntity>;
            table.InsertOnSubmit((TLEntity)myEntity.Entity);
            BEList.Add(Entity);
            return true;
        }
        public bool DeleteBusinessEntityFromList(TBEntity Entity)
        {
            ILinqEntityAccessor myEntity = (ILinqEntityAccessor)Entity;
            Table<TLEntity> table = this.Context.GetTable(typeof(TLEntity)) as Table<TLEntity>;
            table.DeleteOnSubmit((TLEntity)myEntity.Entity);
            BEList.Remove(Entity);
            return true;
        }
    }

    public class BusinessEntity<TEntity> : BusinessEntityBase
        where TEntity : LinqEntity, new()
    {

        public BusinessEntity()
        {
            this.Entity = new TEntity();
        }

        protected TEntity Entity
        {
            get { return (TEntity)BaseEntity; }
            set { BaseEntity = value; }
        }

        #region Properties


        /// <summary>
        /// Instance of an exception object that caused the last error
        /// </summary>                
        public Exception ErrorException { get; set; }

        /// <summary>
        /// A collection that can be used to hold errors. This collection
        /// is set by the AddValidationError method.
        /// </summary>
        public override ValidationErrorCollection ValidationErrors
        {
            get
            {
                if (this._ValidationErrors == null)
                    this._ValidationErrors = new ValidationErrorCollection();
                return _ValidationErrors;
            }
        }
        ValidationErrorCollection _ValidationErrors;

        /// <summary>
        /// Error Message of the last exception
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                if (this.ErrorException == null)
                    return "";
                return this.ErrorException.Message;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.ErrorException = null;
                else
                    // *** Assign a new exception
                    this.ErrorException = new ApplicationException(value);
            }
        }

        /// <summary>
        /// Determines whether or not the Save operation causes automatic
        /// validation
        /// </summary>        
        public override bool AutoValidate { get; set; }

        #endregion

        /// <summary>
        /// Validate() is used to validate business rules on the business object. 
        /// Generally this method consists of a bunch of if statements that validate 
        /// the data of the business object and adds any errors to the 
        /// <see>wwBusiness.ValidationErrors</see> collection.
        /// 
        /// If the <see>wwBusiness.AutoValidate</see> flag is set to true causes Save()
        ///  to automatically call this method. Must be overridden to perform any 
        /// validation.
        /// <seealso>Class wwBusiness Class ValidationErrorCollection</seealso>
        /// </summary>
        /// <returns>True or False.</returns>
        public bool Validate(TEntity entity)
        {
            this.ValidationErrors.Clear();

            this.CheckValidationRules();

            if (this.ValidationErrors.Count > 0)
            {
                this.SetError(this.ValidationErrors.ToString());
                return false;
            }

            return true;
        }

        public override bool Validate(ValidationErrorCollection ErrorCollection)
        {
            return Validate(ErrorCollection, "");
        }

        public override bool Validate(ValidationErrorCollection ErrorCollection, string BusinessEntityName)
        {
            this.ValidationErrors.Clear();

            this.CheckValidationRules();

            ErrorCollection.CopyValidationErrors(this.ValidationErrors, BusinessEntityName);

            return (this.ValidationErrors.Count <= 0);
        }

        //}
        /// <summary>
        /// Validates the current entity object
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return this.Validate(this.Entity);
        }

        protected virtual void CheckValidationRules()
        {
        }

        /// <summary>
        /// Sets an internal error message.
        /// </summary>
        /// <param name="Message"></param>
        public override void SetError(string Message)
        {
            if (string.IsNullOrEmpty(Message))
            {
                this.ErrorException = null;
                return;
            }

            this.ErrorException = new ApplicationException(Message);

            //if (this.Options.ThrowExceptions)
            //    throw this.ErrorException;
        }

        /// <summary>
        /// Sets an internal error exception
        /// </summary>
        /// <param name="ex"></param>
        public void SetError(Exception ex)
        {
            this.ErrorException = ex;

            //if (ex != null && this.Options.ThrowExceptions)
            throw ex;
        }
        /// <summary>
        /// Clear out errors
        /// </summary>
        public void SetError()
        {
            this.ErrorException = null;
        }


    }
}
