<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AISModalUpdateProgress.ascx.cs" Inherits="ZurichNA.AIS.WebSite.App_Shared.AISModalUpdateProgress" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
   <asp:Panel ID="pnlPop" runat="server" Style ="display:none">
        <div style="text-align:center;z-index:1000; border-width:thin; border-style:solid; border-color:Blue; background-color:#fff; " >
            <b>
                <asp:Label ID="lblProgressText" Font-Size="Smaller" runat="server" Text="Page is loading, please wait." />
             </b>
             <br /><br />
            <img id="Img3" alt="" src="../images/ProgressBar.gif" runat="server" />
        </div>
    </asp:Panel>
    <asp:Button ID="btnok" runat="server" Style="display:none" />
    <cc1:ModalPopupExtender ID="mdlPopUpExt" runat="server" 
    BackgroundCssClass="modelbackground" PopupControlID="pnlPop" TargetControlID="btnok"></cc1:ModalPopupExtender>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="2000">
    <ProgressTemplate>
    </ProgressTemplate>
    </asp:UpdateProgress>
