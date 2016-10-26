angular.module('articleList').
  component('articleList', {
    // Note: The URL is relative to our `index.html` file
    templateUrl: 'article-list/article-list.template.html',
    controller: function ArticleListController($http) {
      var self = this;

      $http.get('article-service/get-articles?pageNumber=1').then(function(response) {
        self.articles = response.data;
      });
    }
  });