using System;
using System.Collections.Generic;

namespace SecurePhoneRetriever.Model {
    public interface IPNManager {
        event Action<string> GettingPNCompleted;
        event Action<(bool success, IList<string> errors)> SettingPNCompleted;

        void GetPN();
        void SetPN(string phoneNumber);
        void ClearPN();
    }
}
