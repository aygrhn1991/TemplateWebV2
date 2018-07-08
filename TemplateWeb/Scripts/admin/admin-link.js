var app = angular.module('app', ['ngTable']);
app.controller('linkList', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.sortType = {
            up: 'up',
            down: 'down'
        };
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Admin/LinkList_Get').success(function (d) {
            $scope.data = d;
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Save = function (e) {
        window.LayerOpen();
        $http.post('/Admin/Link_Add_Edit', e).success(function (d) {
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
        if (confirm('是否删除：' + e.title)) {
            window.LayerOpen();
            $http.post('/Admin/Link_Delete', {
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
        $http.post('/Admin/Link_Add_Edit', e).success(function (d) {
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
        $http.post('/Admin/Link_Sort', {
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
app.controller('linkAdd', function ($scope, $http) {
    $scope.Init = function () {
        $scope.linkModel = {
            id: 0,
            title: null,
            enable: false,
            mode: 0,
            url: null,
            page_id: null,
            sort: null,
        };
    };
    $scope.Save = function () {
        window.LayerOpen();
        $http.post('/Admin/Link_Add_Edit', $scope.linkModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $('#link-add').modal('hide');
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
app.controller('linkContent', function ($scope, $http) {
    $scope.Init = function () {
        $scope.modeType = [
            { key: 0, value: 'url外链' },
            { key: 1, value: '单页' },
            { key: 2, value: '子链接' },
        ];
        $scope.id = parseInt(window.GetUrlParam('id'));
        window.LayerOpen();
        $http.post('/Admin/Link_Get', {
            id: $scope.id
        }).success(function (d) {
            $scope.linkModel = d;
            $scope.PageLoad();
            $scope.SublinkLoad();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.SetMode = function (e) {
        $scope.linkModel.mode = e.key;
    }
    $scope.PageLoad = function () {
        window.LayerOpen();
        $http.post('/Admin/PageList_Get').success(function (d) {
            $scope.pageList = d;
            $scope.linkModelPageTitle = '暂未选择单页';
            d.forEach(function (e) {
                if (e.id == $scope.linkModel.page_id) {
                    $scope.linkModelPageTitle = e.title;
                }
            });
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.SetPage = function (e) {
        $scope.linkModel.page_id = e.id;
        $scope.linkModelPageTitle = e.title;
    };
    $scope.SublinkLoad = function () {
        window.LayerOpen();
        $http.post('/Admin/SublinkList_Get', {
            id: $scope.id,
        }).success(function (d) {
            $scope.sublinkList = d;
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Save = function () {
        window.LayerOpen();
        $http.post('/Admin/Link_Add_Edit', $scope.linkModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                self.location.href = '/Admin/LinkList';
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
app.controller('sublinkList', function ($scope, $http) {
    $scope.Init = function () {
        $scope.sortType = {
            up: 'up',
            down: 'down'
        };
    };
    $scope.Save = function (e) {
        window.LayerOpen();
        $http.post('/Admin/Sublink_Add_Edit', e).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $scope.SublinkLoad();
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
        if (confirm('是否删除：' + e.title)) {
            window.LayerOpen();
            $http.post('/Admin/Sublink_Delete', {
                id: e.id
            }).success(function (d) {
                if (d == true) {
                    alert('删除成功');
                    $scope.SublinkLoad();
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
    $scope.SetEnable = function (e) {
        window.LayerOpen();
        e.enable = !e.enable;
        $http.post('/Admin/Sublink_Add_Edit', e).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $scope.SublinkLoad();
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
        $http.post('/Admin/Sublink_Sort', {
            id: e.id,
            sortType: sortType
        }).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $scope.SublinkLoad();
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
app.controller('sublinkAdd', function ($scope, $http) {
    $scope.Init = function () {
        $scope.sublinkModel = {
            id: 0,
            link_id: parseInt(window.GetUrlParam('id')),
            title: null,
            enable: false,
            mode: 0,
            sort: null,
            page_id: null,
            url: null,
        };
    };
    $scope.Save = function () {
        window.LayerOpen();
        $http.post('/Admin/Sublink_Add_Edit', $scope.sublinkModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $('#sublink-add').modal('hide');
                $scope.SublinkLoad();
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
app.controller('sublinkContent', function ($scope, $http) {
    $scope.Init = function () {
        $scope.modeType = [
            { key: 0, value: 'url外链' },
            { key: 1, value: '单页' },
        ];
        $scope.id = parseInt(window.GetUrlParam('id'));
        window.LayerOpen();
        $http.post('/Admin/Sublink_Get', {
            id: $scope.id
        }).success(function (d) {
            $scope.sublinkModel = d;
            $scope.PageLoad();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.SetMode = function (e) {
        $scope.sublinkModel.mode = e.key;
    }
    $scope.PageLoad = function () {
        window.LayerOpen();
        $http.post('/Admin/PageList_Get').success(function (d) {
            $scope.pageList = d;
            $scope.sublinkModelPageTitle = '暂未选择单页';
            d.forEach(function (e) {
                if (e.id == $scope.sublinkModel.page_id) {
                    $scope.sublinkModelPageTitle = e.title;
                }
            });
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.SetPage = function (e) {
        $scope.sublinkModel.page_id = e.id;
        $scope.sublinkModelPageTitle = e.title;
    };
    $scope.Save = function (e) {
        window.LayerOpen();
        $http.post('/Admin/Sublink_Add_Edit', $scope.sublinkModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                self.location.href = '/Admin/LinkContent?id=' + $scope.sublinkModel.link_id;
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