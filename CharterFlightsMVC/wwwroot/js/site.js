// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () { 

    // DODATI ZA SVE ATRIBUTE....
    $('#spinner').hide();
    $(document).ready(function () {
        if (window.location.search) {

            $('#spinner').show();
            $.get('/Flights/FilterFlights/' + window.location.search + "&currency=" + $('#currency').val(), function (partial) {
                $('#origin').text("");
                $('#destination').text("");

                $('#spinner').hide();
                $("#tableContainer").html(partial);
                //window.history.pushState(data, "GetItineraries", partialUrl);
            });
        }
    });
    //call filterFlights on load

    window.onpopstate = function (e) {
        if (e.state) {
            $('#spinner').show();
            $.get('/Flights/FilterFlights/' + window.location.search + "&currency=" + $('#currency').val(), function (partial) {
                $('#origin').text("");
                $('#destination').text("");

                $('#spinner').hide();
                $("#tableContainer").html(partial);
                //window.history.pushState(data, "GetItineraries", partialUrl);
            });
            
        }
    };
    
    $('#btnSubmit').on('click', function () {

        if ($('#origin').text() == "" || $('#destination').text() == "" || $('#departureDate').val() == "")
            return;
        $('#spinner').show();
        let data = {
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

        var partialUrl = "/?origin=" + $.trim($('#origin').text()).substring(0, 3) + "&destination=" + $.trim($('#destination').text()).substring(0, 3)
                       + "&departureDate=" + $('#departureDate').val() + "&currency=" + $('#currency').val();
        $.get('/Flights/FilterFlights', data, function (partial) { 
            $('#origin').text("");
            $('#destination').text("");

            $('#spinner').hide();
            
            $("#tableContainer").html(partial);
            window.history.pushState(data, "GetItineraries", partialUrl);
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