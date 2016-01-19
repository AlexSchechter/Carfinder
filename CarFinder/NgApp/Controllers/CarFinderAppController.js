angular.module('CarFinderApp').controller('CarFinderAppController', ['$scope', '$http' ,function ($scope, $http) {
    $scope.years = [];
    $scope.makes = [];
    $scope.models = [];
    $scope.trims = [];

    $scope.selectedYear = '';
    $scope.selectedMake = '';
    $scope.selectedModel = '';
    $scope.selectedTrim = '';
    $scope.carData = {};
   // $scope.nameCapitals = $scope.carData.;

    $scope.getYears = function () {
        //declare options object (if necessary)
        $http.get('api/cars/years').then(function (response) {
            //assign result to a $scope variable
            $scope.years = response.data;
        });
        //assign result to a $scope variable
    };

    $scope.getMakes = function () {
        var options = { params: { year: $scope.selectedYear } }; //pass the selected year to the API
        $http.get('api/cars/MakesByYear', options).then(function (response){
            $scope.makes = response.data;
        });
    };

    $scope.getModels = function () {
        var options = { params: { year: $scope.selectedYear, make: $scope.selectedMake } };
        $http.get('api/cars/ModelsByYearAndMake', options).then(function (response) {
            $scope.models = response.data;
        });
    };

    $scope.getTrims = function () {
        var options = { params: {year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel } };
        $http.get('api/cars/TrimsByYearMakeAndModel', options).then(function (response) {
            if (response.data == "") {
                $scope.trims = ["Standard Trim"];
            }
            else {
                $scope.trims = response.data;
            }
            var x;
            //$scope.getCar();
        });
    };

    $scope.getCar = function () {
        var cleanTrimSelection = $scope.selectedTrim;
        if ($scope.selectedTrim == "Standard Trim") {
            cleanTrimSelection = "";
        };

        var options = { params: { year: $scope.selectedYear, make: $scope.selectedMake, model: $scope.selectedModel, trim: "null" } };
        $http.get('api/cars/CarData', options).then(function (response) {
            $scope.carData = response.data;
            displayElements.hidden = false;
        })
        
    };


    $scope.getYears();

}]);