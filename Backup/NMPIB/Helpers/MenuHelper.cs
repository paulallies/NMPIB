using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace NMPIB.Helpers
{
    public static class MenuHelper
    {
        private static void LoopBranch(SiteMapNodeCollection nodeCollection, ref StringBuilder sb, ref HtmlHelper helper)
        {
            foreach (SiteMapNode node in nodeCollection)
            {

                sb.AppendLine("<li>");

                if (SiteMap.CurrentNode == node)
                {
                    if (node.ChildNodes.Count > 0)
                        sb.AppendFormat("<a class='selectedMenuItem' href='{0}'>{1}</a>", node.Url, helper.Encode(node.Title));
                    else
                        sb.AppendFormat("<a class='selectedMenuItem' href='{0}'>{1}</a>", node.Url, helper.Encode(node.Title));
                }
                else
                {
                    if (node.ChildNodes.Count > 0)
                        sb.AppendFormat("<a href='{0}'>{1}</a>", node.Url, helper.Encode(node.Title));
                    else
                        sb.AppendFormat("<a href='{0}'>{1}</a>", node.Url, helper.Encode(node.Title));

                }

                if (node.ChildNodes.Count > 0)
                {
                    sb.Append("<ul>");
                    LoopBranch(node.ChildNodes, ref sb, ref helper);
                    sb.Append("</ul>");
                }
                sb.AppendLine("</li>");




            }
        }


        public static string Menu(this HtmlHelper helper)
        {
            var sb = new StringBuilder();

            //Create opening unordered list tag 
            var topLevelNodes = SiteMap.RootNode.ChildNodes;

            sb.Append("<div class='NavContainer'>");

            LoopBranch2(topLevelNodes, ref sb, ref helper);

            sb.Append("</div>");

            return sb.ToString();

        }

        public static void LoopBranch2(SiteMapNodeCollection nodeCollection, ref StringBuilder sb, ref HtmlHelper helper)
        {
            sb.Append("<div id='TopNavigation'>"); //Open TopNavigation
            sb.Append("<div id='topNavLeftBorder'>"); //Open topNavLeftBorder
            sb.Append("<img alt='left' src='/content/images/topnav/topnav_left.png' />");
            sb.Append("</div>");//Close topNavLeftBorder
            sb.Append("<div class='topNavCenter'>");
            int count = 1;
            foreach (SiteMapNode node in nodeCollection)
            {
                bool nodeDisplayStatus = false;
                foreach (var role in node.Roles)
                {
                    if (role.Equals("*"))
                    {
                        nodeDisplayStatus = true;
                        break;
                    }

                    if (HttpContext.Current.User.IsInRole(role.ToString()))
                    {
                        nodeDisplayStatus = true;
                        break;
                    }
                }
                if (HttpContext.Current.User.Identity.IsAuthenticated && nodeDisplayStatus)
                {
                    sb.AppendFormat("<div id='topnav_" + count.ToString() + "' class='topnav_item' onclick=\"goToLink('{0}');\">", node.Url);

                    if (SiteMap.CurrentNode == node)
                    {

                        sb.Append("<div class='topnav_item_left_selected' ></div>");
                        sb.Append("<div class='topnav_item_middle_selected'>");//Open middle
                        sb.AppendFormat("<div class='topnav_item_text'><a href='{0}'>{1}</a></div>", node.Url, node.Title);
                        sb.Append("</div>");//close middle
                        sb.Append("<div class='topnav_item_right_selected'></div>");
                    }
                    else
                    {
                        sb.Append("<div class='topnav_item_left' ></div>");
                        sb.Append("<div class='topnav_item_middle'>");
                        sb.AppendFormat("<div class='topnav_item_text'><a href='{0}'>{1}</a></div>", node.Url, node.Title);
                        sb.Append("</div>");
                        sb.Append("<div class='topnav_item_right'></div>");
                    }
                    sb.Append("</div>");//Close topnav_
                    sb.Append("<div class='topnav_item_divider'></div>");


                }
                count++;
            }
            sb.Append("</div>");//close topNavCenter

            sb.Append("<div id='topNavRightBorder'></div>");

            sb.Append("</div>");//close topNavigation

            sb.Append("<div class='Subnavigation'>");//Open Subnavigation
            sb.Append("<div id='SubNav_Leftcap'><img alt='left' src='/content/images/hero_topshadow_left.png'/></div>");
            sb.Append("<div class='SubNavItems'>");
            count = 1;
            foreach (SiteMapNode node in nodeCollection)
            {


                if (node.HasChildNodes)
                {
                    bool subnodeDisplayStatus = false;
                    foreach (var role in node.Roles)
                    {
                        if (role.Equals("*"))
                        {
                            subnodeDisplayStatus = true;
                            break;
                        }

                        if (HttpContext.Current.User.IsInRole(role.ToString()))
                        {
                            subnodeDisplayStatus = true;
                            break;
                        }
                    }
                    if (HttpContext.Current.User.Identity.IsAuthenticated && subnodeDisplayStatus)
                    {

                        sb.Append("<div id='subnav_" + count.ToString() + "' class='subnav_item'>");
                        int branchcount = 1;
                        foreach (SiteMapNode cnode in node.ChildNodes)
                        {
                            sb.AppendFormat("<div id='subnavItem" + count.ToString() + "_" + branchcount.ToString() + "' class='newSubNnav_item' onclick=\"goToLink('{0}')\">", cnode.Url);
                            if (SiteMap.CurrentNode == cnode)
                            {
                                sb.Append("<div class='newSubNnav_item_left_selected'></div>");
                                sb.Append("<div class='newSubNnav_item_middle_selected'>");//Open Middle
                                sb.AppendFormat("<div class='newSubNnav_item_text'><a id='A3' href='{0}'>{1}</a></div>", cnode.Url, cnode.Title);
                                sb.Append("</div>");//Close Middle
                                sb.Append("<div class='newSubNnav_item_right_selected'></div>");
                            }
                            else
                            {
                                sb.Append("<div class='newSubNnav_item_left'></div>");
                                sb.Append("<div class='newSubNnav_item_middle'>");
                                sb.AppendFormat("<div class='newSubNnav_item_text'><a id='A3' href='{0}'>{1}</a></div>", cnode.Url, cnode.Title);
                                sb.Append("</div>");
                                sb.Append("<div class='newSubNnav_item_right'></div>");
                            }
                            sb.Append("</div>");//Close subnavItem
                            sb.Append("<div class='subnav_item_divider'></div>");
                            branchcount++;
                        }
                        sb.Append("</div>");//Close subnav_

                    }

                }
                count++;
            }
            sb.Append("</div>");//Close SubNavItems
            sb.Append("<div id='SubNav_Rightcap'><img alt='left' src='/content/images/hero_topshadow_right.png'/></div>");
            sb.Append("</div>");//Close Subnavigation

        }
    }
}
