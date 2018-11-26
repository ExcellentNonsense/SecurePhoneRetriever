using System;
using System.Collections.Generic;

namespace SecurePhoneRetriever.UI {
    public interface IPNProcessorView {
        event Action ViewLoading;
        event Action<string> PNSettingAttempt;
        event Action<string> PNPart_1_GettingAttempt;
        event Action PNPart_2_GettingAttempt;
        event Action<string> PNPart_3_GettingAttempt_1;
        event Action<string> PNPart_3_GettingAttempt_2;

        void ShowSetPNScreen();
        void ShowDisplayPNScreen();
        void DisplayPNSetAttemptResult((bool success, IList<string> errors) result);
        void DisplayPNPart_1(string phoneNumberPart1);
        void DisplayPNPart_2(string phoneNumberPart2);
        void UpdateCaptcha();
        void DisplayPNPart_3(string phoneNumberPart3);
        void ShowPNAccessDeniedScreen();
    }
}
