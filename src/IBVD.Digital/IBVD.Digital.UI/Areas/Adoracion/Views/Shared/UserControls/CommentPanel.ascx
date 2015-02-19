<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.String>" %>
<div class="ui-state-highlight ui-corner-all ui-widget Comment" style="margin-top: 2px;padding: 5px;">
    <div class="Tabla" style="margin:0px;padding:0px;">
        <div class="Fila" style="margin:0px;padding:0px;">
            <div class="Celda" style="vertical-align: top; width: 10px;margin:0px;padding:0px;">
                <span class="ui-icon ui-icon-info" style="margin-right: .3em;"></span>
            </div>
            <div class="Celda" style="vertical-align: top;margin:0px;padding:0px;">
                <p style="vertical-align: top;margin:0px;padding:0px;">
                    <%= Model %>
                </p>
            </div>
        </div>
    </div>
</div>
