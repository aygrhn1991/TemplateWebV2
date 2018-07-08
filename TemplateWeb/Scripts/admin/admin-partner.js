var app = angular.module('app', ['ngTable']);
app.controller('partnerList', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.sortType = {
            up: 'up',
            down: 'down'
        };
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Admin/PartnerList_Get').success(function (d) {
            $scope.data = d;
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Save = function (e) {
        window.LayerOpen();
        $http.post('/Admin/Partner_Add_Edit', e).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $scope.LoadData();
            } else {
                alert('保存失败');
                window.LayerClose();
            }
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Delete = function (e) {
        if (confirm('是否删除：' + e.title)) {
            window.LayerOpen();
            $http.post('/Admin/Partner_Delete', {
                id: e.id
            }).success(function (d) {
                if (d == true) {
                    alert('删除成功');
                    $scope.LoadData();
                } else {
                    alert('删除失败');
                    window.LayerClose();
                }
            }).error(function () {
                console.log('http错误');
            });
        }
    };
    $scope.SetEnable = function (e) {
        window.LayerOpen();
        e.enable = !e.enable;
        $http.post('/Admin/Partner_Add_Edit', e).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $scope.LoadData();
            } else {
                alert('保存失败');
                window.LayerClose();
            }
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.SetSort = function (e, sortType) {
        window.LayerOpen();
        $http.post('/Admin/Partner_Sort', {
            id: e.id,
            sortType: sortType
        }).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $scope.LoadData();
            } else {
                alert('保存失败');
                window.LayerClose();
            }
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Init();
});
app.controller('partnerAdd', function ($scope, $http) {
    $('#easyContainer').easyUpload({
        allowFileTypes: '*.jpg;*.png;*.gif;',
        note: '提示：支持格式为：jpg、png、gif',
        url: '/Plugin/easyupload/handler/UploadHandler.ashx',
        formParam: { type: 'partner' },
        multi: false,
        successFunc: function (res) {
            $scope.partnerModel.path = res.imgUrl;
        },
        errorFunc: function (res) {
            alert('文件上传失败');
        },
    });
    $scope.Init = function () {
        $scope.partnerModel = {
            id: 0,
            title: null,
            enable: false,
            url: null,
            sort: null,
            path: null,
        };
    };
    $scope.Save = function () {
        window.LayerOpen();
        $http.post('/Admin/Partner_Add_Edit', $scope.partnerModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                self.location.href = '/Admin/PartnerList';
            } else {
                alert('保存失败');
                window.LayerClose();
            }
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Init();
});