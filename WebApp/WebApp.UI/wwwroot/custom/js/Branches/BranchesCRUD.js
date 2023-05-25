var AddOrUpdate = function (id) {

    resetControls();

    var url = "/Branches/" + id;
    if (id > 0) {
        $('#title').html("Edit Branch");
    }
    else {
        $("#addModelBody").load(url, function (response, status, xhr) {
            if (status == "error") {
                var msg = "Sorry but there was an error: ";
                /* $("#error").html(msg + xhr.status + " " + xhr.statusText);*/

                ShowErrorSwal(msg + xhr.status + " " + xhr.statusText);
            }
            else {
                $("#kt_modal_branch").modal("show");
            }

        });
    }
};

var resetControls = function (id) {
    $("BranchCode").val('');
    $("Address").val('');
    $("Location").val('');

    $("errorBranchCode").text('');
    $("errorAddress").text('');
    $("errorLocation").text('');
    $("errorCompanyId").text('');

    if (id > 0) {
        $("#Id").val(0);
    }
}

$(document).ready(function () {
    $("#BranchCode").keyup(function () {
        if ($.trim($('#BranchCode').val()) === '') {
            $("#errorBranchCode").text("Branch code is required");
        }
        else {
            $("#errorBranchCode").text("");
        }
    });

    $("#Address").keyup(function () {
        if ($.trim($('#Address').val()) === '') {
            $("#errorAddress").text("Address is required");
        }
        else {
            $("#errorAddress").text("");
        }
    });


    $("#Location").keyup(function () {
        if ($.trim($('#Location').val()) === '') {
            $("#errorLocation").text("Location is required");
        }
        else {
            $("#errorLocation").text("");
        }
    });

});


var validations = function () {
    var isValid = true;

    if ($.trim($('#BranchCode').val()) === '') {

        $("#errorBranchCode").text("Branch code is required");
        isValid = false;
    }
    else {
        $("#errorBranchCode").text("");
    }

    if ($.trim($('#Address').val()) == '') {
        $("#errorAddress").text("Address is required");
        isValid = false;
    }
    else {
        $("#errorAddress").text("");
    }

    if ($.trim($('#Location').val()) === '') {
        $("#errorLocation").text("Location is required");
        isValid = false;
    }
    else {
        $("#errorLocation").text("");
    }

    $("#CompanyId").each(function () {
        if ($(this).attr("checked") != "checked") {
            $("#errorCompanyId").text("Please select the company");
            isValid = false;
        }
        else {
            $("#errorCompanyId").text("");
        }
    });


    return isValid;

}

$('body').on('click', "#btnSubmit", function () {

    if (validations()) {

        var myformdata = $("#addBranchForm").serialize();

        $.ajax({
            type: "POST",
            url: "/branches/addbranch",
            data: myformdata,
            success: function (result) {

                if (result != null) {
                    if (result.statuscode != 200) {
                        ShowErrorSwal(result.message);
                    }
                    else {
                        $("#kt_modal_branch").modal("hide");
                        ShowSuccessSwal("Branch is added successfully !", "/branches");
                    }
                }
            },
            error: function (errormessage) {
                console.log(errormessage.responseText);

                ShowErrorSwal("Error");
            }
        });
    }
});


var deleteBranch = function (id) {
    if (id > 0) {
        $.ajax({
            type: "DELETE",
            url: "/branches/" + id,
            success: function (result) {

                if (result != null) {
                    if (result.statuscode != 200) {
                        ShowErrorSwal(result.message);
                    }
                    else {
                        $("#kt_modal_branch").modal("hide");

                        ShowSuccessSwal("Branch is deleted successfully !", "/branches");
                    }
                }
            },
            error: function (errormessage) {
                console.log(errormessage.responseText);

                ShowErrorSwal("Error");
            }
        });
    }
    else {
        ShowErrorSwal("Branch id is not valid");
    }
}

var deleteConfirmation = function (id) {

    if (id > 0) {

        Swal.fire({
            //text: "Are you sure you want to delete?",
            //icon: "warning",
            //showCancelButton: !0,
            //buttonsStyling: !1,
            //confirmButtonText: "Yes, cancel it!",
            //cancelButtonText: "No, return",
            //customClass: { confirmButton: "btn btn-primary", cancelButton: "btn btn-active-light" }

            text: "Are you sure you want to delete?",
            icon: "warning",
            showCancelButton: !0,
            buttonsStyling: !1,
            confirmButtonText: "Yes, delete!",
            cancelButtonText: "No, cancel",
            customClass: { confirmButton: "btn fw-bold btn-danger", cancelButton: "btn fw-bold btn-active-light-primary" }
        })
            .then((function (t) {
                t.value ? deleteBranch(id) :


                    "cancel" === t.dismiss
                //&& Swal.fire({
                //    text: "Delete operation is cancelled!.", icon: "error", buttonsStyling: !1,
                //    confirmButtonText: "Ok, got it!", customClass: { confirmButton: "btn btn-primary" }
                //})
            }));
    } else {
        ShowErrorSwal("Branch id is not valid");
    }
}


var changeStatusConfirmation = function (id) {

    if (id > 0) {

        
        Swal.fire({
            //text: "Are you sure you want to delete?",
            //icon: "warning",
            //showCancelButton: !0,
            //buttonsStyling: !1,
            //confirmButtonText: "Yes, cancel it!",
            //cancelButtonText: "No, return",
            //customClass: { confirmButton: "btn btn-primary", cancelButton: "btn btn-active-light" }

            text: "Are you sure you want to change status?",
            icon: "warning",
            showCancelButton: !0,
            buttonsStyling: !1,
            confirmButtonText: "Yes, do it!",
            cancelButtonText: "No, cancel",
            customClass: { confirmButton: "btn fw-bold btn-danger", cancelButton: "btn fw-bold btn-active-light-primary" }
        })
            .then((function (t) {
                t.value ? changeBranchStatus(id) :


                    "cancel" === t.dismiss
                //&& Swal.fire({
                //    text: "Delete operation is cancelled!.", icon: "error", buttonsStyling: !1,
                //    confirmButtonText: "Ok, got it!", customClass: { confirmButton: "btn btn-primary" }
                //})
            }));
    } else {
        ShowErrorSwal("Branch id is not valid");
    }
}


var changeBranchStatus = function (id) {
    if (id > 0) {
        $.ajax({
            type: "PATCH",
            url: "/branches/" + id,
            success: function (result) {

                if (result != null) {
                    if (result.statuscode != 200) {
                        ShowErrorSwal(result.message);
                    }
                    else {
                        $("#kt_modal_branch").modal("hide");

                        ShowSuccessSwal("Branch status is changed successfully !", "/branches");
                    }
                }
            },
            error: function (errormessage) {
                console.log(errormessage.responseText);

                ShowErrorSwal("Error");
            }
        });
    }
    else {
        ShowErrorSwal("Branch id is not valid");
    }
}