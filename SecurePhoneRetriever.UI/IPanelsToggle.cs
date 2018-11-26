using System.Windows.Forms;

namespace SecurePhoneRetriever.UI {
    internal interface IPanelsToggle {
        void ToggleToPanel(string name, Control panelContainer);
    }
}
