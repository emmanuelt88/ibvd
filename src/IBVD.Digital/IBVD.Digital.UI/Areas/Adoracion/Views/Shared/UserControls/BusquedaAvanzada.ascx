<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% 
        StringBuilder comentarioDatos = new StringBuilder();

        comentarioDatos.Append("Utilice la siguiente grilla para la consulta avanzada de los vers&iacute;culos<br/>");
        Html.RenderPartial("UserControls/CommentPanel", comentarioDatos.ToString());
    %>
<div style="width:100%;">
        <div class="gridContainer">
            <table id="listaVersiculos" class="scroll" cellpadding="0" cellspacing="5" style="max-height:500px;"> 
            </table>
            <div id="pager" class="scroll" style="text-align: center;">
            </div>
        </div>

        <script type="text/javascript">
         $(document).ready(function() {
	        $("#listaVersiculos").jqGrid({
	            url: '<%= Url.Action("GetVersiculos", "Biblia") %>',
	            datatype: 'json',
	            mtype: 'GET',
	            sortable: true,
	            colNames: ['Biblia', 'Libro', 'Capítulo', 'Versículo','Texto'],
	            colModel: [
                                  { name: 'Biblia', index: 'CodigoBiblia', width: 70, align: 'center', searchoptions: { sopt: comboFilter, dataUrl: '<%= Url.Action("GetBibliasComboHTML","Biblia") %>'} },
                                  { name: 'Libro', index: 'Versiculo.Libro', width: 150, align: 'center', searchoptions: { sopt: comboFilter, dataUrl: '<%= Url.Action("GetLibrosComboHTML","Biblia") %>'} },
                                  { name: 'Capítulo', index: 'Capitulo', width: 60, align: 'center', searchoptions: { sopt: numberFilter, dataInit: onlyInputInt} },
                                  { name: 'Versículo', index: 'Versiculo', width: 60, align: 'center', searchoptions: { sopt: numberFilter, dataInit: onlyInputInt} },
                                  { name: 'Texto', index: 'Texto', width: 550, align: 'left', searchoptions: { sopt: textFilter, dataInit: onlyNormalize} },
                              ],
	            pager: '#pager',
	            loadtext: 'Cargando...',
	            rowNum: 10,
	            rowList: [5, 10, 20, 50],
	            sortname: 'CodigoBiblia',
	            sortorder: "asc",
	            viewrecords: true,
	            caption: "&nbsp;",
	            forceFit: true,
	            autowidth: true,
	            height: '100%',
	            forceFit: true,
	            autowidth: true
	        });
	        
	        $("#listaVersiculos").navGrid('#pager',
                            {
                                edit: false, add: false, del: false, search: true, refresh: true
                            },
                            {}, // edit options
                            {}, // add options
                            {}, //del options
                            {multipleSearch: true, closeAfterSearch: true, closeOnEscape:true} // search options
                        );
	    });

	    function anularReunion(id) {
	        mostrarConfirmacionPopup("¿Desea anular la reunión?",
	         function() { // Si confirma
	            blockUIWaiting();

                  $.getJSON('<%= Url.Action("Anular", "Reunion") %>', 
                  {Reunion_IdReunion : id},
	            function(resultData){
    	            $.unblockUI();
	                handleResultData(resultData, function() {
                        if(resultData.Success)
	                        $("#listaVersiculos").trigger("reloadGrid");
	                });
	            });

	        },
	        function() { // Sino
	        });
	      
	    }
        </script>

    </div>