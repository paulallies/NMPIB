<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/nmp.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Test
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

       <div class="titleBox">
        <div class="inputDiv">
            <h2>Add Image</h2>
        </div>
    </div>
    <div id="SilverlightControlHost">
        <object data="data:application/x-silverlight," type="application/x-silverlight-2"
        width="100%" height="500">
        <param name="source" value="/ClientBin/ImageBrowser.xap" />
        <param name="onerror" value="onSilverlightError" />
        <param name="background" value="white" />
        <a href="http://go.microsoft.com/fwlink/?LinkId=149156" style="text-decoration:none">
        To use this page, Microsoft Silverlight to be installed
        <br />
        <center>
                <img src="/content/images/install-silverlight-316x127.png" alt="Get Silverlight" text="Get Silverlight" />
        </center>

        
        </a>
        
        </object>
    
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
