"use strict";

define([], function () {
    
    var app = angular.module("PostcardApp", ["ngRoute", "ngFileUpload", "ngTable", "routeResolverService"]);

    app.config(["$routeProvider", "$controllerProvider", "$provide", "routeResolverProvider",
        function ($routeProvider, $controllerProvider, $provide, routeResolverProvider) {

            var route = routeResolverProvider.route;

            $routeProvider
                .when("/home", route.resolve({ module: "home" }))
                .when("/image", route.resolve({ module: "image" }))
                .otherwise({ redirectTo: "/home" });

            app.register = {
                controller: $controllerProvider.register,
                service: $provide.service
            };
        }
    ]);

    return app;
});
