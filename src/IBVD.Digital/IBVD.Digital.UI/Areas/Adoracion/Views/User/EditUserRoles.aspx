<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IBVD.Digital.UI.Models.UserViewData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% 
        Html.RenderPartial("UserControls/CommentPanel",
            string.Format("Modifique los roles del usuario <strong>{0}</strong> con la selecci&oacute;n en la lista de los roles", Model.User.UserName));
    %>
    <% using (Html.BeginForm("SaveUserRoles", "User", new { userName = Model.User.UserName }, FormMethod.Post, new { id = "frmSabeUserRoles" }))
       {%>
    <input type="hidden" name="Usuario_UserName" value="<%= Model.User.UserName %>" />
    <fieldset>
        <legend>Roles</legend>
        <span id="pnlRoles">
        <%foreach (var item in Model.Roles)
          {
              Response.Write("<p>");
              var roles = Model.User.RolesList.ToList();
              string isChecked = roles.Exists(m => m.Id == item.Id) ? "checked" : string.Empty;

              Response.Write(string.Format("<input type='checkbox' class='hand' value='{0}' {1} id='role{0}' ></input><label for='role{0}' class='hand'>{2}</label>", item.Id, isChecked, item.Nombre));
              Response.Write("</p>");
          } %>
          </span>
    </fieldset>
    <fieldset>
        <legend>Operaciones</legend>
        <div>
        <% Html.RenderPartial("~/Views/Role/Operaciones.ascx", Model.GetTreeItems()); %>
        </div>
    </fieldset>
    <input type="hidden" value="" id="ListaRoles" name="ListaRoles" />
    <div class="PanelBotonera">
        <hr />
        <input type="button" value="Guardar" onclick="guardarRoles();" />
        <%= Html.ActionButton<UserController>("Volver al listado", m => m.Index()) %>
    </div>
    <% } %>

    <script type="text/javascript">

        $(document).ready(function() {

            updateItemsChecked();

            $(":checkbox").click(function() {

                updateItemsChecked();

            });

        });
        function updateItemsChecked(resultFunction) {
            updateOperacionesItemsChecked(function() {
            var itemsChecked = $("#pnlRoles :checked");
                var checkedList = '';
                $.each(itemsChecked,
                function(i) {
                    checkedList = checkedList + $(this).attr('value') + ',';

                }
            );

                if (checkedList.length > 0) {
                    checkedList = checkedList.substring(0, checkedList.length - 1);
                }

                $('#ListaRoles').val(checkedList);
                try {
                    resultFunction();
                }
                catch (e) {
                }
            });
        }

        function guardarRoles() {
            blockUIWaiting();
            updateItemsChecked(function() {
                $.post("SaveUserRoles",
	            $("#frmSabeUserRoles").serialize(),
	            function(resultData) {
	                $.unblockUI();
	                handleResultData(resultData, function() {
	                    if (resultData.Success) {
	                        document.location.href = resultData.Action;
	                    }
	                });
	            },
	            'json');
            });

        }
		
    </script>

</asp:Content>
