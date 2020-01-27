using NetOdt.Class;
using NetOdt.Enumerations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace NetOdt.Helper
{
    /// <summary>
    /// Helper class to easier work with ODT document styles
    /// </summary>
    internal static class StyleHelper
    {
        /// <summary>
        /// Add all needed styles for all <see cref="TextStyle"/> combinations to the style content
        /// </summary>
        /// <param name="styleContent">The content container for the style</param>
        /// <param name="neededStyles">List that contains all needed styles</param>
        internal static void AddStandardTextStyles(in StringBuilder styleContent, IEnumerable<Style> neededStyles)
        {
            foreach(var style in neededStyles)
            {
                AppendXmlStyleStart(styleContent, style);
                AppendXmlStyle(styleContent, style);
                AppendXmlStyleEnd(styleContent);
            }
        }

        /// <summary>
        /// Add all needed styles for simple tables
        /// </summary>
        /// <param name="styleContent">The content container for the style</param>
        internal static void AddTableStyles(in StringBuilder styleContent)
        {
            AppendXmlStyleStart(styleContent, new Style("Tabelle1", StyleFamily.Table, TextStyle.None, string.Empty, 0.0, Color.Black, Color.Transparent));
            styleContent.Append("<style:table-properties style:width=\"17cm\" table:align=\"margins\"/>");
            AppendXmlStyleEnd(styleContent);

            AppendXmlStyleStart(styleContent, new Style("Tabelle1.A", StyleFamily.TableColumn, TextStyle.None, string.Empty, 0.0, Color.Black, Color.Transparent));
            styleContent.Append("<style:table-column-properties style:column-width=\"3.401cm\" style:rel-column-width=\"13107*\"/>");
            AppendXmlStyleEnd(styleContent);

            AppendXmlStyleStart(styleContent, new Style("Tabelle1.A1", StyleFamily.TableCell, TextStyle.None, string.Empty, 0.0, Color.Black, Color.Transparent));
            styleContent.Append("<style:table-cell-properties fo:padding=\"0.097cm\" fo:border-left=\"0.05pt solid #000000\" fo:border-right=\"none\" fo:border-top=\"0.05pt solid #000000\" fo:border-bottom=\"0.05pt solid #000000\"/>");
            AppendXmlStyleEnd(styleContent);

            AppendXmlStyleStart(styleContent, new Style("Tabelle1.E1", StyleFamily.TableCell, TextStyle.None, string.Empty, 0.0, Color.Black, Color.Transparent));
            styleContent.Append("<style:table-cell-properties fo:padding=\"0.097cm\" fo:border=\"0.05pt solid #000000\"/>");
            AppendXmlStyleEnd(styleContent);

            AppendXmlStyleStart(styleContent, new Style("Tabelle1.A2", StyleFamily.TableCell, TextStyle.None, string.Empty, 0.0, Color.Black, Color.Transparent));
            styleContent.Append("<style:table-cell-properties fo:padding=\"0.097cm\" fo:border-left=\"0.05pt solid #000000\" fo:border-right=\"none\" fo:border-top=\"none\" fo:border-bottom=\"0.05pt solid #000000\"/>");
            AppendXmlStyleEnd(styleContent);

            AppendXmlStyleStart(styleContent, new Style("Tabelle1.E2", StyleFamily.TableCell, TextStyle.None, string.Empty, 0.0, Color.Black, Color.Transparent));
            styleContent.Append("<style:table-cell-properties fo:padding=\"0.097cm\" fo:border-left=\"0.05pt solid #000000\" fo:border-right=\"0.05pt solid #000000\" fo:border-top=\"none\" fo:border-bottom=\"0.05pt solid #000000\"/>");
            AppendXmlStyleEnd(styleContent);
        }

        /// <summary>
        /// Add all needed styles for simple pictures
        /// </summary>
        /// <param name="styleContent">The content container for the style</param>
        internal static void AddPictureStyle(in StringBuilder styleContent)
        {
            AppendXmlStyleStart(styleContent, new Style("fr1", StyleFamily.Graphic, TextStyle.None, string.Empty, 0.0, Color.Black, Color.Transparent));
            styleContent.Append("<style:graphic-properties style:mirror=\"none\" fo:clip=\"rect(0cm, 0cm, 0cm, 0cm)\" draw:luminance=\"0%\" draw:contrast=\"0%\" draw:red=\"0%\" draw:green=\"0%\" draw:blue=\"0%\" draw:gamma=\"100%\" draw:color-inversion=\"false\" draw:image-opacity=\"100%\" draw:color-mode=\"standard\"/>");
            AppendXmlStyleEnd(styleContent);
        }

        /// <summary>
        /// Append a XML start element for XML style informations
        /// </summary>
        /// <param name="styleContent">The content container for the style</param>
        /// <param name="style">The style for the style informations</param>
        internal static void AppendXmlStyleStart(in StringBuilder styleContent, in Style style)
        {
            styleContent.Append($"<");
            styleContent.Append($"style:style");
            styleContent.Append($" style:name=\"{style.Name}\"");
            styleContent.Append($" style:family=\"{style.FamilyName}\"");

            switch(style.Family)
            {
                case StyleFamily.Footer:
                case StyleFamily.Graphic:
                case StyleFamily.Header:
                case StyleFamily.Paragraph:
                    styleContent.Append($" style:parent-style-name=\"{style.ParentName}\"");
                    break;
            }

            styleContent.Append($">");
        }

        /// <summary>
        /// Append the given text style to the XML style informations
        /// </summary>
        /// <param name="styleContent">The content container for the style</param>
        /// <param name="style">The style for the style informations</param>
        internal static void AppendXmlStyle(in StringBuilder styleContent, Style style)
        {
            AppendTextProperties(styleContent, style);
            AppendParagraphProperties(styleContent, style);
        }

        /// <summary>
        /// Append the given text properties to the XML style informations
        /// </summary>
        /// <param name="styleContent">The content container for the style</param>
        /// <param name="style">The style for the style informations</param>
        internal static void AppendTextProperties(in StringBuilder styleContent, Style style)
        {
            // Note: Don't forget the spaces between the XML properties

            styleContent.Append("<");
            styleContent.Append("style:text-properties");

            styleContent.Append($" style:font-name=\"{style.FontName}\"");

            if(!style.HaveParentFontSize)
            {
                styleContent.Append($" fo:font-size=\"{style.FontSize}pt\"");
                styleContent.Append($" style:font-size-asian=\"{style.FontSize}pt\"");
                styleContent.Append($" style:font-size-complex=\"{style.FontSize}pt\"");
            }

            styleContent.Append($" fo:color=\"{GetColorValue(style.ForegroundColor)}\"");
            styleContent.Append($" fo:background-color=\"{GetColorValue(style.BackgroundColor)}\"");

            if(style.TextStyle.HasFlag(TextStyle.Bold))
            {
                styleContent.Append(" fo:font-weight=\"bold\"");
                styleContent.Append(" style:font-weight-asian=\"bold\"");
                styleContent.Append(" style:font-weight-complex=\"bold\"");
            }
            else
            {
                styleContent.Append(" fo:font-weight=\"normal\"");
                styleContent.Append(" style:font-weight-asian=\"normal\"");
                styleContent.Append(" style:font-weight-complex=\"normal\"");
            }

            if(style.TextStyle.HasFlag(TextStyle.Italic))
            {
                styleContent.Append(" fo:font-style=\"italic\"");
                styleContent.Append(" style:font-style-asian=\"italic\"");
                styleContent.Append(" style:font-style-complex=\"italic\"");
            }
            else
            {
                styleContent.Append(" fo:font-style=\"normal\"");
                styleContent.Append(" style:font-style-asian=\"normal\"");
                styleContent.Append(" style:font-style-complex=\"normal\"");
            }

            if(style.TextStyle.HasFlag(TextStyle.UnderlineSingle))
            {
                styleContent.Append(" style:text-underline-style=\"solid\"");
                styleContent.Append(" style:text-underline-width=\"auto\"");
                styleContent.Append(" style:text-underline-color=\"font-color\"");
            }
            else if(style.TextStyle.HasFlag(TextStyle.UnderlineDouble))
            {
                styleContent.Append(" style:text-underline-style=\"double\"");
                styleContent.Append(" style:text-underline-width=\"auto\"");
                styleContent.Append(" style:text-underline-color=\"font-color\"");
            }
            else if(style.TextStyle.HasFlag(TextStyle.UnderlineBold))
            {
                styleContent.Append(" style:text-underline-style=\"solid\"");
                styleContent.Append(" style:text-underline-width=\"bold\"");
                styleContent.Append(" style:text-underline-color=\"font-color\"");
            }
            else if(style.TextStyle.HasFlag(TextStyle.UnderlineDotted))
            {
                styleContent.Append(" style:text-underline-style=\"dotted\"");
                styleContent.Append(" style:text-underline-width=\"auto\"");
                styleContent.Append(" style:text-underline-color=\"font-color\"");
            }
            else if(style.TextStyle.HasFlag(TextStyle.UnderlineDottedBold))
            {
                styleContent.Append(" style:text-underline-style=\"dotted\"");
                styleContent.Append(" style:text-underline-width=\"bold\"");
                styleContent.Append(" style:text-underline-color=\"font-color\"");
            }
            else if(style.TextStyle.HasFlag(TextStyle.UnderlineDash))
            {
                styleContent.Append(" style:text-underline-style=\"dash\"");
                styleContent.Append(" style:text-underline-width=\"auto\"");
                styleContent.Append(" style:text-underline-color=\"font-color\"");
            }
            else if(style.TextStyle.HasFlag(TextStyle.UnderlineLongDash))
            {
                styleContent.Append(" style:text-underline-style=\"long-dash\"");
                styleContent.Append(" style:text-underline-width=\"auto\"");
                styleContent.Append(" style:text-underline-color=\"font-color\"");
            }
            else if(style.TextStyle.HasFlag(TextStyle.UnderlineDotDash))
            {
                styleContent.Append(" style:text-underline-style=\"dot-dash\"");
                styleContent.Append(" style:text-underline-width=\"auto\"");
                styleContent.Append(" style:text-underline-color=\"font-color\"");
            }
            else if(style.TextStyle.HasFlag(TextStyle.UnderlineDotDotDash))
            {
                styleContent.Append(" style:text-underline-style=\"dot-dot-dash\"");
                styleContent.Append(" style:text-underline-width=\"auto\"");
                styleContent.Append(" style:text-underline-color=\"font-color\"");
            }
            else if(style.TextStyle.HasFlag(TextStyle.UnderlineWave))
            {
                styleContent.Append(" style:text-underline-style=\"wave\"");
                styleContent.Append(" style:text-underline-width=\"auto\"");
                styleContent.Append(" style:text-underline-color=\"font-color\"");
            }
            else
            {
                styleContent.Append(" style:text-underline-style=\"none\"");
            }

            if(style.TextStyle.HasFlag(TextStyle.Stroke))
            {
                styleContent.Append(" style:text-line-through-style=\"solid\"");
                styleContent.Append(" style:text-line-through-type=\"single\"");
            }
            else
            {
                styleContent.Append(" style:text-line-through-style=\"none\"");
                styleContent.Append(" style:text-line-through-type=\"none\"");
            }

            // A text paragraph can only use superscript or subscript not both at same time
            if(style.TextStyle.HasFlag(TextStyle.Subscript))
            {
                styleContent.Append(" style:text-position=\"sub 58%\"");
            }
            else if(style.TextStyle.HasFlag(TextStyle.Superscript))
            {
                styleContent.Append(" style:text-position=\"super 58%\"");
            }

            styleContent.Append("/>");
        }

        /// <summary>
        /// Append the given paragraph properties to the XML style informations
        /// </summary>
        /// <param name="styleContent">The content container for the style</param>
        /// <param name="style">The style for the style informations</param>
        internal static void AppendParagraphProperties(in StringBuilder styleContent, Style style)
        {
            if(!style.TextStyle.HasFlag(TextStyle.Left)
            && !style.TextStyle.HasFlag(TextStyle.Center)
            && !style.TextStyle.HasFlag(TextStyle.Right)
            && !style.TextStyle.HasFlag(TextStyle.Justify)
            && !style.TextStyle.HasFlag(TextStyle.PageBreak))
            {
                return;
            }

            // Note: Don't forget the spaces between the XML properties

            styleContent.Append("<");
            styleContent.Append("style:paragraph-properties");

            if(style.TextStyle.HasFlag(TextStyle.Left))
            {
                styleContent.Append(" fo:text-align=\"start\"");
                styleContent.Append(" style:justify-single-word=\"false\"");
            }

            if(style.TextStyle.HasFlag(TextStyle.Center))
            {
                styleContent.Append(" fo:text-align=\"center\"");
                styleContent.Append(" style:justify-single-word=\"false\"");
            }

            if(style.TextStyle.HasFlag(TextStyle.Right))
            {
                styleContent.Append(" fo:text-align=\"end\"");
                styleContent.Append(" style:justify-single-word=\"false\"");
            }

            if(style.TextStyle.HasFlag(TextStyle.Justify))
            {
                styleContent.Append(" fo:text-align=\"justify\"");
                styleContent.Append(" style:justify-single-word=\"false\"");
            }

            if(style.TextStyle.HasFlag(TextStyle.Justify))
            {
                styleContent.Append(" fo:break-before=\"page\"");
            }

            styleContent.Append("/>");
        }

        /// <summary>
        /// Append a XML end element for XML style informations
        /// </summary>
        /// <param name="styleContent">The content container for the style</param>
        internal static void AppendXmlStyleEnd(in StringBuilder styleContent)
            => styleContent.Append("</style:style>");

        /// <summary>
        /// Return the name representation for a style for the given table cell
        /// </summary>
        /// <param name="rowNumber">The row number of the current cell</param>
        /// <param name="columnNumber">The column number of the current cell</param>
        /// <param name="columnCount">The column count of the current row</param>
        /// <returns>The name representation of a table column</returns>
        internal static string GetTableCellStyleName(in int rowNumber, in int columnNumber, in int columnCount)
        {
            var number = rowNumber == 1 ? "1" : "2";
            var prefix = columnCount == columnNumber ? "E" : "A";

            return prefix + number;
        }

        /// <summary>
        /// Return the color value of the given <see cref="Color"/>
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to convert into a value</param>
        /// <returns>A color value</returns>
        internal static string GetColorValue(Color color)
            => $"#{color.R:X2}{color.G:X2}{color.B:X2}";

        /// <summary>
        /// Return the <see cref="OfficeValueType"/> for the given value
        /// </summary>
        /// <param name="value">The value for the <see cref="OfficeValueType"/></param>
        /// <returns>A <see cref="OfficeValueType"/></returns>
        internal static OfficeValueType GetValueType(object value)
            => value switch
            {
                bool _          => OfficeValueType.Boolean,
                byte _          => OfficeValueType.Float,
                sbyte _         => OfficeValueType.Float,
                short _         => OfficeValueType.Float,
                ushort _        => OfficeValueType.Float,
                int _           => OfficeValueType.Float,
                uint _          => OfficeValueType.Float,
                long _          => OfficeValueType.Float,
                ulong _         => OfficeValueType.Float,
                float _         => OfficeValueType.Float,
                double _        => OfficeValueType.Float,
                decimal _       => OfficeValueType.Currency,
                char _          => OfficeValueType.String,
                string _        => OfficeValueType.String,
                StringBuilder _ => OfficeValueType.String,
                DateTime _      => OfficeValueType.Date,
                TimeSpan _      => OfficeValueType.Time,
                _               => throw new ArgumentOutOfRangeException(nameof(value), value, "Object type for cell not supported"),
            };

        /// <summary>
        /// Return the name of the given <see cref="OfficeValueType"/>
        /// </summary>
        /// <param name="valueType">The <see cref="OfficeValueType"/> for the name</param>
        /// <returns>A name of a value type</returns>
        internal static string GetValueTypeName(OfficeValueType valueType)
            => valueType switch
            {
                OfficeValueType.Float         => "float",
                OfficeValueType.Percentage    => "percentage",
                OfficeValueType.Currency      => "currency",
                OfficeValueType.Date          => "date",
                OfficeValueType.Time          => "time",
                OfficeValueType.Scientific    => "float",
                OfficeValueType.Fraction      => "float",
                OfficeValueType.Boolean       => "boolean",
                OfficeValueType.String        => "string",
                _                       => throw new ArgumentOutOfRangeException(nameof(valueType),
                                                                                 valueType,
                                                                                 "The value type is not supported"),
            };

        /// <summary>
        /// Return the specific arguments for the "office values" of the given value
        /// </summary>
        /// <param name="value">The value for the "office values"</param>
        /// <returns>The "office values"</returns>
        internal static string GetOfficeValues(object value)
        {
            // see https://docs.microsoft.com/de-de/dotnet/standard/base-types/standard-numeric-format-strings

            var valueType     = GetValueType(value);
            var valueTypeName = GetValueTypeName(valueType);

            switch (valueType)
            {
                case OfficeValueType.Float:
                    return $"office:value-type=\"{valueTypeName}\" office:value=\"{value:F}\"";

                case OfficeValueType.Percentage:
                    return $"office:value-type=\"{valueTypeName}\" office:value=\"{value:F}\"";

                case OfficeValueType.Currency:
                    return $"office:value-type=\"{valueTypeName}\" office:currency=\"EUR\" office:value=\"{value:C}\"";

                case OfficeValueType.Date:
                    return $"office:value-type=\"{valueTypeName}\" office:date-value=\"{value:yyyy}-{value:MM}-{value:dd}\"";

                case OfficeValueType.Time:
                    return $"office:value-type=\"{valueTypeName}\" office:time-value=\"PT{value:hh}H{value:mm}M{value:ss}S\"";

                case OfficeValueType.Scientific:
                    return $"office:value-type=\"{valueTypeName}\" office:value=\"{value:E}\"";

                case OfficeValueType.Fraction:
                    return $"office:value-type=\"{valueTypeName}\" office:value=\"{value:F}\"";

                case OfficeValueType.Boolean:
                    return $"office:value-type=\"{valueTypeName}\" office:boolean-value=\"{value}\"";

                case OfficeValueType.String:
                    return $"office:value-type=\"{valueTypeName}\"";

                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                                                          value,
                                                          "The type of value is not supported for 'office values'");
            }
        }

        /// <summary>
        /// Return a formated value base on the type of the given value
        /// </summary>
        /// <param name="value">The value to formate</param>
        /// <returns>A formated value</returns>
        internal static string GetFormatedValue(object value)
        {
            // see https://docs.microsoft.com/de-de/dotnet/standard/base-types/standard-numeric-format-strings

            switch (GetValueType(value))
            {
                case OfficeValueType.Float:
                    return $"{value:F}";
                case OfficeValueType.Percentage:
                    return $"{value:P}";
                case OfficeValueType.Currency:
                    return $"{value:C}";
                case OfficeValueType.Date:
                    return $"{value:yyyy}-{value:MM}-{value:dd}";
                case OfficeValueType.Time:
                    return $"{value:hh}:{value:mm}:{value:ss}";
                case OfficeValueType.Scientific:
                    return $"{value:E}";
                case OfficeValueType.Fraction:
                    return $"{value}";         // There is currently no function to generate a ggt or similar
                case OfficeValueType.Boolean:
                    return $"{value}";
                case OfficeValueType.String:
                    return $"{value}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(value),
                                                          value,
                                                          "The type of value is not supported");
            }
        }
    }
}
