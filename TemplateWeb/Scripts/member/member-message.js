var app = angular.module('app', ['ngTable']);
app.controller('messageList', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Member/MessageList_Get').success(function (d) {
            $scope.data = d;
            $scope.dt = new NgTableParams({
                count: 10,
            }, {
                    counts: [10, 20, 50],
                    dataset: d,
                });
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Delete = function (e) {
        if (confirm('是否删除：' + e.id)) {
            window.LayerOpen();
            $http.post('/Member/Message_Delete', {
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
                window.LayerClose();
            });
        }
    };
    $scope.SetRead = function (e) {
        if (e.state_read == false) {
            window.LayerOpen();
            e.state_read = !e.state_read;
            $http.post('/Member/Message_Add_Edit', e).success(function (d) {
                if (d == true) {
                    window.LayerClose();
                    $scope.LoadData();
                    $scope.tempMessage = e;
                    $('#message-read').modal('show');
                } else {
                    alert('保存失败');
                    window.LayerClose();
                }
            }).error(function () {
                console.log('http错误');
                window.LayerClose();
            });
        } else {
            $scope.tempMessage = e;
            $('#message-read').modal('show');
        }
    };
    $scope.Init();
});