var doctorsListApp = angular.module('doctorsListApp', []);

doctorsListApp.controller("listingController", function ($scope, $http) {
    $scope.getDoctor = function () {
        var selectedVals = $("#hdnSelectedItem").val().split("|");
        var url = "";
        if (selectedVals.length > 0) {
            switch(selectedVals[0]){
                case "H":
                    url = "hid=" + selectedVals[1];
                    break;
                case "S":
                    url = "sid=" + selectedVals[1];
                    break;
            }   
            if (selectedVals != null && selectedVals.length > 0) {
                $http.get('/api/doctor/?' + url).
                    then(function (response) {
                        $scope.doctors = response.data;
                    }, function (response) {

                    });
            }
        }
    }
});

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
            $("#hdnSelectedItem").val(suggestion.data);
            var splitsVal = suggestion.data.split('|');
            if(splitsVal.length > 0){
                if(splitsVal[0] == "H") {
                    window.location.href = "/Doctors/Index/?hid=" + splitsVal[1];
                }
            }          
            
        },
        onHint: function (hint) {
            $('#autocomplete-ajax-x').val(hint);
        },
        onInvalidateSelection: function () {
            $('#selction-ajax').html('You selected: none');
        }
    });
});