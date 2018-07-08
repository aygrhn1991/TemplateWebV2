var app = angular.module('app', ['ngTable']);
app.controller('bannerList', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.sortType = {
            up: 'up',
            down: 'down'
        };
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Admin/BannerList_Get').success(function (d) {
            $scope.data = d;
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Save = function (e) {
        window.LayerOpen();
        $http.post('/Admin/Banner_Add_Edit', e).success(function (d) {
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
            $http.post('/Admin/Banner_Delete', {
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
        $http.post('/Admin/Banner_Add_Edit', e).success(function (d) {
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
        $http.post('/Admin/Banner_Sort', {
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
app.controller('bannerAdd', function ($scope, $http) {
    $('#easyContainer').easyUpload({
        allowFileTypes: '*.jpg;*.png;*.gif;',
        note: '提示：支持格式为：jpg、png、gif',
        url: '/Plugin/easyupload/handler/UploadHandler.ashx',
        formParam: { type: 'banner' },
        multi: false,
        successFunc: function (res) {
            $scope.bannerModel.path = res.imgUrl;
        },
        errorFunc: function (res) {
            alert('文件上传失败');
        },
    });
    $scope.Init = function () {
        $scope.bannerModel = {
            id: 0,
            title: null,
            enable: false,
            mode: 0,
            url: null,
            page_id: null,
            sort: null,
            path: null,
        };
    };
    $scope.Save = function () {
        window.LayerOpen();
        $http.post('/Admin/Banner_Add_Edit', $scope.bannerModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                self.location.href = '/Admin/BannerList';
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
app.controller('bannerContent', function ($scope, $http) {
    $scope.Init = function () {
        $scope.modeType = [
            { key: 0, value: 'url外链' },
            { key: 1, value: '单页' },
        ];
        $scope.id = parseInt(window.GetUrlParam('id'));
        window.LayerOpen();
        $http.post('/Admin/Banner_Get', {
            id: $scope.id
        }).success(function (d) {
            $scope.bannerModel = d;
            $scope.PageLoad();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.SetMode = function (e) {
        $scope.bannerModel.mode = e.key;
    }
    $scope.PageLoad = function () {
        window.LayerOpen();
        $http.post('/Admin/PageList_Get').success(function (d) {
            $scope.pageList = d;
            $scope.bannerModelPageTitle = '暂未选择单页';
            d.forEach(function (e) {
                if (e.id == $scope.bannerModel.page_id) {
                    $scope.bannerModelPageTitle = e.title;
                }
            });
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.SetPage = function (e) {
        $scope.bannerModel.page_id = e.id;
        $scope.bannerModelPageTitle = e.title;
    };
    $scope.Save = function (e) {
        window.LayerOpen();
        $http.post('/Admin/Banner_Add_Edit', $scope.bannerModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                self.location.href = '/Admin/BannerList';
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