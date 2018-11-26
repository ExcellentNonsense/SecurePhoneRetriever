using System.Collections.Generic;

namespace SecurePhoneRetriever.Model {
    internal interface IPNValidator {
        IList<string> Validate(string phoneNumber);
    }
}
