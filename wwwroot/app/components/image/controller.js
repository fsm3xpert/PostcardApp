"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope"];

    var controller = function ($scope) {

        $scope.title = "Postcard Image Page";
    };

    controller.$inject = injectParams;

    app.register.controller("imageController", controller);

});
