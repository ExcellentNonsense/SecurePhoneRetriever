using System.Linq;
using System.Windows.Forms;
using static SecurePhoneRetriever.UI.StringResources;

namespace SecurePhoneRetriever.UI {
    internal sealed class PanelsToggle : IPanelsToggle {
        public void ToggleToPanel(string name, Control panelContainer) {
            var panelsInContainer = panelContainer.Controls.OfType<Panel>();
            foreach (var p in panelsInContainer) {
                p.Visible = false;
            }

            Panel pnl = (Panel)panelContainer.Controls.Find(name, false).FirstOrDefault();

            if (pnl != null) {
                pnl.Visible = true;
            }
            else {
                pnl = (Panel)panelContainer.Controls.Find(ControlsNames.pnlCommonError, false).FirstOrDefault();
                pnl.Visible = true;
            }
        }
    }
}
