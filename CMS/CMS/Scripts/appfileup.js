var $url = "";
var $acceptedFiles = "";
var $dictFileTooBig = "";
var $maxFilesize = "50";
var $removeUrl = "";

var myDropzone;
function createFileUp(type) {
    Dropzone.options.myAwesomeDropzone = {
        url: $url +"/"+ type,
        maxFilesize: $maxFilesize,
        maxFiles: 1,
        //默认false。如果设为true，则会给文件添加上传取消和删除预览图片的链接  
        addRemoveLinks: true,
        //指明允许上传的文件类型，格式是逗号分隔的 MIME type 或者扩展名。例如：image/*,application/pdf,.psd,.obj  
        //acceptedFiles: "image/*",
        acceptedFiles: $acceptedFiles,
        //指明是否允许 Dropzone 一次提交多个文件。默认为false。如果设为true，则相当于 HTML 表单添加multiple属性。  
        uploadMultiple: true,
        //关闭自动上传功能，默认会true会自动上传  
        //也就是添加一张图片向服务器发送一次请求  
        autoProcessQueue: true,
        //每次上传的最多文件数，经测试默认为2，  
        //记得修改web.config 限制上传文件大小的节  
        parallelUploads: 100,
        dictDefaultMessage: "拖拽文件或者点击",
        dictFileTooBig: $dictFileTooBig,
        dictRemoveFile: "移除",
        dictCancelUpload: "取消上传",
        dictResponseError: "文件上传失败!",
        dictMaxFilesExceeded: "您一次最多只能上传{{maxFiles}}个文件",
        dictInvalidFileType: "您上传的文件类型不正确",
        dictCancelUploadConfirmation: "你确定要取消上传吗?",
        dictFileTooBig: "文件过大({{filesize}}MB). 上传文件最大支持: {{maxFilesize}}MB.",
        init: function () {

            myDropzone = this;
            this.on("sendingmultiple", function () {
            });
            this.on("successmultiple", function (files, response) {
                $("#hidden_filename").val(files[0].name);
            });
            this.on("errormultiple", function (files, response) {
            });
            this.on("removedfile", function (files, response) {
                $.post($removeUrl + "/" + type + "?filename=" + escape(files.name));
            });
        }
    }
}