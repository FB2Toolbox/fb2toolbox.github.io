using System;
using System.Text;

namespace FB2Toolbox.Utilities
{
    public sealed class FB2EncoderFallbackBuffer : EncoderFallbackBuffer
    {
        #region FB2EncoderFallbackBuffer
        // Store our fallback string
        private string _strFallback = string.Empty;
        private int _fallbackCount = -1;
        private int _fallbackIndex = -1;
        // Construction
        public FB2EncoderFallbackBuffer()
        {
        }
        // Fallback Methods
        public override bool Fallback(char charUnknown, int index)
        {
            // If we had a buffer already we're being recursive, throw, it's probably at the suspect
            // character in our array.
            if (_fallbackCount >= 1)
            {
                // Presumably you'd want a prettier exception:
                throw new Exception("Recursive Fallback Exception");
            }

            // Go ahead and get our fallback
            _strFallback = string.Format("&#{0};", (int)charUnknown);
            _fallbackCount = _strFallback.Length;
            _fallbackIndex = -1;

            return _fallbackCount != 0;
        }
        public override bool Fallback(char charUnknownHigh, char charUnknownLow, int index)
        {
            // In this example, we didn't really expect surrogates.

            // If we had a buffer already we're being recursive, throw, it's probably at the suspect
            // character in our array.
            if (_fallbackCount >= 1)
            {
                // Presumably you'd want a prettier exception:
                throw new Exception("Recursive Fallback Exception");
            }

            // Go ahead and get our fallback
            // Note that we're doing this 2X, once for each char.  That won't effect the
            // EncoderNumberFallback.MaxCharCount though because it is counting per char,
            // and although we're 2X that here, we also have 2x chars.
            _strFallback = string.Format("&#{0};&#{1};", (int)charUnknownHigh, (int)charUnknownLow);
            _fallbackCount = _strFallback.Length;
            _fallbackIndex = -1;

            return _fallbackCount != 0;
        }
        public override char GetNextChar()
        {
            // We want it to get < 0 because == 0 means that the current/last character is a fallback
            // and we need to detect recursion.  We could have a flag but we already have this counter.
            _fallbackCount--;
            _fallbackIndex++;

            // Do we have anything left? 0 is now last fallback char, negative is nothing left
            if (_fallbackCount < 0)
            {
                return (char)0;
            }

            // Need to get it out of the buffer.
            return _strFallback[_fallbackIndex];
        }
        public override bool MovePrevious()
        {
            // Back up one, only if we just processed the last character (or earlier)
            if (_fallbackCount >= -1 && _fallbackIndex >= 0)
            {
                _fallbackIndex--;
                _fallbackCount++;
                return true;
            }

            // Return false 'cause we couldn't do it.
            return false;
        }
        // How many characters left to output?
        public override int Remaining =>
                // Our count is 0 for 1 character left.
                (_fallbackCount < 0) ? 0 : _fallbackCount;
        #endregion
    }
}
