namespace SecurePhoneRetriever.Common {
    public interface IAppSettings {
        byte MinPNLength { get; }
        byte MaxPNLength { get; }
        byte StepsQuantityForDisplayPN { get; }
    }
}
