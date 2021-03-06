using System.Collections.Generic;

namespace PPMLib
{
    public class PPMLayer
    {        
        private bool _visibility;
        internal byte[] _layerData = new byte[32 * 192];        
        internal byte[] _linesEncoding = new byte[48];
        public LineEncoding LinesEncoding(int lineIndex)
            => (LineEncoding)((_linesEncoding[lineIndex >> 2] >> ((lineIndex & 0x3) << 1)) & 0x3);

        /// <summary>
        /// Apply a line encoding value to a line of pixels
        /// </summary>
        /// <param name="lineIndex">Line Index</param>
        /// <param name="value">Line Encoding to apply onto the line</param>
        public void SetLineEncoding(int lineIndex, LineEncoding value)
        {
            int o = lineIndex >> 2;
            int pos = (lineIndex & 0x3) * 2;
            var b = _linesEncoding[o];
            b = (byte)(b & (byte)~(0x3 << pos));
            b = (byte)(b | (byte)((int)value << pos));
            _linesEncoding[o] = b;
        }

        #region Line-Related Functions
        /// <summary>
        /// Set the line encoding for the whole layer
        /// </summary>
        /// <param name="y">Line Index</param>
        /// <returns>New encoding type for the line</returns>
        public LineEncoding ChooseLineEncoding(int y)
        {
            var _0chks = 0;
            var _1chks = 0;
            int i = 32 * y;
            for (var b = 0; b < 32; b++) 
            {
                _0chks += (this[i] == 0x00) ? 1 : 0;
                _1chks += (this[i++] == 0xFF) ? 1 : 0;
            }
            if (_0chks == 32)            
                return LineEncoding.SkipLine;
            if (_0chks == 0 && _1chks == 1)             
                return LineEncoding.RawLineData;                        
            return ((_0chks > _1chks) ? LineEncoding.CodedLine : LineEncoding.InvertedCodedLine);            
        }

        /// <summary>
        /// Insert a line in current layer
        /// </summary>
        /// <param name="lineData">Line Data</param>
        /// <param name="y">Index Of Line</param>
        private void InsertLineInLayer(List<byte> lineData, int y)
        {
            List<byte> chks = new List<byte>();
            switch (LinesEncoding(y))
            {
                case 0:
                    {
                        return;
                    }
                case (LineEncoding)1:
                case (LineEncoding)2:
                    {
                        uint flag = 0;
                        for (var x = 0; x <= 32; x++)
                        {
                            byte chunk = 0;
                            for (var x_ = 0; x_ <= 8; x_++)
                            {
                                if (this[8 * x + x_, y]) 
                                {
                                    chunk = (byte)(chunk | (byte)(1 << x_));
                                }
                            }
                            if (chunk != ((LinesEncoding(y) == (PPMLib.LineEncoding)1) ? 0x0 : 0xFF))
                            {
                                flag |= (1U << (31 - x));
                                chks.Add(chunk);
                            }
                        }
                        lineData.Add((byte)((flag & 0xFF000000U) >> 24));
                        lineData.Add((byte)((flag & 0xFF0000U) >> 16));
                        lineData.Add((byte)((flag & 0xFF00U) >> 8));
                        lineData.Add((byte)(flag & 0xFFU));
                        lineData.AddRange(chks);
                        return;
                    }
                case (LineEncoding)3:
                    {
                        for (var x = 0; x <= 32; x++)
                        {
                            byte chunk = 0;
                            for (var x_ = 0; x_ <= 8; x_++)
                            {
                                if (this[8 * x + x_, y]) 
                                {
                                    chunk = (byte)(chunk | (byte)(1 << x_));
                                }
                            }
                            chks.Add(chunk);
                        }
                        break;
                    }
            }
        }
        #endregion

        /// <summary>
        /// Set the visibility of the layer
        /// </summary>
        public bool Visible
        {
            get
            {
                return _visibility;
            }
            set
            {
                _visibility = value;
            }
        }     

        public byte this[int p]
        {
            get => _layerData[p];
            set => _layerData[p] = value;
        }

        public bool this[int x, int y]
        {
            get
            {
                int p = 256 * y + x;
                return (_layerData[p >> 3] & ((byte)(1 << (p & 7)))) != 0;
            }
            set
            {
                int p = 256 * y + x;
                _layerData[p >> 3] &= (byte)(~(1 << (p & 0x7)));
                _layerData[p >> 3] |= (byte)((value ? 1 : 0) << (p & 0x7));
            }
        }
        public PenColor PenColor { get; set; }
    }
}