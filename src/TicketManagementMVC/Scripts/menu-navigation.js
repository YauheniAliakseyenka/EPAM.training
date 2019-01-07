$(document).ready(function () {
    $('.nav-link').click(function () {
        $('#menu ul li .nav-link').removeClass('active');
        $('div .dropdown-menu').removeClass('show');

        $(this).addClass('active');
    });

    $('.profile-left').click(function () {
        $('div .dropdown-menu').removeClass('show');
    });

    //event manager dropdown list
    $('#eventmanager-dropdown').click(function () {
        $('#eventmanager-dropdown-menu').addClass('show');
    });

    //profile dropdown list
    $('#profile-dropdown').click(function () {
        $('div .dropdown-menu').removeClass('show');
        $('#profile-dropdown-menu').addClass('show');
    });

    $('.body-content, .dropdown-item').click(function () {
        $('div .dropdown-menu').removeClass('show');
    });

    $('.dropdown-item').click(function () {
        $('div .dropdown-menu').removeClass('show');
    });

    //closing modal window
    $('#loginModal').on('hidden.bs.modal', function () {
        $('#errorMessages').hide();
        $('#username').val('');
        $('#password').val('');
    })
});

function SubmitLogin() {
    $('#errorMessagesLogin').hide();
    $.ajax({
        type: "POST",
        url: "/Account/Login",
        data: $('#loginForm').serialize(),
        success: function (result) {
            if (!result.success) {
                $('#errorMessagesLogin').hide();
                var errors = '';
                $.each(result.errors, function (index, value) {
                    errors += value + ' <br />';
                });
                $('#username').val('');
                $('#password').val('');
                $('#errorMessagesLogin').html(errors +' <br />');
                $('#errorMessagesLogin').show('200');
            }
            else
                location.href = '/Home/Index';
        }
    });
};

function ShowNotify(type, message) {
    $.notify(message, {
        type: type,
        allow_dismiss: true,
        newest_on_top: true,
        placement: {
            from: "bottom",
            align: "left"
        },
        timer: 1000,
        delay: 100,
        template: '<div data-notify="container" class="col-xs-8 col-sm-3 alert alert-{0}" role="alert">' +
            '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
            '<span data-notify="message">{2}</span></div>'
    });
}