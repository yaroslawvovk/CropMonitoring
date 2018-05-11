using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropMonitoring.Graphics
{
    class MapColorWorker
    {
        public void ApplyColors(SvgDocument svgDocument, int provinceId, double _vhi)
        {
            foreach (Svg.SvgElement item in svgDocument.Children)
            {
                ChangeFill(item, provinceId, _vhi);
            }
        }

        private void ChangeFill(SvgElement element, int provinceId, double _vhi)
        {
            if (element is SvgPath)
            {
                Color color = ColorByVHI(_vhi);
                (element as SvgPath).Fill = new SvgColourServer(color);
                return;
            }

            if (element.Children.Count > 0)
            {
                foreach (var item in element.Children)
                {

                    if (item.ID != null)
                    {
                        if (item.ID == "id" + provinceId)
                            ChangeFill(item, provinceId, _vhi);
                    }
                }
            }

        }
        //VHI <40 - stress conditions;
        //VHI> 60 - favorable conditions;
        //VHI<15 - drought, the intensity of which from average to extraordinary;
        //VHI<35 - drought, the intensity of which from moderate to extreme.
        private Color ColorByVHI(double _vhi)
        {
            ColorConverter converter = new ColorConverter();
            if (_vhi < 15)
                return (Color)converter.ConvertFromString("#991515");
            else if(_vhi>=15&&_vhi<35)
                return (Color)converter.ConvertFromString("#c68501");
            else if (_vhi >= 35 && _vhi < 40)
                return (Color)converter.ConvertFromString("#b2c601");
            else if (_vhi>=40)
                return (Color)converter.ConvertFromString("#13a326");
            return Color.White;
        }

    }
}
