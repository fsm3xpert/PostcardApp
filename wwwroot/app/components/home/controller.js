"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$window", "homeService"];

    var controller = function ($scope, $window, homeService) {

        var canvas = {};

        //#region "Private Methods"

        var init = function () {
            canvas = new fabric.Canvas("postcardCanvas");
            canvas.setWidth(768);
            canvas.setHeight(576);
        };

        var sendEmailWithGeoTag = function (geoTag) {
            var data = {
                geoTag: JSON.stringify(geoTag),
                sentEmailTo: $scope.sentEmailTo,
                subject: $scope.subject,
                body: $scope.body,
                file: canvas.toDataURL({ format: "jpeg", multiplier: 1.0 })
            };
            homeService.sendEmail(data).then(
                function (output) {
                    alert("Email has been sent successfully.");
                },
                function (output) {
                    alert("Some error occurred, please contact system administrator.");
                }
            );
        }

        //#endregion

        $scope.imageText = "Hello World !!!";
        $scope.showWebcam = false;
        $scope.activeStream = {};
        $scope.webcamChannel = {
            videoWidth: 768,
            videoHeight: 576
        };

        //#region "Image Upload Methods"

        $scope.addImage = function () {
            var imageUrl = "img/01.jpg";
            fabric.Image.fromURL(imageUrl, function (img) {
                canvas.add(img);
            });
            $("#headingTwo button").click();
        };

        $scope.uploadImage = function ($files) {
            if ($files.length == 1) {
                if ($files[0].size > 10485760) {
                    alert("File cannot be uploaded, due to it exceed the 10 MB limit.");
                }
                else {
                    var data = {
                        file: $files[0]
                    };
                    homeService.uploadImage(data).then(
                        function (output) {
                            var imageUrl = "temp/" + output.data;
                            fabric.Image.fromURL(imageUrl, function (img) {
                                canvas.add(img);
                            });
                            $("#headingTwo button").click();
                        },
                        function (output) {
                            alert("Some error occurred, please contact system administrator.");
                        }
                    );
                }
            }
        };

        $scope.resetImage = function () {
            canvas.clear();
        };

        //#endregion

        //#region "Webcam Methods"

        $scope.openWebcam = function () {
            $scope.showWebcam = true;
        };

        $scope.closeWebcam = function () {
            $scope.showWebcam = false;
        };

        $scope.takeSnapshot = function () {
            var hiddenCanvas = document.createElement("canvas");
            hiddenCanvas.width = $scope.webcamChannel.video.width;
            hiddenCanvas.height = $scope.webcamChannel.video.height;
            var ctx = hiddenCanvas.getContext("2d");
            ctx.drawImage($scope.webcamChannel.video, 0, 0, $scope.webcamChannel.video.width, $scope.webcamChannel.video.height);
            var data = {
                file: hiddenCanvas.toDataURL()
            };
            homeService.uploadSnapshot(data).then(
                function (output) {
                    var imageUrl = "temp/" + output.data;
                    fabric.Image.fromURL(imageUrl, function (img) {
                        canvas.add(img);
                    });
                    $("#headingTwo button").click();
                },
                function (output) {
                    alert("Some error occurred, please contact system administrator.");
                }
            );
        };

        $scope.onStream = function (webcamStream) {
            debugger;
            $scope.activeStream = webcamStream;
            return $scope.activeStream;
        };

        $scope.webcamErrorCallback = function (error) {
            if (error.name == "PermissionDeniedError") {
                $scope.showWebcam = false;
                alert("Please give permission to webcam to take snapshot.");
                $window.location.reload();
            }
        };

        //#endregion

        //#region "Image Modification Methods"

        $scope.addText = function () {
            var text = new fabric.Text($scope.imageText, {
                left: 300,
                top: 300,
                fontSize: 32,
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

        //#endregion

        //#region "Email Methods"

        $scope.sendEmail = function () {
            homeService.getIPStack().then(
                function (output) {
                    var geoTag = {
                        ipAddress: output.data.ip,
                        latitude: output.data.latitude,
                        longitude: output.data.longitude
                    };
                    sendEmailWithGeoTag(geoTag);
                },
                function (output) {
                    alert("Some error occurred, please contact system administrator.");
                }
            );
        };

        //#endregion

        (function () {
            init();
        })();
    };

    controller.$inject = injectParams;

    app.register.controller("homeController", controller);

});
