// Write your Javascript code.


$(document).ready(function () {
    //Registration form 
    var formPhone = $("#enter-phone-form");
    var formCode = $("#enter-phone-code-form");
    var submitPhoneButton = $('#phone-submit').ladda();
    var submitPhoneCodeButton = $('#phone-code-submit').ladda();
    var phoneBlock = $('#phone-block');
    var phoneCodeBlock = $('#phone-code-block');
    var phoneInput = $('#phone-input');
    var phoneVerificationInput = $('#phone-verification-input');
    var logOutForm = $("#logout-form");


    //Holders page
    var formAddHolder = $("#add-holder-form");
    var submitAddHolder = $("#add-holder-submit-button").ladda();
    var holdersTable = $("#holders-table");

    formAddHolder.on("submit", function (e) {
        e.preventDefault();
        submitAddHolder.ladda("start");
        var formData = formAddHolder.serialize();
        var request = $.ajax({
            url: "/admin/holders",
            type: "POST",
            dataType: "json",
            data: formData,
            success: function (response) {
                submitAddHolder.ladda('stop');
                switch (response.code) {
                    case 200:
                        holdersTable.append(response.message);
                        break;
                    default:
                        alert(response.message);
                        break;
                }
            }
        });
    });

    logOutForm.on("submit", function (e) {
        e.preventDefault();
        var formData = logOutForm.serialize();
        var currentUrl = window.location.pathname;
        var request = $.ajax({
            url: currentUrl + "?handler=logout",
            type: "POST",
            dataType: "json",
            data: formData,
            success: function (response) {
                switch (response.code) {
                    case 200:
                        window.location.href = "/";
                        break;
                    default:
                        alert(response.error);
                        break;
                }
            }
        });
    });

    formPhone.on("submit", function (e) {
        e.preventDefault();
        submitPhoneButton.ladda('start');
        var formData = formPhone.serialize();

        var request = $.ajax({
            url: "/costumer/registration?handler=phone",
            type: "POST",
            dataType: "json",
            data: formData,
            success: function (response) {
                submitPhoneButton.ladda('stop');
                switch (response.code) {
                    case 200:
                        phoneVerificationInput.val(phoneInput.val());
                        phoneBlock.hide();
                        phoneCodeBlock.show();
                        break;
                    default:
                        alert(response.error);
                        break;
                }
            }
        });
    });

    formCode.on("submit", function (e) {
        e.preventDefault();
        submitPhoneCodeButton.ladda('start');
        var formData = formCode.serialize();

        var request = $.ajax({
            url: "/costumer/registration?handler=code",
            type: "POST",
            dataType: "json",
            data: formData,
            success: function (response) {
                submitPhoneCodeButton.ladda('stop');
                switch (response.code) {
                    case 200:
                        window.location.href = "/";
                        break;
                    default:
                        alert(response.message);
                        break;
                }
            }
        });
    });
});

