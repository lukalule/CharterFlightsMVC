// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () { 

    var pomOrigin;
    var pomDestination;

    //let long_name = 'Australia';
    //let $element = $('.country-js');
    //let val = $element.find("option:contains('" + long_name + "')").val();
    //$element.val(val).trigger('change.select2');

    $('#spinner').hide();
    $('#btnSubmit').on('click', function () {

        if ($('#origin').text() == "" || $('#destination').text() == "" || $('#departureDate').val() == "")
            return;

        pomOrigin = $('#origin').text();
        pomDestination = $('#destination').text();

        $('#spinner').show();
        var data = {
            origin: $('#origin').text(),
            destination: $('#destination').text(),
            departureDate: $('#departureDate').val(),
            returndate: $('#returnDate').val(),
            currency: $('#currency').val(),
            adults: $('#adults').val(),
            children: $('#children').val(),
            infants: $('#infants').val(),
            seniors: $('#seniors').val()
        };

        //$('#origin').text("");
        //$('#destination').text("");

        //$('#origin').val(null).trigger('change');
        //$('#destination').val(null).trigger('change');
        //$('#origin').val(pomOrigin).trigger('change');
        //$('#destination').val(pomDestination).trigger('change');
        var c = $('#origin').text();
        var d = $('#destination').text();

        $.get('/Flights/FilterFlights', data, function (partial) {           
            $('#spinner').hide();
            $("#tableContainer").html(partial);   

           
        });
        
        
    });


    $('#origin').select2({
        placeholder: 'Input airport code, city or state..',
        ajax: {       
            delay: 250,
            url: '/Flights/Select2Data',
            dataType: 'json',
            data: function (params) {
                var query = {
                    searchTerm: params.term,
                    type: 'public'
                }

                // Query parameters will be ?search=[term]&type=public
                return query;
            },
            processResults: function (data) {
                // Tranforms the top-level key of the response object from 'items' to 'results'
                return {
                    results: data
                };
            }            
        },        
    });

    $('#destination').select2({
        placeholder: 'Input airport code, city or state..',
        ajax: {
            delay: 250,
            url: '/Flights/Select2Data',
            dataType: 'json',
            data: function (params) {
                var query = {
                    searchTerm: params.term,
                    type: 'public'
                }

                // Query parameters will be ?search=[term]&type=public
                return query;
            },
            processResults: function (data) {
                // Tranforms the top-level key of the response object from 'items' to 'results'
                return {
                    results: data
                };
            }
        }
    });

    
});