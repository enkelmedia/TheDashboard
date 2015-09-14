app.requires.push('ngTable');
angular.module("umbraco").controller("TheUserDashboard.Controller",
    function($scope, $http,activityAPI, ngTableParams) {
		$scope.logItems = [];
		activityAPI.getAvailableDates().success(function (response) {
			$scope.dates = response;
			$scope.selectedDate = response[0];
		});
		
		/*
		* Handles when the user selects a log date from the dropdown.
		* Calls out to the  API to get the logs for the given date. Updates
		* the counts and sets the table data for the new logs
		*/
		$scope.searchActivities = function () {
			$scope.actionInProgress = true;
			activityAPI.getAllData($scope.selectedDate).success(function (response) {
				$scope.logItems = response.usersActivitiesLog;

				$scope.tableParams.total($scope.logItems.length);
				$scope.tableParams.reload();
				$scope.actionInProgress = false;
			});
		};
		
		$scope.onAdminFilterClick = function (filterText) {
			$scope.selectedFilter = filterText;
			$scope.tableParams.filter().userTypeAlias = filterText;
		};
		/**
        $scope.text = '';
		
        $scope.searchActivities = function() {
            $scope.actionInProgress = true;
            if ($scope.text) {
                var dataPost = JSON.stringify("text: " + $scope.text);
                console.log(dataPost);
                $http.post('/umbraco/backoffice/api/TheUserDashboard/SearchActivities', dataPost)
                    .then(function(data, status, headers, config) {
                        $scope.text = '';
                        $scope.vm = data.data;
                        $scope.actionInProgress = false;
                    });
            }
        };**/
		$scope.tableParams = new ngTableParams({
			page: 1,            // show first page
			count: 100           // count per page
		}, {
			total: 0,
			getData: function ($defer, params) {
				//Do we have logItems yet?
				if($scope.logItems.length == 0){
					//initial log
					activityAPI.getAllData('1987-05-10').success(function (response) {
						var data = response.usersActivitiesLog || [];
						params.total(data.length);
						$defer.resolve(data.slice((params.page() - 1) * params.count(), params.page() * params.count()));
					});
					
				}else{
					var data = $scope.logItems || [];
					params.total(data.length);
					$defer.resolve(data.slice((params.page() - 1) * params.count(), params.page() * params.count()));
				}
			}
		});
});
angular.module("umbraco.resources").factory("activityAPI", function ($http) {
    return {
        getAllData: function(date)
        {
			if(date == '' || date == null){
				date = '1987-05-09';
			}
            return $http.get("/umbraco/backoffice/api/TheUserDashboard/GetActivitiesByDate?date=" + encodeURIComponent(date));
        },

        getAvailableDates: function()
        {
            return $http.get("/umbraco/backoffice/api/TheUserDashboard/GetAvailableDates");
        }
		
    };
});