using System;
using System.Collections.Generic;
using System.Text;

namespace SecurePhoneRetriever.Model {
    internal sealed class PNCoder : IPNCoder {
        private readonly char[] symbols = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private readonly int extraSymbolsQuantity = 10;
        private readonly string securityKey = "sk";

        public string Encode(string phoneNumber) {
            foreach (char c in phoneNumber) {
                if ('0' > c || c > '9') {
                    phoneNumber = String.Empty;
                    break;
                }
            }

            if (!String.IsNullOrWhiteSpace(phoneNumber)) {
                StringBuilder encoded = new StringBuilder();
                Random random = new Random();
                int max = symbols.Length;

                foreach (char c in phoneNumber) {
                    encoded.Append(symbols[(int)Char.GetNumericValue(c)]);
                    encoded.Append(symbols[random.Next(max)]);
                }

                for (int i = 0; i < extraSymbolsQuantity; i++) {
                    encoded.Append(symbols[random.Next(max)]);
                }

                encoded.Append(securityKey);

                phoneNumber = encoded.ToString();
            }

            return phoneNumber;
        }

        public string Decode(string phoneNumber) {
            if (!String.IsNullOrWhiteSpace(phoneNumber)) {

                if (phoneNumber.Length > extraSymbolsQuantity + securityKey.Length &&
                    phoneNumber.Substring(phoneNumber.Length - securityKey.Length)
                    .Equals(securityKey, StringComparison.Ordinal)) {

                    phoneNumber = phoneNumber.Substring(0,
                        phoneNumber.Length - extraSymbolsQuantity - securityKey.Length);

                    if (phoneNumber.Length % 2 == 0) {
                        char[] pNSymbols = new char[phoneNumber.Length / 2];
                        for (int i = 0, j = 0; i < phoneNumber.Length; i += 2, j++) {
                            pNSymbols[j] = phoneNumber[i];
                        }

                        Dictionary<char, int> symbolDigitPairs = new Dictionary<char, int>();
                        for (int i = 0; i < 10; i++) {
                            symbolDigitPairs.Add(symbols[i], i);
                        }

                        StringBuilder decoded = new StringBuilder();
                        foreach (char c in pNSymbols) {
                            if (symbolDigitPairs.TryGetValue(c, out int digit)) {
                                decoded.Append(digit);
                            }
                            else {
                                decoded.Clear();
                                break;
                            }
                        }

                        phoneNumber = decoded.ToString();
                    }
                    else { phoneNumber = String.Empty; }
                }
                else { phoneNumber = String.Empty; }
            }

            return phoneNumber;
        }
    }
}
