using System;
using System.IO;

namespace SecurePhoneRetriever.Model {
    internal sealed class PNRepository : IPNRepository {
        private readonly string supplementFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".supplement");

        public string Read() {
            string phoneNumber = String.Empty;

            try {
                using (StreamReader file = new StreamReader(supplementFilePath)) {
                    phoneNumber = file.ReadToEnd();
                }
            }
            catch (Exception ex) {
                if (ex is FileNotFoundException ||
                    ex is DirectoryNotFoundException ||
                    ex is IOException) {  }
                else { throw; }
            }

            return phoneNumber;
        }

        public bool Write(string phoneNumber) {
            bool success = false;
            try {
                using (StreamWriter file = new StreamWriter(supplementFilePath)) {
                    file.Write(phoneNumber);
                }
                success = true;
            }
            catch (Exception ex) {
                if (ex is UnauthorizedAccessException ||
                    ex is DirectoryNotFoundException ||
                    ex is PathTooLongException ||
                    ex is IOException) { }
                else { throw; }
            }

            return success;
        }
    }
}
