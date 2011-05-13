<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/nmp.Master" Inherits="System.Web.Mvc.ViewPage<NMPIB.Models.tbl_Image>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Details
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="titleBox">
        <div class="inputDiv">
            <h2>Image Details</h2>
        </div>
    </div>
    <span style="color: Red">
        <%=ViewData["Error"] %></span>
    <div class="titleBox">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="vertical-align: top">
                    <div class="inputDiv" style="width: 550px;">
                        <ul>
                            <li>
                                <label for="magazine_id">Magazine:</label>
                                <%= Html.Encode(Model.tbl_publication.name) %>
                            </li>
                            <li>
                                <label for="magazine_issue">Magazine Issue:</label>
                                <%= Html.Encode(Model.magazine_issue) %>
                            </li>
                            <li>
                                <label for="shoot_description">Shoot Description:</label>
                                <%= Html.Encode(Model.shoot_description) %>
                            </li>
                            <li>
                                <label for="shoot_date">Shoot Date:</label>
                                <%= Html.Encode(Model.shoot_date.Value.ToString("dd-MMM-yyyy")) %>
                            </li>
                            <li>
                                <label for="description">Description:</label>
                                <%= Html.Encode(Model.description) %>
                            </li>
                            <li>
                                <label for="keywords">Keywords:</label>
                                <%= Html.Encode(Model.keywords) %>
                            </li>
                            <li>
                                <label for="photographer">Uploaded By:</label>
                                <%try
                                  { %>
                                <%= Html.Encode(Model.tbl_user.UserName)%>
                                <%}
                                  catch { } %>
                            </li>
                            <li>
                                <label for="imagename">Image Name:</label>
                                <%= Html.Encode(Model.name) %>
                            </li>
                        </ul>
                        <%string MySubject = "Image Order: " + Model.name; %>
                        <%StringBuilder MyBody = new StringBuilder();%>
                        <%MyBody.Append("%0D%0A%0D%0A%0D%0A%0D%0A%0D%0A "); %>
                        <%MyBody.Append("Image Details%0D%0AImage ID: " + Model.id.ToString()); %>
                        <%MyBody.Append("%0D%0AMagazine: " + Model.tbl_publication.name); %>
                        <%MyBody.Append("%0D%0AMagazine Issue: " + Model.magazine_issue); %>
                        <%MyBody.Append("%0D%0AShoot Description: " + Model.shoot_description); %>
                        <%MyBody.Append("%0D%0AShoot Date: " + Model.shoot_date.Value.ToString("dd-MMM-yyyy")); %>
                        <%MyBody.Append("%0D%0ADescription: " + Model.description); %>
                        <%MyBody.Append("%0D%0AName: " + Model.name); %>
                        <div class="orderImage">
                            <a href="mailto:muneer.manie@newmediapub.co.za?subject=<%=Html.Encode(MySubject) %>&Body=<%=Html.Encode(MyBody) %>">
                                Order Image</a>
                        </div>
                    </div>
                </td>
                <td style="vertical-align: top">
                    <div class="inputDiv" style="width: 392px; height: 100%">
                        <center>
                            <img src="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/photos/preview/<%= Html.Encode(Model.preview_location) %>"
                                alt="" title="" />
                        </center>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <%if (User.IsInRole("PhotographyUser")) %>
        <%{ %>
        <a href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Image/Delete/<%= Html.Encode(Model.id) %>">Delete</a> | 
        <a id="aEdit" href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Image/Edit/<%= Html.Encode(Model.id) %>">Edit</a> |
        <%} %>
        <a id="aBackToList" href="<%=HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/Image/index">Back to List</a> |
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
        var urlPath = rtrim('<%=ResolveUrl("~/")%>', "/");
        var SearchString = "";
        var SearchStatus = false;
        var pageIndex = 1;
        var sortOrder = "";
        $(function() {

            if ($.jqURL.get('pageindex') > 1)
                pageIndex = $.jqURL.get('pageindex');

            if ($.jqURL.get('search') != "" && $.jqURL.get('search') != null)
                SearchString = $.jqURL.get('search');

            if ($.jqURL.get('status') != "" && $.jqURL.get('status') != null)
                SearchStatus = $.jqURL.get('status');

            if ($.jqURL.get('sord') != "" && $.jqURL.get('sord') != null)
                sortOrder = $.jqURL.get('sord');

            $("#aBackToList").attr("href", urlPath + "/Image/index?pageindex=" + pageIndex + "&search=" + SearchString + "&sord=" + sortOrder);
            var editattr = $("#aEdit").attr("href");
           
            $("#aEdit").attr("href", editattr + "?pageindex=" + pageIndex + "&search=" + SearchString + "&sord=" + sortOrder);


        });
            


    </script>

</asp:Content>
