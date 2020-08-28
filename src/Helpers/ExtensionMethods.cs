using System.Drawing;

namespace JacksiroKe.AnisiCtrls
{
    public static class ExtensionMethods
    {
        public static Color FromHex(this string hex)
        {
            return ColorTranslator.FromHtml(hex);
        }
    }
}
