angular.module('CarFinderApp').controller('CarFinderAppController', ['$scope', '$http', '$location', '$anchorScroll', '$timeout',
    function ($scope, $http, $location, $anchorScroll, $timeout) {
        $scope.years = null;
        $scope.makes = null;
        $scope.models = null;
        $scope.trims = null;
        $scope.carData = null;

        $scope.selectedYear = '';
        $scope.selectedMake = '';
        $scope.selectedModel = '';
        $scope.selectedTrim = '';
    
        $scope.slideInterval = 3000;
        $scope.isCollapsed = true;

        $scope.getYears = function () {
            $http.get('../api/cars/years').then(function (response) {
                $scope.years = response.data;
            });
        };

        $scope.getMakes = function () {
            $scope.selectedMake = '';
            $scope.selectedModel = '';
            $scope.selectedTrim = '';
            var options = { params: { year: $scope.selectedYear } }; 
            $http.get('../api/cars/MakesByYear', options).then(function (response){
                $scope.makes = response.data;
                $scope.models = null;
                $scope.trims = null;
                $scope.carData = null;
            });
        };

        $scope.getModels = function () {
            $scope.selectedModel = '';
            $scope.selectedTrim = '';
            var options = { params: { year: $scope.selectedYear, make: $scope.selectedMake } };
            $http.get('../api/cars/ModelsByYearAndMake', options).then(function (response) {
                $scope.models = response.data;
                $scope.trims = null;
                $scope.carData = null;
            });
        };

        $scope.getTrims = function () {
            $scope.selectedTrim = '';
            var options = { params: {year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel } };
            $http.get('../api/cars/TrimsByYearMakeAndModel', options).then(function (response) {
                if (response.data == "") {
                    $scope.trims = ["Standard"];
                }
                else {
                    $scope.trims = response.data;
                }
                $scope.carData = null;
            });
        };

        $scope.getCar = function () {
            $scope.isCollapsed = true;
            var options = {};
            if ($scope.selectedTrim === "Standard") {
                options = { params: { year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel } };
            }
            else {
                options = { params: { year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel, trim: $scope.selectedTrim } };
            };
            $http.get('../api/cars/CarData', options).then(function (response) {
                $scope.carData = response.data;
            })
            $timeout(function () {
                $location.hash('carHeading');
                $anchorScroll.yOffset = 55;
                $anchorScroll();
            }, 2000);
        };

        $scope.getYears();

        $scope.viewRecalls = function () {
            $scope.isCollapsed = !$scope.isCollapsed;
            if (!$scope.isCollapsed) {
                $timeout(function () {
                    $location.hash('myRecalls');
                    $anchorScroll.yOffset = 55;
                    $anchorScroll();
                }, 300);
            };
        };

}]);