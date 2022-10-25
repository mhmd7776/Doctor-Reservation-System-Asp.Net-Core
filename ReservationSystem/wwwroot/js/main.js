function SubmitFilterFormPagination(pageId) {
    $("#CurrentPage").val(pageId);
    $("#FilterForm").submit();
}

var inputImage = document.getElementById("DoctorAvatarInput");
var previewBox = document.getElementById("DoctorAvatarPreview");

if (inputImage !== null) {
    inputImage.onchange = event => {
        const [file] = inputImage.files;
        if (file) {
            previewBox.src = URL.createObjectURL(file);
        }
    }
}

function StartLoading(selector = 'body') {
    $(selector).waitMe({
        effect: 'bounce',
        text: 'لطفا صبر کنید ...',
        bg: 'rgba(255, 255, 255, 0.7)',
        color: '#000'
    });
}

function EndLoading(selector = 'body') {
    $(selector).waitMe('hide');
}

$("#ChooseAvatarImageBtn").on("click", function (event) {
    inputImage.click();
});

function OpenCreateDoctorTimingModal(doctorId) {
    $.ajax({
        url: `/LoadCreateDoctorTimingPartial/${doctorId}`,
        type: "get",
        beforeSend: function () {
            StartLoading();
        },
        success: function (response) {
            EndLoading();
            $("#CreateDoctorTimingModalBody").html(response);
            $('#CreateDoctorTimingForm').removeData('validator', 'unobtrusiveValidation');
            $.validator.unobtrusive.parse('#CreateDoctorTimingForm');
            $("#CreateDoctorTimingModal").modal("show");
        },
        error: function () {
            EndLoading();
            swal.fire({
                title: 'خطا',
                text: 'عملیات با خطا مواجه شد لطفا مجدد تلاش کنید',
                icon: 'error',
                confirmButtonText: 'باشه'
            });
        }
    });
}

function CreateDoctorTimingDone(response) {
    EndLoading("#CreateDoctorTimingModalBody");
    if (response.status === "Success") {
        swal.fire({
            title: 'اعلان',
            text: response.message,
            icon: 'success',
            confirmButtonText: 'باشه'
        });
        $("#DoctorTimingListBox").load(location.href + " #DoctorTimingListBox");
        $("#CreateDoctorTimingModal").modal("hide");
    }
    else {
        swal.fire({
            title: 'خطا',
            text: response.message,
            icon: 'error',
            confirmButtonText: 'باشه'
        });
    }
}

function DeleteDoctor(doctorId) {
    Swal.fire({
        title: 'از انجام عملیات اطمینان دارید؟',
        text: "در صورت حذف قادر به بازگردانی آن نمی باشید",
        icon: 'question',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonText: 'بله',
        cancelButtonText: 'انصراف'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/DeleteDoctor',
                data: {
                    doctorId: doctorId
                },
                type: "post",
                beforeSend: function () {
                    StartLoading();
                },
                success: function (response) {
                    EndLoading();
                    if (response.status === "Success") {
                        swal.fire({
                            title: 'اعلان',
                            text: response.message,
                            icon: 'success',
                            confirmButtonText: 'باشه'
                        });
                        $("#DoctorsListBox").load(location.href + " #DoctorsListBox");
                    }
                    else {
                        swal.fire({
                            title: 'خطا',
                            text: response.message,
                            icon: 'error',
                            confirmButtonText: 'باشه'
                        });
                    }
                },
                error: function () {
                    EndLoading();
                    swal.fire({
                        title: 'خطا',
                        text: 'عملیات با خطا مواجه شد لطفا مجدد تلاش کنید',
                        icon: 'error',
                        confirmButtonText: 'باشه'
                    });
                }
            });
        }
    });
}

function DeleteDoctorTiming(timingId) {
    Swal.fire({
        title: 'از انجام عملیات اطمینان دارید؟',
        text: "در صورت حذف قادر به بازگردانی آن نمی باشید",
        icon: 'question',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonText: 'بله',
        cancelButtonText: 'انصراف'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/DeleteDoctorTiming',
                data: {
                    timingId: timingId
                },
                type: "post",
                beforeSend: function () {
                    StartLoading();
                },
                success: function (response) {
                    EndLoading();
                    if (response.status === "Success") {
                        swal.fire({
                            title: 'اعلان',
                            text: response.message,
                            icon: 'success',
                            confirmButtonText: 'باشه'
                        });
                        $("#DoctorTimingListBox").load(location.href + " #DoctorTimingListBox");
                    }
                    else {
                        swal.fire({
                            title: 'خطا',
                            text: response.message,
                            icon: 'error',
                            confirmButtonText: 'باشه'
                        });
                    }
                },
                error: function () {
                    EndLoading();
                    swal.fire({
                        title: 'خطا',
                        text: 'عملیات با خطا مواجه شد لطفا مجدد تلاش کنید',
                        icon: 'error',
                        confirmButtonText: 'باشه'
                    });
                }
            });
        }
    });
}

function OpenFinalizeReservationModal(doctorId, date) {
    $.ajax({
        url: `/LoadFinalReservationPartial`,
        type: "get",
        data: {
            doctorId: doctorId,
            date: date
        },
        beforeSend: function () {
            StartLoading();
        },
        success: function (response) {
            EndLoading();
            if (response.status === "Failed") {
                swal.fire({
                    title: 'هشدار',
                    text: response.message,
                    icon: 'warning',
                    confirmButtonText: 'باشه'
                });
                return;
            }
            $("#FinalizeReservationModalBody").html(response);
            $('#FinalReservationForm').removeData('validator', 'unobtrusiveValidation');
            $.validator.unobtrusive.parse('#FinalReservationForm');
            $("#FinalizeReservationModal").modal("show");
        },
        error: function () {
            EndLoading();
            swal.fire({
                title: 'خطا',
                text: 'عملیات با خطا مواجه شد لطفا مجدد تلاش کنید',
                icon: 'error',
                confirmButtonText: 'باشه'
            });
        }
    });
}

function FinalReservationDone(response) {
    EndLoading('#FinalizeReservationModalBody');
    if (response.status === "Failed") {
        swal.fire({
            title: 'هشدار',
            text: response.message,
            icon: 'warning',
            confirmButtonText: 'باشه'
        });
        return;
    }
    $("#FinalReservationMainBox").html(response);
}