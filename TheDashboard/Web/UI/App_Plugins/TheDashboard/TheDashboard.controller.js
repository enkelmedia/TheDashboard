angular.module("umbraco")
    .controller("TheDashboard.Controller",
    function ($scope, $http, notificationsService) {

        $http.get('/umbraco/backoffice/api/TheDashboard/GetViewModel').
            success(function (data, status, headers, config) {
                $scope.vm = data;
            }).
            error(function (data, status, headers, config) {
              
            });
    });
