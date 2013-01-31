/*
·--------------------------------------------------------------------·
| ReportingCloud - Engine                                            |
| Copyright (c) 2010, FlexibleCoder.                                 |
| https://sourceforge.net/projects/reportingcloud                    |
·--------------------------------------------------------------------·
| This library is free software; you can redistribute it and/or      |
| modify it under the terms of the GNU Lesser General Public         |
| License as published by the Free Software Foundation; either       |
| version 2.1 of the License, or (at your option) any later version. |
|                                                                    |
| This library is distributed in the hope that it will be useful,    |
| but WITHOUT ANY WARRANTY; without even the implied warranty of     |
| MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  |
| Lesser General Public License for more details.                    |
|                                                                    |
| GNU LGPL: http://www.gnu.org/copyleft/lesser.html                  |
·--------------------------------------------------------------------·
*/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ReportingCloud.Engine
{
    ///<summary>
    /// Renders a report to PDF. This is a page oriented formatting renderer.
    ///</summary>
    internal class RenderPdf : IPresent
    {
        //report
        private Report report;
        //where the output is going
        private Stream outputStream;

        //the page height
        private int pageHeight;
        
        //document
        private Document document = new Document();
        private MemoryStream memoryStream = new MemoryStream();
        private PdfContentByte pdfContent;

        //list iTextSharp BaseFont added
        private List<BaseFont> BaseFonts = new List<BaseFont>();
        //list font names
        private List<string> BaseFontNames = new List<string>();

        public RenderPdf(Report report, IStreamGen stream)
        {
            this.report = report;
            outputStream = stream.GetStream();
        }

        public Report Report()
        {
            return report;
        }

        public bool IsPagingNeeded()
        {
            return true;
        }

        /// <summary>
        /// Start the process
        /// </summary>
        public void Start()
        {
            //register the system font folder
            FontFactory.RegisterDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));

            //create the pdf writer and open the document
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            //assigne the pdf content to the writer
            pdfContent = writer.DirectContent;

            //stamp the pdf info
            document.AddAuthor(report.Author);
            document.AddCreationDate();
            document.AddCreator("ReportingCloud");
            document.AddSubject(report.Description);
            document.AddTitle(report.Name);
        }

        /// <summary>
        /// Finalize the process
        /// </summary>
        public void End()
        {
            //close the document
            document.Close();

            //write out ItextSharp pdf stream
            byte[] contentbyte = memoryStream.GetBuffer();
            outputStream.Write(contentbyte, 0, contentbyte.Length);
            memoryStream.Dispose();
            BaseFonts.Clear();
            BaseFontNames.Clear();
        }

        public void RunPages(Pages pages)
        {
            //loop thru the pages
            foreach (Page page in pages)
            {
                //set the page height
                pageHeight = (int)report.ReportDefinition.PageHeight.Points;

                //set the page size in the document and create a new document page
                document.SetPageSize(new iTextSharp.text.Rectangle(report.ReportDefinition.PageWidth.Points, report.ReportDefinition.PageHeight.Points));
                document.NewPage();

                //process the page
                processPage(pages, page);
            }
        }

        /// <summary>
        /// Render all the objects in a page
        /// </summary>
        private void processPage(Pages pages, IEnumerable page)
        {
            //loop thru the items in the page
            foreach (PageItem pageItem in page)
            {
                if (pageItem.SI.BackgroundImage != null)
                {
                    //put out any background image
                    PageImage backgroundImage = pageItem.SI.BackgroundImage;

                    float imageWidth = RSize.PointsFromPixels(pages.G, backgroundImage.SamplesW);
                    float imageHeight = RSize.PointsFromPixels(pages.G, backgroundImage.SamplesH);
                    int repeatX = 0;
                    int repeatY = 0;
                    float itemWidth = pageItem.W - (pageItem.SI.PaddingLeft + pageItem.SI.PaddingRight);
                    float itemHeight = pageItem.H - (pageItem.SI.PaddingTop + pageItem.SI.PaddingBottom);

                    switch (backgroundImage.Repeat)
                    {
                        case ImageRepeat.Repeat:
                            repeatX = (int)Math.Floor(itemWidth / imageWidth);
                            repeatY = (int)Math.Floor(itemHeight / imageHeight);
                            break;

                        case ImageRepeat.RepeatX:
                            repeatX = (int)Math.Floor(itemWidth / imageWidth);
                            repeatY = 1;
                            break;

                        case ImageRepeat.RepeatY:
                            repeatY = (int)Math.Floor(itemHeight / imageHeight);
                            repeatX = 1;
                            break;

                        case ImageRepeat.NoRepeat:
                        default:
                            repeatX = repeatY = 1;
                            break;
                    }

                    //make sure the image is drawn at least 1 times 
                    repeatX = Math.Max(repeatX, 1);
                    repeatY = Math.Max(repeatY, 1);

                    float currX = pageItem.X + pageItem.SI.PaddingLeft;
                    float currY = pageItem.Y + pageItem.SI.PaddingTop;
                    float startX = currX;
                    float startY = currY;
                    for (int i = 0; i < repeatX; i++)
                    {
                        for (int j = 0; j < repeatY; j++)
                        {
                            currX = startX + i * imageWidth;
                            currY = startY + j * imageHeight;

                            addImage(backgroundImage.SI, currX, currY, imageWidth, imageHeight, RectangleF.Empty,
                                backgroundImage.ImageData, null, pageItem.Tooltip);
                        }
                    }
                }
                else if (pageItem is PageTextHtml)
                {
                    PageTextHtml pageTextHtml = pageItem as PageTextHtml;
                    pageTextHtml.Build(pages.G);
                    processPage(pages, pageTextHtml);
                    continue;
                }
                else if (pageItem is PageText)
                {
                    PageText pageText = pageItem as PageText;
                    float[] textwidth;
                    string[] measureStrings = RenderUtility.MeasureString(pageText, pages.G, out textwidth);
                    addText(pageText.X, pageText.Y, pageText.W, pageText.H, measureStrings, pageText.SI, textwidth, pageText.CanGrow, pageText.HyperLink, pageText.NoClip, pageText.Tooltip);
                    continue;
                }
                else if (pageItem is PageLine)
                {
                    PageLine pageLine = pageItem as PageLine;
                    addLine(pageLine.X, pageLine.Y, pageLine.X2, pageLine.Y2, pageLine.SI);
                    continue;
                }
                else if (pageItem is PageEllipse)
                {
                    PageEllipse pageEllipse = pageItem as PageEllipse;
                    addEllipse(pageEllipse.X, pageEllipse.Y, pageEllipse.W, pageEllipse.H, pageEllipse.SI, pageEllipse.HyperLink);
                    continue;
                }
                else if (pageItem is PageImage)
                {
                    PageImage pageImage = pageItem as PageImage;

                    //Duc Phan added 20 Dec, 2007 to support sized image 
                    RectangleF r2 = new RectangleF(pageImage.X + pageImage.SI.PaddingLeft, pageImage.Y + pageImage.SI.PaddingTop,
                        pageImage.W - pageImage.SI.PaddingLeft - pageImage.SI.PaddingRight, pageImage.H - pageImage.SI.PaddingTop - pageImage.SI.PaddingBottom);

                    //work rectangle 
                    RectangleF adjustedRect;
                    RectangleF clipRect = RectangleF.Empty;
                    switch (pageImage.Sizing)
                    {
                        case ImageSizingEnum.AutoSize:
                            adjustedRect = new RectangleF(r2.Left, r2.Top, r2.Width, r2.Height);
                            break;

                        case ImageSizingEnum.Clip:
                            adjustedRect = new RectangleF(r2.Left, r2.Top, RSize.PointsFromPixels(pages.G, pageImage.SamplesW),
                                RSize.PointsFromPixels(pages.G, pageImage.SamplesH));
                            clipRect = new RectangleF(r2.Left, r2.Top, r2.Width, r2.Height);
                            break;

                        case ImageSizingEnum.FitProportional:
                            float height;
                            float width;
                            float ratioIm = (float)pageImage.SamplesH / pageImage.SamplesW;
                            float ratioR = r2.Height / r2.Width;
                            height = r2.Height;
                            width = r2.Width;
                            if (ratioIm > ratioR)
                            {
                                //this means the rectangle width must be corrected 
                                width = height * (1 / ratioIm);
                            }
                            else if (ratioIm < ratioR)
                            {
                                //this means the rectangle height must be corrected 
                                height = width * ratioIm;
                            }
                            adjustedRect = new RectangleF(r2.X, r2.Y, width, height);
                            break;

                        case ImageSizingEnum.Fit:
                        default:
                            adjustedRect = r2;
                            break;
                    }

                    if (pageImage.ImgFormat != System.Drawing.Imaging.ImageFormat.Wmf && pageImage.ImgFormat != System.Drawing.Imaging.ImageFormat.Emf)
                        addImage(pageImage.SI, adjustedRect.X, adjustedRect.Y, adjustedRect.Width, adjustedRect.Height,
                            clipRect, pageImage.ImageData, pageImage.HyperLink, pageImage.Tooltip);
                    continue;
                }
                else if (pageItem is PageRectangle)
                {
                    PageRectangle pageRectangle = pageItem as PageRectangle;
                    addRectangle(pageRectangle.X, pageRectangle.Y, pageRectangle.W, pageRectangle.H, pageItem.SI, pageItem.HyperLink, pageItem.Tooltip);
                    continue;
                }
                else if (pageItem is PagePie)
                {
                    PagePie pagePie = pageItem as PagePie;
                    addPie(pagePie.X, pagePie.Y, pagePie.W, pagePie.H, pageItem.SI, pageItem.HyperLink, pageItem.Tooltip);
                    continue;
                }
                else if (pageItem is PagePolygon)
                {
                    PagePolygon pagePolygon = pageItem as PagePolygon;
                    addPolygon(pagePolygon.Points, pageItem.SI, pageItem.HyperLink);
                    continue;
                }
                else if (pageItem is PageCurve)
                {
                    PageCurve pageCurve = pageItem as PageCurve;
                    addCurve(pageCurve.Points, pageItem.SI);
                    continue;
                }
            }
        }


        /// <summary>
        /// Page line element at the X Y to X2 Y2 position
        /// </summary>
        /// <returns></returns>
        private void addLine(float x, float y, float x2, float y2, StyleInfo styleInfo)
        {
            addLine(x, y, x2, y2, styleInfo.BWidthTop, styleInfo.BColorTop, styleInfo.BStyleTop);
        }

        /// <summary>
        /// Page line element at the X Y to X2 Y2 position
        /// </summary>
        /// <returns></returns>
        private void addLine(float x, float y, float x2, float y2, float width, Color color, BorderStyleEnum borderStyle)
        {
            //set the line color and width
            pdfContent.SetRGBColorStroke(color.R, color.G, color.B);
            pdfContent.SetLineWidth(width / 2);

            //set the line style Dotted - Dashed - Solid (default)
            switch (borderStyle)
            {
                case BorderStyleEnum.Dotted:
                    pdfContent.SetLineDash(new float[] { '2' }, 0);
                    break;
                case BorderStyleEnum.Dashed:
                    pdfContent.SetLineDash(new float[] { '3', '2' }, 0);
                    break;
                default:
                    pdfContent.SetLineDash(new float[] { }, 0);
                    break;
            }

            //designe the line
            pdfContent.MoveTo(x, pageHeight - y);
            pdfContent.LineTo(x2, pageHeight - y2);
            pdfContent.Stroke();
        }

        /// <summary>
        /// Add a filled rectangle
        /// </summary>
        /// <returns></returns>
        private void addFillRectangle(float x, float y, float width, float height, Color color)
        {
            pdfContent.SetColorFill(new BaseColor(color));
            pdfContent.Rectangle(x, pageHeight - y - height, width, height);
            pdfContent.Fill();
        }

        /// <summary>
        /// Add a filled and stroked rectangle
        /// </summary>
        /// <returns></returns>
        private void addFillRectangle(float x, float y, float width, float height, StyleInfo styleInfo)
        {
            //first design the filled rectangle
            addFillRectangle(x, y, width, height, styleInfo.BackgroundColor);

            //second, stroke the rectangle
            pdfContent.SetColorStroke(new BaseColor(styleInfo.Color));
            pdfContent.ClosePathStroke();
        }

        /// <summary>
        /// Add border
        /// </summary>
        private void addBorder(StyleInfo styleInfo, float x, float y, float width, float height)
        {
            //no bounding box to use
            if (height <= 0 || width <= 0)
                return;

            float bottom = y + height;
            float right = x + width;

            if (styleInfo.BStyleTop != BorderStyleEnum.None && styleInfo.BWidthTop > 0)
                addLine(x, y, right, y, styleInfo.BWidthTop, styleInfo.BColorTop, styleInfo.BStyleTop);

            if (styleInfo.BStyleRight != BorderStyleEnum.None && styleInfo.BWidthRight > 0)
                addLine(right, y, right, bottom, styleInfo.BWidthRight, styleInfo.BColorRight, styleInfo.BStyleRight);

            if (styleInfo.BStyleLeft != BorderStyleEnum.None && styleInfo.BWidthLeft > 0)
                addLine(x, y, x, bottom, styleInfo.BWidthLeft, styleInfo.BColorLeft, styleInfo.BStyleLeft);

            if (styleInfo.BStyleBottom != BorderStyleEnum.None && styleInfo.BWidthBottom > 0)
                addLine(x, bottom, right, bottom, styleInfo.BWidthBottom, styleInfo.BColorBottom, styleInfo.BStyleBottom);
        }

        /// <summary>
        /// Add image
        /// </summary>
        private void addImage(StyleInfo styleInfo, float x, float y, float width, float height, 
            RectangleF clipRectangle, byte[] image, string url, string tooltip)
        {
            //add the image with the zoom and position
            iTextSharp.text.Image pdfImge = iTextSharp.text.Image.GetInstance(image);
            pdfImge.ScaleAbsolute(width, height);
            pdfImge.SetAbsolutePosition(x, pageHeight - y - height);
            document.Add(pdfImge);

            //add url
            if (url != null)
                document.Add(new Annotation(x, pageHeight - y, height, width, url));

            //add tooltip
            if (!string.IsNullOrEmpty(tooltip))
                document.Add(new Annotation(x, pageHeight - y, height, width, tooltip));

            //add any required border
            addBorder(styleInfo, x - styleInfo.PaddingLeft, y - styleInfo.PaddingTop, width + styleInfo.PaddingLeft + styleInfo.PaddingRight, 
                height + styleInfo.PaddingTop + styleInfo.PaddingBottom);			
        }

        /// <summary>
        /// Add polygon
        /// </summary>
        internal void addPolygon(PointF[] points, StyleInfo styleInfo, string url)
        {
            //nothing to do
            if (styleInfo.BackgroundColor.IsEmpty)
                return;         

            //get the fill color - could be a gradient or pattern etc ...
            Color color = styleInfo.BackgroundColor;
            addPoints(points);
            pdfContent.SetRGBColorFillF(color.R, color.G, color.B);
            pdfContent.ClosePathFillStroke();
        }

        /// <summary>
        /// Add points
        /// </summary>
        private void addPoints(PointF[] points)
        {
            if (points.Length > 0)
            {
                pdfContent.MoveTo(points[0].X, pageHeight - points[0].Y);
                for (int point = 1; point < points.Length; point++)
                    pdfContent.LineTo(points[point].X, pageHeight - points[point].Y);
            }
        }

        /// <summary>
        /// Add rectangle at the X Y position
        /// </summary>
        private void addRectangle(float x, float y, float width, float height, StyleInfo styleInfo, string url, string tooltip)
        {
            //draw background rectangle if needed (background color, height and width are specified)
            if (!styleInfo.BackgroundColor.IsEmpty && height > 0 && width > 0)
                addFillRectangle(x, y, width, height, styleInfo);

            //add any required border
            addBorder(styleInfo, x, y, width, height);			

            //add url
            if (url != null)
                document.Add(new Annotation(x, pageHeight - y, height, width, url));

            //add tooltip
            if (!string.IsNullOrEmpty(tooltip))
                document.Add(new Annotation(x, pageHeight - y, height, width, tooltip));
        }

        /// <summary>
        /// Add pie
        /// </summary>
        private void addPie(float x, float y, float width, float height, StyleInfo si, string url, string tooltip)
        {
            //add any required border
            addBorder(si, x, y, width, height);			

            //add url
            if (url != null)
                document.Add(new Annotation(x, pageHeight - y, height, width, url));

            //add tooltip
            if (!string.IsNullOrEmpty(tooltip))
                document.Add(new Annotation(x, pageHeight - y, height, width, tooltip));
        }

        /// <summary>
        /// Add curve
        /// </summary>
        private void addCurve(PointF[] points, StyleInfo styleInfo)
        {
            //do a spline curve or just do a line segment
            if (points.Length > 2)
                doCurve(points, getCurveTangents(points), styleInfo);
            else
                addLine(points[0].X, points[0].Y, points[1].X, points[1].Y, styleInfo);
        }

        /// <summary>
        /// Create curve
        /// </summary>
        private void doCurve(PointF[] points, PointF[] tangents, StyleInfo styleInfo)
        {
            for (int i = 0; i < points.Length - 1; i++)
            {
                float x0 = points[i].X;
                float y0 = points[i].Y;

                float x1 = points[i].X + tangents[i].X;
                float y1 = points[i].Y + tangents[i].Y;

                float x2 = points[i + 1].X - tangents[i + 1].X;
                float y2 = points[i + 1].Y - tangents[i + 1].Y;

                float x3 = points[i + 1].X;
                float y3 = points[i + 1].Y;

                addCurve(x0, y0, x1, y1, x2, y2, x3, y3, styleInfo, null);
            }
        }

        /// <summary>
        /// Get curve tangents
        /// </summary>
        private PointF[] getCurveTangents(PointF[] points)
        {
            //this is the tension used on the DrawCurve GDI call.
            float tension = .5f;
            float coefficient = tension / 3.0f;

            PointF[] tangents = new PointF[points.Length];

            //initialize everything to zero to begin with
            for (int i = 0; i < tangents.Length; i++)
            {
                tangents[i].X = 0;
                tangents[i].Y = 0;
            }

            if (tangents.Length <= 2)
                return tangents;

            for (int i = 0; i < tangents.Length; i++)
            {
                int r = i + 1;
                int s = i - 1;

                if (r >= points.Length)
                    r = points.Length - 1;
                if (s < 0)
                    s = 0;

                tangents[i].X += (coefficient * (points[r].X - points[s].X));
                tangents[i].Y += (coefficient * (points[r].Y - points[s].Y));
            }

            return tangents;
        }

        /// <summary>
        /// Add curve
        /// </summary>
        private void addCurve(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, StyleInfo styleInfo, string url)
        {
            if (styleInfo.BStyleTop != BorderStyleEnum.None)
            {
                switch (styleInfo.BStyleTop)
                {
                    case BorderStyleEnum.Dashed:
                        pdfContent.SetLineDash(new float[] { '3', '2' }, 0);
                        break;
                    case BorderStyleEnum.Dotted:
                        pdfContent.SetLineDash(new float[] { '2' }, 0);
                        break;
                    case BorderStyleEnum.Solid:
                    default:
                        pdfContent.SetLineDash(new float[] { }, 0);
                        break;
                }
                pdfContent.SetRGBColorStroke(styleInfo.BColorTop.R, styleInfo.BColorTop.G, styleInfo.BColorTop.B);
            }

            if (!styleInfo.BackgroundColor.IsEmpty)
            {
                pdfContent.SetRGBColorStrokeF(styleInfo.BackgroundColor.R, styleInfo.BackgroundColor.G, styleInfo.BackgroundColor.B);
            }

            pdfContent.CurveTo(x1, pageHeight - y1, x2, pageHeight - y1, x3, pageHeight - y3);

            if (styleInfo.BackgroundColor.IsEmpty)
                pdfContent.ClosePathStroke();
            else
                pdfContent.ClosePathFillStroke();
        }

        /// <summary>
        /// Add ellipse
        /// </summary>
        private void addEllipse(float x, float y, float width, float height, StyleInfo styleInfo, string url)
        {
            if (styleInfo.BStyleTop != BorderStyleEnum.None)
            {
                switch (styleInfo.BStyleTop)
                {
                    case BorderStyleEnum.Dashed:
                        pdfContent.SetLineDash(new float[] { '3', '2' }, 0);
                        break;
                    case BorderStyleEnum.Dotted:
                        pdfContent.SetLineDash(new float[] { '2' }, 0);
                        break;
                    case BorderStyleEnum.Solid:
                    default:
                        pdfContent.SetLineDash(new float[] { }, 0);
                        break;
                }
                pdfContent.SetRGBColorStroke(styleInfo.BColorTop.R, styleInfo.BColorTop.G, styleInfo.BColorTop.B);
            }

            pdfContent.Ellipse(x, pageHeight - y, x + (width / 2.0f), y + (height / 2.0f));

            if (!styleInfo.BackgroundColor.IsEmpty)
                pdfContent.SetRGBColorStrokeF(styleInfo.BackgroundColor.R, styleInfo.BackgroundColor.G, styleInfo.BackgroundColor.B);

            if (styleInfo.BackgroundColor.IsEmpty)
                pdfContent.ClosePathStroke();
            else
                pdfContent.ClosePathFillStroke();
        }

        /// <summary>
        /// Add text at the X Y position; multiple lines handled
        /// </summary>
        private void addText(float x, float y, float width, float height, string[] textLines,
            StyleInfo styleInfo, float[] textWidths, bool wrap, string url, bool noClip, string tooltip)
        {
            RenderFont renderFont = RenderUtility.GetRenderFont(styleInfo);
            BaseFont baseFont = null;

            //convert the render font type to iTextSharp type
            int fontType = 0;
            switch (renderFont.Style)
            {
                case RenderFont.FontStyle.Bold:
                    fontType = iTextSharp.text.Font.BOLD;
                    break;
                case RenderFont.FontStyle.Italic:
                    fontType = iTextSharp.text.Font.ITALIC;
                    break;
                case RenderFont.FontStyle.BoldItalic:
                    fontType = iTextSharp.text.Font.BOLDITALIC;
                    break;
                default:
                    fontType = iTextSharp.text.Font.NORMAL;
                    break;
            }

            //get the index of the font name in the list font name
            int indexBaseFont = BaseFontNames.FindIndex(delegate(string _fontname) { return _fontname == renderFont.FaceName + "_" + fontType; });

            //if not found then add the new BaseFont
            if (indexBaseFont == -1)
            {
                //create a new font or a with a new type
                baseFont = FontFactory.GetFont(renderFont.FaceName, BaseFont.IDENTITY_H, 0, fontType).BaseFont;

                //add the font face name and font to the lists
                BaseFontNames.Add(renderFont.FaceName + "_" + fontType);
                BaseFonts.Add(baseFont);
            }
            else
                baseFont = BaseFonts[indexBaseFont];

            //text alignment
            int align = 0;

            //first position x and y and the leading (usefull for all kind of justified text)
            float firstStartX = -1;
            float firstStartY = -1;
            float leading = -1;

            //chunks of text
            List<Chunk> chunks = new List<Chunk>();

            //loop thru the lines of text
            for (int i = 0; i < textLines.Length; i++)
            {
                string text = textLines[i];
                float textwidth = textWidths[i];

                float startX = x + styleInfo.PaddingLeft;						    // TODO: handle tb_rl
                float startY = y + styleInfo.PaddingTop + (i * styleInfo.FontSize);	// TODO: handle tb_rl

                if (styleInfo.WritingMode == WritingModeEnum.lr_tb)
                {
                    //calculate the x position
                    switch (styleInfo.TextAlign)
                    {
                        case TextAlignEnum.Center:
                            if (width > 0)
                            {
                                startX = x + styleInfo.PaddingLeft + (width - styleInfo.PaddingLeft - styleInfo.PaddingRight) / 2 - textwidth / 2;
                                align = Element.ALIGN_CENTER;
                            }
                            break;

                        case TextAlignEnum.Right:
                            if (width > 0)
                            {
                                startX = x + width - textwidth - styleInfo.PaddingRight - 2;
                                align = Element.ALIGN_RIGHT;
                            }
                            break;

                        case TextAlignEnum.Justified:
                        case TextAlignEnum.JustifiedLine:
                        case TextAlignEnum.JustifiedDottedLine:
                            if (width > 0)
                            {
                                startX += 2;
                                align = Element.ALIGN_JUSTIFIED;
                            }
                            break;

                        case TextAlignEnum.Left:
                        default:
                            if (width > 0)
                                startX += 2;
                            align = Element.ALIGN_LEFT;
                            break;
                    }

                    //calculate the y position
                    switch (styleInfo.VerticalAlign)
                    {
                        case VerticalAlignEnum.Middle:
                            if (height <= 0)
                                break;

                            //calculate the middle of the region
                            startY = y + styleInfo.PaddingTop + (height - styleInfo.PaddingTop - styleInfo.PaddingBottom) / 2 - styleInfo.FontSize / 2;

                            //now go up or down depending on which line
                            if (textLines.Length == 1)
                                break;

                            //even number
                            if (textLines.Length % 2 == 0)
                                startY = startY - ((textLines.Length / 2 - i) * styleInfo.FontSize) + styleInfo.FontSize / 2;
                            else
                                startY = startY - ((textLines.Length / 2 - i) * styleInfo.FontSize);
                            break;

                        case VerticalAlignEnum.Bottom:
                            if (height <= 0)
                                break;

                            startY = y + height - styleInfo.PaddingBottom - (styleInfo.FontSize * (textLines.Length - i));
                            break;

                        case VerticalAlignEnum.Top:
                        default:
                            break;
                    }
                }
                else
                {
                    //move x in a little - it draws to close to the edge of the rectangle (25% of the font size seems to work!) and Center or right align vertical text
                    startX += styleInfo.FontSize / 4;

                    switch (styleInfo.TextAlign)
                    {
                        case TextAlignEnum.Center:
                            if (height > 0)
                                startY = y + styleInfo.PaddingLeft + (height - styleInfo.PaddingLeft - styleInfo.PaddingRight) / 2 - textwidth / 2;
                            break;

                        case TextAlignEnum.Right:
                            if (width > 0)
                                startY = y + height - textwidth - styleInfo.PaddingRight;
                            break;

                        case TextAlignEnum.Left:
                        default:
                            break;
                    }
                }

                //mark the first x position for justify text
                if (firstStartX == -1)
                    firstStartX = startX;

                //mark the first y position for justify text
                if (firstStartY == -1)
                    firstStartY = startY;

                //mark the first leading height for justify text
                else if (leading == -1)
                    leading = startY - firstStartY;

                //draw background rectangle if needed (only put out on the first line, since we do whole rectangle)
                if (!styleInfo.BackgroundColor.IsEmpty && height > 0 && width > 0 && i == 0)
                    addFillRectangle(x, y, width, height, styleInfo.BackgroundColor);

                //set the clipping path, (Itext have no clip)
                if (height > 0 && width > 0)
                {
                    pdfContent.SetRGBColorFill(styleInfo.Color.R, styleInfo.Color.G, styleInfo.Color.B);

                    if (align == Element.ALIGN_JUSTIFIED)
                        chunks.Add(new Chunk(text));
                    else
                    {
                        if (styleInfo.WritingMode == WritingModeEnum.lr_tb)
                        {
                            //if textline after measure with word break can fit just simple show Text
                            if (width >= textwidth)
                            {
                                pdfContent.SaveState();
                                pdfContent.BeginText();
                                pdfContent.SetFontAndSize(baseFont, styleInfo.FontSize);
                                //same fonts dont have nativelly bold so we could simulate that
                                if (renderFont.SimulateBold)
                                {
                                    pdfContent.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL_STROKE);
                                    pdfContent.SetLineWidth(0.2f);
                                }
                                pdfContent.SetTextMatrix(startX, pageHeight - startY - styleInfo.FontSize);
                                pdfContent.ShowText(text);
                                pdfContent.EndText();
                                pdfContent.RestoreState();
                            }
                            else
                            {
                                //else use Column text to wrap or clip (wrap: for example a text like an URL so word break is not working here
                                //itextsharp ColumnText do the work for us)
                                ColumnText columnText = new ColumnText(pdfContent);
                                columnText.SetSimpleColumn(new Phrase(text, new iTextSharp.text.Font(baseFont, styleInfo.FontSize)), 
                                    x + styleInfo.PaddingLeft, pageHeight - startY, x + width - styleInfo.PaddingRight, 
                                    pageHeight - y - styleInfo.PaddingBottom - height, 10f, align);
                                columnText.Go();
                            }
                        }
                        else
                        {
                            //not checked
                            double rads = -283.0 / 180.0;
                            double radsCos = Math.Cos(rads);
                            double radsSin = Math.Sin(rads);
                            pdfContent.BeginText();
                            pdfContent.SetFontAndSize(baseFont, styleInfo.FontSize);
                            pdfContent.SetTextMatrix((float)radsCos, (float)radsSin, (float)-radsSin, (float)radsCos, startX, pageHeight - startY);
                            pdfContent.ShowText(text);
                            pdfContent.EndText();
                        }
                    }
                    
                    //add URL
                    if (url != null)
                        document.Add(new Annotation(x, pageHeight - y, height, width, url));

                    //add tooltip
                    if (tooltip != null)
                        document.Add(new Annotation(x, pageHeight - y, height, width, tooltip));
                }

                //handle underlining etc ...
                float maxX;
                switch (styleInfo.TextDecoration)
                {
                    case TextDecorationEnum.Underline:
                        maxX = width > 0 ? Math.Min(x + width, startX + textwidth) : startX + textwidth;
                        addLine(startX, startY + styleInfo.FontSize + 1, maxX, startY + styleInfo.FontSize + 1, 1, styleInfo.Color, BorderStyleEnum.Solid);
                        break;

                    case TextDecorationEnum.LineThrough:
                        maxX = width > 0 ? Math.Min(x + width, startX + textwidth) : startX + textwidth;
                        addLine(startX, startY + (styleInfo.FontSize / 2) + 1, maxX, startY + (styleInfo.FontSize / 2) + 1, 1, styleInfo.Color, BorderStyleEnum.Solid);
                        break;

                    case TextDecorationEnum.Overline:
                        maxX = width > 0 ? Math.Min(x + width, startX + textwidth) : startX + textwidth;
                        addLine(startX, startY + 1, maxX, startY + 1, 1, styleInfo.Color, BorderStyleEnum.Solid);
                        break;

                    case TextDecorationEnum.None:
                    default:
                        break;
                }
            }

            //add text to justify
            if (chunks.Count > 0)
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Alignment = align;
                paragraph.Leading = leading;
                paragraph.Font = new iTextSharp.text.Font(baseFont, styleInfo.FontSize);

                //join all text (is necessary to justify alignment)
                foreach (Chunk chunk in chunks)
                    paragraph.Add(chunk);

                //add separator for kind of justified alignments
                if (styleInfo.TextAlign == TextAlignEnum.JustifiedLine)
                {
                    iTextSharp.text.pdf.draw.LineSeparator lineSeparator = new iTextSharp.text.pdf.draw.LineSeparator();
                    lineSeparator.Offset = 3f;
                    lineSeparator.LineColor = new BaseColor(styleInfo.Color);
                    paragraph.Add(new Chunk(lineSeparator));
                }
                else if (styleInfo.TextAlign == TextAlignEnum.JustifiedDottedLine)
                {
                    iTextSharp.text.pdf.draw.DottedLineSeparator dottedLineSeparator = new iTextSharp.text.pdf.draw.DottedLineSeparator();
                    dottedLineSeparator.Offset = 3f;
                    dottedLineSeparator.LineColor = new BaseColor(styleInfo.Color);
                    paragraph.Add(new Chunk(dottedLineSeparator));
                }

                ColumnText columnText = new ColumnText(pdfContent);
                //start from the top of the column
                columnText.UseAscender = true;
                //width, y position from the bottom page (compensate 2 units), x position, 0
                columnText.SetSimpleColumn(firstStartX, pageHeight - firstStartY - 2, firstStartX + width - styleInfo.PaddingRight, 0);
                columnText.AddElement(paragraph);
                columnText.Go();
            }

            //add any required border
            addBorder(styleInfo, x, y, width, height);
        }

        // Body: main container for the report
        public void BodyStart(Body b)
        {
        }

        public void BodyEnd(Body b)
        {
        }

        public void PageHeaderStart(PageHeader ph)
        {
        }

        public void PageHeaderEnd(PageHeader ph)
        {
        }

        public void PageFooterStart(PageFooter pf)
        {
        }

        public void PageFooterEnd(PageFooter pf)
        {
        }

        public void Textbox(Textbox tb, string t, Row row)
        {
        }

        public void DataRegionNoRows(DataRegion d, string noRowsMsg)
        {
        }

        // Lists
        public bool ListStart(List l, Row r)
        {
            return true;
        }

        public void ListEnd(List l, Row r)
        {
        }

        public void ListEntryBegin(List l, Row r)
        {
        }

        public void ListEntryEnd(List l, Row r)
        {
        }

        // Tables					// Report item table
        public bool TableStart(Table t, Row row)
        {
            return true;
        }

        public void TableEnd(Table t, Row row)
        {
        }

        public void TableBodyStart(Table t, Row row)
        {
        }

        public void TableBodyEnd(Table t, Row row)
        {
        }

        public void TableFooterStart(Footer f, Row row)
        {
        }

        public void TableFooterEnd(Footer f, Row row)
        {
        }

        public void TableHeaderStart(Header h, Row row)
        {
        }

        public void TableHeaderEnd(Header h, Row row)
        {
        }

        public void TableRowStart(TableRow tr, Row row)
        {
        }

        public void TableRowEnd(TableRow tr, Row row)
        {
        }

        public void TableCellStart(TableCell t, Row row)
        {
            return;
        }

        public void TableCellEnd(TableCell t, Row row)
        {
            return;
        }

        public bool MatrixStart(Matrix m, MatrixCellEntry[,] matrix, Row r, int headerRows, int maxRows, int maxCols)				// called first
        {
            return true;
        }

        public void MatrixColumns(Matrix m, MatrixColumns mc)	// called just after MatrixStart
        {
        }

        public void MatrixCellStart(Matrix m, ReportItem ri, int row, int column, Row r, float h, float w, int colSpan)
        {
        }

        public void MatrixCellEnd(Matrix m, ReportItem ri, int row, int column, Row r)
        {
        }

        public void MatrixRowStart(Matrix m, int row, Row r)
        {
        }

        public void MatrixRowEnd(Matrix m, int row, Row r)
        {
        }

        public void MatrixEnd(Matrix m, Row r)				// called last
        {
        }

        public void Chart(Chart c, Row r, ChartBase cb)
        {
        }

        public void Image(ReportingCloud.Engine.Image i, Row r, string mimeType, Stream ior)
        {
        }

        public void Line(Line l, Row r)
        {
            return;
        }

        public bool RectangleStart(Engine.Rectangle rect, Row r)
        {
            return true;
        }

        public void RectangleEnd(Engine.Rectangle rect, Row r)
        {
        }

        public void Subreport(Subreport s, Row r)
        {
        }

        public void GroupingStart(Grouping g)			// called at start of grouping
        {
        }

        public void GroupingInstanceStart(Grouping g)	// called at start for each grouping instance
        {
        }

        public void GroupingInstanceEnd(Grouping g)	// called at start for each grouping instance
        {
        }

        public void GroupingEnd(Grouping g)			// called at end of grouping
        {
        }
    }
}