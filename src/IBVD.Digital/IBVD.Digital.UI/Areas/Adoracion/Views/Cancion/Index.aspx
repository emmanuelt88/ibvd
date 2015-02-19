<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IBVD.Digital.UI.Models.CancionViewData>" %>

<%@ Import Namespace="MVCSecrets.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% 
        StringBuilder comentarioDatos = new StringBuilder();

        comentarioDatos.Append("Utilice la siguiente grilla para la consulta y administraci&oacute;n de las canciones<br/>");
        comentarioDatos.Append("<i>Coloque el mouse sobre la celda de la letra para ver un popup con el texto</i>");
        Html.RenderPartial("UserControls/CommentPanel", comentarioDatos.ToString());

        string urlToCreate = Url.Action<CancionController>(m => m.Create());
        string urlDetailsUC = Url.Action<CancionController>(m => m.DetailsUC(null));
    %>
    <div>
        <div class="gridContainer">
            <table id="listaCanciones" class="scroll" cellpadding="0" cellspacing="5">
            </table>
            <div id="pager" class="scroll" style="text-align: center;">
            </div>
        </div>
        <div id="divDetalle" class="Detalle" style="display:none;"></div>
        <div class="PanelBotonera Detalle" style="display:none;">
        <hr />
        <input type="button" class="button" 
            value="Editar la canci&oacute;n" onclick="editarCancionActual();" />
        <input type="button" class="button" onclick="mostrarListado();"
            value="Ir al listado de canciones" />
          </div>
        <div id="ToolTips">
        </div>
        <script type="text/javascript">

	    $(document).ready(function() {
	        $("#listaCanciones").jqGrid({
	            url: '/Cancion/GetCanciones',
	            datatype: 'json',
	            mtype: 'GET',
	            sortable: true,
	            colNames: ['Id', 'Titulo', 'Tono', 'Compas','Letra','Duracion Estimada', 'Acciones'],
	            colModel: [
                                  { name: 'Id', index: 'Id', width: 20, align : 'right', searchoptions: { sopt: numberFilter, dataInit: onlyInputInt}  },
                                  { name: 'Titulo', index: 'Titulo' ,align : 'center', searchoptions: { sopt: textFilter} },
                                  { name: 'Tono', index: 'Tono',align : 'center', width: 30, searchoptions: { sopt: comboFilter, dataUrl: '<%= Url.Action("GetTonoComboHTML","Cancion") %>'}  },
                                  { name: 'Compas', index: 'Compas',align : 'center', width:30, searchoptions: { sopt: comboFilter, dataUrl: '<%= Url.Action("GetCompasComboHTML","Cancion") %>'}  },
                                  {title:false, name: 'Letra',sortable : false, title:false,index: 'Letra' , width:20,  searchoptions: { sopt: textFilter}, align: 'center' },
                                  { name: 'Duracion Estimada', index: 'DuracionEstimada', width: 40, align : 'center', searchoptions: { sopt: numberFilter, dataInit: onlyInputInt}  },
                                  {title:false, name: 'Acciones', index: 'Acciones', sortable : false, width:80 ,search:false}
                              ],
	            pager: '#pager',
	            loadtext: 'Cargando...',
	            rowNum: 10,
	            rowList: [5, 10, 20, 50],
	            sortname: 'Titulo',
	            sortorder: "asc",
	            viewrecords: true,
	            caption: "&nbsp;",
	            forceFit: true,
	            autowidth: true,
	            height: '100%',
	            forceFit: true,
	            autowidth: true,
	            loadComplete: function(){
	             $("#listaCanciones .CancionLetraTooltip:hidden").each(function () {
	                var item = $("<div class='CoolToolTipPopup'/>")
	                item.attr('id',$(this).attr('title'));
	                item.html($(this).val());
	                
	                $("#ToolTips").append(item);
	                
	                $(this).parent().CoolToolTip();
	                 
	             });
	             
	             }
	         
	            
	             
	        });
	        
	        $("#listaCanciones").navGrid('#pager',
                            {
                                edit: false, add: false, del: false, search: true, refresh: true
                            },
                            {}, // edit options
                            {}, // add options
                            {}, //del options
                            {multipleSearch: true, closeAfterSearch: true, closeOnEscape:true} // search options
                        )
                          <%
                            if(Model.ShowCrearButton){
                         %>
                         .navSeparatorAdd('#pager', { sepclass: "ui-separator", sepcontent: '' })
                        .navButtonAdd('#pager', {
                            caption: "Crear nueva",
                            title: "Nueva canción",
                            buttonicon: "ui-icon-document",
                            onClickButton: function() {
                            location.href = "<%=  urlToCreate %>"  
                            },
                            position: "last"
                        })   <% } %>
                        ;
                        
                       
	    
	    });
	    
	    function editarCancionActual(){
	        var id = $("#IdCancion").val();
	        
	        location.href = "/Cancion/Edit?Cancion_IdCancion=" + id;
	    }

        function mostrarListado(){
            setTituloPagina("Lista de canciones");
             $(".Detalle").hide();
            $(".gridContainer").fadeIn(1000);
        }
        function verCancion(id){
            setTituloPagina("Detalle de la canción");
            blockDIV("#divDetalle");
            $(".gridContainer").hide();
            $(".Detalle").fadeIn(1000);
           
            
            $("#divDetalle").load('<%= urlDetailsUC %>?Cancion_IdCancion=' + id,
            function(resultData){
                
            });
        }
	    function eliminarCancion(id) {
	        mostrarConfirmacionPopup("¿Desea eliminar la canción?",
	        function() { // Si confirma
	            blockUIWaiting();

	            $.getJSON("Delete",
	            { Cancion_IdCancion: id },
	            function(resultData) {
	            $.unblockUI();
	                
	                handleResultData(resultData, function() {
                        if(resultData.Success)
	                        $("#listaCanciones").trigger("reloadGrid");
	                });
	            });

	        },
	        function() { // Sino
	        });
	    }
	    
        </script>
        </div>
</asp:Content>
