<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>


<div style="text-align:right;width:100%;" class="OcultoInicio">
<%
    if (Request.IsAuthenticated) {
%>

<div >
        
        Bienvenido <b><%= Html.Encode(Page.User.Identity.Name) %></b>!
        [ <%= Html.ActionLink("Logout", "LogOff", "Account") %> ]
        
         || <%= Html.ActionLink("Modificar datos", "Edit", "User", new { Usuario_UserName = Page.User.Identity.Name },null)%>
</div>
        
        
     
        
<%
    }
    else {
%> 
        [ <%= Html.ActionLink("Login", "Login", "Account") %> ]
<%
    }
%>

</div>