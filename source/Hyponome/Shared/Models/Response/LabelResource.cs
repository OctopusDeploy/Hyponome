using Newtonsoft.Json;
using Octokit;

namespace Hyponome.Shared.Models.Response
{
    public class LabelResource
    {
        [JsonConstructor]
        public LabelResource() {}

        public LabelResource(string name, string backgroundColor,string description)
        {
            Name = name;
            BackgroundColor = backgroundColor;
            TextColor = PickTextColorBasedOnBgColor(backgroundColor);
            Description = description;
        }
        
        public string Name { get; set; }
        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }
        public string Description { get; set; }
        
        private const string DarkColor = "text-body";
        private const string LightColor = "text-white";
        private static string PickTextColorBasedOnBgColor(string bgColor)
        {
            var r = int.Parse(bgColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber); // hexToR
            var g = int.Parse(bgColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber); // hexToG
            var b = int.Parse(bgColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber); // hexToB
            return (((r * 0.299) + (g * 0.587) + (b * 0.114)) > 186) ? DarkColor : LightColor;
        }

    }
}