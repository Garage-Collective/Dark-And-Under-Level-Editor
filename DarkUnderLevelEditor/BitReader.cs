using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkUnderLevelEditor { 
        
    public class BitReader : IDisposable {

        private readonly bool _leaveOpen;
        private readonly Stream _stream;
        private int _byte;
        private bool _canRead;
        private int _counter = 0;

        public BitReader(Stream stream, bool leaveOpen = true) {

            if (stream == null) throw new ArgumentNullException("stream");
            _stream = stream;
            _leaveOpen = leaveOpen;

        }

        public void Dispose() {

            if (!_leaveOpen) {
                _stream.Dispose();
            }

        }

        public int ReadBits(int bitsToRead) {

            var buffer = new BitArray(bitsToRead);


            for (var i = 0; i < bitsToRead; i++) {
                byte bit;

                if (!Read(out bit)) {
                    return i;
                }

                buffer[i] = (bit == 1);
            }

            byte[] bytes = new byte[1];
            buffer.CopyTo(bytes, 0);
            return bytes[0];

        }

        public bool Read(out byte bit) {


            // Do we need to provision our bit buffer ?

            if (!_canRead) {
                _byte = _stream.ReadByte();
                _canRead = true;
            }

            // We are at EOF ..

            if (_byte == -1) {
                bit = 0;
                return false;
            }

            // Get current bit and update our counter ..

            var value = ((_byte & (1 << _counter)) > 0 ? 1 : 0);
            _counter++;

            if (_counter > 7) {
                _counter = 0;
                _canRead = false;
            }

            bit = (byte)value;
            return true;

        }

    }

}