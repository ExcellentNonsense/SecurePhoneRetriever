using System;

namespace SecurePhoneRetriever.Common {
    internal class AppSettings : IAppSettings {
        private const byte minPNLength_Threshold = 5;
        private const byte maxPNLength_Threshold = 17;
        private const byte minStepsQuantityForDisplayPN_Threshold = 1;

        private byte minPNLength = minPNLength_Threshold;
        private byte maxPNLength = maxPNLength_Threshold;
        private byte stepsQuantityForDisplayPN = minStepsQuantityForDisplayPN_Threshold;

        public byte MinPNLength {
            get {
                return minPNLength;
            }
            private set {
                if (minPNLength_Threshold <= value && value <= maxPNLength_Threshold) {
                    minPNLength = value;
                }
                else {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        public byte MaxPNLength {
            get {
                return maxPNLength;
            }
            private set {
                if (MinPNLength <= value && value <= maxPNLength_Threshold) {
                    maxPNLength = value;
                }
                else {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        public byte StepsQuantityForDisplayPN {
            get {
                return stepsQuantityForDisplayPN;
            }
            private set {
                if (minStepsQuantityForDisplayPN_Threshold <= value && value <= MaxPNLength) {
                    stepsQuantityForDisplayPN = value;
                }
                else {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        // Called by the DI Container.
        public AppSettings() {
            MinPNLength = 5;
            MaxPNLength = 17;
            StepsQuantityForDisplayPN = 3;
        }

        // Called from the unit test project.
        public AppSettings(byte minPNLength, byte maxPNLength, byte stepsQuantityForDisplayPN) {
            MinPNLength = minPNLength;
            MaxPNLength = maxPNLength;
            StepsQuantityForDisplayPN = stepsQuantityForDisplayPN;
        }
    }
}
