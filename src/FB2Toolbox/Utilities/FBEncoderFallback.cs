using System.Text;

namespace FB2Toolbox.Utilities
{
    public class FBEncoderFallback : EncoderFallback
    {
        #region FBEncoderFallback
        public override EncoderFallbackBuffer CreateFallbackBuffer()
        {
            return new FB2EncoderFallbackBuffer();
        }
        public override int MaxCharCount => 8;
        public FBEncoderFallback()
        {
        }
        #endregion
    }
}
