<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedAccountList.ascx.cs" Inherits="App_Shared_RelAccountUserDropdown" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:UpdatePanel ID="drpUpdatepanel" runat="server" UpdateMode="Conditional">

<ContentTemplate>
<table>
<tr>
<td>
<asp:LinkButton ID="LinkButton1" runat="server" onclick="LButton_Click" 
        ToolTip="displays the account range starts with [a,b,c] letters">A</asp:LinkButton>&nbsp;&nbsp; 
    &nbsp;

    <asp:LinkButton ID="LinkButton2" runat="server" onclick="LButton_Click" 
        ToolTip="displays the account range starts with [d,e,f] letters">D</asp:LinkButton>&nbsp;&nbsp; &nbsp;
   
        <asp:LinkButton ID="LinkButton3" runat="server" onclick="LButton_Click" 
        ToolTip="displays the account range starts with [g,h,i] letters">G</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
   
        <asp:LinkButton ID="LinkButton4" runat="server" onclick="LButton_Click" 
        ToolTip="displays the account range starts with [j,k,l] letters">J</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
   
        <asp:LinkButton ID="LinkButton5" runat="server" onclick="LButton_Click" 
        ToolTip="displays the account range starts with [m,n,o] letters">M</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
   
        <asp:LinkButton ID="LinkButton6" runat="server" onclick="LButton_Click" 
        ToolTip="displays the account range starts with [p,q,r] letters">P</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
   
        <asp:LinkButton ID="LinkButton7" runat="server" onclick="LButton_Click" 
        ToolTip="displays the account range starts with [s,t,u] letters">S</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
   
        <asp:LinkButton ID="LinkButton8" runat="server" onclick="LButton_Click" 
        ToolTip="displays the account range starts with [v,w,x] letters">V</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
   
        <asp:LinkButton ID="LinkButton9" runat="server" onclick="LButton_Click" 
        ToolTip="displays the account range starts with [y,z] letters">Y</asp:LinkButton>
       
        
       </td> 
</tr>
<tr>

<td>
<asp:DropDownList ID="ddlAccountlist" runat="server" Width="231px">
        
    </asp:DropDownList>
  
</td>
</tr>
</table>
</ContentTemplate>
<Triggers >


</Triggers>

 </asp:UpdatePanel>