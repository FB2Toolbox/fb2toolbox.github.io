using System.Reflection;
using System.Windows.Forms;

namespace FB2Toolbox
{
    public static class Extensions
    {
        public static void SetDoubleBuffered(Control control, bool enable)
        {
            var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            doubleBufferPropertyInfo.SetValue(control, enable, null);
        }
    }
}
