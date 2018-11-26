using System;
using System.Collections.Generic;

namespace SecurePhoneRetriever.Model {
    internal sealed class PNManager : IPNManager {
        private readonly IPNRepository repository;
        private readonly IPNValidator validator;
        private readonly IPNCoder coder;

        public PNManager(IPNRepository pNRepository, IPNValidator pNValidator, IPNCoder pNCoder) {
            repository = pNRepository ?? throw new ArgumentNullException(nameof(pNRepository));

            validator = pNValidator ?? throw new ArgumentNullException(nameof(pNValidator));

            coder = pNCoder ?? throw new ArgumentNullException(nameof(pNCoder));
        }

        public event Action<string> GettingPNCompleted = delegate { };
        public event Action<(bool success, IList<string> errors)> SettingPNCompleted = delegate { };

        public void GetPN() {
            string phoneNumber = repository.Read();

            if (!String.IsNullOrWhiteSpace(phoneNumber)) {
                phoneNumber = coder.Decode(phoneNumber);

                if (!String.IsNullOrWhiteSpace(phoneNumber)) {
                    var validationErrors = validator.Validate(phoneNumber);

                    if (validationErrors.Count != 0) {
                        phoneNumber = String.Empty;
                    }
                }
            }

            GettingPNCompleted(phoneNumber);
        }

        public void SetPN(string phoneNumber) {
            bool success = false;

            var validationErrors = validator.Validate(phoneNumber);

            if (validationErrors.Count == 0) {
                phoneNumber = coder.Encode(phoneNumber);

                if (!String.IsNullOrWhiteSpace(phoneNumber)) {
                    success = repository.Write(phoneNumber);
                }
            }

            SettingPNCompleted((success, validationErrors));
        }

        public void ClearPN() {
            repository.Write(String.Empty);
        }
    }
}
