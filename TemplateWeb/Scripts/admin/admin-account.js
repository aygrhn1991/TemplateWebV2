var app = angular.module('app', ['ngTable']);
app.controller('login', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.account = {
            phone: null,
            password: null,
        };
    };
    $scope.Login = function () {
        window.LayerOpen();
        $http.post('/Admin/Login', {
            phone: $scope.account.phone,
            password: $scope.account.password,
        }).success(function (d) {
            if (d == true) {
                window.location.href = '/Admin/Index';
            } else {
                alert(d);
            }
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Init();
});
