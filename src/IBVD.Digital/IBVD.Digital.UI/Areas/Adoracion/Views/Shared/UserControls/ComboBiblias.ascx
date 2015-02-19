<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    $(document).ready(function() {
        $.getJSON('<%= Url.Action("GetBibliasBasico","Biblia") %>',
    null,
    function(resultData) {
        var items = eval('(' + resultData.Data + ');');
        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            var nuevo = $("<option></option>");
            nuevo.val(item.Codigo);
            nuevo.attr('title', item.Nombre);
            nuevo.html(item.Nombre);
          
            $("select.ComboBiblias").append(nuevo);
        }
        $("select.ComboBiblias").show();
    });

    });
</script>

<select class="ComboBiblias" style="display:none;">
</select>

