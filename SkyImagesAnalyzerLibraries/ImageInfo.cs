using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace SkyImagesAnalyzerLibraries
{
    public class ImageInfo
    {
        Image image;
        public Dictionary<PropertyTagId, KeyValuePair<PropertyTagType, Object>> imageProps = new Dictionary<PropertyTagId,KeyValuePair<PropertyTagType,object>>();
        
        public ImageInfo(Image newImage)
        {
            image = newImage;
            BuildPropsHash();
        }



        public object getValueByKey(String valKey)
        {
            PropertyTagId theIndex = (PropertyTagId)Enum.Parse(typeof(PropertyTagId), valKey);
            try
            {
                return (object)imageProps[theIndex].Value;
            }
            catch (Exception)
            {
                return null;
            }
            
        }


        private void BuildPropsHash()
        {
            foreach (PropertyItem property in image.PropertyItems)
            {
                Object propValue =  new Object();
                switch ((PropertyTagType)property.Type)
                {
                    case PropertyTagType.ASCII:
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        propValue = encoding.GetString(
                            property.Value, 0, property.Len - 1);
                        break;
                    case PropertyTagType.Int16:
                        propValue = BitConverter.ToInt16(
                            property.Value, 0);
                        break;
                    case PropertyTagType.SLONG:
                    case PropertyTagType.Int32:
                        propValue = BitConverter.ToInt32(
                            property.Value, 0);
                        break;
                    case PropertyTagType.SRational:
                    case PropertyTagType.Rational:
                        UInt32 numberator =
                            BitConverter.ToUInt32(
                            property.Value, 0);
                        UInt32 denominator =
                            BitConverter.ToUInt32(
                            property.Value, 4);
                        try
                        {
                            propValue = ((double)numberator
                                / (double)denominator).ToString();

                            if (propValue.ToString() == "NaN")
                                propValue = "0";
                        }
                        catch (DivideByZeroException)
                        {
                            propValue = "0";
                        }
                        break;
                    case PropertyTagType.Undefined:
                        propValue = "Undefined Data";
                        break;
                }
                imageProps.Add(NumToEnum<PropertyTagId>(property.Id),
                    new KeyValuePair<PropertyTagType,object>(
                        NumToEnum<PropertyTagType>(property.Type),
                        propValue));
            }
        }

        public T NumToEnum<T>(int number)
        {
            return (T)Enum.ToObject(typeof(T), number);
        }
    }
}
