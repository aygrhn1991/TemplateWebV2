var app = angular.module('app', ['ngTable']);
app.controller('employTypeList', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Admin/EmployTypeList_Get').success(function (d) {
            $scope.data = d;
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Save = function (e) {
        window.LayerOpen();
        $http.post('/Admin/EmployType_Add_Edit', e).success(function (d) {
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
        if (confirm('是否删除：' + e.name)) {
            window.LayerOpen();
            $http.post('/Admin/EmployType_Delete', {
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
    $scope.Init();
});
app.controller('employTypeAdd', function ($scope, $http) {
    $scope.Init = function () {
        $scope.employTypeModel = {
            id: 0,
            name: null,
        };
    };
    $scope.Save = function () {
        window.LayerOpen();
        $http.post('/Admin/EmployType_Add_Edit', $scope.employTypeModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $('#employ-add').modal('hide');
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
app.controller('employAdd', function ($scope, $http) {
    $scope.Init = function () {
        window.LayerOpen();
        $http.post('/Admin/EmployTypeList_Get').success(function (d) {
            $scope.employType = d;
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
        $scope.id = parseInt(window.GetUrlParam('id'));
        if ($scope.id == 0) {
            $scope.employModel = {
                id: 0,
                type_id: 0,
                position_name: null,
                salary: null,
                education: null,
                experience: null,
                work_place: null,
                employ_number: null,
                position_description_1: null,
                position_description_2: null,
                position_description_3: null,
                position_description_4: null,
                position_requirement_1: null,
                position_requirement_2: null,
                position_requirement_3: null,
                position_requirement_4: null,
                benefit: null,
                remark: null,
            };
        } else {
            window.LayerOpen();
            $http.post('/Admin/Employ_Get', {
                id: $scope.id
            }).success(function (d) {
                $scope.employModel = d;
                window.LayerClose();
            }).error(function () {
                console.log('http错误');
                window.LayerClose();
            });
        }
    };
    $scope.Save = function () {
        window.LayerOpen();
        $http.post('/Admin/Employ_Add_Edit', $scope.employModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                self.location.href = '/Admin/EmployList';
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
app.controller('employList', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Admin/EmployList_Get').success(function (d) {
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
        if (confirm('是否删除：' + e.name)) {
            window.LayerOpen();
            $http.post('/Admin/Employ_Delete', {
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
    $scope.Init();
});