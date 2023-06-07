// LAUNCH THE ADD OR UPDATE POPUP
var AddOrUpdate = function (id) {

    resetControls();

    var url = "/fundingagency/" + id;
    if (id > 0) {
        $('#title').html("Edit Funding Agency");
    }

    $("#fundingAgencyModelBody").load(url, function (response, status, xhr) {
        if (status == "error") {
            var msg = "Sorry but there was an error: ";
            /* $("#error").html(msg + xhr.status + " " + xhr.statusText);*/

            ShowErrorSwal(msg + xhr.status + " " + xhr.statusText);
        }
        else {
            $("#kt_modal").modal("show");
        }

    });
};

// RESET THE CONTROLS
var resetControls = function (id) {
    $("Name").val('');
    $("Description").val('');
    $("ContactPerson").val('');
    $("Email").val('');
    $("Phone").val('');
    $("Address").val('');

    $("errorName").text('');
    $("errorContactPerson").text('');
    $("errorAddress").text('');

    if (id > 0) {
        $("#Id").val(0);
    }
}

// ON SUBMIT VALIDATIONS
var validations = function () {
    var isValid = true;

    if ($.trim($('#Name').val()) === '') {

        $("#errorName").text("Name is required");
        isValid = false;
    }
    else {
        $("#errorName").text("");
    }

    if ($.trim($('#ContactPerson').val()) === '') {
        $("#errorContactPerson").text("Contact person is required");
        isValid = false;
    }
    else {
        $("#errorContactPerson").text("");
    }

    if ($.trim($('#Address').val()) == '') {
        $("#errorAddress").text("Address is required");
        isValid = false;
    }
    else {
        $("#errorAddress").text("");
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
            url: "/fundingagency/addfundingagency",
            data: myformdata,
            success: function (result) {

                if (result != null) {
                    if (result.statuscode != 200) {
                        ShowErrorSwal(result.message);
                    }
                    else {
                        $("#kt_modal").modal("hide");
                        ShowSuccessSwal("Funding agency is added successfully !", "/fundingagency?sortExpression=Id_Desc");
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
var deleteRecord = function (id) {
    if (id > 0) {
        $.ajax({
            type: "DELETE",
            url: "/fundingagency/" + id,
            success: function (result) {

                if (result != null) {
                    if (result.statuscode != 200) {
                        ShowErrorSwal(result.message);
                    }
                    else {
                        $("#kt_modal").modal("hide");

                        ShowSuccessSwal("Funding agency is deleted successfully !", "/fundingagency");
                    }
                }
            },
            error: function (errormessage) {

                ShowErrorSwal("Error");
            }
        });
    }
    else {
        ShowErrorSwal("Funding agency id is not valid");
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
                t.value ? deleteRecord(id) :
                    "cancel" === t.dismiss
            }));
    } else {
        ShowErrorSwal("Funding agency id is not valid");
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
                t.value ? changeStatus(id) :
                    "cancel" === t.dismiss
            }));
    } else {
        ShowErrorSwal("Funding agency id is not valid");
    }
}

// CHANGE THE RECORD STATUS
var changeStatus = function (id) {
    if (id > 0) {
        $.ajax({
            type: "PATCH",
            url: "/fundingagency/" + id,
            success: function (result) {

                if (result != null) {
                    if (result.statuscode != 200) {
                        ShowErrorSwal(result.message);
                    }
                    else {
                        $("#kt_modal").modal("hide");

                        ShowSuccessSwal("Funding agency status is changed successfully !", "/fundingagency");
                    }
                }
            },
            error: function (errormessage) {
 
                ShowErrorSwal("Error");
            }
        });
    }
    else {
        ShowErrorSwal("Funding agency id is not valid");
    }
}

// UPDATE THE RECORD
var updateRecord = function (id) {

    if (id > 0) {

        if (validations()) {

            var myformdata = $("#updateForm").serialize();

            $.ajax({
                type: "PUT",
                url: "/fundingagency/updatefundingagency",
                data: myformdata,
                success: function (result) {

                    if (result != null) {
                        if (result.statuscode != 200) {
                            ShowErrorSwal(result.message);
                        }
                        else {

                            $("#kt_modal").modal("hide");
                            ShowSuccessSwal("Funding agency is updated successfully !", "/fundingagency");
                        }
                    }
                },
                error: function (errormessage) {

                    console.log(errormessage);
                    ShowErrorSwal("Error");
                }
            });
        }
    }
    else {
        ShowErrorSwal("Funding agency id is not valid");
    }
};

