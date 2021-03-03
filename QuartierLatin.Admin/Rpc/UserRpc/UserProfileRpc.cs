using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using QuartierLatin.Admin.Dto;
using QuartierLatin.Admin.Managers;
using QuartierLatin.Admin.Models.Repositories;
using QuartierLatin.Admin.Utils;
using SkiaSharp;

namespace QuartierLatin.Admin.Rpc.UserRpc
{
    public class UserProfileRpc : UserRpcBase
    {
        private readonly IUserRepository _users;
        private readonly BlobManager _imageStorage;

        public UserProfileRpc(IUserRepository users, BlobManager imageStorage)
        {
            _users = users;
            _imageStorage = imageStorage;
        }

        public UserProfileDto GetProfile() => UserProfileDto.FromUser(User);

        public Result UpdateProfile(UserProfileDto profile)
        {
            var user = User;
            user.Name = profile.Name;

            return Result.Catch(() => _users.Update(user), ErrorCode.DatabaseError);
        }

        public async Task<Result> UploadUserPhoto(List<byte> data)
        {
            var user = User;

            if (user.AvatarImage != null)
            {
                var removeUserPhoto = RemoveUserPhoto();
                if (!removeUserPhoto.Success) return removeUserPhoto;
            }

            using var bitmap = SKBitmap.Decode(data.ToArray());
            using var scaledBitmap = bitmap.Resize(new SKSizeI(256, 256), SKFilterQuality.High);
            using var image = SKImage.FromBitmap(scaledBitmap);
            using var skStream = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = skStream.AsStream();

            var blobId = await _imageStorage.CreateBlob(stream);
            user.AvatarImage = blobId;

            return Result.Catch(() => _users.Update(user), ErrorCode.DatabaseError);
        }

        public Result RemoveUserPhoto()
        {
            var user = User;
            if (User.AvatarImage == 0) return Result.Succeeded;

            return Result.Catch(() =>
            {
                _imageStorage.RemoveBlob(User.AvatarImage).GetAwaiter().GetResult();
                user.AvatarImage = 0;
                _users.Update(user);
            }, ErrorCode.DatabaseError);
        }
    }
}