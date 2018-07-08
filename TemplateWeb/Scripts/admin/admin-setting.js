var app = angular.module('app', ['ngTable']);
app.controller('setting', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.fileType = {
            logo: 'logo',
            favicon: 'favicon',
            qr: 'qr'
        };
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Admin/Setting_Get').success(function (d) {
            $scope.data = d;
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Upload = function (e) {
        var file = $('#' + e).get(0).files[0];
        if ('undefined' == typeof (file) || file == null || file == '') {
            alert('请选择一张图片');
            return;
        }
        if (e == $scope.fileType.logo && !/\.(gif|jpg|png|GIF|JPG|PNG)$/.test(file.name)) {
            alert('文件格式不支持');
            return;
        }
        if (e == $scope.fileType.favicon && !/\.(ico)$/.test(file.name)) {
            alert('请上传.ico格式文件');
            return;
        }
        if (e == $scope.fileType.qr && !/\.(gif|jpg|png|GIF|JPG|PNG)$/.test(file.name)) {
            alert('文件格式不支持');
            return;
        }
        window.LayerOpen();
        var formData = new FormData();
        formData.append('file', file);
        formData.append('key', e);
        $.ajax({
            url: '/Admin/Setting_Upload',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                $scope.LoadData();
            },
            error: function () {
                console.log('http错误');
                window.LayerClose();
            }
        })
    };
    $scope.Save = function (e, f) {
        window.LayerOpen();
        $http.post('/Admin/Setting_Save', {
            key: e,
            value: f
        }).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $scope.LoadData();
            } else {
                alert('保存失败');
                window.LayerClose();
            }
        }).error(function (e) {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Init();
});