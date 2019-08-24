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
        request.error(e)
        {
            submitPhoneButton.ladda('stop');
        }
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
                        window.location.href = "/costumer";
                        break;
                    default:
                        alert(response.message);
                        break;
                }
            }
        });
        request.error(e)
        {
            submitPhoneCodeButton.ladda('stop');
        }
    });
})