var app = angular.module('app', ['ngTable']);
app.controller('pageList', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Admin/PageList_Get').success(function (d) {
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
        if (confirm('是否删除：' + e.title)) {
            window.LayerOpen();
            $http.post('/Admin/Page_Delete', {
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
    $scope.Init();
});
app.controller('pageAdd', function ($scope, $http) {
    $scope.Init = function () {
        $scope.id = parseInt(window.GetUrlParam('id'));
        if ($scope.id == 0) {
            $scope.pageModel = {
                id: 0,
                title: null,
                content: null,
            };
        } else {
            window.LayerOpen();
            $http.post('/Admin/Page_Get', {
                id: $scope.id
            }).success(function (d) {
                $scope.pageModel = d;
                $('#summernote').summernote('code', d.content);
                window.LayerClose();
            }).error(function () {
                console.log('http错误');
                window.LayerClose();
            });
        }
    };
    $scope.Save = function () {
        window.LayerOpen();
        $scope.pageModel.content = $('#summernote').summernote('code');
        $http.post('/Admin/Page_Add_Edit', $scope.pageModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                self.location.href = '/Admin/PageList';
            } else {
                alert('保存失败');
                window.LayerClose();
            }
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $('#summernote').summernote({
        lang: 'zh-CN',
        minHeight: '60%',
        callbacks: {
            onInit: function () {
                $scope.Init();
            },
            onImageUpload: function (files) {
                window.EditorImageUpload(files);
            }
        }
    });
});