document.addEventListener('DOMContentLoaded', function () {
    const logoUpload = document.getElementById('logoUpload');
    if (!logoUpload) return;

    const uploadLabel = document.getElementById('upload-label');
    const imagePreviewContainer = document.getElementById('image-preview-container');
    const imagePreview = document.getElementById('image-preview');
    const removeImageBtn = document.getElementById('remove-image-btn');

    logoUpload.addEventListener('change', function (e) {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (event) {
                imagePreview.src = event.target.result;
                imagePreviewContainer.classList.remove('d-none');
                uploadLabel.classList.add('d-none');
            }
            reader.readAsDataURL(file);
        }
    });

    removeImageBtn.addEventListener('click', function () {
        logoUpload.value = '';
        imagePreview.src = '#';
        imagePreviewContainer.classList.add('d-none');
        uploadLabel.classList.remove('d-none');
    });
});