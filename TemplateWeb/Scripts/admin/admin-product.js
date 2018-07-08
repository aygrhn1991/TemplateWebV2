var app = angular.module('app', ['ngTable']);
app.controller('productTypeList', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Admin/ProductTypeList_Get').success(function (d) {
            $scope.data = d;
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Save = function (e) {
        window.LayerOpen();
        $http.post('/Admin/ProductType_Add_Edit', e).success(function (d) {
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
            $http.post('/Admin/ProductType_Delete', {
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
app.controller('productTypeAdd', function ($scope, $http) {
    $scope.Init = function () {
        $scope.productTypeModel = {
            id: 0,
            name: null,
        };
    };
    $scope.Save = function () {
        window.LayerOpen();
        $http.post('/Admin/ProductType_Add_Edit', $scope.productTypeModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $('#product-add').modal('hide');
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
app.controller('productAdd', function ($scope, $http) {
    $scope.Init = function () {
        window.LayerOpen();
        $http.post('/Admin/ProductTypeList_Get').success(function (d) {
            $scope.productType = d;
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
        $scope.id = parseInt(window.GetUrlParam('id'));
        if ($scope.id == 0) {
            $scope.productModel = {
                id: 0,
                type_id: 0,
                name: null,
                path: null,
                description: null,
                content: null,
                top: false,
                price: null,
                delete: false,
            };
            $('#easyContainer').easyUpload({
                allowFileTypes: '*.jpg;*.png;*.gif;',
                note: '提示：支持格式为：jpg、png、gif',
                url: '/Plugin/easyupload/handler/UploadHandler.ashx',
                formParam: { type: 'product' },
                multi: false,
                successFunc: function (res) {
                    $scope.productModel.path = res.imgUrl;
                },
                errorFunc: function (res) {
                    alert('文件上传失败');
                },
            });
        } else {
            window.LayerOpen();
            $http.post('/Admin/Product_Get', {
                id: $scope.id
            }).success(function (d) {
                $scope.productModel = d;
                $('#summernote').summernote('code', d.content);
                $('#easyContainer').easyUpload({
                    allowFileTypes: '*.jpg;*.png;*.gif;',
                    note: '提示：支持格式为：jpg、png、gif',
                    url: '/Plugin/easyupload/handler/UploadHandler.ashx',
                    formParam: { type: 'product' },
                    multi: false,
                    successFunc: function (res) {
                        $scope.productModel.path = res.imgUrl;
                    },
                    errorFunc: function (res) {
                        alert('文件上传失败');
                    },
                });
                window.LayerClose();
            }).error(function () {
                console.log('http错误');
                window.LayerClose();
            });
        }
    };
    $scope.Save = function () {
        window.LayerOpen();
        $scope.productModel.content = $('#summernote').summernote('code');
        $http.post('/Admin/Product_Add_Edit', $scope.productModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                self.location.href = '/Admin/ProductList';
            } else {
                alert('保存失败');
                window.LayerClose();
            }
        }).error(function (e) {
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

    ///临时加参，也是因为上传插件bug，不得不这样先顶替着，找到好的插件再说
    $scope.uploadAtt = function () {
        var formData = new FormData();
        formData.append('type', 'product-attachment');
        formData.append('file', $('#att')[0].files[0]);
        $.ajax({
            url: '/Plugin/easyupload/handler/UploadHandler.ashx',
            type: 'post',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.code == 200) {
                    alert('附件上传成功');
                $scope.productModel.attachment = result.imgUrl;
                }
            },
        });
    };
});
app.controller('productList', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Admin/ProductList_Get').success(function (d) {
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
            $http.post('/Admin/Product_Delete', {
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
    $scope.SetTop = function (e) {
        window.LayerOpen();
        e.top = !e.top;
        $http.post('/Admin/Product_Add_Edit', e).success(function (d) {
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