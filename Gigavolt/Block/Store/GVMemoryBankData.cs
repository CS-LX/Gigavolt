using Engine;
using Engine.Media;
using GameEntitySystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public class GVMemoryBankData : IEditableItemData
    {

        public uint m_ID;
        public string m_worldDirectory;
        public Image m_data;
        public Image Data
        {
            get
            {
                return m_data;
            }
            set
            {
                m_dataChanged = true;
                m_data = value;
            }
        }
        public bool m_dataChanged;
        public GVMemoryBankData()
        {
            m_ID = GVStaticStorage.GetUniqueGVMBID();
            m_worldDirectory = null;
            m_data = null;
        }
        public GVMemoryBankData(uint ID, string worldDirectory, Image image=null, uint lastOutput = 0)
        {
            m_ID = ID;
            m_worldDirectory = worldDirectory;
            m_data = image;
            LastOutput = lastOutput;
            GVStaticStorage.GVMBIDDataDictionary.Add(m_ID, this);
        }
        public uint LastOutput
        {
            get;
            set;
        }

        public uint Read(uint col, uint row)
        {
            if (Data == null)
            {
                return 0;
            }
            int col_int = MathUint.ToInt(col);
            int row_int = MathUint.ToInt(row);
            if (col_int < Data.Width && row_int < Data.Height)
            {
                return Data.GetPixel(col_int, row_int).PackedValue;
            }
            return 0;
        }

        public void Write(uint col, uint row, uint data)
        {
            if (Data == null)
            {
                return;
            }
            int col_int = MathUint.ToInt(col);
            int row_int = MathUint.ToInt(row);
            if (col_int < Data.Width && row_int < Data.Height)
            {
                Data.SetPixel(col_int, row_int, new Color(data));
            }
        }

        public IEditableItemData Copy()
        {
            return new GVMemoryBankData(m_ID, m_worldDirectory, Data == null ? null : new Image(Data), LastOutput);
        }
        public void LoadData()
        {
            if (m_worldDirectory != null)
            {
                try
                {
                    m_data = Image.Load($"{m_worldDirectory}/GVMB/{m_ID.ToString("X", null)}.png", ImageFileFormat.Png);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
        public void LoadString(string data)
        {
            string[] array = data.Split(new char[1]
            {
                ';'
            }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length >= 1)
            {
                string text = array[0];
                m_ID = uint.Parse(text, System.Globalization.NumberStyles.HexNumber, null);
                LoadData();
                GVStaticStorage.GVMBIDDataDictionary.Add(m_ID, this);
            }
            if (array.Length >= 2)
            {
                LastOutput = uint.Parse(array[1], System.Globalization.NumberStyles.HexNumber, null);
            }
        }

        public string SaveString()
        {
            return SaveString(saveLastOutput: true);
        }

        public string SaveString(bool saveLastOutput)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(m_ID.ToString("X", null));
            if (saveLastOutput)
            {
                stringBuilder.Append(';');
                stringBuilder.Append(LastOutput.ToString("X", null));
            }
            if (Data != null && m_dataChanged)
            {
                try
                {
                    Image.Save(Data, $"{m_worldDirectory}/GVMB/{m_ID.ToString("X", null)}.png", ImageFileFormat.Png, true);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
            return stringBuilder.ToString();
        }
        public static Image String2Image(string data, int width = 0, int height = 0)
        {
            List<uint[]> rowList = new List<uint[]>();
            int maxColLength = 0;
            string[] rows = data.Split(';');
            foreach (string row in rows)
            {
                string[] cols = row.Split(',');
                if (cols.Length > maxColLength)
                {
                    maxColLength = cols.Length;
                }
                uint[] uints = new uint[cols.Length];
                for (int i = 0; i < cols.Length; i++)
                {
                    uints[i] = uint.Parse(cols[i], System.Globalization.NumberStyles.HexNumber, null);
                }
                rowList.Add(uints);
            }
            Image image = new Image(width == 0 ? maxColLength : width, height == 0 ? rows.Length : height);
            for (int i = 0; i < image.Height; i++)
            {
                if (i == rowList.Count)
                {
                    break;
                }
                for (int j = 0; j < rowList[i].Length; j++)
                {
                    if (j == image.Width)
                    {
                        break;
                    }
                    image.SetPixel(j, i, new Color(rowList[i][j]));
                }
            }
            return image;
        }

        public static string Image2String(Image image)
        {
            string[] result = new string[image.Height];
            for (int i = 0; i < image.Height; i++)
            {
                int lastNotZero = -1;
                for (int j = image.Width - 1; j >= 0; j--)
                {
                    if (image.GetPixel(j, i).PackedValue > 0)
                    {
                        lastNotZero = j;
                        break;
                    }
                }
                StringBuilder stringBuilder = new StringBuilder();
                for (int j = 0; j < lastNotZero; j++)
                {
                    stringBuilder.Append(image.GetPixel(j, i).PackedValue.ToString("X", null));
                    stringBuilder.Append(',');
                }
                if (lastNotZero > -1)
                {
                    stringBuilder.Append(image.GetPixel(lastNotZero, i).PackedValue.ToString("X", null));
                }
                result[i] = stringBuilder.ToString();
            }
            return string.Join(";", result);
        }
        public static byte[] Image2Bytes(Image image)
        {
            byte[] bytes = new byte[image.Pixels.Length * 4];
            for (int i = 0; i < image.Pixels.Length; i++)
            {
                bytes[i * 4 + 3] = (byte)(image.Pixels[i].PackedValue & 0xFF);
                bytes[i * 4 + 2] = (byte)((image.Pixels[i].PackedValue >> 8) & 0xFF);
                bytes[i * 4 + 1] = (byte)((image.Pixels[i].PackedValue >> 16) & 0xFF);
                bytes[i * 4] = (byte)((image.Pixels[i].PackedValue >> 24) & 0xFF);
            }
            return bytes;
        }
        public static short[] Image2Shorts(Image image)
        {
            short[] shorts = new short[image.Pixels.Length * 2];
            for (int i = 0; i < image.Pixels.Length; i++)
            {
                shorts[i * 2 + 1] = (short)((image.Pixels[i].PackedValue) & 0xFFFF);
                shorts[i * 2] = (short)((image.Pixels[i].PackedValue >> 16) & 0xFFFF);
            }
            return shorts;
        }
    }
}