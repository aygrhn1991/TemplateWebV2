var app = angular.module('app', ['ngTable']);
app.controller('info', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Member/Info_Get').success(function (d) {
            $scope.infoModel = d;
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Save = function () {
        window.LayerOpen();
        $http.post('/Member/Info_Add_Edit', $scope.infoModel).success(function (d) {
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
app.controller('password', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.passwordModel = {
            password: null,
            confirmPassword: null,
        };
    };
    $scope.Save = function () {
        if ($scope.passwordModel.confirmPassword != $scope.passwordModel.password) {
            alert('两次密码不一致');
            return;
        }
        window.LayerOpen();
        $http.post('/Member/Password_Add_Edit', {
            password: $scope.passwordModel.password,
        }).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $scope.Init();
                window.LayerClose();
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