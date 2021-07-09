(function() {
    "use strict";

    function controller($http,userService, dateHelper) {
        
        
        /**
         * Processes dates into a nice format based on user culture.
         * @param {any} arrItemsToProcess
         * @param {any} callback
         */
        var processDates = function(arrItemsToProcess, callback) {

            userService.getCurrentUser().then(function (currentUser) {

                angular.forEach(arrItemsToProcess, function (item) {
                    item.datestampFormatted = dateHelper.getLocalDate(item.datestamp, currentUser.locale, 'LLL');

                    if (item.scheduledPublishDate != null) {
                        item.scheduledPublishDateFormatted = dateHelper.getLocalDate(item.datestamp, currentUser.locale, 'LLL');
                    }
                });

                callback();

            });

        }


        var vm = this;
        
        $http.get('backoffice/api/TheDashboard/GetAllRecentActivities').then(function(res) {

            processDates(res.data.allItems, function() {
                processDates(res.data.yourItems, function() {
                    vm.recentActivities = res.data;
                });
            });

        });

        
        $http.get('backoffice/api/TheDashboard/GetUnpublished').then(function(res) {

            processDates(res.data.items, function() {
                vm.unpublished = res.data;
            });

        });


        $http.get('backoffice/api/TheDashboard/GetCounters').then(function(res) {
            vm.counters = res.data;
        });
        
        vm.clickElement = function(elementSelector) {
            // Just clicks on the element provides as parameter.
            $(elementSelector).click();
        }


        return vm;
    }

    angular.module("umbraco").controller("Our.Umbraco.TheDashboard.Controller", ['$http','userService', 'dateHelper', controller]);
})();
