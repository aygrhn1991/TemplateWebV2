var app = angular.module('app', ['ngTable']);
app.controller('noticeList', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.sortType = {
            up: 'up',
            down: 'down'
        };
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Admin/NoticeList_Get').success(function (d) {
            $scope.data = d;
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Save = function (e) {
        window.LayerOpen();
        $http.post('/Admin/Notice_Add_Edit', e).success(function (d) {
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
        if (confirm('是否删除：' + e.id)) {
            window.LayerOpen();
            $http.post('/Admin/Notice_Delete', {
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
        $http.post('/Admin/Notice_Add_Edit', e).success(function (d) {
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
        $http.post('/Admin/Notice_Sort', {
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
app.controller('noticeAdd', function ($scope, $http) {
    $scope.Init = function () {
        $scope.noticeModel = {
            id: 0,
            content: null,
        };
    };
    $scope.Save = function () {
        window.LayerOpen();
        $http.post('/Admin/Notice_Add_Edit', $scope.noticeModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $('#notice-add').modal('hide');
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