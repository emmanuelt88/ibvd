<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IDictionary<string, string>>" %>
<%
    string controlId = Model["ControlID"];
    string exploreText = Model.ContainsKey("ExploreText") ? Model["ExploreText"] : "Seleccionar";
    string guardarText = Model.ContainsKey("GuardarText") ? Model["GuardarText"] : "Guardar";
    string functionOnComplete = Model.ContainsKey("OnComplete") ? Model["OnComplete"] + "(fileObj, response, data);" : "";
    string filesType = Model.ContainsKey("FilesType") ? Model["FilesType"] : "*.*";
    string filesDescription = Model.ContainsKey("FilesDescription") ? Model["FilesDescription"] : "Todos los archivos";
    string auto = Model.ContainsKey("Auto") ? Model["Auto"] : "true";
    string displayUploadButton = auto.Equals("true")? "display:none;": string.Empty;
    string itemToDisableOnProgress = Model.ContainsKey("ItemsToDisableOnProgress") ? Model["ItemsToDisableOnProgress"] : string.Empty;
    string multiple = Model.ContainsKey("Multiple") ? Model["Multiple"] : "false";
    string labelSeleccionar = Model.ContainsKey("LabelSeleccionar") ? Model["LabelSeleccionar"] : "Seleccionar archivo";
    string functionOnAllComplete = Model.ContainsKey("OnAllComplete") ? Model["OnAllComplete"] + "(event,data);" : string.Empty;
    string token = Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value;
    
%>

<script type="text/javascript">


    $(document).ready(function() {
        $("#btn<%= controlId %>Guardar").disable();
        $('#<%= controlId %>').uploadify({
            fileExt : '<%= filesType %>',
            fileDesc: '<%= filesDescription %>',
            wmode: 'transparent',
            scriptData: { token: '<%= token %>' },
            'uploader': '/Scripts/uploadify.swf',
            'script': '/Upload/UploadFile',
            'folder': '/Files',
            fileDataName: 'FileData',
            buttonText: '<%= exploreText %>',
            hideButton: false,
            width: 32,
            height: 32,
            sizeLimit: 2097152,
            scriptAccess: 'always',
            auto : <%= auto %>,
            multi : <%= multiple %>,
            cancelImg: "/Content/Themes/base/images/iconos/ErrorCircle16.png",
            buttonImg : "/Content/Themes/base/images/iconos/explore32.png",
            onSelectOnce : function(event, data){
            },
            onComplete: function(event, queueID, fileObj, response, data) {
                <%= functionOnComplete %>
                
                if('<%= itemToDisableOnProgress %>' != '' && <%= auto %>){
                    $("#btn<%= controlId %>Guardar").disable();
                    $("<%= itemToDisableOnProgress %>").enable();
                }
            },
            onAllComplete:function(event, data){
            
                 $("#btn<%= controlId %>Guardar").disable();
                 $("<%= itemToDisableOnProgress %>").enable();
                 <%= functionOnAllComplete %>
                 
            }
            ,
            onProgress: function(event,queueID, fileObj,data){
            
            },
            onSelect: function(event,data){
                $("#btn<%= controlId %>Guardar").enable();
                
                if('<%= itemToDisableOnProgress %>' != '' && <%= auto %>){
                    $("<%= itemToDisableOnProgress %>").disable();
                     $("#btn<%= controlId %>Guardar").disable();
                }
            },
            onCancel: function(event, queueID, fileObj,data){
                if(data.fileCount == 0){
                    $("#btn<%= controlId %>Guardar").disable();
                    
                    if('<%= itemToDisableOnProgress %>' != '' && <%= auto %>){
                    $("<%= itemToDisableOnProgress %>").enable();
                }
                }
                
                
            },
            onError: function (a, b, c, d) {  
            
                 if (d.status == 404)  
                    mostrarErrorPopup("La direcci&oaacute;n del servidor es inv&aacute;lida",null);
                 else if (d.type === "HTTP")  
                   mostrarErrorPopup("Error "+d.type+": "+d.status,null);  
                 else if (d.type ==="File Size")  
                   mostrarErrorPopup("El archivo supera el tama&ntilde;o m&aacute;ximo por archivo. El l&iacute;mite es: 1.5MB",null);  
                 else  
                   mostrarErrorPopup("Error "+d.type+": "+d.text,null);  
                 }

        });


    });
    
    function comenzarDescarga_<%= controlId %>(){
    $('#btn<%= controlId %>Guardar').disable();
    $('#<%= controlId %>').uploadifyUpload();
    $("<%= itemToDisableOnProgress %>").disable();
    
    }
    
</script>

<fieldset>

<div style="display: table;">
    <div style="display: table-row;">
        <div style="display: table-cell; vertical-align: top; margin: 5px; padding: 5px;">
               &nbsp;&nbsp;
               
               <input type="button" style="<%=displayUploadButton %>" id="btn<%= controlId %>Guardar" onclick="comenzarDescarga_<%= controlId %>();" value="<%= guardarText %>"/>
                 &nbsp;&nbsp;&nbsp;<%= labelSeleccionar %>
           
        </div>
        <div style="display: table-cell;vertical-align:top;">
            
            <input type="file" id="<%= controlId %>" name="<%= controlId %>" style="display:none;" /> 
            
        </div>
        <div style="display: table-cell;vertical-align:top; width:300px;">
        <div style="overflow-y:auto; max-height:150px;" id="<%= controlId %>Container">
        </div>
        </div>
        
    </div>
</div>
</fieldset>