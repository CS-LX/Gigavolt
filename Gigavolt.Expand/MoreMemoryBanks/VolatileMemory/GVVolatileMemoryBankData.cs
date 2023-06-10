﻿using System;
using System.Globalization;

namespace Game {
    public class GVVolatileMemoryBankData : GVMemoryBankData {
        public GVVolatileMemoryBankData() {
            m_ID = GVStaticStorage.GetUniqueGVMBID();
            m_worldDirectory = null;
            m_data = null;
            m_isDataInitialized = false;
            m_updateTime = DateTime.Now;
        }

        public GVVolatileMemoryBankData(uint ID, uint[] image = null, uint width = 0, uint height = 0) {
            m_ID = ID;
            m_data = image;
            m_width = width;
            m_height = height;
            m_isDataInitialized = image != null;
            GVStaticStorage.GVMBIDDataDictionary[m_ID] = this;
            m_updateTime = DateTime.Now;
        }

        public override uint LastOutput {
            get => 0u;
            set { }
        }

        public override IEditableItemData Copy() => new GVVolatileMemoryBankData(m_ID, m_isDataInitialized ? (uint[])Data.Clone() : null, m_width, m_height);

        public override void LoadString(string data) {
            string[] array = data.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length >= 1) {
                string text = array[0];
                m_ID = uint.Parse(text, NumberStyles.HexNumber, null);
                GVStaticStorage.GVMBIDDataDictionary[m_ID] = this;
            }
        }

        public override string SaveString() => m_ID.ToString("X", null);
    }
}