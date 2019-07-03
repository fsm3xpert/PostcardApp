"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope"];

    var controller = function ($scope) {

        $scope.appTitle = "PostcardApp";
        
        
    };

    controller.$inject = injectParams;

    app.controller("indexController", controller);

});
