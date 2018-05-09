function GithubController($scope, $http, $filter) {
    var URL = 'http://localhost:59888';
    $scope.name = "";
    $scope.getRepositories = function () {
        $http({
            method: 'GET',
            contentType: 'application/json;',
            url: URL + '/api/Github?rname=' + $scope.name,
        }).then(function getSuccess(response) {
            $scope.Repositories = [];
            if (response.data) {
                $scope.Repositories = response.data;
                //console.log('getSuccess');
            }
        }, function getError(response) {
            $scope.Repositories = [];
            console.error(response.statusText);
        });
    };
    $scope.setBookmark = function (repos) {
        if (repos) {
            $http({
                method: 'POST',
                url: URL + '/api/Github',
                data: repos
            }).then(function getSuccess(response) {
                if (response.data) {
                    var rep = $filter('filter')($scope.Repositories, { ID: repos.ID }, true)[0];
                    rep.bookmark = !repos.bookmark;
                    //console.log('getSuccess setBookmark');
                }
            }, function getSuccess(response) {
                console.error(response.statusText);
            });
        }
    };
    $scope.getRepositoriesBookMark = function () {
        $scope.Repositories = [];
        $http({
            method: 'GET',
            contentType: 'application/json;',
            url: URL + '/api/Github',
        }).then(function getSuccess(response) {
            if (response.data) {
                $scope.RepositoriesBookmark = response.data;
                //console.log('getSuccess');
            }
        }, function getError(response) {
            console.error(response.statusText);
        });
    };
   
}
isracard.controller('GithubController', ['$scope', '$http','$filter', GithubController]);