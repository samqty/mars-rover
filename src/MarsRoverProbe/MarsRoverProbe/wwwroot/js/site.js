// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var connection = new signalR.HubConnectionBuilder().withUrl("/loghub").build();
connection.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});

var app = angular.module('myApp', []);

app.controller('marsPhotosController', function ($scope, $http) {
    $scope.logs = ['ready!'];
    $scope.result = {};

    $scope.isLoading = false;

    $scope.downloadPhotos = function () {
        $scope.isLoading = true;

        $http.post('/home/downloadphotos')
            .then(response => {
                $scope.result.fileName = response.data.fileName;
                $scope.result.batches = response.data.batches;
                $scope.isLoading = false;
            });
    }

    connection.on("LogAdded", function (log) {
        $scope.logs.unshift(log);
        $scope.$apply();
    });
});
