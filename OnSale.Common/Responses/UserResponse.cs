using OnSale.Common.Business;
using OnSale.Common.Entities;
using OnSale.Common.Enums;
using System;

namespace OnSale.Common.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string RFC { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string ImageId { get; set; }

        //public string ImageFullPath => ImageId == string.Empty
        //    ? $"{Constants.Path.PathNoImage}"
        //    : $"{ImageId}";

        public UserType UserType { get; set; }

        public City City { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string FullNameWithDocument => $"{FirstName} {LastName} - {RFC}";

        public string ImageFacebook { get; set; }

        public LoginType LoginType { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (LoginType == LoginType.Facebook && string.IsNullOrEmpty(ImageFacebook) ||
                    LoginType == LoginType.OnSale && ImageId == string.Empty)
                {
                    return $"{Constants.Path.PathNoImage}";
                }

                if (LoginType == LoginType.Facebook)
                {
                    return ImageFacebook;
                }

                return $"{ImageId}";
            }
        }

        public double Latitude { get; set; }

        public double Logitude { get; set; }

    }
}
