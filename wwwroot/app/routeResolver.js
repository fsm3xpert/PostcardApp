"use strict";

define(["angular"], function (angular) {

    var routeResolver = function () {

        this.$get = function () {
            return this;
        };

        this.route = function () {
            var resolveDependencies = function ($q, $rootScope, dependencies) {
                var defer = $q.defer();
                require(dependencies, function () {
                    $rootScope.$apply(function () {
                        defer.resolve();
                    });
                });
                return defer.promise;
            };
            return {
                resolve: function (rp) {
                    var routeDef = {};
                    routeDef.templateUrl = routeUrl[rp.module].urls.index;
                    routeDef.controller = rp.module + "Controller";
                    routeDef.module = rp.module;
                    routeDef.resolve = {
                        load: ["$q", "$rootScope", function ($q, $rootScope) {
                            var dependencies = [];
                            $.each(routeUrl[rp.module].depend, function (index, d) { dependencies.push("/" + d); });
                            return resolveDependencies($q, $rootScope, dependencies);
                        }]
                    };
                    return routeDef;
                }
            };
        }();
    };

    var routeService = angular.module("routeResolverService", []);

    routeService.provider("routeResolver", routeResolver);
});
