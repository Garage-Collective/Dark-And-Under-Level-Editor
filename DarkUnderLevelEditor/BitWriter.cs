using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkUnderLevelEditor {

    public class BitWriter : System.IO.StreamWriter {

        private bool _reverseBytes = false;
        private bool[] curByte = new bool[8];
        private byte curBitIndx = 0;

        public bool ReverseBytes { get => _reverseBytes; set => _reverseBytes = value; }

        public BitWriter(Stream s) : base(s) { }

        public override void Flush() {

            base.Write(ConvertToByte(curByte).ToString());
            base.Flush();
        }

        public override void Write(string value) {

            char[] chars = value.ToCharArray();

            for (int i = 0; i < value.ToArray().Length; i++) {

                this.Write(chars[i] == '1');
            }

        }

        public override void Write(bool value) {

            curByte[curBitIndx] = value;
            curBitIndx++;

            if (curBitIndx == 8) {

                base.Write(ConvertToByte(curByte).ToString());
                base.Write(", ");
                this.curBitIndx = 0;
                this.curByte = new bool[8];
            }

        }

        private byte ConvertToByte(bool[] bools) {

            byte b = 0;

            if (!_reverseBytes) {

                byte bitIndex = 0;
                for (int i = 7; i >= 0; i--) {
                    if (bools[i]) {
                        b |= (byte)(((byte)1) << bitIndex);
                    }
                    bitIndex++;
                }

            }
            else {

                byte bitIndex = 0;
                for (int i = 0; i < 8; i++) {
                    if (bools[i]) {
                        b |= (byte)(((byte)1) << bitIndex);
                    }
                    bitIndex++;
                }

            }

            return b;
        }

    }

}