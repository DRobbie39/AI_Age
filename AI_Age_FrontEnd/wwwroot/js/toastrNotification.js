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

function handleRegisterSuccess() {
    showToastrNotification('success', 'Đăng ký thành công! Vui lòng đăng nhập.');
}

function handleLoginSuccess(username) {
    showToastrNotification('success', `Đăng nhập thành công! Chào mừng ${username}!`);
}

function handleRegisterError(message) {
    showToastrNotification('error', 'Đăng ký thất bại. Vui lòng thử lại.');
}

function handleLoginError(message) {
    showToastrNotification('error', 'Đăng nhập thất bại. Vui lòng thử lại.');
}

function handleRatingSuccess(message) {
    showToastrNotification('success', 'Đánh giá thành công.');
}

function handleRatingError(message) {
    showToastrNotification('error', 'Đánh giá thất bại.');
}
