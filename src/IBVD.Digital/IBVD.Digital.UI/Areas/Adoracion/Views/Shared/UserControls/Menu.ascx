<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<link type="text/css" href="/Content/menu.css" rel="stylesheet" />
<script type="text/javascript" src="/scripts/menu.js"></script>
<div id="menu">
     <ul class="menu">
       <% foreach (var item in IBVD.Digital.UI.Areas.Adoracion.Helpers.UIConfigurationHelper.GetMenu().Items)
       {
           if (item.HasTreePermission())
            Response.Write(item.GetHTML());
       } %>
     </ul>
</div>

<a style="display:none;" href="http://apycom.com"></a>