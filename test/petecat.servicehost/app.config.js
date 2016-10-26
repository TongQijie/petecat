angular.
  module('blogApp').
  config(['$locationProvider', '$routeProvider',
    function config($locationProvider, $routeProvider) {
      $locationProvider.hashPrefix('!');

      $routeProvider.
        when('/articles', {
          template: '<article-list></article-list>'
        }).
        when('/articles/:articleId', {
          template: '<article-detail></article-detail>'
        }).
        otherwise('/articles');
    }
  ]);