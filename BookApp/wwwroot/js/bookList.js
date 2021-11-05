var BookList = (function ($, loggingDisplay) {
    // 
    var filterApiUrl = null;

    function sendForm(inputElem) {
        var form = $(inputElem).parents('form');
        form.submit();
    }

    function enableDisableFilterDropdown($fsearch, enable) {
        var $fvGroup = $('#filter-value-group');
        if (enable) {
            $fsearch.prop('disabled', false);
            $fvGroup.removeClass('dim-filter-value');
        } else {
            $fsearch.prop('disabled', true);
            $fvGroup.addClass('dim-filter-value');
        }
    }
    function loadFilterValueDropdown(filterByValue, filterValue, ignoreTrace) {
        filterValue = filterValue || '';
        var $fsearch = $('#filter-value-dropdown');
        enableDisableFilterDropdown($fsearch, false);
        if (filterByValue !== "NoFilter") {
            $.ajax({
                url: filterApiUrl,
                data: { FilterBy: filterByValue }
            })
                .done(function (indentAndResult) {
                    if (!ignoreTrace) {
                        loggingDisplay.newTrace(indentAndResult.traceIdentifier, indentAndResult.numLogs);
                    }
                    $fsearch
                        .find('option')
                        .remove()
                        .end()
                        .append($('<option></option>')
                            .attr('value', '')
                            .text('Select filter...'));
                    
                    indentAndResult.result.forEach(function (arrayElem) {
                        $fsearch.append($("<option></option>").attr("value", arrayElem.value).text(arrayElem.text));
                    });
                    $fsearch.val(filterByValue);
                    enableDisableFilterDropdown($fsearch, true);
                })
                .fail(function () {
                    alert("error");
                });
        }
    }
    return {
        initialise: function (filterByValue, filterValue, exFilterApiUrl) {
            filterApiUrl = exFilterApiUrl;
            loadFilterValueDropdown(filterByValue, filterValue, true);
        },

        sendForm: function (inputElem) {
            
            sendForm(inputElem);
        },

        filterByHasChanged: function (filterElem) {
            var filterByValue = $(filterElem).find(":selected").val();
            
            loadFilterValueDropdown(filterByValue);
            if (filterByValue === "0") {
                sendForm(filterElem);
            }
        },

        loadFilterValueDropdown: function (filterByValue, filterValue) {
            loadFilterValueDropdown(filterByValue, filterValue);
        }
    }
}(window.jQuery, LoggingDisplay))