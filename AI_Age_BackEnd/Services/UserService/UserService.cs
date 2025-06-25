using AI_Age_BackEnd.DTOs.UserDTO;
using AI_Age_BackEnd.Repositories.Interfaces;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace AI_Age_BackEnd.Services.UserService
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly Cloudinary _cloudinary;

        public UserService(IUserRepository userRepository, Cloudinary cloudinary)
        {
            _userRepository = userRepository;
            _cloudinary = cloudinary;
        }

        public async Task<UserProfileDto?> GetUserProfileByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            return new UserProfileDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Address = user.Address,
                Avatar = user.Avatar,
                RoleName = user.Role?.RoleName ?? "N/A"
            };
        }

        public async Task<UserProfileDto?> UpdateUserProfileAsync(int userId, UserUpdateDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;
            user.DateOfBirth = dto.DateOfBirth;
            user.Gender = dto.Gender;
            user.Address = dto.Address;
            user.LastLogin = DateTime.Now;

            // Upload ảnh đại diện mới nếu có
            if (dto.AvatarFile != null)
            {
                user.Avatar = await UploadImageToCloudinary(dto.AvatarFile);
            }

            // Cập nhật mật khẩu mới nếu có
            if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            }

            await _userRepository.UpdateUserAsync(user);

            return await GetUserProfileByIdAsync(userId);
        }

        private async Task<string> UploadImageToCloudinary(IFormFile image)
        {
            if (image == null || image.Length == 0) return null;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(image.FileName, image.OpenReadStream()),
                Transformation = new Transformation().Width(300).Height(300).Crop("fill").Gravity("face")
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
            {
                throw new Exception($"Upload ảnh thất bại: {uploadResult.Error.Message}");
            }
            return uploadResult.SecureUrl.ToString();
        }
    }
}
