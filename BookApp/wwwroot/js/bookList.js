var BookList = (function ($) {
    // 
    var filterApiUrl = null;
    return {
        initialise: function (filterByValue, filterValue, exFilterApiUrl) {
            filterApiUrl = exFilterApiUrl;
            console.log(filterByValue, filterValue,filterApiUrl);
        }
    }
}(window.jQuery))