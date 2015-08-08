/*jslint  browser: true, white: true, plusplus: true */
/*global $ */

$(function () {
    'use strict';

    // Initialize ajax autocomplete:
    $('#autocomplete-ajax').autocomplete({
        serviceUrl: '/api/find/',
        dataType: 'json',
        lookupFilter: function (suggestion, originalQuery, queryLowerCase) {
            var re = new RegExp('\\b' + $.Autocomplete.utils.escapeRegExChars(queryLowerCase), 'gi');
            return re.test(suggestion.data);
        },
        onSelect: function (suggestion) {
            $('#findDoctor').click();
            //$('#selction-ajax').html('You selected: ' + suggestion.value + ', ' + suggestion.data);
        },
        onHint: function (hint) {
            $('#autocomplete-ajax-x').val(hint);
        },
        onInvalidateSelection: function () {
            $('#selction-ajax').html('You selected: none');
        }
    });
});