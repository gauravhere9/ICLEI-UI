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



