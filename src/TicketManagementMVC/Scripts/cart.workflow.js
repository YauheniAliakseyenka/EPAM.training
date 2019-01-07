function AddToCart(seatId, object, message) {
    $.ajax({
        type: 'POST',
        url: '/Home/AddSeatToCart?seatId=' + seatId,
        success: function (result) {
            if (!result.success) {
                ShowNotify('danger', result.error);
            }
            else {
                $(object).addClass('locked-seat');
                $(object).children().addClass('seat-unavailable');

                ShowNotify('info', message);
            }
        }
    });
}

function DeleteSeatFromCart(seatId, message, culture, object) {
     $.ajax({
        url: '/Account/DeleteFromCart?seatId=' + seatId,
        success: function () {
            let totalBefore;
            let odds
            let totalAfter;
            if (culture == "en") {
                totalBefore = $('#total').text().substring(1).replace(/[^0-9\.-]+/g, "");
                let price = $(object).parent().parent().find('.price').text().substring(1).replace(/[^0-9\.-]+/g, "");
                odds = parseFloat(totalBefore) - parseFloat(price);
                totalAfter = parseFloat(odds).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
            }
            else {
                totalBefore = $('#total').text().substring(1).replace(/\s/g, '').replace(/\,/, '.');
                let price = $(object).parent().parent().find('.price').text().substring(1).replace(/\s/g, '').replace(/\,/, '.');
                odds = parseFloat(totalBefore) - parseFloat(price);
                totalAfter = parseFloat(odds).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1 ").replace(/\./g, ',');
            }

            $('#orderedList #' + seatId).remove();
            $('#total').text('$' + totalAfter);

            if (odds == 0)
                $('#order-button').hide();

            ShowNotify('info', message);
        }
    }); 
}

function CompleteOrder(message) {
    $.ajax({
        type: 'POST',
        url: '/Account/CompleteOrder',
        success: function (result) {
            if (!result.success) {
                ShowNotify('danger', result.error);
            }
            else {
                ShowNotify('success', message);
                $('#order-button').hide();
                $('tbody').remove();
            }
        }
    }); 
}