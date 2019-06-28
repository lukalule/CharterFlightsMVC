// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () { 

    $('#spinner').hide();
    $('#btnSubmit').on('click', function () {

        if ($('#origin').text() == "" || $('#destination').text() == "")
            return;
        $('#spinner').show();
        var data = {
            origin: $('#origin').text(),
            destination: $('#destination').text(),
            departureDate: $('#departureDate').val(),
            returndate: $('#returnDate').val(),
            currency: $('#currency').val()
        };

        $.get('/Flights/FilterFlights', data, function (partial) { 
            $('#origin').text("");
            $('#destination').text("");

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
        }
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