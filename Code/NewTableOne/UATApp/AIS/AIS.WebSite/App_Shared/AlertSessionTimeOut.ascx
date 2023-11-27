<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlertSessionTimeOut.ascx.cs" Inherits="ZurichNA.AIS.WebSite.App_Shared.AlertSessionTimeOut" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script type="text/javascript">
    //To handle Session TimeOut
    var timeID = null;
    function SetSessionTimeOut() 
    {
        if (timeID != null)
            clearTimeout(timeID);
        var TimeOutValue = '<%=Session.Timeout%>';
        var timeinMS = TimeOutValue * 60 * 1000;
        timeID = setTimeout('HandleTimeOut();', timeinMS);
    }

    function HandleTimeOut() {
        $find("bmdlPopUpTimeOut").show();
    }

    function CloseOk() {
        $find("bmdlPopUpTimeOut").hide();
        window.location= "../AcctSearch.aspx";
    }

    function pageLoad(o, e) {
        SetSessionTimeOut();
    }

    AddBodyOnLoadEvent(SetSessionTimeOut);

    //End of To handle Session TimeOut

</script>

    <asp:Panel ID="pnlPopTimeOut" runat="server" Style ="display:none">
        <div style="text-align:center;z-index:1000; border-width:thin; border-style:solid; border-color:Blue; background-color:#fff; " >
               <table>
               <tr>
                <td>
                <img alt="" id="img1" src="../Images/Warning.gif"/>
                </td><td>
                <asp:Label ID="lblProgressText" style="display: inline; font-weight: bold; font-size: 11px; width: 180px; color: black; font-family: Verdana; height: 15px;"
                runat="server" Text="Your session is expired. <br/> Please click 'Ok' button to redirect <br/> to Account Search page." />
                </td></tr>
                <tr>
                <td></td>
                <td align="center">
                <asp:Button ID="btnClose" runat="server" Text=" Ok " />
                </td></tr>
               </table>
        </div>
    </asp:Panel>

    <asp:Button ID="btnok" runat="server" Style="display:none" />
    <cc1:ModalPopupExtender ID="mdlPopUpTimeOut" BehaviorID="bmdlPopUpTimeOut" runat="server" 
    BackgroundCssClass="modelbackground" PopupControlID="pnlPopTimeOut" TargetControlID="btnok" OkControlID="btnClose" OnOkScript="CloseOk()">
    </cc1:ModalPopupExtender>
  