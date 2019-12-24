﻿// Write your Javascript code.
function get_elapsed_time_string(total_seconds) {
  function pretty_time_string(num) {
    return ( num < 10 ? "0" : "" ) + num;
  }

  var hours = Math.floor(total_seconds / 3600);
  total_seconds = total_seconds % 3600;

  var minutes = Math.floor(total_seconds / 60);
  total_seconds = total_seconds % 60;

  var seconds = Math.floor(total_seconds);

  // Pad the minutes and seconds with leading zeros, if required
  hours = pretty_time_string(hours);
  minutes = pretty_time_string(minutes);
  seconds = pretty_time_string(seconds);

  // Compose the string for display
  var currentTimeString = hours + ":" + minutes + ":" + seconds;

  return currentTimeString;
}



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
    var cardForm = $("#enter-card-form");
    var submitCardCodeButton = $('#card-submit').ladda();

    //Holders page
    var formAddHolder = $("#add-holder-form");
    var submitAddHolder = $("#add-holder-submit-button").ladda();
    var holdersTable = $("#holders-table");

    //Powerbanks page
    var formAddPowerbank = $("#add-powerbank-form");
    var submitAddPowerbank = $("#add-powerbank-submit-button").ladda();
    var powerbanksTable = $("#powerbank-table");

    //Take page
    var enterHolderCodeForm = $("#enter-equipment-code-form");
    var enterHolderCodeSubmit = $("#equipment-code-submit").ladda();
    var takeHolderBlock = $("#equipment-block");
    var takeTimerBlock = $("#timer-equipment-block");
    var timerText = $("#timer-text");

    //Costumer page
    var changeCostumerDataForm = $("#costumer-data-form");
    var changeCostumerDataButton = $("#change-costumer-data-button");

    formAddPowerbank.on("submit", function (e) {
        e.preventDefault();
        var formData = formAddPowerbank.serialize();
        var request = $.ajax({
            url: "/admin/powerbanks",
            type: "POST",
            dataType: "json",
            data: formData,
            success: function (response) {
                switch (response.code) {
                    case 200:
                        powerbanksTable.append(response.message);
                        break;
                    default:
                        alert(response.message);
                        break;
                }
            }
        });
    });

    changeCostumerDataForm.on("submit", function (e) {
        e.preventDefault();
        var dataForm = changeCostumerDataForm.serialize();
        var request = $.ajax({
            url: "/costumer",
            type: "PUT",
            dataType: "json",
            data: dataForm,
            success: function (response) {
                switch (response.code) {
                    case 200:
                        alert("Данные успешно изменены");
                        break;
                    default:
                        alert(response.message);
                        break;
                }
            }
            });
    });

    function check() {
        var token = $('input[name="__RequestVerificationToken"]').val()
        var request = $.ajax({
            url: "/take?handler=check",
            type: "POST",
            dataType: "json",
            data: {
                __RequestVerificationToken: token
            },
            success: function (response) {
                switch (response.code) {
                    case 200:
                        switch (response.message) {
                            case "1":
                                break;
                            case "0":
                                alert("Session is over");
                                location.reload();
                                break;
                            default:
                                alert(response.message);
                                break;
                        }
                        break;
                    default:
                        alert(response.message);
                        break;
                }
            }
        });
            }


    formAddHolder.on("submit", function (e) {
        e.preventDefault();
        var formData = formAddHolder.serialize();
        var request = $.ajax({
            url: "/admin/holders",
            type: "POST",
            dataType: "json",
            data: formData,
            success: function (response) {
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
        var formData = formPhone.serialize();

        var request = $.ajax({
            url: "/costumer/registration?handler=phone",
            type: "POST",
            dataType: "json",
            data: formData,
            success: function (response) {
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
        var formData = formCode.serialize();

        var request = $.ajax({
            url: "/costumer/registration?handler=code",
            type: "POST",
            dataType: "json",
            data: formData,
            success: function (response) {
                switch (response.code) {
                    case 200:
                        window.location.href = "/take";
                        break;
                    default:
                        alert(response.message);
                        break;
                }
            }
        });
    });

    cardForm.on("submit", function (e) {
        e.preventDefault();
        var formData = cardForm.serialize();

        var request = $.ajax({
            url: "/costumer/addcard?handler=card",
            type: "POST",
            dataType: "json",
            data: formData,
            success: function (response) {
                switch (response.code) {
                    case 200:
                        window.location.href = "/costumer/index";
                        break;
                    default:
                        alert(response.message);
                        break;
                }
            }
        });
    });
});

