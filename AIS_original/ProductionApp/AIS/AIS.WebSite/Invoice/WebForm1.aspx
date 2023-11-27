<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ZurichNA.AIS.WebSite.Invoice.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script language="javascript" type="text/javascript">        
    window.setTimeout('SimClick()', 3000);        
    function SimClick()       
     {      
     __doPostBack('<%=lkbDownload.UniqueID%>','')   
      
        $get('ctl00_btnCancel').click();

    
      }      
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:LinkButton runat="server" ID="lkbDownload" Text="x" OnClick="lkbDownload_Click"
            Style="display: none"></asp:LinkButton>
    </div>
    </form>
</body>
</html>
