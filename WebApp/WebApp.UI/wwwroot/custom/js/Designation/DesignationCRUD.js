// LAUNCH THE ADD OR UPDATE POPUP
var AddOrUpdate = function (id) {

    resetControls();

    var url = "/Designations/" + id;
    if (id > 0) {
        $('#title').html("Edit Designations");
    }

    $("#designationModelBody").load(url, function (response, status, xhr) {
        if (status == "error") {
            var msg = "Sorry but there was an error: ";
            /* $("#error").html(msg + xhr.status + " " + xhr.statusText);*/

            ShowErrorSwal(msg + xhr.status + " " + xhr.statusText);
        }
        else {
            $("#kt_modal_designation").modal("show");
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
            url: "/designations/adddesignation",
            data: myformdata,
            success: function (result) {

                if (result != null) {
                    if (result.statuscode != 200) {
                        ShowErrorSwal(result.message);
                    }
                    else {
                        $("#kt_modal_designation").modal("hide");
                        ShowSuccessSwal("Designation is added successfully !", "/designations?sortExpression=Id_Desc");
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
var deleteDesignation = function (id) {
    if (id > 0) {
        $.ajax({
            type: "DELETE",
            url: "/designations/" + id,
            success: function (result) {

                if (result != null) {
                    if (result.statuscode != 200) {
                        ShowErrorSwal(result.message);
                    }
                    else {
                        $("#kt_modal_designation").modal("hide");

                        ShowSuccessSwal("Designation is deleted successfully !", "/designations");
                    }
                }
            },
            error: function (errormessage) {


                ShowErrorSwal("Error");
            }
        });
    }
    else {
        ShowErrorSwal("Designation id is not valid");
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
                t.value ? deleteDesignation(id) :
                    "cancel" === t.dismiss
            }));
    } else {
        ShowErrorSwal("Designation id is not valid");
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
                t.value ? changeDesignationStatus(id) :
                    "cancel" === t.dismiss
            }));
    } else {
        ShowErrorSwal("Designation id is not valid");
    }
}

// CHANGE THE RECORD STATUS
var changeDesignationStatus = function (id) {
    if (id > 0) {
        $.ajax({
            type: "PATCH",
            url: "/designations/" + id,
            success: function (result) {

                if (result != null) {
                    if (result.statuscode != 200) {
                        ShowErrorSwal(result.message);
                    }
                    else {
                        $("#kt_modal_designation").modal("hide");

                        ShowSuccessSwal("Designation status is changed successfully !", "/designations");
                    }
                }
            },
            error: function (errormessage) {
                ShowErrorSwal("Error");
            }
        });
    }
    else {
        ShowErrorSwal("Designation id is not valid");
    }
}

// UPDATE THE RECORD
var updateDesignation = function (id) {

    if (id > 0) {

        if (validations()) {

            var myformdata = $("#updateDesignationForm").serialize();

            $.ajax({
                type: "PUT",
                url: "/designations/updatedesignation",
                data: myformdata,
                success: function (result) {

                    if (result != null) {
                        if (result.statuscode != 200) {
                            ShowErrorSwal(result.message);
                        }
                        else {

                            $("#kt_modal_designation").modal("hide");
                            ShowSuccessSwal("Designation is updated successfully !", "/designations");
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
        ShowErrorSwal("Designation id is not valid");
    }
};

