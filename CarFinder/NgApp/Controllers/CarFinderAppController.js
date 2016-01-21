angular.module('CarFinderApp').controller('CarFinderAppController', ['$scope', '$http' ,function ($scope, $http) {
    $scope.years = null;
    $scope.makes = null;
    $scope.models = null;
    $scope.trims = null;
    $scope.carData = null;

    $scope.selectedYear = '';
    $scope.selectedMake = '';
    $scope.selectedModel = '';
    $scope.selectedTrim = '';
    
    $scope.slideInterval = 300000;
    $scope.isCollapsed = true;

    $scope.getYears = function () {
        //declare options object (if necessary)
        $http.get('api/cars/years').then(function (response) {
            //assign result to a $scope variable
            $scope.years = response.data;
            //$scope.makes = [];
            //$scope.models = [];
            //$scope.trims = [];
            //$scope.carData = {};
        });
        //assign result to a $scope variable
    };

    $scope.getMakes = function () {
        $scope.selectedMake = '';
        $scope.selectedModel = '';
        $scope.selectedTrim = '';
        var options = { params: { year: $scope.selectedYear } }; //pass the selected year to the API
        $http.get('api/cars/MakesByYear', options).then(function (response){
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
        $http.get('api/cars/ModelsByYearAndMake', options).then(function (response) {
            $scope.models = response.data;
            $scope.trims = null;
            $scope.carData = null;
        });
    };

    $scope.getTrims = function () {
        $scope.selectedTrim = '';
        var options = { params: {year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel } };
        $http.get('api/cars/TrimsByYearMakeAndModel', options).then(function (response) {
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
        var options = {};
        //var cleanTrimSelection = $scope.selectedTrim;
        if ($scope.selectedTrim === "Standard") {
            options = { params: { year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel } };
        }
        else {
            options = { params: { year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel, trim: $scope.selectedTrim } };
        };
        $http.get('api/cars/CarData', options).then(function (response) {
            $scope.carData = response.data;
        })
        $scope.isCollapsed = true;
    };

    $scope.getYears();
}]);