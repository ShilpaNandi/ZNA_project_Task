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
    public class QCMasterIssueListBS : BusinessServicesBase<QCMasterIssueListBE, QLTY_CNTRL_MSTR_ISSU_LISTDA>
    {
        /// <summary>
        /// get all records based on IssCatID
        /// </summary>
        /// <param name="IssCatID"></param>
        /// <returns>IList<QCMasterIssueListBE></returns>
        public IList<QCMasterIssueListBE> getIssuesList(int IssCatID)
        {
            QCMasterIssueListDA comm = new QCMasterIssueListDA();
            IList<QCMasterIssueListBE> result = comm.getQltyIssueList(IssCatID);
            return result;
        }
        public IList<QCMasterIssueListBE> getIssuesListALL(int IssCatID)
        {
            QCMasterIssueListDA comm = new QCMasterIssueListDA();
            IList<QCMasterIssueListBE> result = comm.getQltyIssueListALL(IssCatID);
            return result;
        }
        /// <summary>
        /// Used to add or Update Record
        /// </summary>
        /// <param name="qltyMasterIssueBE"></param>
        /// <returns>bool True/False</returns>
        public bool Update(QCMasterIssueListBE qltyMasterIssueBE)
        {
            bool succeed = false;
            try
            {
            if (qltyMasterIssueBE.QualityCntrlMstrIsslstID > 0) //On Update
            {
                succeed = this.Save(qltyMasterIssueBE);
            }
            else //On Insert
            {
                
                succeed = DA.Add(qltyMasterIssueBE);
            }
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                return succeed;
            }
            return succeed;
           
        }
        /// <summary>
        /// Retrieves all QCMasterIssueList Data based on QCMasterIssueID
        /// </summary>
        /// <param name="QCMasterIssueID"></param>
        /// <returns>QCMasterIssueListBE</returns>
        public QCMasterIssueListBE getQCMasterIssueRow(int QCMasterIssueID)
        {
            QCMasterIssueListBE list = new QCMasterIssueListBE();
            list = DA.Load(QCMasterIssueID);
            return list;
        }
        /// <summary>
        /// checks for Duplicate Record of QCMasterIssueData
        /// </summary>
        /// <param name="LookupID"></param>
        /// <param name="intQCMasterIssueID"></param>
        /// <param name="strIssueName"></param>
        /// <returns>>bool True/False</returns>
        public bool IsExistsIssueName(int LookupID, int intQCMasterIssueID, string strIssueName)
        {
            QCMasterIssueListDA QCMasterDA = new QCMasterIssueListDA();
            return QCMasterDA.IsExistsIssueName(LookupID,intQCMasterIssueID,strIssueName);
        }
    }
}
