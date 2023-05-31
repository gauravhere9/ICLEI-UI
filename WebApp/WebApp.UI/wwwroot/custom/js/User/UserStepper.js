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
                        if (validateStep1()) {
                            (e.goNext(), KTUtil.scrollTop())
                        }
                    } else if (xx == 1) {
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
                    addUserDetails();
                })),
                $(i.querySelector('[name="BranchId"]')).on("change",
                    (function () {
                        if ($("#ddlBranch").val() <= 0 || $("#ddlBranch").val() == "undefined") {
                            $("#errorBranch").text("Branch is required");
                        }
                        else {
                            $("#errorBranch").text("");
                        }
                    })),

                $(i.querySelector('[name="DesignationId"]')).on("change",
                    (function () {
                        if ($("#ddlDesignation").val() <= 0 || $("#ddlDesignation").val() == "undefined") {
                            $("#errorDesignation").text("Designation is required");
                        }
                        else {
                            $("#errorDesignation").text("");
                        }
                    })),

                $(i.querySelector('[name="UserTypeId"]')).on("change",
                    (function () {
                        if ($("#ddlUserType").val() <= 0 || $("#ddlUserType").val() == "undefined") {
                            $("#errorUserType").text("User type is required");
                        }
                        else {
                            $("#errorUserType").text("");
                        }
                    })),

                $(i.querySelector('[name="GenderId"]')).on("change",
                    (function () {
                        if ($("#ddlGender").val() <= 0 || $("#ddlGender").val() == "undefined") {
                            $("#errorGender").text("Gender is required");
                        }
                        else {
                            $("#errorGender").text("");
                        }
                    })),

                $(i.querySelector('[name="MaritalStatusId"]')).on("change",
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
                    }))
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

    return result;
}


var addUserDetails = function () {

    if (validateStep1()) {
        if (validateStep2()) {
            var myformdata = $("#addForm").serialize();

            console.log(myformdata);

            $.ajax({
                type: "POST",
                url: "/users/adduser",
                data: myformdata,
                success: function (result) {

                    if (result != null) {
                        if (result.statuscode != 200) {
                            ShowErrorSwal(result.message);
                        }
                        else {
                            ShowSuccessSwal("User is added successfully !", "/users?sortExpression=Id_Desc");
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