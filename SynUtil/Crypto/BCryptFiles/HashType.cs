//Source copied from from https://github.com/BcryptNet/bcrypt.net

namespace BCrypt.Net
{
    public enum HashType
    {
        None = -1,
        SHA256 = 0,
        SHA384 = 1,
        SHA512 = 2,
        /// <summary>
        /// Will hash key using SHA384 but will not Base64 encode it prior to Crypt
        /// </summary>
        Legacy384 = 3
    }
}