<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreviewInvoice.aspx.cs" Inherits="ZurichNA.AIS.WebSite.AdjCalc.PreviewInvoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Preview Invoice</title>
    <link href="../CSS/retro.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmMain" runat="server">
    
  <table width="100%">
  <tr>
  <td align="Left">
    <div id="ErrorSummary">
        <asp:ValidationSummary ID="AISSummary" runat="server" DisplayMode="BulletList" CssClass="ValidationSummary"
            ValidationGroup="AISError" />
    </div>
    <asp:CustomValidator ID="AISErrorValidator" runat="server" Display="None" OnServerValidate="RaiseError"
        ValidationGroup="AISError" CssClass="ValidationSummary">
    </asp:CustomValidator>
  </td>
  </tr>
  </table>
        </form>
</body>
</html>
