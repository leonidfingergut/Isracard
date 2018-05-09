var isracard = angular.module('isracard', ["ngRoute"]);
function config($routeProvider) {
    $routeProvider
    .when("/", {
        templateUrl: "html/repositories.html"
    })
    .when("/repository", {
        templateUrl: "html/repositories.html"
    })
    .when("/bookmark", {
        templateUrl: "html/bookmark.html"
    })
};

isracard.config(['$routeProvider', config]);
