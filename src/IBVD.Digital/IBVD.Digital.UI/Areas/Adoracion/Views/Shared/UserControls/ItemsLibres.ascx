<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">

    function agregarItemLibre() {
        blockUIWaiting();
        $.post('<%= Url.Action("CrearItemLibre","Reunion") %>',
        { titulo: $("#txtTituloItemLibre").val(),
            detalle: $("#txtDetalleItemLibre").val()
        },
        function(resultData) {
            $.unblockUI();

            if (resultData.Success) {
                itemLibreAgregado($("#txtTituloItemLibre").val(), $("#txtDetalleItemLibre").val(), resultData.Data.cacheKEY);

                $("#txtTituloItemLibre").val('');
                $("#txtDetalleItemLibre").val('');
            }
            else {
                handleResultData(resultData, function() {
                   
                });
            }
        },
        'json'
        );
    }
</script>

<fieldset class="formulario">
    <legend>Items libres</legend>
    <p>
        <label for="txtTituloItemLibre">T&iacute;tulo:</label>
    <input type="text" id="txtTituloItemLibre"  size="150" maxlength="200"/><em>*</em>
    </p>
    <p>
    <label for="txtDetalleItemLibre">Detalle:</label>
    <textarea cols="40" rows="5" style="width: 400px;"  id="txtDetalleItemLibre"></textarea>
    </p>
        
    <div class="PanelBotonera">
        <input type="button" id="btnAgregarItemLibre" onclick="agregarItemLibre();" value="Agregar"/>
    </div>
    
</fieldset>