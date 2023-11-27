using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.LSP.Framework.DataAccess
{
    public interface IDataAccess<TBusinessEntity>
    {
        TBusinessEntity Load(object pk);
        bool Add(BusinessEntityBase entity);
        bool Update(BusinessEntityBase entity);
        bool Delete(BusinessEntityBase entity);
        TransactionWrapperBase ApplicationTransactionWrapper { set; }
    }

    public class DataAccessor<TLinqEntity, TBusinessEntity, TLinqContext> : IDataAccess<TBusinessEntity>
        where TLinqEntity : LinqEntity, new()
        where TBusinessEntity : BusinessEntityBase, new()
        where TLinqContext : LinqContext, new()
    {

        #region Properties
//        public Common common = null;
        private LinqEntity linqEntity;
        protected bool TransactionParticipant = false;

        /// <summary>
        /// Instance of the Data Context that is used for this class.
        /// Note that this is a primary instance only - other instances
        /// can be used in other situations.
        /// </summary>
        protected TLinqContext Context { get; set; }

        /// <summary>
        /// Contains information about the primary table that is mapped
        /// to this Gateway. Contains table name, Pk and version
        /// field info. 
        /// 
        /// Values are automatically set by the constructor so ensure
        /// that the base constructor is always called.
        /// </summary>        
        protected TableInfo TableInfo { get; set; }

        /// <summary>
        /// Instance of a locally managed Linq entity object. Set with Load and New
        /// methods.
        /// </summary>
        /// 
        protected TLinqEntity LinqEntity
        {
            get { return (TLinqEntity)linqEntity; }
            set { linqEntity = value; }
        }

        /// <summary>
        /// Instance of a locally managed Business entity object. Set with Load and New
        /// methods.
        /// </summary>
        protected TBusinessEntity BusinessEntity { get; set; }

        #endregion

        #region Object Initialization

        /// <summary>
        /// Base constructor - initializes the business object's
        /// context and table mapping info.
        /// </summary>
        public DataAccessor()
        {
            //common = new Common(this.GetType());
            this.Initialize();
        }

        /// <summary>
        /// Constructore that allows passing in an existing "Transaction"
        /// so several business objects can participate in the transaction.
        /// This is actually done via a DataContext held by the Transaction.
        /// </summary>
        /// <param name="context"></param>
        public DataAccessor(TransactionWrapper<TLinqContext> AppTransactionWrapper)
        {
            //common = new Common(this.GetType());
            ApplicationTransactionWrapper = AppTransactionWrapper;
            this.Initialize();
        }

        public TransactionWrapperBase ApplicationTransactionWrapper
        {
            set
            {
                this.Context = (TLinqContext)((ILinqContextAccessor)value).Context;
                TransactionParticipant = true;
                Initialize();
            }
        }

        /// <summary>
        /// Initializes the business object explicitly.
        /// 
        /// This method can be overridden by any subclasses that want to customize
        /// the instantiation behavior and should call back to the base method
        /// 
        /// The core features this method performs are:
        /// - Create a new context        
        /// </summary>
        protected virtual void Initialize()
        {
            // *** Create a default context
            if (this.Context == null)
            {
                //this.Context = this.CreateContext("Data Source=DAVDB1320;Initial Catalog=AIS; Persist Security Info=True;User ID=Retro_User;Password=Cscrac8ng");
                this.Context = this.CreateContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
            }

            // *** Initialize Table Info 
            this.TableInfo = new TableInfo(Context, typeof(TLinqEntity));
        }

        /// <summary>
        /// Creates an instance of the context object.
        /// </summary>
        /// <returns></returns>
        protected virtual TLinqContext CreateContext()
        {
            return Activator.CreateInstance<TLinqContext>() as TLinqContext;
        }

        /// <summary>
        /// Allows creating a new context with a specific connection string.
        /// 
        /// The connection string can either be a full connection string or
        /// a connection string .Config entry.
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        protected virtual TLinqContext CreateContext(string ConnectionString)
        {
            return Activator.CreateInstance(typeof(TLinqContext), ConnectionString) as TLinqContext;
        }
        #endregion


        #region CRUD Methods

        /// <summary>
        /// Takes in a new Entity.   The new entity may be part of a transaction.
        /// In which case, it will already have a context.  
        /// </summary>
        /// <param name="BusinessEntity"></param>
        /// <returns></returns>
        /// 
        public virtual bool Add(BusinessEntityBase entity)
        {

            ILinqEntityAccessor myEntity = (ILinqEntityAccessor)entity;
            ILinqContextAccessor myContext = (ILinqContextAccessor)entity;

            // See if this Entity is participating in a Transaction
            // It can only have a context if it was instantiated with a Transaction
            if (myContext.Context == null)
                this.Initialize();
            else
                this.Context = (TLinqContext)myContext.Context;

            Table<TLinqEntity> table = this.Context.GetTable(typeof(TLinqEntity)) as Table<TLinqEntity>;
            table.InsertOnSubmit((TLinqEntity)myEntity.Entity);
            SubmitChanges();
            myContext.Context = this.Context;
            return true;
        }

        public virtual bool Update(BusinessEntityBase entity)
        {
            ILinqContextAccessor myContext = (ILinqContextAccessor)entity;
            if (myContext.Context == null)
            {
                this.Add(entity);
            }
            else
            {
                this.Context = (TLinqContext)myContext.Context;
                SubmitChanges();
            }

            return true;

        }

        /// <summary>
        /// Takes in an Entity to be deleted.   The entity may be part of a transaction.
        /// In which case, it will already have a context.  
        /// </summary>
        /// <param name="BusinessEntity"></param>
        /// <returns></returns>
        public virtual bool Delete(BusinessEntityBase entity)
        {
            ILinqEntityAccessor myEntity = (ILinqEntityAccessor)entity;
            ILinqContextAccessor myContext = (ILinqContextAccessor)entity;

            // See if this Entity is participating in a Transaction
            // It can only have a context if it was instantiated with a Transaction
            if (myContext.Context == null)
            {
                this.Initialize();
                myContext.Context = this.Context;
            }
            else
                this.Context = (TLinqContext)myContext.Context;

            Table<TLinqEntity> table = this.Context.GetTable(typeof(TLinqEntity)) as Table<TLinqEntity>;
            table.DeleteOnSubmit((TLinqEntity)myEntity.Entity);
            return true;
        }

        /// <summary>
        /// Loads an individual instance of an object
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public virtual TBusinessEntity Load(object pk)
        {
            if (this.Context == null) { this.Initialize(); }

            string sql = "select * from " + this.TableInfo.Tablename + " where " + this.TableInfo.PkField + "={0}";
            LinqEntity = this.LoadBase(sql, pk);
            
            BusinessEntity = new TBusinessEntity();
            // Declare an interface instance "myEntity":
            // This is for layered encapsulation.
            // Only the Data Access components can access this interface
            // as it is defined within the LinqModel project.
            ILinqEntityAccessor myLinqEntity = (ILinqEntityAccessor)BusinessEntity;
            myLinqEntity.Entity = (LinqEntity)LinqEntity;
            ILinqContextAccessor myLinqContext = (ILinqContextAccessor)BusinessEntity;
            myLinqContext.Context = this.Context;

            return BusinessEntity;
        }
        protected virtual void SubmitChanges()
        {
            if (!TransactionParticipant)
            {
                this.Context.SubmitChanges(ConflictMode.ContinueOnConflict);
            }
        }

        /// <summary>
        /// Loads a single record based on a generic SQL command. Can be used
        /// for customized Load behaviors where entities are loaded up.
        /// </summary>
        /// <param name="sqlLoadCommand"></param>
        /// <returns></returns>
        protected virtual TLinqEntity LoadBase(string sqlLoadCommand, params object[] args)
        {
            // this.SetError();

            try
            {
                TLinqContext context = this.Context;

                // *** If disconnected we'll create a new context
                //if (this.Options.TrackingMode == TrackingModes.Disconnected)
                //    context = this.CreateContext();

                IEnumerable<TLinqEntity> entityList = context.ExecuteQuery<TLinqEntity>(sqlLoadCommand, args);

                TLinqEntity entity = null;
                entity = entityList.Single();

                // *** Assign to local entity
                this.LinqEntity = entity;

                // *** and return instance
                return entity;
            }
            catch (InvalidOperationException)
            {
                // *** Handles errors where an invalid Id was passed, but SQL is valid
                //this.SetError("Couldn't load entity - invalid key provided.");
                this.LinqEntity = this.NewEntity();
                return null;
            }
            catch //(Exception ex)
            {
                // *** handles Sql errors
                //this.SetError(ex);
                this.LinqEntity = this.NewEntity();
            }

            return null;
        }

        /// <summary>
        /// Create a disconnected entity object
        /// </summary>
        /// <returns></returns>
        public virtual TLinqEntity NewEntity()
        {
            //this.SetError();

            try
            {
                TLinqEntity entity = Activator.CreateInstance<TLinqEntity>();

                //entity = Activator.CreateInstance(typeof(TLinqEntity), "Parm1");

                this.LinqEntity = entity;

                //if (this.Options.TrackingMode == TrackingModes.Disconnected)
                //    return entity;

                Table<TLinqEntity> table = this.Context.GetTable(typeof(TLinqEntity)) as Table<TLinqEntity>;
                table.InsertOnSubmit(entity);

                return entity;
            }
            catch //(Exception ex)
            {
                //this.SetError(ex);
                return null;
            }
        }

        /// <summary>
        /// Gets next available PK ID for table
        /// </summary>
        /// <returns>Next PK ID</returns>
        public Nullable<int> GetNextPkID()
        {
            if (this.TableInfo.PkFieldType == typeof(int))
            {
                string sql = "select MAX(" + this.TableInfo.PkField + ") from " + this.TableInfo.Tablename;
                System.Collections.IEnumerable max = this.Context.ExecuteQuery(typeof(int), sql, new object[0]);
                int value = 0;
                try
                {
                    value = max.Cast<int>().First();
                }
                catch { return value + 1;  }
                

                return  value+ 1;
            }

            //if the pk field is not of type int, then return null
            return null;

        }

        #endregion
    }

    /// <summary>
    /// Field structure for holding table information.
    /// A Table Info object is specific for a mapped entity
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// Default constructor - no assignments of any sort are applied
        /// </summary>
        public TableInfo() { }

        /// <summary>
        /// Initializes the TableInfo with information
        /// from a provided context
        /// </summary>
        /// <param name="context"></param>
        public TableInfo(LinqContext context, Type entityType)
        {
            // *** Retrieve the name of the mapped table from the schema            
            MetaTable metaTable = context.Mapping.GetTable(entityType);

            this.Tablename = metaTable.TableName;

            if (metaTable.RowType.IdentityMembers.Count < 0)
                throw new ApplicationException(this.Tablename + " doesn't have a primary key. Not supported for a business object mapping table.");

            this.PkField = metaTable.RowType.IdentityMembers[0].Name;
            this.PkFieldType = metaTable.RowType.IdentityMembers[0].Type;

            // We will allow this as some of the tables do not have Timestamps.
            // However, it is up to the developer write code to handle those situations.
            // Also, this is really used for the disconnected model which we aren't using.
            if (metaTable.RowType.VersionMember != null)
                this.VersionField = metaTable.RowType.VersionMember.Name;
            //else
            //    throw new ApplicationException(this.Tablename + " doesn't have a version field. Business object tables mapped require a version field.");

        }

        /// <summary>
        /// The name of the table that is mapped by the main Entity associated
        /// with this business object.
        /// </summary>
        public string Tablename;

        /// <summary>
        /// The version field used by this table. Version fields are required
        /// </summary>
        public string VersionField;

        /// <summary>
        /// The primary key id field used by this table.
        /// </summary>
        public string PkField;

        /// <summary>
        /// The type of the PK field.
        /// </summary>
        public Type PkFieldType;
    }


}
