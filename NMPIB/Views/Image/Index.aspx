<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/nmp.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input type="hidden" id="URL" value="/photos" />
    <input type="hidden" id="pageIndex" value="" />
    <input type="hidden" id="Search" value="" />
    <input type="hidden" id="Status" value="" />
    <div class="inputDiv" style="height: 25px; text-align: center">
      
        <input type="text" id="txtSearch" value="" />
      <%= Html.DropDownList("ddlSearchby", (SelectList)ViewData["SearchOptions"], new { @style="width: 120px" })%>
        <input type="button" value="search" id="btnSearch" />
    </div>
    <div class="titleBox" style="text-align: center;">
    </div>
  
    <div id="mainSearch">
    </div>
    <div class="titleBox" style="text-align: center;">
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
 <link href="<%=Url.Content("~/Content/Pager.css")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var urlPath = rtrim('<%=ResolveUrl("~/")%>', "/");
        var SearchString = "";
        var pageIndex = 1;
        var sortOrder = "";
        var contentObject = {
            id: "",
            detailurl: "",
            thumbsrc: "",
            previewsrc: "",
            name: "",
            description: "",
            magazine: "",
            shootdate : null
        };

        $(function() {
            events();

            var xOffset = 10;
            var yOffset = 30;

            //Get All Query Variables
            if ($.jqURL.get('pageindex') > 1)
                pageIndex = $.jqURL.get('pageindex');

            if ($.jqURL.get('search') != "" && $.jqURL.get('search') != null)
                SearchString = $.jqURL.get('search');

            if ($.jqURL.get('status') != "" && $.jqURL.get('status') != null)
                SearchStatus = $.jqURL.get('status');

            if ($.jqURL.get('sord') != "" && $.jqURL.get('sord') != null)
                sortOrder = $.jqURL.get('sord');

            //fill the search textbox
            $("#txtSearch").val(SearchString);
            $("#ddlSearchby").val(sortOrder);
            SearchService();

        });

        function events() {
            $("#btnSearch").click(function() {
                pageIndex = 1;
                SearchService();
            });

            $("#txtSearch").keydown(function(event) {
                if (event.keyCode == 13) {
                    pageIndex = 1;
                    SearchService();
                }
            });
        }

        function SearchService() {
            $("#mainSearch").empty();
            $('.titleBox').empty();
            var xOffset = 10;
            var yOffset = 30;
            sortOrder = $("#ddlSearchby").val();
            SearchString = $("#txtSearch").val();
            $.ajax(
            {
                type: 'GET',
                url: urlPath + '/Image/GetImages',
                contentType: "application/json; charset=utf-8",
                data: { pIndex: pageIndex, SearchString: SearchString, PageSize: 15, sord: sortOrder },
                dataType: 'json',
                success:
                function(Json) {
                    $('.titleBox').append(pageNumbers(Json.pageIndex, Json.totalPages));
                    $("#ddlPager").change(function() {
                        debugger;
                        gotoPage($(this).val());
                    });
                    jQuery.each(Json.ImageList, function(i, val) {
                        var newcontent = Object(contentObject);
                        newcontent.id = val.id;
                        newcontent.detailurl = urlPath + "/Image/Details/" + val.id + "?pageindex=" + pageIndex + "&search=" + SearchString + "&sord=" + sortOrder;
                        newcontent.thumbsrc = urlPath + "/photos/thumb/" + val.thumb_location;
                        newcontent.previewsrc = urlPath + "/photos/preview/" + val.preview_location;
                        newcontent.name = val.name;
                        newcontent.description = val.description;
                        newcontent.magazine = val.magazine;
                        newcontent.shootdate = val.shootdate;
                        var testcontent = content(newcontent);
                        jQuery('#mainSearch').append(testcontent);
                    }
              );
                    



                    $("a.preview").hover(function(e) {
                        var px = e.pageX - xOffset;
                        var py = e.pageY + yOffset;
                        if (e.pageX > 900)
                            px = e.pageX - xOffset - 300;
                        this.t = this.title;
                        this.title = "";
                        var c = (this.t != "") ? "<br/>" + this.t : "";
                        $("body").append("<p id='preview'><img src='" + this.id + "' alt='Image preview' />" + c + "</p>");
                        $("#preview").css("top", (py) + "px").css("left", (px) + "px").fadeIn("fast");
                    }, function() { this.title = this.t; $("#preview").remove(); });
                    $("a.preview").mousemove(function(e) {
                        var px = e.pageX - xOffset;
                        var py = e.pageY + yOffset;
                        if (e.pageX > 900)
                            px = e.pageX - xOffset - 300;
                        $("#preview").css("top", (py) + "px").css("left", (px) + "px");
                    });
                }
            });
        }

        function gotoPage(page) {
            pageIndex = page;
            SearchService();
        }

        function pageNumbers(pageIndex, totalPages) {

          
            var content = '<div class="inputDiv">';

            content += '<a title="first" href="#" onclick="gotoPage(1)" ><img class="ico_first" src="'+urlPath+'/content/blank.gif"  complete="complete"/></a>';
            var prevpage = 1;
            var nextpage = 1;
            if (pageIndex == 1) prevpage = 1; else prevpage = pageIndex - 1;
            content += '<a title="prev" onclick="gotoPage(' + prevpage + ')" href="#"><img class="ico_prev" src="'+ urlPath+'/content/blank.gif" complete="complete"/></a>';
            content += '<span>Page ' + pageIndex + ' of ' + totalPages + '</span>';
            if (pageIndex == totalPages) nextpage = totalPages; else nextpage = pageIndex + 1;
            content += '<a title="next" onclick="gotoPage(' + nextpage + ')" href="#"  ><img class="ico_next" src="' + urlPath + '/content/blank.gif"  complete="complete"/></a>';
            content += '<a title="last" onclick="gotoPage(' + totalPages + ')" href="#" ><img class="ico_last" src="' + urlPath + '/content/blank.gif"  complete="complete"/></a>';
            return content;
        }

        function changePager(e) {

            var currentPage = pageIndex;
            gotoPage(currentPage);
        }

        function content(contentdetail) {
            return '<div  style="border: 1px solid #E6E6E6; float:left; padding:2px" id="imageTableDisplayItem_' + contentdetail.id + '">' +
       '<div style="height: 110px; width: 110px;">' +
            '<div style="float:left;">' +
                    '<a href="' + contentdetail.detailurl + '" id="' + contentdetail.previewsrc + '" alt="" title="' + contentdetail.description + '" class="preview">' +
                        '<img id="imageThumb_' + contentdetail.id + '"  src="' + contentdetail.thumbsrc + '"  />' +
                    '</a>' +
            '</div>' +
        '</div>' +
        '<a  href="' + contentdetail.detailurl + '">#' + contentdetail.id + '</a>' +
        '<br/>' +
        '<span style="font-size:7pt">' + contentdetail.magazine + "</span>" +
        '<br/>'+
        '<span style="font-size:7pt">' + contentdetail.description + "</span>" +
        '<br/>' +
        '<span style="font-size:7pt">' + getCalendarDate(contentdetail.shootdate) + "</span>" +
        '</div>';
        }


    </script>
</asp:Content>
