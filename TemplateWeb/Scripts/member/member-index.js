var app = angular.module('app', []);
app.controller('layout', function ($scope, $http) {
    $scope.Init = function () {
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        $http.post('/Home/Layout_Get').success(function (d) {
            $scope.param = d;
        }).error(function () {
            console.log('http错误');
        });
    };
    $scope.Init();
});