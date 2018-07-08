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
        $http.post('/Member/Login', {
            phone: $scope.account.phone,
            password: $scope.account.password,
        }).success(function (d) {
            if (d == true) {
                var redirectUrl = GetUrlParam('redirectUrl');
                if (redirectUrl == null || redirectUrl == '' || redirectUrl == undefined) {
                    window.location.href = '/Member/Index';
                } else {
                    window.location.href = redirectUrl;
                }
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
app.controller('register', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.account = {
            phone: null,
            password: null,
            confirmPassword: null,
            code: null,
        };
    };
    var countDown = 60;
    function CountDown() {
        var btn = $('#codeBtn');
        if (countDown == 0) {
            btn.attr("disabled", false);
            btn.text("获取验证码");
            countDown = 60;
            return;
        } else {
            btn.attr("disabled", true);
            btn.text("(" + countDown + ") s 重新发送");
            countDown--;
        }
        setTimeout(function () { CountDown() }, 1000);
    }
    $scope.SendCode = function () {
        if ($scope.account.phone == null || $scope.account.phone.length != 11) {
            alert('请输入正确的手机号码');
            return;
        }
        $http.post('/Member/SendSMSCode', {
            phone: $scope.account.phone,
        }).success(function (d) {
            if (d == true) {
                CountDown();
            } else {
                alert(d);
            }
        }).error(function () {
            console.log('http错误');
        });
    };
    $scope.Register = function () {
        if ($scope.account.phone == null || $scope.account.phone.length != 11 ||
            $scope.account.password == null || $scope.account.phone.password == '' ||
            $scope.account.code == null || $scope.account.code.length != 4) {
            alert('请填写正确的信息');
            return;
        }
        if ($scope.account.confirmPassword != $scope.account.password) {
            alert('两次密码不一致');
            return;
        }
        window.LayerOpen();
        $http.post('/Member/Register', {
            phone: $scope.account.phone,
            password: $scope.account.password,
            code: $scope.account.code,
        }).success(function (d) {
            if (d == true) {
                window.location.href = '/Member/Index';
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
