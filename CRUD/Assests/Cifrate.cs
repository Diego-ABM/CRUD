using System.Security.Cryptography;
using System.Text;

namespace CRUD.Assests
{
    public class Cifrate
    {
        // Cifra la contraseña con el algoritmo SHA-256
        public static string PasswordToSha256(string password)
        {
            // Crea una instancia de SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computa el hash de los datos
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convierte el array de bytes a un string hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    // X define qie se convierte el numero a HEX y que las letras sean en mayusculas
                    // el 2 especifica que el numero HEX debe ser representado con 2 digitos
                    builder.Append(bytes[i].ToString("X2"));
                }
                return builder.ToString();
            }
        }
    }
}
