function showToastrNotification(type, message) {
    toastr.options = {
        "closeButton": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000"
    };

    if (type === 'success') {
        toastr.success(message);
    } else if (type === 'error') {
        toastr.error(message);
    }
}

function handleUpdateAIToolCategorySuccess() {
    showToastrNotification('success', 'Cập nhật thành công.');
}

function handleUpdateAIToolCategoryError() {
    showToastrNotification('error', 'Cập nhật không thành công.');
}