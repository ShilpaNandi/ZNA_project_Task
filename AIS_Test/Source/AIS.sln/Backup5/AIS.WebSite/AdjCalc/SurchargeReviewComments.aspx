<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurchargeReviewComments.aspx.cs" Inherits="ZurichNA.AIS.WebSite.AdjCalc.SurchargeReviewComments" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Surcharge Assesment Comments</title>
    
</head>
<script type="text/javascript">
  
  function Close()
  {
//     var btnSave = document.getElementById('hidSaveClicked');
//    btnSave.value="1";
var oldcmmnts = document.getElementById('hidCmmnts');
    var newcmmnts = document.getElementById('txtComments');
    var btnSave = document.getElementById('btnSave1');
    if(newcmmnts.value!=null && newcmmnts.value!="" && newcmmnts.value != oldcmmnts.value)
    {
    btnSave.click();
     window.close();
     }
  }
//window.onunload=Close();
  function CmmntsAdded()
  {
    
    var oldcmmnts = document.getElementById('hidCmmnts');
    var newcmmnts = document.getElementById('txtComments');
    var btnSave = document.getElementById('btnSave1');
    if(newcmmnts.value!=null && newcmmnts.value!="" && newcmmnts.value != oldcmmnts.value)
    {
     btnSave.click();
     window.close();
    }
    else
    {
      alert('you need to enter comments before closing the window');
    }
  }
</script>
<body>
    <form id="form1" runat="server">
    <div>
      
        <asp:Label ID="lblHeader" runat="server" Text="Please enter the Comments"></asp:Label>
   
           
               <asp:TextBox ID="txtPrgPerdId" runat="server" Height="0px" 
            Width="16px" Visible="false"></asp:TextBox>
              <asp:TextBox ID="txtPremAdjId" runat="server" Height="0px" 
            Width="16px" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="txtCustmrID" runat="server" Height="0px" 
            Width="16px" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtUserId" runat="server" Height="0px" 
            Width="16px" Visible="false"></asp:TextBox>
                    <asp:HiddenField ID="hidCmmnts" runat="server" />
                    <asp:HiddenField ID="hidSaveClicked" runat="server" />
    </div>
    <p>
   
           <table>
           <tr>
           <td align="center">
                    <asp:Label ID="lblComments" runat="server" Text="Comments"></asp:Label>
              </td>
              <td>
                    <asp:TextBox ID="txtComments" runat="server" Height="82px" TextMode="MultiLine" 
                        Width="210px" onchange="CmmntsAdded();"></asp:TextBox></td>
                        </tr>
             </table>
                    </p>
    <p>
              
            <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" OnClientClick="CmmntsAdded();"/>
             <asp:Button ID="btnSave1" runat="server" Text="Save" Width="0px" Height="0px" onclick="btnSave_Click" OnClientClick="CmmntsAdded();"/>
              
    </p>
    </form>
</body>
</html>
