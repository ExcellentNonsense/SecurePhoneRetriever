namespace SecurePhoneRetriever.Model {
    internal interface IPNRepository {
        string Read();
        bool Write(string phoneNumber);
    }
}
