using System;
using System.Xml.Linq;

namespace Game
{
    public class EditGVTruthTableDialog : Dialog
    {
        public Action<bool> m_handler;

        public ButtonWidget m_okButton;

        public ButtonWidget m_cancelButton;

        public TextBoxWidget m_linearTextBox;

        public GVTruthTableData m_truthTableData;

        public bool m_ignoreTextChanges;

        public EditGVTruthTableDialog(GVTruthTableData truthTableData, Action<bool> handler)
        {
            XElement node = ContentManager.Get<XElement>("Dialogs/EditGVTruthTableDialog");
            LoadContents(this, node);
            m_okButton = Children.Find<ButtonWidget>("EditGVTruthTableDialog.OK");
            m_cancelButton = Children.Find<ButtonWidget>("EditGVTruthTableDialog.Cancel");
            m_linearTextBox = Children.Find<TextBoxWidget>("EditGVTruthTableDialog.LinearText");
            m_handler = handler;
            m_truthTableData = truthTableData;
        }

        public override void Update()
        {
            if (m_okButton.IsClicked)
            {
                Dismiss(result: true);
            }
            if (Input.Cancel || m_cancelButton.IsClicked)
            {
                Dismiss(result: false);
            }
        }

        public void Dismiss(bool result)
        {
            DialogsManager.HideDialog(this);
            m_handler?.Invoke(result);
        }
    }
}
