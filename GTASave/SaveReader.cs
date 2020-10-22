/*
 * This file is part of gtasave-cs, licensed under the MIT License (MIT).
 * 
 * Copyright (c) Jamie Mansfield <https://www.jamiemansfield.me/>
 * Copyright (c) contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using GTASave.Blocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GTASave
{
    public class SaveReader : BinaryReader
    {
        public SaveReader(Stream input) : base(input, Encoding.ASCII)
        {
        }

        public SaveReader(Stream input, bool leaveOpen) : base(input, Encoding.ASCII, leaveOpen)
        {
        }

        public Save ReadSave()
        {
            Save save = new Save();
            save.Block00 = ReadBlock00();
            return save;
        }

        /// <summary>
        /// Reads a blocks header, "BLOCK".
        /// </summary>
        /// <exception cref="InvalidBlockHeaderException">Block header doesn't match expected</exception>
        public void ReadBlockHeader()
        {
            try
            {
                string header = new string(ReadChars(5));
                if (!header.Equals("BLOCK"))
                {
                    throw new InvalidBlockHeaderException("Expected \"BLOCK\" not \"" + header + "\"");
                }
            }
            catch(IOException ex)
            {
                throw new InvalidBlockHeaderException("Failed to read block header", ex);
            }
        }

        public Block00 ReadBlock00()
        {
            ReadBlockHeader();
            Block00 block = new Block00();
            block.Version = ReadInt32();
            block.SaveName = ReadGTAString(100);
            return block;
        }

        /// <summary>
        /// Reads a string from the save file, that has been encoded with a fixed
        /// length and padded with raw zeros.
        /// </summary>
        /// <param name="length">The fixed length of the string in the save file</param>
        /// <returns>The value, omitting the padding</returns>
        /// <exception cref="ArgumentOutOfRangeException">length is negative</exception>
        public string ReadGTAString(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length is negative");
            }

            char[] raw = ReadChars(length);

            int realLength = 0;
            for (int i = 0; i < raw.Length; i++)
            {
                if (raw[i] != 0)
                {
                    realLength = i;
                }
                else
                {
                    break;
                }
            }
            realLength++;

            char[] real = new char[realLength];
            Array.Copy(raw, real, realLength);
            return new string(real);
        }
    }
}
