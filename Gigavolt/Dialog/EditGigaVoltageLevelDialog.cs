using System;
using System.Xml.Linq;

namespace Game
{
    public class EditGigaVoltageLevelDialog : Dialog
    {
        public Action m_handler;

        public ButtonWidget m_okButton;

        public ButtonWidget m_cancelButton;

        public TextBoxWidget m_voltageLevelTextBox;

        public GigaVoltageLevelData m_blockData;

        public EditGigaVoltageLevelDialog(GigaVoltageLevelData blockData, Action handler)
        {
            XElement node = ContentManager.Get<XElement>("Dialogs/EditGigaVoltageLevelDialog");
            LoadContents(this, node);
            m_okButton = Children.Find<ButtonWidget>("EditGigaVoltageLevelDialog.OK");
            m_cancelButton = Children.Find<ButtonWidget>("EditGigaVoltageLevelDialog.Cancel");
            m_voltageLevelTextBox = Children.Find<TextBoxWidget>("EditGigaVoltageLevelDialog.GigaVoltageLevel");
            m_voltageLevelTextBox.Text = blockData.Data.ToString("X", null);
            m_handler = handler;
            m_blockData = blockData;
        }

        public override void Update()
        {
            if (m_okButton.IsClicked)
            {
                if(uint.TryParse(m_voltageLevelTextBox.Text, System.Globalization.NumberStyles.HexNumber,null,out uint v))
                {
                    m_blockData.Data = uint.Parse(m_voltageLevelTextBox.Text, System.Globalization.NumberStyles.HexNumber, null);
                    m_blockData.SaveString();
                    Dismiss(true);
                }
                else
                {
                    DialogsManager.ShowDialog(null, new MessageDialog("��������", "����ת��Ϊ��Ȼ��", "OK", null, null));
                }
            }
            if (base.Input.Cancel || m_cancelButton.IsClicked)
            {
                Dismiss(false);
            }
        }

        public void Dismiss(bool result)
        {
            DialogsManager.HideDialog(this);
            if (m_handler != null && result)
            {
                m_handler();
            }
        }
    }
}