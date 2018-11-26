using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static SecurePhoneRetriever.UI.StringResources;

namespace SecurePhoneRetriever.UI {
    internal partial class MainForm : Form, IPNProcessorView {
        private readonly IPanelsToggle pnlsToggle;
        private string displayedPNDigits;

        public MainForm(IPanelsToggle panelsToggle) {
            InitializeComponent();

            pnlsToggle = panelsToggle ?? throw new ArgumentNullException(nameof(panelsToggle));
        }


        #region Form loading.

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            ViewLoading();
        }

        public event Action ViewLoading = delegate { };

        public void ShowSetPNScreen() {
            pnlsToggle.ToggleToPanel(ControlsNames.pnlSetPN, this);
        }

        public void ShowDisplayPNScreen() {
            pnlsToggle.ToggleToPanel(ControlsNames.pnlDisplayPN_Step1, this);
        }
        #endregion


        #region Setting the phone number.

        public event Action<string> PNSettingAttempt = delegate { };

        public void DisplayPNSetAttemptResult((bool success, IList<string> errors) result) {
            if (result.success) {
                tbxPN_SetPNPnl.Text = String.Empty;
                lblValidationErrors_SetPNPnl.Text = SavePNResults.SuccessfulSavePhoneNumber;
            }
            else {
                if (result.errors.Count != 0) {
                    lblValidationErrors_SetPNPnl.Text = result.errors
                        .Aggregate((i, j) => i + "\r\n" + j);
                }
                else {
                    lblValidationErrors_SetPNPnl.Text = SavePNResults.FailureSavePhoneNumber;
                }
            }
        }

        private void BtnSet_SetPNPnl_Click(object sender, EventArgs e) {
            PNSettingAttempt(tbxPN_SetPNPnl.Text);
        }
        #endregion


        #region Displaying the phone number.

        public event Action<string> PNPart_1_GettingAttempt = delegate { };
        public event Action PNPart_2_GettingAttempt = delegate { };
        public event Action<string> PNPart_3_GettingAttempt_1 = delegate { };
        public event Action<string> PNPart_3_GettingAttempt_2 = delegate { };

        public void DisplayPNPart_1(string phoneNumberPart1) {
            btnA_pnlDisplayPN_Step4.Enabled = false;
            pnlsToggle.ToggleToPanel(ControlsNames.pnlDisplayPN_Step4, this);

            TmrStt.Specify(
                digits: phoneNumberPart1,
                writingArea: lblPN_pnlDisplayPN_Step4,
                digitsWritten: () => {
                    btnA_pnlDisplayPN_Step4.Enabled = true;
                    displayedPNDigits += phoneNumberPart1;
                });

            tmrDigitsWriter.Start();
        }

        public void DisplayPNPart_2(string phoneNumberPart2) {
            btnA_pnlDisplayPN_Step6.Enabled = false;
            pnlsToggle.ToggleToPanel(ControlsNames.pnlDisplayPN_Step6, this);

            TmrStt.Specify(
                digits: phoneNumberPart2,
                writingArea: lblPN_pnlDisplayPN_Step6,
                digitsWritten: () => {
                    btnA_pnlDisplayPN_Step6.Enabled = true;
                    displayedPNDigits += phoneNumberPart2;
                });

            tmrDigitsWriter.Start();
        }

        public void UpdateCaptcha() {
            lblPN_pnlDisplayPN_Step8.Text = displayedPNDigits;
            pnlsToggle.ToggleToPanel(ControlsNames.pnlDisplayPN_Step8, this);
        }

        public void DisplayPNPart_3(string phoneNumberPart3) {
            btnA_pnlDisplayPN_Step9.Enabled = false;
            lblPN_pnlDisplayPN_Step9.Text = displayedPNDigits;
            pnlsToggle.ToggleToPanel(ControlsNames.pnlDisplayPN_Step9, this);

            TmrStt.Specify(
                digits: phoneNumberPart3,
                writingArea: lblPN_pnlDisplayPN_Step9,
                digitsWritten: () => {
                    btnA_pnlDisplayPN_Step9.Enabled = true;
                    displayedPNDigits += phoneNumberPart3;
                });

            tmrDigitsWriter.Start();
        }

        public void ShowPNAccessDeniedScreen() {
            pnlsToggle.ToggleToPanel(ControlsNames.pnlPNAccessDenied, this);
        }


        #region Buttons event handlers.

        private void BtnA_pnlDisplayPN_Step1_Click(object sender, EventArgs e) {
            tmrPanic.Start();
            pnlsToggle.ToggleToPanel(ControlsNames.pnlDisplayPN_Step2, this);
        }

        private void BtnA_pnlDisplayPN_Step2_Click(object sender, EventArgs e) {
            tmrPanic.Stop();
            pnlsToggle.ToggleToPanel(ControlsNames.pnlDisplayPN_Step3, this);
        }

        private void BtnA_pnlDisplayPN_Step3_Click(object sender, EventArgs e) {
            string answer = ((Button)sender).Text;
            PNPart_1_GettingAttempt(answer);
        }

        private void BtnA_pnlDisplayPN_Step4_Click(object sender, EventArgs e) {
            lblPN_pnlDisplayPN_Step5.Text = displayedPNDigits;
            pnlsToggle.ToggleToPanel(ControlsNames.pnlDisplayPN_Step5, this);
        }

        private void BtnA_pnlDisplayPN_Step5_Click(object sender, EventArgs e) {
            lblPN_pnlDisplayPN_Step6.Text = displayedPNDigits;
            PNPart_2_GettingAttempt();
        }

        private void BtnA_pnlDisplayPN_Step6_Click(object sender, EventArgs e) {
            lblPN_pnlDisplayPN_Step7.Text = displayedPNDigits;
            pnlsToggle.ToggleToPanel(ControlsNames.pnlDisplayPN_Step7, this);
        }

        private void BtnA_pnlDisplayPN_Step7_Click(object sender, EventArgs e) {
            PNPart_3_GettingAttempt_1(tbxCaptcha_pnlDisplayPN_Step7.Text);
        }

        private void BtnA_pnlDisplayPN_Step8_Click(object sender, EventArgs e) {
            PNPart_3_GettingAttempt_2(tbxCaptcha_pnlDisplayPN_Step8.Text);
        }

        private void BtnA_pnlDisplayPN_Step9_Click(object sender, EventArgs e) {
            lblPN_pnlDisplayPN_Step10.Text = displayedPNDigits;
            pnlsToggle.ToggleToPanel(ControlsNames.pnlDisplayPN_Step10, this);
        }

        private void Exit_Click(object sender, EventArgs e) {
            this.Close();
        }
        #endregion


        #region Timers.

        private void TmrPanic_Tick(object sender, EventArgs e) {
            if (pbxPanic_pnlDisplayPN_Step2.Tag == null) {
                pbxPanic_pnlDisplayPN_Step2.Tag = 1u;
            }

            uint shot = (uint)pbxPanic_pnlDisplayPN_Step2.Tag;

            pbxPanic_pnlDisplayPN_Step2.Image = (System.Drawing.Bitmap)(Properties.Resources.ResourceManager.GetObject("Panic" + shot));

            pbxPanic_pnlDisplayPN_Step2.Tag = ++shot % 3;
        }

        private static class TmrStt {
            public static char Flicker => '_';
            public static bool WriteFlicker { get; set; } = true;
            public static int FlickerWrittenCount { get; set; }
            public static int WriteFlickerCount => 6; // Only even.

            public static string Digits { get; set; }
            public static int DigitsQuantity { get; private set; }
            public static int ProcessedDigits { get; set; }
            public static Label WritingArea { get; private set; }

            public static void Specify(string digits, Label writingArea, Action digitsWritten) {
                Digits = digits;
                WritingArea = writingArea;
                DigitsWritten = digitsWritten;
                DigitsQuantity = Digits.Length;
            }

            public static Action DigitsWritten = delegate { };
        }

        private void TmrDigitsWriter_Tick(object sender, EventArgs e) {
            if (TmrStt.WriteFlicker) {
                TmrStt.WritingArea.Text += TmrStt.Flicker;
                TmrStt.WriteFlicker = false;
            }
            else {
                TmrStt.WritingArea.Text = TmrStt.WritingArea.Text.TrimEnd(new char[] { TmrStt.Flicker });
                TmrStt.WriteFlicker = true;
            }

            TmrStt.FlickerWrittenCount++;

            if (TmrStt.FlickerWrittenCount == TmrStt.WriteFlickerCount) {
                TmrStt.WritingArea.Text += Convert.ToString(TmrStt.Digits[TmrStt.ProcessedDigits]);
                TmrStt.ProcessedDigits++;
                TmrStt.FlickerWrittenCount = 0;
            }

            if (TmrStt.ProcessedDigits == TmrStt.DigitsQuantity) {
                tmrDigitsWriter.Stop();
                TmrStt.ProcessedDigits = 0;
                TmrStt.DigitsWritten();
            }
        }
        #endregion

        #endregion

        private void BtnAboutProgram_Click(object sender, EventArgs e) {
            if (tbxAboutProgram.Visible) {
                tbxAboutProgram.Visible = false;
            }
            else {
                tbxAboutProgram.Visible = true;
                tbxAboutProgram.BringToFront();
            }
        }
    }
}