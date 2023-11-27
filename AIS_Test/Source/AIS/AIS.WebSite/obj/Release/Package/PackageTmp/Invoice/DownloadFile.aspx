<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownloadFile.aspx.cs" Inherits="ZurichNA.AIS.WebSite.Invoice.DownloadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>DownloadFile</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
  <table width="100%">
  <tr>
  <td align="Left">
  <%-- <div id="ErrorSummary">
        <asp:ValidationSummary ID="AISSummary" runat="server" DisplayMode="BulletList" CssClass="ValidationSummary"
            ValidationGroup="AISError" />
    </div>
    <asp:CustomValidator ID="AISErrorValidator" runat="server" Display="None" OnServerValidate="RaiseError"
        ValidationGroup="AISError" CssClass="ValidationSummary">
    </asp:CustomValidator>--%>
    <asp:Label ID="lblZDWerror" runat="server" ForeColor="Red"></asp:Label>
  </td>
  </tr>
  </table>
    
    </div>
    </form>
</body>
</html>
