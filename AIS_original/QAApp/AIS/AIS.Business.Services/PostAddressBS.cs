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
    /// <summary>
    /// This Class is used to interact with Postal Address  Table
    /// </summary>
    public class PostAddressBS : BusinessServicesBase<PostalAddressBE, PostAddressDA>
    {
        /// <summary>
        /// Retrieves the row from Postal Address Table based on PostalAddressID
        /// </summary>
        /// <param name="PostAddrID">PostalAddressID ID PrimaryKey</param>
        /// <returns>PostalAddressBE row based on PersonID</returns>
        public PostalAddressBE getPostAddrRow(int PostAddrID)
        {
            PostalAddressBE PostAddrBE = new PostalAddressBE();
            PostAddrBE = DA.Load(PostAddrID);
            return PostAddrBE;
        }
        /// <summary>
        /// update the information in Postal Address table
        /// </summary>
        /// <param name="postBE">PostalAddressBE</param>
        /// <param name="PerID">person ID</param>
        /// <returns>Bool (True/false) i.e., updated or not </returns>
        public bool Update(PostalAddressBE postBE, int PerID)
        {
            bool succeed = false;
            try
            {
                if (postBE.POSTALADDRESSID > 0) //On Update
                {
                    succeed = this.Save(postBE);
                }
                else //On Insert
                {
                    postBE.PERSON_ID = PerID;
                    succeed = DA.Add(postBE);
                }
            }
            catch (Exception ex)
            {
                return succeed;
            }
            return succeed;

        }
    }

}
