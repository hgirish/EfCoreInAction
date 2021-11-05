var BookList = (function ($) {
    // 
    var filterApiUrl = null;

    function sendForm(inputElem) {
        var form = $(inputElem).parents('form');
        form.submit();
    }
    return {
        initialise: function (filterByValue, filterValue, exFilterApiUrl) {
            filterApiUrl = exFilterApiUrl;
            console.log(filterByValue, filterValue,filterApiUrl);
        },

        sendForm: function (inputElem) {
            sendForm(inputElem);
        }
    }
}(window.jQuery))