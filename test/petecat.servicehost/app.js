// Define the `phonecatApp` module
angular.module('blogApp', [
  // ...which depends on the `phoneList` module
  'ngRoute',
  'articleList',
  'articleDetail'
]);