using SecurePhoneRetriever.Common;
using SecurePhoneRetriever.Model;
using SecurePhoneRetriever.UI;
using System;
using System.Collections.Generic;

namespace SecurePhoneRetriever.Presenter {
    internal sealed class PNProcessingPresenter : IViewGetter {
        private readonly IPNProcessorView processor;
        private readonly IPNManager manager;
        private readonly int partsQuantityForSplittingPN;
        private string fullPhoneNumber;
        private List<string> phoneNumberParts;
        private readonly Dictionary<int, string> validationKeys = new Dictionary<int, string> { [1] = "dolphin", [2] = "4AY", [3] = "AAY" };

        public PNProcessingPresenter(IPNProcessorView pNProcessorView, IPNManager pNManager, IAppSettings appSettings) {
            processor = pNProcessorView ?? throw new ArgumentNullException(nameof(pNProcessorView));

            manager = pNManager ?? throw new ArgumentNullException(nameof(pNManager));

            if (appSettings == null) { throw new ArgumentNullException(nameof(appSettings)); }

            partsQuantityForSplittingPN = appSettings.StepsQuantityForDisplayPN;

            processor.ViewLoading += ShowStartingScreen;
            processor.PNSettingAttempt += SetPN;
            processor.PNPart_1_GettingAttempt += HandlePNPart_1_GettingAttempt;
            processor.PNPart_2_GettingAttempt += HandlePNPart_2_GettingAttempt;
            processor.PNPart_3_GettingAttempt_1 += HandlePNPart_3_GettingAttempt_1;
            processor.PNPart_3_GettingAttempt_2 += HandlePNPart_3_GettingAttempt_2;

            manager.GettingPNCompleted += HandleResult_GettingPN;
            manager.SettingPNCompleted += HandleResult_SettingPN;

            InitializePN();
        }

        #region Initializing the phone number.

        private void InitializePN() {
            manager.GetPN();
        }

        private void HandleResult_GettingPN(string phoneNumber) {
            fullPhoneNumber = phoneNumber;
            
            if (!String.IsNullOrWhiteSpace(fullPhoneNumber)) {
                SplitPNIntoParts(fullPhoneNumber);
            }
        }

        private void SplitPNIntoParts(string phoneNumber) {
            phoneNumberParts = new List<string>();

            int digitsQuantityPerPart = phoneNumber.Length / partsQuantityForSplittingPN;
            int digitsQuantityInRest = phoneNumber.Length % partsQuantityForSplittingPN;

            for (int i = 0, j = 0; i < partsQuantityForSplittingPN; i++, j += digitsQuantityPerPart) {
                phoneNumberParts.Add(phoneNumber.Substring(j, digitsQuantityPerPart));
            }

            phoneNumberParts[partsQuantityForSplittingPN - 1] +=
                (phoneNumber.Substring(partsQuantityForSplittingPN * digitsQuantityPerPart, digitsQuantityInRest));
        }
        #endregion


        #region View loading.

        private void ShowStartingScreen() {
            if (String.IsNullOrWhiteSpace(fullPhoneNumber)) {
                processor.ShowSetPNScreen();
            }
            else {
                processor.ShowDisplayPNScreen();
            }
        }
        #endregion


        #region Setting the phone number.

        private void SetPN(string phoneNumber) {
            manager.SetPN(phoneNumber);
        }

        private void HandleResult_SettingPN((bool success, IList<string> errors) result) {
            processor.DisplayPNSetAttemptResult(result);
        }
        #endregion
        

        #region Displaying the phone number.

        private void HandlePNPart_1_GettingAttempt(string answer) {
            if (answer.IndexOf(validationKeys[1], StringComparison.OrdinalIgnoreCase) >= 0) {
                processor.DisplayPNPart_1(phoneNumberParts[0]);
            }
            else {
                manager.ClearPN();
                processor.ShowPNAccessDeniedScreen();
            }
        }

        private void HandlePNPart_2_GettingAttempt() {
            processor.DisplayPNPart_2(phoneNumberParts[1]);
        }

        private void HandlePNPart_3_GettingAttempt_1(string answer) {
            if (answer.Equals(validationKeys[2], StringComparison.Ordinal)) {
                processor.UpdateCaptcha();
            }
            else {
                manager.ClearPN();
                processor.ShowPNAccessDeniedScreen();
            }
        }

        private void HandlePNPart_3_GettingAttempt_2(string answer) {
            if (answer.Equals(validationKeys[3], StringComparison.Ordinal)) {
                processor.DisplayPNPart_3(phoneNumberParts[2]);
            }
            else {
                processor.ShowPNAccessDeniedScreen();
            }
            manager.ClearPN();
        }
        #endregion


        public IPNProcessorView View {
            get {
                return processor;
            }
        }
    }
}
