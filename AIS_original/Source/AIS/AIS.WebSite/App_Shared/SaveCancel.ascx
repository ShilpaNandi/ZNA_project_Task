<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_Shared_SaveCancel" Codebehind="SaveCancel.ascx.cs" %>

  
<table style="border-style: solid; border-width: 0px; border-color: inherit; width: 12%;">
<tr>
<td align="right">
    <asp:Button ID="cmdSave" runat="server" Text="Save" Width="55px" 
        CommandName="SAVE" oncommand="UC_Click" />
        
</td>
<td>
    <asp:Button ID="cmdCancel" runat="server" Text="Cancel" Width="55px" 
        CommandName="CANCEL" oncommand="UC_Click" />
        </td>
</tr>
</table>
