using MlkPwgen;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace API.Models
{
    public class AuthUser
    {
        [JsonProperty(PropertyName = "username")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        private static Dictionary<string, string> PasswordResetTokens  = new Dictionary<string, string>();

        public static AuthUser Authenticate(string email, string password)
        {
            
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT email, password FROM auth_view WHERE email = @email;", conn))
                    {
                    cmd.Parameters.AddWithValue("@email", email);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.Read()) return null;

                    if (!BCrypt.Net.BCrypt.Verify(password, reader.GetString("password"))){
                        return null;
                    }

                    return AuthUser.FromDB(reader);
                }
            }
        }

        public static bool UserExists(string email)
        {
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT email, password FROM auth_view WHERE email = @email", conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.Read()) return false;

                    return true;
                }
            }
        }

        public static int RequestPasswordReset(string username)
        {
            string email;
            string name;
            /*VERIFICA SE O UTILIZADOR EXISTE*/
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM staff_view WHERE email = @email", conn))
                {
                    cmd.Parameters.AddWithValue("@email", username);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.Read()) return -1;

                    email = reader["email"].ToString();
                    name = reader["nome"].ToString();

                }
                conn.Close();
            }

            string token = PasswordGenerator.Generate(length: 64, allowed: Sets.Alphanumerics);

            if (PasswordResetTokens.ContainsKey(email))
            {
                PasswordResetTokens.Remove(email);
            }
            PasswordResetTokens.Add(email, token);


            return SMTPClient.ResetPasswordEmail(name, email, token) == false ? 0 : 1;
        }

        public static int ChangePassword(string token, string newPassword)
        {
            if (!PasswordResetTokens.ContainsValue(token))
            {
                return -2;
            }

            string email = PasswordResetTokens.Keys.FirstOrDefault(s => PasswordResetTokens[s] == token);

            int idStaff;
           
            /*VERIFICA SE O UTILIZADOR EXISTE*/
            using (MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.DB))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM staff_view WHERE email = @email", conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (!reader.Read()) return -1;

                    idStaff = int.Parse(reader["idStaff"].ToString());


                }
                conn.Close();

                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("UPDATE `staff` SET password = @password WHERE idStaff = @idStaff", conn))
                {
                    cmd.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(newPassword,13));
                    cmd.Parameters.AddWithValue("@idStaff", idStaff);
                    int nrRows = cmd.ExecuteNonQuery();
                    if (nrRows <= 0) return 0;
                    
        
                }
                conn.Close();
            }

            /*SENDS CONFIRMATION EMAIl*/
           SMTPClient.PasswordChangedEmail(email);

            return 1;
        }
        
        public static AuthUser FromDB(MySqlDataReader reader)
        {
            string email = reader["email"].ToString();
            string password = reader["password"].ToString();

            return new AuthUser()
            {
               Email = email,
               Password = password
            };
        }
    }
}