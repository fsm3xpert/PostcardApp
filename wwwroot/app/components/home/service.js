"use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "Upload"];

    var service = function ($http, $q, Upload) {

        var serviceBase = "http://localhost:5000/api/postcard/";

        this.uploadImage = function (data) {
            return Upload.upload({
                url: serviceBase + "uploadImage/",
                method: "POST",
                data: data
            });
        };

        this.uploadSnapshot = function (data) {
            return Upload.upload({
                url: serviceBase + "uploadSnapshot/",
                method: "POST",
                data: data
            });
        };

        this.sendEmail = function (data) {
            return Upload.upload({
                url: serviceBase + "sendEmail/",
                method: "POST",
                data: data
            });
        };

        this.getIPStack = function () {
            return $http({
                url: "http://api.ipstack.com/check?access_key=XYZ",
                method: "GET"
            });
        };
    };

    service.$inject = injectParams;

    app.register.service("homeService", service);

});
