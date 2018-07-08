window.GetUrlParam = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)
        return unescape(r[2]);
    return null;
}
window.EditorImageUpload = function (files) {
    var formData = new FormData();
    for (var i = 0; i < files.length; i++) {
        formData.append("files", files[i]);
    }
    $.ajax({
        data: formData,
        type: "POST",
        url: "/Plugin/summernote/handler/UploadHandler.ashx",
        cache: false,
        contentType: false,
        processData: false,
        success: function (d) {
            d.forEach(function (e) {
                $("#summernote").summernote('insertImage', e, e);
            });
        }
    });
};
layerIndex = null;
window.LayerOpen = function () {
    layerIndex = layer.load(1, { shade: [0.1, '#000'] });
};
window.LayerClose = function () {
    layer.close(layerIndex);
};