using System.Security.Cryptography;

namespace SchoolHub.Helpers
{
    public static class UserUtility
    {
        public static string GenerateUserName(int id, string paternalSurname, string maternalSurname)
        {
            paternalSurname = paternalSurname.Replace(" ", "");
            maternalSurname = maternalSurname.Replace(" ", "");

            string partPaternal = paternalSurname.Length >= 4 ? paternalSurname[..4] : paternalSurname;
            string partMaternal = maternalSurname.Length >= 4 ? maternalSurname[..4] : maternalSurname;

            var username = string.Concat(partPaternal, "s", id.ToString(), partMaternal);

            return username.ToLower();
        }

        public static string GeneratePassword()
        {
            byte[] passBytes = new byte[8];

            RandomNumberGenerator.Fill(passBytes);

            string temporaryPassword = Convert.ToBase64String(passBytes);

            int randomDigit = RandomNumberGenerator.GetInt32(0, 10);

            return $"{temporaryPassword}{randomDigit}";
        }
    }
}