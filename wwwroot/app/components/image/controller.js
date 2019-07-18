"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "NgTableParams", "imageService"];

    var controller = function ($scope, NgTableParams, imageService) {

        $scope.listTable = new NgTableParams(
            {
                page: 1,
                count: 10
            }, {
                getData: function (params) {
                    return imageService.listImages().then(
                        function (output) {
                            params.total(output.data.length);
                            return output.data;
                        },
                        function (output) {
                            alert("Some error occurred, please contact system administrator.");
                        }
                    );
                }
            }
        );
    };

    controller.$inject = injectParams;

    app.register.controller("imageController", controller);

});
