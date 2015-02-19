<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    $(document).ready(function() {
        blockDIV("#pnlBuscadorSimple");
        $("#pnlBuscadorSimple").load(
        '<%= Url.Action("GetBuscadorUC","Biblia") %>',
        function(resultData) {
            unBlockDIV("#pnlBuscadorSimple");
        });

        // Acordion de buscadores
        var icons = {
            header: "ui-icon-circle-arrow-e",
            headerSelected: "ui-icon-circle-arrow-s"
        };
        $("#accordionBuscadoresVersiculo").accordion({
            icons: icons,
            autoHeight: false ,
        });

    });
</script>

<div id="accordionBuscadoresVersiculo">
    <h3>
        <a href="#">Selecci&oacute;n del pasaje</a></h3>
    <div style="padding: 5px 20px 5px 20px;">
        <div class="Tabla" style="width: 100%;">
            <div class="Fila">
                <div class="Celda">
                    <div id="pnlBuscadorSimple" style="height: 80px; width: 100%;">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <h3>
        <a href="#">Consulta avanzada</a></h3>
    <div>
        <div class="Tabla" style="width: 100%; height:100%;">
            <div class="Fila">
                <div class="Celda">
                    <div id="pnlBuscadorAvanzado" style="width: 100%;">
                        <% Html.RenderPartial("UserControls/BusquedaAvanzada"); %>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
