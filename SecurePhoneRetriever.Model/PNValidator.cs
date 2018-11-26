using SecurePhoneRetriever.Common;
using System;
using System.Collections.Generic;
using static SecurePhoneRetriever.Model.StringResources.PNValidationErrors;
using static System.String;

namespace SecurePhoneRetriever.Model {
    internal sealed class PNValidator : IPNValidator {
        private readonly int minLen;
        private readonly int maxLen;

        public PNValidator(IAppSettings appSettings) {
            if (appSettings == null) { throw new ArgumentNullException(nameof(appSettings)); }

            minLen = appSettings.MinPNLength;
            maxLen = appSettings.MaxPNLength;
        }

        public IList<string> Validate(string phoneNumber) {
            List<string> errors = new List<string>();

            // Contains only digits.
            foreach (char c in phoneNumber) {
                if ('0' > c || c > '9') {
                    errors.Add(OnlyDigitsAllowed_Err);
                    break;
                }
            }

            // The length is in the allowed range.
            if (minLen > phoneNumber.Length || phoneNumber.Length > maxLen) {
                if (minLen == maxLen) {
                    errors.Add(Format(RequiredPNLength_Err, minLen));
                }
                else {
                    errors.Add(Format(MinMaxPNLength_Err, minLen, maxLen));
                }
            }

            return errors;
        }
    }
}
