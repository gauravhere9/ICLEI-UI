"use strict";
var KTCreateAccount = function () {
    var e, t, i, o, a, r, s = [];
    return {
        init: function () {
            (e = document.querySelector("#kt_add_user")), (t = document.querySelector("#kt_create_account_stepper")) && (i = t.querySelector("#kt_create_account_form"),
                o = t.querySelector('[data-kt-stepper-action="submit"]'),
                a = t.querySelector('[data-kt-stepper-action="next"]'),
                (r = new KTStepper(t)).on("kt.stepper.changed",
                    (function (e) {
                        4 === r.getCurrentStepIndex()
                            ? (o.classList.remove("d-none"), o.classList.add("d-inline-block"), a.classList.add("d-none"))
                            : 5 === r.getCurrentStepIndex()
                                ? (o.classList.add("d-none"), a.classList.add("d-none"))
                                : (o.classList.remove("d-inline-block"),
                                    o.classList.remove("d-none"), a.classList.remove("d-none"))
                    })),
                r.on("kt.stepper.next", (function (e) {
                    var xx = e.getCurrentStepIndex() - 1;
                    if (xx == 0) {
                        // Validate Official Information
                        if (validateStep1()) {
                            (e.goNext(), KTUtil.scrollTop())
                        }

                        //Reset STEP 2 Error Controls
                        ResetStep2Errors();

                    } else if (xx == 1) {

                        // Validate Personal Information
                        if (validateStep2()) {
                            (e.goNext(), KTUtil.scrollTop())
                        }
                    }
                })),
                r.on("kt.stepper.previous", (function (e) {
                    e.goPrevious(), KTUtil.scrollTop()
                })),
                o.addEventListener("click", (function (e) {
                    e.preventDefault();
                    addORUpdateUserDetails();
                })),
                $("#ddlBranch").on("change",
                    (function () {
                        if ($("#ddlBranch").val() <= 0 || $("#ddlBranch").val() == "undefined") {
                            $("#errorBranch").text("Branch is required");
                        }
                        else {
                            $("#errorBranch").text("");
                        }
                    })),

                $("#ddlDesignation").on("change",
                    (function () {
                        if ($("#ddlDesignation").val() <= 0 || $("#ddlDesignation").val() == "undefined") {
                            $("#errorDesignation").text("Designation is required");
                        }
                        else {
                            $("#errorDesignation").text("");
                        }
                    })),

                $("#ddlUserType").on("change",
                    (function () {
                        if ($("#ddlUserType").val() <= 0 || $("#ddlUserType").val() == "undefined") {
                            $("#errorUserType").text("User type is required");
                        }
                        else {
                            $("#errorUserType").text("");
                        }
                    })),

                $("#ddlGender").on("change",
                    (function () {
                        if ($("#ddlGender").val() <= 0 || $("#ddlGender").val() == "undefined") {
                            $("#errorGender").text("Gender is required");
                        }
                        else {
                            $("#errorGender").text("");
                        }
                    })),

                $("#ddlMaritalStatus").on("change",
                    (function () {
                        if ($("#ddlMaritalStatus").val() > 0) {

                            if ($("#ddlMaritalStatus").val() == 1) {
                                $("#lblSpouseName").show();
                                $("#SpouseName").show();
                            }
                            else if ($("#ddlMaritalStatus").val() == 2) {
                                $("#lblSpouseName").hide();
                                $("#SpouseName").hide();
                            }
                        }
                        else {
                            $("#lblSpouseName").hide();
                            $("#SpouseName").hide();
                        }
                    })),

                $("#DOB").flatpickr({ enableTime: !0, dateFormat: "d, M Y, H:i" })

            )
        }
    }
}(); KTUtil.onDOMContentLoaded((function () { KTCreateAccount.init() }));

var validateStep1 = function () {
    var result = true;

    if ($("#ddlBranch").val() <= 0 || $("#ddlBranch").val() == "undefined") {
        result = false;
        $("#errorBranch").text("Branch is required");

    }

    if ($("#ddlDesignation").val() <= 0 || $("#ddlDesignation").val() == "undefined") {
        result = false;
        $("#errorDesignation").text("Designation is required");

    }

    if ($("#ddlUserType").val() <= 0 || $("#ddlUserType").val() == "undefined") {
        result = false;
        $("#errorUserType").text("User type is required");

    }

    return result;
}

var validateStep2 = function () {
    var result = true;

    if ($("#Name").val().trim() == '' || $("#Name").val().trim() == "undefined") {
        result = false;
        $("#errorName").text("Name is required");
    }

    if ($("#EmployeeCode").val().trim() == '' || $("#EmployeeCode").val().trim() == "undefined") {
        result = false;
        $("#errorEmployeeCode").text("Employee code is required");
    }

    if ($("#Email").val().trim() == '' || $("#Email").val().trim() == "undefined") {
        result = false;
        $("#errorEmail").text("Email is required");
    }

    if ($("#ddlGender").val() <= 0 || $("#ddlGender").val() == "undefined") {
        result = false;
        $("#errorGender").text("Gender is required");

    }
 
    if ($("#Mobile").val().trim() === "") {
        $("#errorMobile").text("");
    }
    else {
        var mobileValue = $("#Mobile").val();

        var regex1 = new RegExp('^([+]\d{2}[ ])?\d{10}$');
        var regex2 = new RegExp('0{5,}');

        var isMatched1 = regex1.test(mobileValue);
        var isMatched2 = regex2.test(mobileValue);

        if (!isMatched1 || !isMatched2) {
            result = false;
            $("#errorMobile").text("Mobile/Phone number is not in a valid format");
        }
        else {
            $("#errorMobile").text("");
        }
    }

    if ($("#PAN").val().trim() === "") {
        $("#errorPAN").text("");
    }
    else {
        var panValue = $("#PAN").val();

        var regex1 = new RegExp('^[A-Z]{5}[0-9]{4}[A-Z]{1}$');
         
        var isMatched1 = regex1.test(panValue);
         
        if (!isMatched1) {
            result = false;
            $("#errorPAN").text("PAN is not in a valid format");
        }
        else {
            $("#errorPAN").text("");
        }
    }

    if ($("#AadharNo").val().trim() === "") {
        $("#errorAadharNo").text("");
    }
    else {
        var aadharValue = $("#AadharNo").val();

        var regex1 = new RegExp('^[2-9]{1}[0-9]{3}\\s[0-9]{4}\\s[0-9]{4}$');
         
        var isMatched1 = regex1.test(aadharValue);
         
        if (!isMatched1) {
            result = false;
            $("#errorAadharNo").text("Aadhar number is not in a valid format");
        }
        else {
            $("#errorAadharNo").text("");
        }
    }
 
    return result;
}

var ResetStep2Errors = function () {
    $("#errorName").text("");
    $("#errorEmployeeCode").text("");
    $("#errorEmail").text("");
    $("#errorGender").text("");
}

var addORUpdateUserDetails = function () {
    if (validateStep1()) {
        if (validateStep2()) {
            var myformdata = $("#userForm").serialize();

            var _url = "";
            var _method = "";
            var _successMessage = "";

            var id = $("#Id").val();

            if (id != "undefined" || id > 0) {
                _url = "/users/updateuser";
                _method = "PUT";
                _successMessage = "User is updated successfully !";
            }
            else {
                _url = "/users/adduser";
                _method = "POST";
                _successMessage = "User is added successfully !";
            }
 
            $.ajax({
                type: _method,
                url: _url,
                data: myformdata,
                success: function (result) {
                    if (result != null) {
                        if (result.statuscode != 200) {
                            ShowErrorSwal(result.message);
                        }
                        else {
                            ShowSuccessSwal(_successMessage, "/users?sortExpression=Id_Desc");
                        }
                    }
                },
                error: function (errormessage) {
                    ShowErrorSwal("Error");
                }
            });
        }
    }
}