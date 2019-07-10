var routeUrl = (function () {

    var factory = {};

    factory.home = {

        urls: {
            index: "app/components/home/index.html"
        },
        depend: [
            "app/components/home/controller.js",
            "app/components/home/service.js"
        ]
    };

    factory.image = {

        urls: {
            index: "app/components/image/index.html"
        },
        depend: [
            "app/components/image/controller.js",
            "app/components/image/service.js"
        ]
    };

    return factory;
})();
