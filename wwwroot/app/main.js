require.config({
    baseUrl: "app",
    urlAr1gs: "v=1.0.0.1",
    paths: {
        'jquery': "../lib/jquery",
        'bootstrap': "../lib/bootstrap.bundle",
        'angular': "../lib/angular",
        'angular-route': "../lib/angular-route",
        'ng-file-upload': "../lib/ng-file-upload",
        'fabric': "../lib/fabric.min"
    },
    shim: {
        'bootstrap': ["jquery"],
        'angular': {
            'exports': "angular",
            'deps': ["jquery"]
        },
        'angular-route': ["angular"],
        'ng-file-upload': ["angular"]
    },
});

require(
    [
        "bootstrap",
        "angular",
        "angular-route",
        "ng-file-upload",
        "fabric",
        "routeResolver",
        "routeUrl",
        "indexController"
    ],
    function (bootstrap, angular) {

        var urls = [
            "../lib/bootstrap.css",
            "../app/site.css"
        ];

        function loadCss(urlList) {
            urlList.forEach(function (url) {
                var link = document.createElement("link");
                link.type = "text/css";
                link.rel = "stylesheet";
                link.href = require.toUrl(url);
                document.getElementsByTagName("head")[0].appendChild(link);
            });
        }

        loadCss(urls);

        angular.element(document).ready(function () {
            angular.bootstrap(document, ["PostcardApp"]);
        });
    },
    function (err) {

        if (!err.requireModules) {
            throw err;
        }

        err.requireModules.forEach(function (module) {
            require.undef(module);
        });

        setTimeout(function () {
            require(err.requireModules, null);
        }, 3000 + Math.random() * 500);
    }
);
