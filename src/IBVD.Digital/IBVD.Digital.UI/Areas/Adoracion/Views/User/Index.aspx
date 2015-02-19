<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IBVD.Digital.UI.Models.UserViewData>" %>

<%@ Import Namespace= MVCSecrets.Helpers %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% 
        StringBuilder comentarioDatos = new StringBuilder();

        comentarioDatos.Append("Utilice la siguiente grilla para la consulta y administraci&oacute;n de los usuarios<br/>");
        Html.RenderPartial("UserControls/CommentPanel", comentarioDatos.ToString());
    %>
    <div>
	  <div class="gridContainer">
        <table id="listaUsuarios" class="scroll" cellpadding="0" cellspacing="5">
        </table>
        <div id="pager" class="scroll" style="text-align: center;">
        </div>
    </div>
    
	<script type="text/javascript">

	    $(document).ready(function() {
	        $("#listaUsuarios").jqGrid({
	            url: '/User/GetUsers',
	            datatype: 'json',
	            mtype: 'GET',
	            sortable: true,
	            colNames: ['UserName','Foto', 'Email', 'Estado', 'Acciones'],
	            colModel: [
                              { name: 'UserName', index: 'UserName', width: 40 , align:'center' , searchoptions: { sopt: textFilter}},
                              { title:false,name: 'Foto', index: 'Foto', sortable: false, width: 15, align: 'center',search:false },
                              { name: 'Email', index: 'Email', align:'center'  , searchoptions: { sopt: textFilter}},
                              { name: 'Estado', index: 'Estado', hidden:true, sortable: false, width: 40, align: 'left' },
                              { title:false,name: 'Acciones', index: 'Acciones', sortable: false, width: 120, align: 'center',search:false }
                          ],
	            pager: '#pager',
	            loadtext: 'Cargando...',
	            rowNum: 10,
	            rowList: [5, 10, 20, 50],
	            sortname: 'UserName',
	            sortorder: "asc",
	            viewrecords: true,
	            caption: "&nbsp;",
	            forceFit: true,
	            autowidth: true,
	            height: '100%',
	            forceFit: true,
	            autowidth: true
	        });

	        $("#listaUsuarios").navGrid('#pager',
                            {
                                edit: false, add: false, del: false, search: true, refresh: true
                            },
                            {}, // edit options
                            {}, // add options
                            {}, //del options
                            {multipleSearch: true, closeAfterSearch: true, closeOnEscape: true} // search options
                        )
                        <%
                            if(Model.ShowCrearButton){
                         %>
                        .navSeparatorAdd('#pager', { sepclass: "ui-separator", sepcontent: '' })
                        .navButtonAdd('#pager', {
                            caption: "Crear nuevo",
                            title: "Nuevo usuario",
                            buttonicon: "ui-icon-document",
                            onClickButton: function() {
                                location.href = "<%=   Url.Action<UserController>(m=> m.Create()) %>"
                            },
                            position: "last"
                        })
                          .navButtonAdd('#pager', {
                            caption: "Recordar datos",
                            title: "Recordar datos a los usuarios",
                            buttonicon: "ui-icon-mail-open",
                            onClickButton: function() {
                                  $.post("RecordarTodos",
	                                    { },
	                                    function(resultData) {
                        	            
	                                        handleResultData(resultData, function() {
                        	                
	                                        });
	                                    },'json');
                            },
                            position: "last"
                        })
                        <% } %>
                        ;

	    });


	    

	    function habilitarUsuario(userName) {
	        mostrarConfirmacionPopup("¿Desea habilitar el usuario?",
	        function() { // Si confirma
	            blockUIWaiting();

	            $.post("HabilitarUsuario",
	            { Usuario_UserName: userName },
	            function(resultData) {
	                handleResultData(resultData, function() {
	                $.unblockUI();
	                $("#listaUsuarios").trigger("reloadGrid");
	                });
	            },'json');

	            
	        },
	        function() { // Sino
	        });
	        
	    }

	    function deshabilitarUsuario(userName) {
	        mostrarConfirmacionPopup("¿Desea deshabilitar el usuario?",
	        function() { // Si confirma
	            blockUIWaiting();

	            $.post("DeshabilitarUsuario",
	            { Usuario_UserName: userName },
	            function(resultData) {
	                handleResultData(resultData, function() {
	                $.unblockUI();
	                $("#listaUsuarios").trigger("reloadGrid");
	                });
	            },'json');

	        },
	        function() { // Sino
	        });
	    }
	    
	    function recordarPassword(userName){
	            $.post("RecordarPassword",
	            { Usuario_UserName: userName },
	            function(resultData) {
	            
	                handleResultData(resultData, function() {
	                
	                });
	            },'json');
	    }
	</script>
</div>


</asp:Content>
