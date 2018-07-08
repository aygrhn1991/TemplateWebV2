var app = angular.module('app', ['ngTable']);
app.controller('navList', function ($scope, $http, NgTableParams) {
    $scope.Init = function () {
        $scope.sortType = {
            up: 'up',
            down: 'down'
        };
        $scope.LoadData();
    };
    $scope.LoadData = function () {
        window.LayerOpen();
        $http.post('/Admin/NavList_Get').success(function (d) {
            $scope.data = d;
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Save = function (e) {
        window.LayerOpen();
        $http.post('/Admin/Nav_Add_Edit', e).success(function (d) {
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
            $http.post('/Admin/Nav_Delete', {
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
        $http.post('/Admin/Nav_Add_Edit', e).success(function (d) {
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
        $http.post('/Admin/Nav_Sort', {
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
app.controller('navAdd', function ($scope, $http) {
    $scope.Init = function () {
        $scope.navModel = {
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
        $http.post('/Admin/Nav_Add_Edit', $scope.navModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $('#nav-add').modal('hide');
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
app.controller('navContent', function ($scope, $http) {
    $scope.Init = function () {
        $scope.modeType = [
            { key: 0, value: 'url外链' },
            { key: 1, value: '单页' },
            { key: 2, value: '子导航' },
        ];
        $scope.id = parseInt(window.GetUrlParam('id'));
        window.LayerOpen();
        $http.post('/Admin/Nav_Get', {
            id: $scope.id
        }).success(function (d) {
            $scope.navModel = d;
            $scope.PageLoad();
            $scope.SubnavLoad();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.SetMode = function (e) {
        $scope.navModel.mode = e.key;
    }
    $scope.PageLoad = function () {
        window.LayerOpen();
        $http.post('/Admin/PageList_Get').success(function (d) {
            $scope.pageList = d;
            $scope.navModelPageTitle = '暂未选择单页';
            d.forEach(function (e) {
                if (e.id == $scope.navModel.page_id) {
                    $scope.navModelPageTitle = e.title;
                }
            });
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.SetPage = function (e) {
        $scope.navModel.page_id = e.id;
        $scope.navModelPageTitle = e.title;
    };
    $scope.SubnavLoad = function () {
        window.LayerOpen();
        $http.post('/Admin/SubnavList_Get', {
            id: $scope.id,
        }).success(function (d) {
            $scope.subnavList = d;
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.Save = function () {
        window.LayerOpen();
        $http.post('/Admin/Nav_Add_Edit', $scope.navModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                self.location.href = '/Admin/NavList';
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
app.controller('subnavList', function ($scope, $http) {
    $scope.Init = function () {
        $scope.sortType = {
            up: 'up',
            down: 'down'
        };
    };
    $scope.Save = function (e) {
        window.LayerOpen();
        $http.post('/Admin/Subnav_Add_Edit', e).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $scope.SubnavLoad();
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
            $http.post('/Admin/Subnav_Delete', {
                id: e.id
            }).success(function (d) {
                if (d == true) {
                    alert('删除成功');
                    $scope.SubnavLoad();
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
        $http.post('/Admin/Subnav_Add_Edit', e).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $scope.SubnavLoad();
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
        $http.post('/Admin/Subnav_Sort', {
            id: e.id,
            sortType: sortType
        }).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $scope.SubnavLoad();
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
app.controller('subnavAdd', function ($scope, $http) {
    $scope.Init = function () {
        $scope.subnavModel = {
            id: 0,
            nav_id: parseInt(window.GetUrlParam('id')),
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
        $http.post('/Admin/Subnav_Add_Edit', $scope.subnavModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                $('#subnav-add').modal('hide');
                $scope.SubnavLoad();
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
app.controller('subnavContent', function ($scope, $http) {
    $scope.Init = function () {
        $scope.modeType = [
            { key: 0, value: 'url外链' },
            { key: 1, value: '单页' },
        ];
        $scope.id = parseInt(window.GetUrlParam('id'));
        window.LayerOpen();
        $http.post('/Admin/Subnav_Get', {
            id: $scope.id
        }).success(function (d) {
            $scope.subnavModel = d;
            $scope.PageLoad();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.SetMode = function (e) {
        $scope.subnavModel.mode = e.key;
    }
    $scope.PageLoad = function () {
        window.LayerOpen();
        $http.post('/Admin/PageList_Get').success(function (d) {
            $scope.pageList = d;
            $scope.subnavModelPageTitle = '暂未选择单页';
            d.forEach(function (e) {
                if (e.id == $scope.subnavModel.page_id) {
                    $scope.subnavModelPageTitle = e.title;
                }
            });
            window.LayerClose();
        }).error(function () {
            console.log('http错误');
            window.LayerClose();
        });
    };
    $scope.SetPage = function (e) {
        $scope.subnavModel.page_id = e.id;
        $scope.subnavModelPageTitle = e.title;
    };
    $scope.Save = function (e) {
        window.LayerOpen();
        $http.post('/Admin/Subnav_Add_Edit', $scope.subnavModel).success(function (d) {
            if (d == true) {
                alert('保存成功');
                self.location.href = '/Admin/NavContent?id=' + $scope.subnavModel.nav_id;
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