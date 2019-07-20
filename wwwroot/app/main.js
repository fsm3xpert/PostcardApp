require.config({
    baseUrl: "app",
    urlArgs: "v=1.0.2",
    paths: {
        'jquery': "../lib/jquery",
        'bootstrap': "../lib/bootstrap.bundle",
        'angular': "../lib/angular",
        'angular-route': "../lib/angular-route",
        'ng-file-upload': "../lib/ng-file-upload",
        'ng-table': "../lib/ng-table.min",
        'webcam': "../lib/webcam.min",
        'fabric': "../lib/fabric.min"
    },
    shim: {
        'bootstrap': ["jquery"],
        'angular': {
            'exports': "angular",
            'deps': ["jquery"]
        },
        'angular-route': ["angular"],
        'ng-file-upload': ["angular"],
        'ng-table': ["angular"],
        'webcam': ["angular"]
    },
});

require(
    [
        "bootstrap",
        "angular",
        "angular-route",
        "ng-file-upload",
        "ng-table",
        "webcam",
        "fabric",
        "routeResolver",
        "routeUrl",
        "indexController"
    ],
    function (bootstrap, angular) {

        var urls = [
            "../lib/bootstrap.css",
            "../lib/ng-table.css",
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
