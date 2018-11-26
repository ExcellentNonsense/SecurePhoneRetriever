namespace SecurePhoneRetriever.Model {
    internal interface IPNCoder {
        string Decode(string phoneNumber);
        string Encode(string phoneNumber);
    }
}
