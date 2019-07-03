"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "homeService"];

    var controller = function ($scope, homeService) {

        var canvas = {};

        var init = function () {
            canvas = new fabric.Canvas("postcardCanvas");
            canvas.setWidth(768);
            canvas.setHeight(576);
        };

        $scope.imageText = "Hello World !!!";

        $scope.addImage = function () {
            var imageUrl = "img/01.jpg";
            fabric.Image.fromURL(imageUrl, function (img) {
                canvas.add(img);
            });
            $("#headingTwo button").click();
        };

        $scope.uploadImage = function () {
            //
        };

        $scope.resetImage = function () {
            canvas.clear();
        };

        $scope.addText = function () {
            var text = new fabric.Text($scope.imageText, {
                left: 10,
                top: 5,
                fontSize: 15,
                fontFamily: 'Verdana',
                fill: 'white'
            });
            canvas.add(text);
        };

        $scope.clearText = function () {
            var objects = canvas.getObjects("text");
            objects.forEach(function (o) {
                canvas.remove(o);
            });
        };

        $scope.sendEmail = function () {

            var data = {
                sentEmailTo: $scope.sentEmailTo,
                subject: $scope.subject,
                body: $scope.body,
                file: canvas.toDataURL("image/png")
            };

            homeService.sendEmail(data).then(
                function (output) {
                    alert("Email has been sent successfully.");
                },
                function (output) {
                    alert("Some error occurred, please contact system administrator.");
                }
            );
        };

        (function () {
            init();
        })();
    };

    controller.$inject = injectParams;

    app.register.controller("homeController", controller);

});
