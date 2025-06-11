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

// Hàm hiển thị thông báo đăng ký thành công
function handleRegisterSuccess() {
    showToastrNotification('success', 'Đăng ký thành công! Vui lòng đăng nhập.');
}

// Hàm hiển thị thông báo đăng nhập thành công
function handleLoginSuccess(username) {
    showToastrNotification('success', `Đăng nhập thành công! Chào mừng ${username}!`);
}
