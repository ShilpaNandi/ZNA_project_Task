using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZurichNA.LSP.Framework.DataAccess
{
    // This is inherited by the Linq Model Entities
    public class LinqEntity
    {
        public LinqEntity() { }
    }

    // This is inherited by the Linq Model Data Context
    public class LinqContext : System.Data.Linq.DataContext
    {
        static LinqContext() { }

        public LinqContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource) { }

        public LinqContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) :
            base(connection, mappingSource) { }
    }

    public interface ILinqEntityAccessor
    { LinqEntity Entity { get; set; } }

    public interface ILinqContextAccessor
    { LinqContext Context { get; set; } }

}
