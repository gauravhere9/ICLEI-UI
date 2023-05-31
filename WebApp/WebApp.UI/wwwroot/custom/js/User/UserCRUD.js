// LAUNCH THE ADD OR UPDATE POPUP
var AddOrUpdate = function (id) {

    resetControls();

    var url = "/Users/" + id;
    if (id > 0) {
        $('#title').html("Edit User");
    }

    $("#userModelBody").load(url, function (response, status, xhr) {
        if (status == "error") {
            var msg = "Sorry but there was an error: ";

            ShowErrorSwal(msg + xhr.status + " " + xhr.statusText);
        }
        else {
            $("#kt_modal_user").modal("show");
        }

    });
};

// RESET THE CONTROLS
var resetControls = function (id) {
    $("#Name").val('');
    $("#Description").val('');

    $("#errorName").text('');
    $("#errorDescription").text('');

    if (id > 0) {
        $("#Id").val(0);
    }
}

// ON SUBMIT VALIDATIONS
var validations = function () {
    var isValid = true;

    if ($.trim($('#Name').val()) === '') {
        ShowErrorSwal("Designation code is required");
        $("#errorName").text("Designation code is required");
        isValid = false;
    }
    else {
        $("#errorName").text("");
    }

    if ($.trim($('#Description').val()) == '') {
        ShowErrorSwal("Description is required");
        $("#errorDescription").text("Description is required");
        isValid = false;
    }
    else {
        $("errorDescription").text("");
    }

    return isValid;
}

// ADD THE RECORD
$('body').on('click', "#btnSubmit", function (e) {
    e.preventDefault();
    if (validations()) {

        var myformdata = $("#addForm").serialize();

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
                        $("#kt_modal_user").modal("hide");
                        ShowSuccessSwal("User is added successfully !", "/users?sortExpression=Id_Desc");
                    }
                }
            },
            error: function (errormessage) {
                ShowErrorSwal("Error");
            }
        });
    }

});

//DELETE THE RECORD
var deleteUser = function (id) {
    if (id > 0) {
        $.ajax({
            type: "DELETE",
            url: "/users/" + id,
            success: function (result) {

                if (result != null) {
                    if (result.statuscode != 200) {
                        ShowErrorSwal(result.message);
                    }
                    else {
                        $("#kt_modal_user").modal("hide");

                        ShowSuccessSwal("User is deleted successfully !", "/users");
                    }
                }
            },
            error: function (errormessage) {


                ShowErrorSwal("Error");
            }
        });
    }
    else {
        ShowErrorSwal("User id is not valid");
    }
}

//DELETE THE RECORD - CONFIRMATION
var deleteConfirmation = function (id) {

    if (id > 0) {

        Swal.fire({

            text: "Are you sure you want to delete?",
            icon: "warning",
            showCancelButton: !0,
            buttonsStyling: !1,
            confirmButtonText: "Yes, delete!",
            cancelButtonText: "No, cancel",
            customClass: { confirmButton: "btn fw-bold btn-danger", cancelButton: "btn fw-bold btn-active-light-primary" }
        })
            .then((function (t) {
                t.value ? deleteUser(id) :
                    "cancel" === t.dismiss
            }));
    } else {
        ShowErrorSwal("User id is not valid");
    }
}

// CHANGE THE RECORD STATUS - CONFIRMATION
var changeStatusConfirmation = function (id) {

    if (id > 0) {
        Swal.fire({
            text: "Are you sure you want to change status?",
            icon: "warning",
            showCancelButton: !0,
            buttonsStyling: !1,
            confirmButtonText: "Yes, do it!",
            cancelButtonText: "No, cancel",
            customClass: { confirmButton: "btn fw-bold btn-danger", cancelButton: "btn fw-bold btn-active-light-primary" }
        })
            .then((function (t) {
                t.value ? changeUserStatus(id) :
                    "cancel" === t.dismiss
            }));
    } else {
        ShowErrorSwal("User id is not valid");
    }
}

// CHANGE THE RECORD STATUS
var changeUserStatus = function (id) {
    if (id > 0) {
        $.ajax({
            type: "PATCH",
            url: "/users/" + id,
            success: function (result) {

                if (result != null) {
                    if (result.statuscode != 200) {
                        ShowErrorSwal(result.message);
                    }
                    else {
                        $("#kt_modal_user").modal("hide");

                        ShowSuccessSwal("User status is changed successfully !", "/users");
                    }
                }
            },
            error: function (errormessage) {
                ShowErrorSwal("Error");
            }
        });
    }
    else {
        ShowErrorSwal("User id is not valid");
    }
}

// UPDATE THE RECORD
var updateUser = function (id) {

    if (id > 0) {

        if (validations()) {

            var myformdata = $("#updateForm").serialize();

            $.ajax({
                type: "PUT",
                url: "/users/updateuser",
                data: myformdata,
                success: function (result) {

                    if (result != null) {
                        if (result.statuscode != 200) {
                            ShowErrorSwal(result.message);
                        }
                        else {

                            $("#kt_modal_user").modal("hide");
                            ShowSuccessSwal("User is updated successfully !", "/users");
                        }
                    }
                },
                error: function (errormessage) {
                    ShowErrorSwal("Error");
                }
            });
        }
    }
    else {
        ShowErrorSwal("User id is not valid");
    }
};


$('body').on('change', "#ddlBranch", function (e) {
    e.preventDefault();

    var branchId = $("#ddlBranch").val();

    console.log(branchId);

    if (branchId > 0) {
        $.ajax({
            type: "GET",
            url: "/users/" + branchId + "/reporting-to",
            success: function (result) {

                console.log(result);

                if (result != null) {
                    if (result.statusCode != 200) {
                        ShowErrorSwal(result.message);
                    }
                    else {
                        $.each(result.data, function (index, value) {
                            $("#ddlReportingTo").append($("<option></option>").val(this.dataValue).html(this.dataText));
                        });
                    }
                }
            },
            error: function (errormessage) {
                ShowErrorSwal("Error");
            }
        });
    }
});