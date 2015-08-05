angular.module("umbraco")
    .controller("TheDevDashboard.Controller",
    function ($scope, $http) {

        $http.get('/umbraco/backoffice/api/TheDevDashboard/GetViewModel').
            success(function (data, status, headers, config) {
                $scope.vm = data;
            }).
            error(function (data, status, headers, config) {
              
            });
    });
