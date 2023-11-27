using System;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;

public class DBAccess
{
    protected DBAccess DataAccessConn;
    public SqlConnection ConnectionID;

    public DBAccess()
    {
        OpenDBConnection("DBConnectionLINQ");
    }

    /// <summary>
    /// <para>
    /// DBAccess:OpenDBConnection( )
    /// </para>
    /// Opens the DB connection.
    /// </summary>
    /// <param name="connectionName">Name of the connection.</param>
    private void OpenDBConnection(string connectionName)
    {
        string connectionString = GetConnectionString(connectionName);
        try
        {
            ConnectionID = new SqlConnection(connectionString);
            ConnectionID.Open();
        }
        catch (Exception ex)
        {

        }
    }

    /// <summary>
    /// <para>
    /// DBAccess:GetConnectionString( )
    /// </para>
    /// Reads the connection string from the web.config file
    /// </summary>
    /// <param name="connectionName">Name of the connection.</param>
    /// <returns></returns>
    private string GetConnectionString(string connectionName)
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ToString();
        return (connectionString);
    }

    /// <summary>
    /// <para>
    /// DBAccess:ExecuteDataSet( )
    /// </para>
    /// Executes SQL/Stored Procedure, which does not accept parameters, that returns a result set.  
    /// </summary>
    /// <param name="selectString">SQL/Stored Procedure to execute</param>
    /// <param name="selectString">DataSet name</param>
    public void ExecuteDataSet(string selectString, DataSet ds)
    {
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlCommand command = new SqlCommand(selectString, ConnectionID);

        try
        {
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 40;
            adapter.SelectCommand = command;
            adapter.Fill(ds);
        }
        catch (Exception ex)
        {

        }

        finally
        {
            adapter.Dispose();
            command.Dispose();
        }
    }
}

public partial class CreateProcs : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        lblInvalidPath.Visible = false;
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
    }

    /*******************************************************************/
    // All
    /*******************************************************************/
    protected void cmdOpen_OnClick3(Object sender, EventArgs e)
    {
        this.CreatePair();
    }
    /*******************************************************************/
    // CreatePair
    /*******************************************************************/
    protected void cmdOpen_OnClick1(Object sender, EventArgs e)
    {

        this.CreatePair();
    }

    /*******************************************************************/
    // Copy Option 
    /*******************************************************************/
    protected void cmdOpen_OnClick2(Object sender, EventArgs e)
    {
    }

    // ***********************
    private void CreatePair()
    {
        bool columnwritten = false;

        string fullname;
        string SPname, TableName, storedProc;
        StreamWriter fs;
        // Loop through each stored procedure / file to create

        if (Directory.Exists(txtPath.Text))
        {
            for (int i = 0; i < ListBox1.Items.Count; i++)
            {
                SPname = ListBox1.Items[i].Text;
                TableName = ListBox1.Items[i].Value;
                fullname = txtPath.Text + "\\" + ListBox1.Items[i].Text + ".sql";
                // Delete the file if it exists.
                if (File.Exists(fullname))
                {
                    File.Delete(fullname);
                }

                fs = File.CreateText(fullname);
                FirstPart(fs, SPname, TableName, true);

                DataSet ds = new DataSet();
                // SQL Server 2000 Code
                storedProc = "select c.name, c.colorder, t.name ";
                storedProc += "from dbo.sysobjects t, dbo.syscolumns c ";
                storedProc += "where t.name = '" + TableName + "' ";
                storedProc += "and t.type = 'U' ";
                storedProc += "and t.id = c.id ";
                storedProc += "order by colid; ";
                DBAccess dd = new DBAccess();
                dd.ExecuteDataSet(storedProc, ds);
                columnwritten = false;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    // Identity column - first column in table
                    if (ds.Tables[0].Rows[j][1].ToString().Trim() == "1")
                    {
                        fs.WriteLine(getPrefix(columnwritten));
                    }
                    else
                    {
                        fs.WriteLine(getPrefix(columnwritten) + ds.Tables[0].Rows[j][0]);
                        columnwritten = true;
                    }
                }

                fs.WriteLine("        )");
                fs.WriteLine("        select");
                columnwritten = false;
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    // identity column
                    if (ds.Tables[0].Rows[j][1].ToString().Trim() == "1")
                    {

                    }
                    else if (ds.Tables[0].Rows[j][0].ToString().Trim() == "rel_prem_adj_id" &&
                         TableName == "PREM_ADJ")
                    {
                        fs.WriteLine(getPrefix(columnwritten) + "@prem_adj_id");
                        columnwritten = true;
                    }
                    else if (ds.Tables[0].Rows[j][0].ToString().Trim() == "prem_adj_id" &&
                     TableName != "PREM_ADJ")
                    {
                        fs.WriteLine(getPrefix(columnwritten) + "@new_prem_adj_id");
                        columnwritten = true;
                    }
                    else if (ds.Tables[0].Rows[j][0].ToString().Trim() == "prem_adj_perd_id" &&
                     TableName != "PREM_ADJ_PERD")
                    {
                        fs.WriteLine(getPrefix(columnwritten) + "@new_prem_adj_perd_id");
                        columnwritten = true;
                    }
                    else if (ds.Tables[0].Rows[j][0].ToString().Trim() == "crte_dt")
                    {
                        fs.WriteLine(getPrefix(columnwritten) + "@ent_timestamp");
                        columnwritten = true;
                    }
                    else if (ds.Tables[0].Rows[j][0].ToString().Trim() == "updt_dt" ||
                        ds.Tables[0].Rows[j][0].ToString().Trim() == "updt_user_id")
                    {
                        fs.WriteLine(getPrefix(columnwritten) + "NULL");
                        columnwritten = true;
                    }
                    else if (ds.Tables[0].Rows[j][0].ToString().Trim() == "prem_adj_parmet_setup_id" &&
                     (TableName == "PREM_ADJ_PARMET_DTL"))
                    {
                        fs.WriteLine(getPrefix(columnwritten) + "@new_prem_adj_parmet_setup_id");
                        columnwritten = true;
                    }
                    else if (ds.Tables[0].Rows[j][0].ToString().Trim() == "prem_adj_retro_id" &&
                     (TableName == "PREM_ADJ_RETRO_DTL"))
                    {
                        fs.WriteLine(getPrefix(columnwritten) + "@new_prem_adj_retro_id");
                        columnwritten = true;
                    }
                    else if (ds.Tables[0].Rows[j][0].ToString().Trim() == "armis_los_pol_id" &&
                     (TableName == "ARMIS_LOS_EXC"))
                    {
                        fs.WriteLine(getPrefix(columnwritten) + "@new_armis_los_pol_id");
                        columnwritten = true;
                    }
                    else
                    {
                        fs.WriteLine(getPrefix(columnwritten) + ds.Tables[0].Rows[j][0]);
                        columnwritten = true;
                    }
                }

                SecondPart(fs, SPname, TableName, true);
                ds.Dispose();
                fs.Close();
            }
        }
        else
        {
            lblInvalidPath.Visible = true;

        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fs"></param>
    /// <param name="SPname"></param>
    /// <param name="TableName"></param>
    /// <param name="CreatePair"></param>
    private void FirstPart(StreamWriter fs, string SPname, string TableName, bool CreatePair)
    {
        fs.WriteLine();
        fs.WriteLine("if exists (select 1 from sysobjects ");
        fs.WriteLine("                where name = '" + SPname + "' and type = 'P')");
        fs.WriteLine("        drop procedure " + SPname);
        fs.WriteLine("go");
        fs.WriteLine(" ");
        fs.WriteLine("set ansi_nulls off");
        fs.WriteLine("go");
        fs.WriteLine("");
        fs.WriteLine("---------------------------------------------------------------------");
        fs.WriteLine("-----");
        fs.WriteLine("-----	Proc Name:	" + SPname);
        fs.WriteLine("-----");
        fs.WriteLine("-----	Version:	SQL Server 2005");
        fs.WriteLine("-----");
        fs.WriteLine("-----	Description:	Procedure creates a copy of the " + TableName + " record.");
        fs.WriteLine("-----	");
        fs.WriteLine("-----	On Exit:");
        fs.WriteLine("-----	");
        fs.WriteLine("-----");
        fs.WriteLine("-----	Modified:	11/01/2008  Development Team");
        fs.WriteLine("-----                   Created SP");
        fs.WriteLine("-----");
        fs.WriteLine("---------------------------------------------------------------------");
        fs.WriteLine("create procedure " + SPname);

        /// Parameters sent to the SP
        if (CreatePair)
        {
            fs.WriteLine("      @select         smallint = 0, ");
            fs.WriteLine("      @prem_adj_id    int,");
            if (TableName == "PREM_ADJ")
            {
                fs.WriteLine("      @identity       int output");
            }
            else if (TableName == "PREM_ADJ_PERD")
            {
                fs.WriteLine("      @prem_adj_perd_id    int,");
                fs.WriteLine("      @new_prem_adj_id    int,");
                fs.WriteLine("      @identity       int output");
            }
            else if (TableName == "PREM_ADJ_PARMET_SETUP")
            {
                fs.WriteLine("      @prem_adj_perd_id    int,");
                fs.WriteLine("      @prem_adj_parmet_setup_id    int,");
                fs.WriteLine("      @new_prem_adj_id    int,");
                fs.WriteLine("      @new_prem_adj_perd_id    int,");
                fs.WriteLine("      @identity       int output");
            }
            else if (TableName == "PREM_ADJ_RETRO")
            {
                fs.WriteLine("      @prem_adj_perd_id    int,");
                fs.WriteLine("      @prem_adj_retro_id    int,");
                fs.WriteLine("      @new_prem_adj_id    int,");
                fs.WriteLine("      @new_prem_adj_perd_id    int,");
                fs.WriteLine("      @identity       int output");
            }
            else if (TableName == "ARMIS_LOS_POL")
            {
                fs.WriteLine("      @armis_los_pol_id    int,");
                fs.WriteLine("      @new_prem_adj_id    int,");
                fs.WriteLine("      @identity       int output");
            }
            else if (TableName == "PREM_ADJ_PARMET_DTL")
            {
                fs.WriteLine("      @prem_adj_perd_id    int,");
                fs.WriteLine("      @prem_adj_parmet_setup_id    int,");
                fs.WriteLine("      @new_prem_adj_id    int,");
                fs.WriteLine("      @new_prem_adj_perd_id    int,");
                fs.WriteLine("      @new_prem_adj_parmet_setup_id    int");

            }
            else if (TableName == "PREM_ADJ_RETRO_DTL")
            {
                fs.WriteLine("      @prem_adj_perd_id    int,");
                fs.WriteLine("      @prem_adj_retro_id    int,");
                fs.WriteLine("      @new_prem_adj_id    int,");
                fs.WriteLine("      @new_prem_adj_perd_id    int,");
                fs.WriteLine("      @new_prem_adj_retro_id    int");
            }
            else if (TableName == "ARMIS_LOS_EXC")
            {
                fs.WriteLine("      @armis_los_pol_id    int,");
                fs.WriteLine("      @new_prem_adj_id    int,");
                fs.WriteLine("      @new_armis_los_pol_id    int");
            }
            else if (TableName == "PREM_ADJ_ARIES_CLRING" || TableName == "ARMIS_LOS_POL")
            {
                fs.WriteLine("      @new_prem_adj_id    int");
            }
            else
            {
                fs.WriteLine("      @prem_adj_perd_id    int,");
                fs.WriteLine("      @new_prem_adj_id    int,");
                fs.WriteLine("      @new_prem_adj_perd_id    int");
            }
        }

        fs.WriteLine("");
        fs.WriteLine("as");
        fs.WriteLine("declare   @error      int,");
        fs.WriteLine("          @trancount  int,");
        fs.WriteLine("          @ent_timestamp  datetime");
        fs.WriteLine("");
        fs.WriteLine("");


        /// Set up Stored Procedure variables
        /// Start Transaction
        fs.WriteLine("select    @trancount  = @@trancount,");
        fs.WriteLine("          @ent_timestamp = getdate( )");
        fs.WriteLine("");
        fs.WriteLine("if @trancount = 0");
        fs.WriteLine("begin ");
        fs.WriteLine("	begin transaction ");
        fs.WriteLine("end ");
        fs.WriteLine(" ");
        fs.WriteLine("begin try");
        fs.WriteLine("");
        fs.WriteLine("	insert into " + TableName);
        fs.WriteLine("	(");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fs"></param>
    /// <param name="SPname"></param>
    /// <param name="TableName"></param>
    /// <param name="CreatePair"></param>
    private void SecondPart(StreamWriter fs, string SPname, string TableName, bool CreatePair)
    {
        fs.WriteLine("     from " + TableName);
        if (TableName == "ARMIS_LOS_POL" || TableName == "ARMIS_LOS_EXC")
        {
            fs.WriteLine("     where armis_los_pol_id = @armis_los_pol_id");
        }
        else
        {
            fs.WriteLine("     where prem_adj_id = @prem_adj_id");
        }


        if (TableName != "PREM_ADJ" && TableName != "PREM_ADJ_ARIES_CLRING" &&
            TableName != "ARMIS_LOS_POL" && TableName != "ARMIS_LOS_EXC")
        {
            fs.WriteLine("     and prem_adj_perd_id = @prem_adj_perd_id");
        }

        if (TableName == "PREM_ADJ_PARMET_DTL" || TableName == "PREM_ADJ_PARMET_SETUP")
        {
            fs.WriteLine("     and prem_adj_parmet_setup_id = @prem_adj_parmet_setup_id");
        }

        if (TableName == "PREM_ADJ_RETRO_DTL" || TableName == "PREM_ADJ_RETRO")
        {
            fs.WriteLine("     and prem_adj_retro_id = @prem_adj_retro_id");
        }


        fs.WriteLine(" ");

        if (TableName == "PREM_ADJ" || TableName == "PREM_ADJ_PERD" ||
            TableName == "PREM_ADJ_PARMET_SETUP" || TableName == "PREM_ADJ_RETRO"
            || TableName == "ARMIS_LOS_POL")
        {
            fs.WriteLine("	select  @identity = @@identity");
        }
        fs.WriteLine("       if @trancount = 0");
        fs.WriteLine("       begin");
        fs.WriteLine("                commit transaction " );
        fs.WriteLine("       end");
        fs.WriteLine("end try");
        fs.WriteLine("begin catch");
        fs.WriteLine("");
        fs.WriteLine("  if @trancount = 0");
        fs.WriteLine("  begin");
        fs.WriteLine("        rollback transaction " );
        fs.WriteLine("  end");
        fs.WriteLine("");
        fs.WriteLine("	declare @err_sev varchar(10), ");
        fs.WriteLine("	        @err_msg varchar(500), ");
        fs.WriteLine("	        @err_no varchar(10) ");
        fs.WriteLine("");
        fs.WriteLine("");
        fs.WriteLine("  select ");
        fs.WriteLine("  error_number() AS ErrorNumber,");
        fs.WriteLine("  error_severity() AS ErrorSeverity,");
        fs.WriteLine("  error_state() as ErrorState,");
        fs.WriteLine("  error_procedure() as ErrorProcedure,");
        fs.WriteLine("  error_line() as ErrorLine,");
        fs.WriteLine("  error_message() as ErrorMessage");
        fs.WriteLine("");


        fs.WriteLine("");
        fs.WriteLine("	select  @err_msg = error_message(),");
        fs.WriteLine("	        @err_no = error_number(),");
        fs.WriteLine("		    @err_sev = error_severity()");
        fs.WriteLine("");
        fs.WriteLine("		RAISERROR ( @err_msg, -- Message text. ");
        fs.WriteLine("	                @err_sev, -- Severity. ");
        fs.WriteLine("	                1 -- State. ");
        fs.WriteLine("	               )");
        fs.WriteLine("end catch");
        fs.WriteLine("");
        fs.WriteLine("go");
        fs.WriteLine("");


        fs.WriteLine("if object_id('" + SPname + "') is not null");
        fs.WriteLine("        print 'Created Procedure " + SPname + "'");
        fs.WriteLine("else");
        fs.WriteLine("        print 'Failed Creating Procedure " + SPname + "'");
        fs.WriteLine("go");
        fs.WriteLine("");
        fs.WriteLine("if object_id('" + SPname + "') is not null");
        fs.WriteLine("        grant exec on " + SPname + " to  public");
        fs.WriteLine("go");
        fs.WriteLine(" ");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="j"></param>
    /// <returns></returns>
    private string getPrefix(bool j)
    {
        if (j)
        {
            return "           ,";
        }
        else
        {
            return "            ";
        }
    }
}
