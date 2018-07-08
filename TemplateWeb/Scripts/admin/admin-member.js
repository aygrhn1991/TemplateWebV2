var app = angular.module('app', ['ngTable']);
app.controller('memberList', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Admin/MemberList_Get').success(function (d) {
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
    $scope.SetEnable = function (e) {
        window.LayerOpen();
        e.enable = !e.enable;
        $http.post('/Admin/Member_Add_Edit', e).success(function (d) {
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
    $scope.SetData = function (e) {
        $scope.tempMember = e;
        $('#member-read').modal('show');

    };
    $scope.Reset = function (e) {
        if (confirm('是否重置该会员密码为：1')) {
            window.LayerOpen();
            $http.post('/Admin/Member_Reset', {
                id: e.id
            }).success(function (d) {
                if (d == true) {
                    alert('重置成功');
                    $scope.LoadData();
                } else {
                    alert('重置失败');
                    window.LayerClose();
                }
            }).error(function () {
                console.log('http错误');
                window.LayerClose();
            });
        }
    };
    $scope.Init();
});