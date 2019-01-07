function GetLayouts() {
    let id = $('#venueList').val();

    let dropdown = $('#layoutList');
    dropdown.empty();
    //dropdown.append('<option selected="true">Select layout</option>');
    dropdown.prop('SelectedIndex', 0);
    if (id !== undefined && id !== "") {
        $.get('/Event/GetLayouts?venueId=' + id, function (data) {
            $.each(data.layouts, function (i, entry) {
                dropdown.append($('<option></option>').attr('value', entry.Id).text(entry.Display));
            })
        });
    }
}

function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();

        reader.onload = function (e) {
            $('#event-image')
                .attr('src', e.target.result)
                .width(360)
                .height(360);
        };

        reader.readAsDataURL(input.files[0]);
    }
}

function GetEventsByVenue() {
    let id = $('#venueListToSortEvents').val();

    let dropdown = $('#eventListOnEdit');
    dropdown.empty();
    dropdown.prop('SelectedIndex', 0);
    if (id !== undefined && id !== "") {
        $.getJSON('/Event/GetEventsByVenue?venueId=' + id, function (data) {
            $.each(data.events, function (i, entry) {
                dropdown.append($('<option></option>').attr('value', entry.Id).text(entry.Display));
            })
            GetEventToEdit();
        });
    }
    
}

function GetEventToEdit() {
    let id = $('#eventListOnEdit').val();

    if (id !== undefined && id !== "" && id!== null) {
        $.ajax({
            url: '/Event/GetEventToEdit?eventId=' + id,
            success: function (result) {
                $('#editEventPlaceHolder').html(result);
                $('.text-success').hide();
                $("#venueList option[value='" + $('#venueListToSortEvents').val() + "']").prop('selected', true);
                $.getJSON('/Event/GetLayouts?venueId=' + $('#venueList').val(), function (data) {
                    $.each(data.layouts, function (i, entry) {
                        $('#layoutList').append($('<option></option>').attr('value', entry.Id).text(entry.Display));
                    })
                    $("#layoutList option[value='" + $('#currentLayout').val() + "']").prop('selected', true);
                });            
            }
        }); 
    }
    else
        $('#editEventPlaceHolder').html(null);

}

function SaveEventChanges() {
    $('#errorMessagesEditEvent').hide();

    //Serialize form
    var data = new FormData($('#editEventForm')[0]);

    $.ajax({
        type: "POST",
        url: "/Event/Edit",
        data: data,
        processData: false,
        contentType: false,
        cache: false,
        success: function (result) {
            if (!result.success) {
                $('#errorMessagesEditEvent').hide();
                $('.text-success').hide();
                var errors = '';
                $.each(result.errors, function (index, value) {
                    errors += value + ' <br />';
                });
                $('#errorMessagesEditEvent').html(errors + ' <br />');
                $('#errorMessagesEditEvent').show();
            }
            else {
                let title = $("#eventListOnEdit option:selected").text();
                let newTitle = title.replace(title.match(/.+(?=\s*\(.+\))/g), $('#title').val() + ' ');
                $("#eventListOnEdit option:selected").text(newTitle);
                $('.text-success').show();
               
            }
        }
    });
}

function CreateEvent() {
    $('#errorMessagesCreateEvent').hide();

    //Serialize form
    var data = new FormData($('#createEventForm')[0]);

    $.ajax({
        type: "POST",
        url: "/Event/Create",
        data: data,
        processData: false,
        contentType: false,
        cache: false,
        success: function (result) {
            if (!result.success) {
                $('#errorMessagesCreateEvent').hide();
                var errors = '';
                $.each(result.errors, function (index, value) {
                    errors += value + ' <br />';
                });
                $('#errorMessagesCreateEvent').html(errors + ' <br />');
                $('#errorMessagesCreateEvent').show();
            }
            else {
                $(':input', '#createEventForm')
                    .not(':button, :submit, :reset, :hidden')
                    .val('')
                    .prop('checked', false);
                $('#event-image').attr('src', '@Url.Content("~/Content/Images/default.jpg")');
                $('.text-success').show();
            }
        }
    });
}

function DeleteArea(areaId) {
    if (confirm('Delete selected area?')) {
        $.ajax({
            type: "POST",
            url: '/Event/DeleteArea?areaId=' + areaId,
            success: function (result) {
                if (!result.success) {
                    $('#errorMessagesEditEvent').html(result.error + ' <br />');
                }
                else {
                    $('#areaTable #' + areaId).remove();
                }
            }
        });
    }  
}

function GetAreaToEdit(areaId) {
    let eventId = $('#eventListOnEdit').val();

    if (eventId !== undefined && eventId !== "") {
        $.ajax({
            url: '/Event/EditArea?eventId=' + eventId + '&areaId=' + areaId,
            success: function (result) {
                $('#editAreaPlaceHolder').html(result);
                $('.text-success').hide();
                $('#eventPlaceHolder').hide();
                $('#editAreaPlaceHolder').show();
                $("html, body").animate({ scrollTop: 0 }, 250);
            }
        });
    } 
}

function CreateArea() {
    let eventId = $('#eventListOnEdit').val();

    if (eventId !== undefined && eventId !== "") {
        $.ajax({
            url: '/Event/CreateArea?eventId=' + eventId,
            success: function (result) {
                $('#editAreaPlaceHolder').html(result);
                $('.text-success').hide();
                $('#eventPlaceHolder').hide();
                $('#editAreaPlaceHolder').show();
                $("html, body").animate({ scrollTop: 0 }, "slow");
            }
        });
    }
}

function DeleteSeat(seatId, button) {
    if (seatId !== undefined && seatId !== "") {
        var rowId = $(button).parent().parent().attr('id').match(/(?!\_)\d+/g)[0];
        $(button).parent().parent().remove();
        var rows = $('#seatTable > tbody > tr');

        for (var i = rowId; i <= rows.length; i++) {
            let newIndex;
            $('#seatTable > tbody > #seatRow_' + i).find('input').each(function () {
                newIndex = i - 1;
                $(this).attr('name',
                    $(this).attr('name').replace($(this).attr('name').match(/[(0-9)+]/g), newIndex));
            });
            $('#seatRow_' + i).attr('id', 'seatRow_' + newIndex);
        }
    }
}

function CloseEventAreaForm() {
    $('#editAreaPlaceHolder').hide();
     $('#eventPlaceHolder').show(300);
    var venueId = $('#venueList').val();
    var eventId = $('#eventListOnEdit').val();

    $('#eventListOnEdit').trigger('change');
}

function SaveArea(actionName) {
    $('#errorMessagesEditEvent').hide();
    $('.text-success').hide();
    var url;

    if (actionName == 'EditArea')
        url = '/Event/EditArea';
    else
        url = '/Event/CreateArea';

    $.ajax({
        type: 'POST',
        url: url,
        data: $('#eventAreaForm').serialize(),
        success: function (result) {
            if (!result.success) {
                $('#errorMessagesEditEventArea').hide();
                var errors = '';
                $.each(result.errors, function (index, value) {
                    errors += value + ' <br />';
                });
                $('#errorMessagesEditEventArea').html(errors + ' <br />');
                $('#errorMessagesEditEventArea').show();
            }
            else {
                $('#errorMessagesEditEventArea').hide();
                $('.text-success').show();
            }
        }
    });
}

function AddSeatToTable() {
    var lastrow = $('#seatTable > tbody').find('tr').last().attr('id');
    var index;

    if (lastrow === undefined) {
        index = 0;
    }
    else {
        var rowId = lastrow.match(/(?!\_)\d+/g)[0];
        index = parseInt(rowId, 10) + 1;
    }
  
    var inputRow = '<tr id="seatRow_' + index + '">' +
        '<th><input autocomplete="off" class="form-control" type="text" name="SeatList[' + index + '].Row"></th>' +
        '<td><input autocomplete="off" class="form-control" type="text" name="SeatList[' + index + '].Number"></td>' +
        '<td><input autocomplete="off" value="0" class="form-control" disabled="disabled" name="SeatList[' + index + '].State" type="text"></td>' +
        '<td><input type="button" value="X" onclick="DeleteSeat(null, this)" class="btn btn-primary col-8" name="button"></td>'
    '</tr >'

    if (lastrow === undefined) {
        $('#seatTable > tbody').append(inputRow);
    }
    else {
        $(inputRow).insertAfter('#seatTable > tbody > #' + lastrow);
    }
}

function DeleteEvent(eventId) {
    if (confirm('Delete event?')) {
        $.ajax({
            type: "POST",
            url: '/Event/DeleteEvent?eventId=' + eventId,
            success: function (result) {
                if (!result.success) {
                    $('#errorMessagesEditEvent').html(result.error + ' <br />');
                }
                else {
                    location.href = "/Home/Index";
                }
            }
        });
    }  
}

function SetDatePickerCulture(element,culture) {
    $(element).datepicker($.extend({
        changeMonth: true,
        changeYear: true
    },
        $.datepicker.regional[culture]
    ));
}

function SetTimePicker(element, culture) {
    let options;
    if (culture == "en") {
        options = {
            timeFormat: 'hh:mm p'
        }
    }

    if (culture == "ru" | culture == "be") {
        options = {
            timeFormat: 'HH:mm'
        }
    }
    $(element).timepicker(options);
}
