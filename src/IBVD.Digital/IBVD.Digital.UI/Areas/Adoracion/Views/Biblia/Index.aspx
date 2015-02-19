<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    
    var tooltip = null;
  
    $(document).ready(function() {
        $("#popupDetallePasaje").html($("#TemplateToolTipVersiculo").clone().html());

        
    });

    
    
    
    
    
    
    
    
    
    
    
    function pasajeBibliaEncontrado(keyCache, versiculos, biblia, libro, pasaje){
        $("#popupDetallePasaje").dialog('destroy');
        var texto = $("#popupDetallePasaje").find('.Versiculo');
        texto.html('');
        for (var i = 0; i < versiculos.length; i++) {
            var versiculo = versiculos[i];
            var templateVersiculo = $("#TemplateVersiculo").clone();
            templateVersiculo.find('.NroVersiculo').html(versiculo.Numero);
            templateVersiculo.find('.Texto').html(versiculo.Texto);

            texto.append(templateVersiculo.html());
        }

        $("#popupDetallePasaje").attr('title', libro + ' ' + pasaje + ' || ' + biblia.Nombre);
        $("#popupDetallePasaje").dialog({ autoOpen: true, modal: true, position: ['center', 'center'], width: 500 });

    }

    
</script>
<div id="popupDetallePasaje">
</div>

<% Html.RenderPartial("UserControls/BuscadorVersiculos"); %>

</asp:Content>
