angular.module('articleDetail').
  component('articleDetail', {
    // Note: The URL is relative to our `index.html` file
    templateUrl: 'article-detail/article-detail.template.html',
    controller: ['$routeParams', '$http',
      function ArticleDetailController($routeParams, $http) {
        var self = this;
        
        $http.get('article-service/fetch-article?id=' + $routeParams.articleId).then(function(response) {
          self.article = response.data;
        });
    }]
  });