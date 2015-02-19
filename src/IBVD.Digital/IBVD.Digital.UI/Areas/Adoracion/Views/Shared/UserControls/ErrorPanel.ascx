<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (!ViewData.ModelState.IsValid) {%>
<div class="ui-widget">
    <div class="ui-state-error ui-corner-all error" style="padding: 0 .7em;text-align:left;">
        <p>
            <span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>
            <strong>Alerta:</strong> 
                <%= Html.ValidationSummary(ViewData.ContainsKey(IBVD.Digital.UI.Models.UICommonKeys.ERROR_TITLE)?ViewData[IBVD.Digital.UI.Models.UICommonKeys.ERROR_TITLE].ToString():string.Empty)%>
                    
                <% if (ViewData.ContainsKey(IBVD.Digital.UI.Models.UICommonKeys.ERROR_MESSAGE))
                   {%>
                   <%= ViewData[IBVD.Digital.UI.Models.UICommonKeys.ERROR_MESSAGE].ToString() %>
                <%} %>
            </p>
            
            
    </div>
</div>
<%} %>
<div id="errorPanelAncla"></div>
<div id="errorPanelTemplate" style="display:none;">
<div class="ui-widget">
    <div class="ui-state-error ui-corner-all error" style="padding: 0 .7em;text-align:left;">
        <span class="ui-icon ui-icon-alert" style="float: left; margin-right: .3em;"></span>
        <strong>Alerta: <span id="errorMessage" style="margin:0px; padding:0px;"></span></strong> 
           
            <ul id="errorMessages">
            </ul>     
            
    </div>
</div>
</div>

<script type="text/javascript" language="javascript">
    function mostrarErrores(messages) {
        var messageUL = $("#errorPanelTemplate ul");
        messageUL.html('');
        for (var i = 0; i < messages.length;  i++) {
            messageUL.append($("<li>" + messages[i] + "</li>"));
        }

        mostrarPanelErrores();

    }

    function mostrarErrores(messages, error) {
        var messageUL = $("#errorPanelTemplate ul");
        var message = $("#errorMessage");
        message.html(error);
        messageUL.html('');
        for (var i = 0; i < messages.length; i++) {
            messageUL.append($("<li>" + messages[i] + "</li>"));
        }

        mostrarPanelErrores();

    }

    function mostrarError(error) {
        var message = $("#errorMessage");

        message.html(error);

        
    }

    function mostrarPanelErrores() {
        if ($("#errorPanelTemplate").css('display') == 'none') {
            $("#errorPanelTemplate").toggle('blind', {}, 400);
        }

        scrollBodyToElement("errorPanelTemplate");
    }
    function ocultarPanelErrores() {
        $("#errorPanelTemplate").hide();
    }

    function manejarErrores(resultData) {
        if (resultData.ShowPopup) {
            mostrarErrorPopup(resultData.Error, null);
        } else {
            mostrarErrores(resultData.Errores, resultData.Error);
        }

    
    }
</script>