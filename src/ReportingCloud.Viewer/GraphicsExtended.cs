/*
·--------------------------------------------------------------------·
| ReportingCloud - Viewer                                            |
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

using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace System.Drawing
{
    public class GraphicsExtended
    {
        //kind of justified text line separator
        public enum JustifiedLineSeparator
        {
            None, //without line separator (standard)
            Line, //with a line separator in the last line
            DottedLine //with a dotted line separator in the last line
        }

        //drawstring justified without line separator
        public static void DrawStringJustified(Graphics graphics, string s, Font font, Brush brush, RectangleF layoutRectangle)
        {
            DrawStringJustified(graphics, s, font, brush, layoutRectangle, JustifiedLineSeparator.None);
        }

        //drawstring justified with specific line separator
        public static void DrawStringJustified(Graphics graphics, string s, Font font, Brush brush, RectangleF layoutRectangle, JustifiedLineSeparator justifiedLineSeparator)
        {
            try
            {
                //detect the line separator expression
                string lineSeparator = null;
                switch (justifiedLineSeparator)
                {
                    case JustifiedLineSeparator.Line:
                        lineSeparator = "_";
                        break;
                    case JustifiedLineSeparator.DottedLine:
                        lineSeparator = ".";
                        break;
                    default:
                        lineSeparator = " ";
                        break;
                }

                //save the current state of the graphics handle
                GraphicsState graphicsState = graphics.Save();
                //obtain the font height to be used as line height
                double lineHeight = (double)Math.Round(font.GetHeight(graphics), 1);
                //string builder to format the text
                StringBuilder text = new StringBuilder(s);
                Font originalFont = new Font(font.FontFamily, font.Size, font.Style);

                //adjust the text string to ease detection of carriage returns
                text = text.Replace("\r\n", " <CR> ");
                text = text.Replace("\r", " <CR> ");
                text.Append(" <CR> ");

                //ensure measure string will bring the best measures possible (antialias)
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                //create a string format object with the generic typographic to obtain the most accurate string measurements
                //strange, but the recommended for this case is to use a "cloned" stringformat
                StringFormat stringFormat = (StringFormat)StringFormat.GenericTypographic.Clone();

                //allow the correct measuring of spaces
                stringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

                //create a stringformat for leftalignment
                StringFormat leftAlignHandle = new StringFormat();
                leftAlignHandle.LineAlignment = StringAlignment.Near;

                //create a stringformat for rightalignment
                StringFormat rightAlignHandle = new StringFormat();
                rightAlignHandle.LineAlignment = StringAlignment.Far;

                //measure space for the given font
                SizeF stringSize = graphics.MeasureString(" ", font, layoutRectangle.Size, stringFormat);
                double spaceWidth = stringSize.Width + 1;

                //measure paragraph format for the given font
                double paragraphFormatWidth = 0;
                if (justifiedLineSeparator != JustifiedLineSeparator.None)
                {
                    SizeF paragraphFormatSize = graphics.MeasureString(lineSeparator, new Font(font.FontFamily, font.Size, FontStyle.Regular), layoutRectangle.Size, stringFormat);
                    paragraphFormatWidth = paragraphFormatSize.Width;
                }

                //total word count
                int totalWords = Regex.Matches(text.ToString(), " ").Count;

                //array of words
                ArrayList words = new ArrayList();

                //measure each word
                int n = 0;
                while (true)
                {
                    //original word
                    string word = Regex.Split(text.ToString(), " ").GetValue(n).ToString();

                    //add to words array the word without tags
                    words.Add(new Word(word.Replace("<b>", "").Replace("</b>", "").Replace("<i>", "").Replace("</i>", "")));

                    //marque to start bolding or/and italic
                    if (word.ToLower().Contains("<b>") && word.ToLower().Contains("<i>"))
                        font = new Font(originalFont.FontFamily, originalFont.Size, FontStyle.Bold & FontStyle.Italic);
                    else if (word.ToLower().Contains("<b>"))
                        font = new Font(originalFont.FontFamily, originalFont.Size, FontStyle.Bold);
                    else if (word.ToLower().Contains("<i>"))
                        font = new Font(originalFont.FontFamily, originalFont.Size, FontStyle.Italic);

                    Word currentWord = (Word)words[n];
                    currentWord.StartBold = word.ToLower().Contains("<b>");
                    currentWord.StopBold = word.ToLower().Contains("</b>");
                    currentWord.StartItalic = word.ToLower().Contains("<i>");
                    currentWord.StopItalic = word.ToLower().Contains("</i>");

                    //size of the word
                    SizeF wordSize = graphics.MeasureString(currentWord.String, font, layoutRectangle.Size, stringFormat);
                    float wordWidth = wordSize.Width;

                    if (wordWidth > layoutRectangle.Width && currentWord.String != "<CR>")
                    {
                        int reduce = 1;
                        while (true)
                        {
                            int lengthChars = (int)Math.Round(currentWord.String.Length / (wordWidth / layoutRectangle.Width), 0) - reduce;
                            string cutWord = currentWord.String.Substring(0, lengthChars);

                            //the new size of the word
                            wordSize = graphics.MeasureString(cutWord, font, layoutRectangle.Size, stringFormat);
                            wordWidth = wordSize.Width;

                            //update the word string
                            ((Word)words[n]).String = cutWord;

                            //add new word
                            if (wordWidth <= layoutRectangle.Width)
                            {
                                totalWords++;
                                words.Add(new Word("", 0,
                                    currentWord.StartBold, currentWord.StopBold,
                                    currentWord.StartItalic, currentWord.StopItalic));
                                text.Replace(currentWord.String, cutWord + " " + currentWord.String.Substring(lengthChars + 1), 0, 1);
                                break;
                            }

                            reduce++;
                        }
                    }

                    //update the word size
                    ((Word)words[n]).Length = wordWidth;

                    //marque to stop bolding or/and italic
                    if (word.ToLower().Contains("</b>") && font.Style == FontStyle.Italic)
                        font = new Font(originalFont.FontFamily, originalFont.Size, FontStyle.Italic);
                    else if (word.ToLower().Contains("</i>") && font.Style == FontStyle.Bold)
                        font = new Font(originalFont.FontFamily, originalFont.Size, FontStyle.Bold);
                    else if (word.ToLower().Contains("</b>") || word.ToLower().Contains("</i>"))
                        font = new Font(originalFont.FontFamily, originalFont.Size, FontStyle.Regular);

                    n++;
                    if (n > totalWords - 1)
                        break;
                }

                //before we start drawing, its wise to restore ou graphics objecto to its original state
                graphics.Restore(graphicsState);

                //restore to font to the original values
                font = new Font(originalFont.FontFamily, originalFont.Size, originalFont.Style);

                //start drawing word by word
                int currentLine = 0;
                for (int i = 0; i < totalWords; i++)
                {
                    bool endOfSentence = false;
                    double wordsWidth = 0;
                    int wordsInLine = 0;

                    int j = i;
                    for (j = i; j < totalWords; j++)
                    {
                        if (((Word)words[j]).String == "<CR>")
                        {
                            endOfSentence = true;
                            break;
                        }

                        wordsWidth += ((Word)words[j]).Length + spaceWidth;
                        if (wordsWidth > layoutRectangle.Width && j > i)
                        {
                            wordsWidth = wordsWidth - ((Word)words[j]).Length - (spaceWidth * wordsInLine);
                            break;
                        }

                        wordsInLine++;
                    }

                    if (j > totalWords)
                        endOfSentence = true;

                    double widthOfBetween = 0;
                    if (endOfSentence)
                        widthOfBetween = spaceWidth;
                    else
                        widthOfBetween = (layoutRectangle.Width - wordsWidth) / (wordsInLine - 1);

                    double currentTop = layoutRectangle.Top + (int)(currentLine * lineHeight);

                    if (currentTop > (layoutRectangle.Height + layoutRectangle.Top))
                    {
                        i = totalWords;
                        break;
                    }

                    double currentLeft = layoutRectangle.Left;

                    bool lastWord = false;
                    for (int currentWord = 0; currentWord < wordsInLine; currentWord++)
                    {
                        bool loop = false;

                        if (((Word)words[i]).String == "<CR>")
                        {
                            i++;
                            loop = true;
                        }

                        if (!loop)
                        {
                            //last word in sentence
                            if (currentWord == wordsInLine && !endOfSentence)
                                lastWord = true;

                            if (wordsInLine == 1)
                            {
                                currentLeft = layoutRectangle.Left;
                                lastWord = false;
                            }

                            RectangleF rectangleF;
                            StringFormat stringFormatHandle;

                            if (lastWord)
                            {
                                rectangleF = new RectangleF(layoutRectangle.Left, (float)currentTop, layoutRectangle.Width, (float)(currentTop + lineHeight));
                                stringFormatHandle = rightAlignHandle;
                            }
                            else
                            {
                                //lets zero size for word to drawstring auto-size de word
                                rectangleF = new RectangleF((float)currentLeft, (float)currentTop, 0, 0);
                                stringFormatHandle = leftAlignHandle;
                            }

                            //start bolding and/or italic
                            if (((Word)words[i]).StartBold && ((Word)words[i]).StartItalic)
                                font = new Font(originalFont.FontFamily, originalFont.Size, FontStyle.Bold & FontStyle.Italic);
                            else if (((Word)words[i]).StartBold)
                                font = new Font(originalFont.FontFamily, originalFont.Size, FontStyle.Bold);
                            else if (((Word)words[i]).StartItalic)
                                font = new Font(originalFont.FontFamily, originalFont.Size, FontStyle.Italic);

                            //draw the word
                            graphics.DrawString(((Word)words[i]).String, font, brush, rectangleF, stringFormatHandle);

                            //stop bolding and/or italic
                            if (((Word)words[i]).StopBold && font.Style == FontStyle.Italic)
                                font = new Font(originalFont.FontFamily, originalFont.Size, FontStyle.Regular);
                            else if (((Word)words[i]).StopItalic && font.Style == FontStyle.Bold)
                                font = new Font(originalFont.FontFamily, originalFont.Size, FontStyle.Bold);
                            else if (((Word)words[i]).StopBold || ((Word)words[i]).StopItalic)
                                font = new Font(originalFont.FontFamily, originalFont.Size, FontStyle.Regular);

                            //paragraph formating
                            if (endOfSentence && currentWord == wordsInLine - 1 && justifiedLineSeparator != JustifiedLineSeparator.None)
                            {
                                currentLeft += ((Word)words[i]).Length;
                                //draw until end of line
                                while (currentLeft + paragraphFormatWidth <= layoutRectangle.Left + layoutRectangle.Width)
                                {
                                    float lineSeparatorTop = (float)currentTop;
                                    if (justifiedLineSeparator == JustifiedLineSeparator.Line)
                                        lineSeparatorTop -= (float)(lineHeight / 2.5);
                                    else
                                        lineSeparatorTop -= (float)(lineHeight / 4);

                                    rectangleF = new RectangleF((float)currentLeft, lineSeparatorTop, 0, 0);
                                    //draw the paragraph format
                                    graphics.DrawString(lineSeparator, font, brush, rectangleF, stringFormatHandle);
                                    currentLeft += paragraphFormatWidth;
                                }
                            }
                            else
                                currentLeft += ((Word)words[i]).Length + widthOfBetween;

                            //go to next word
                            i++;
                        }
                    }

                    currentLine++;

                    if (i >= totalWords)
                        break;

                    //compensate endfor
                    if (((Word)words[i]).String != "<CR>")
                        i--;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //class to define the structure of the word
        public class Word
        {
            private string s;
            private double length;
            private bool startBold, stopBold;
            private bool startItalic, stopItalic;

            public Word(string s)
            {
                this.s = s;
                length = 0;
                this.startBold = false;
                this.stopBold = false;
                this.startItalic = false;
                this.stopItalic = false;
            }

            public Word(string s, double length, bool startBold, bool stopBold, bool startItalic, bool stopItalic)
            {
                this.s = s;
                this.length = length;
                this.startBold = startBold;
                this.stopBold = stopBold;
                this.startItalic = startItalic;
                this.stopItalic = stopItalic;
            }

            public string String
            {
                get
                {
                    return s;
                }
                set
                {
                    s = value;
                }
            }

            public double Length
            {
                get
                {
                    return length;
                }
                set
                {
                    length = value;
                }
            }

            public bool StartBold
            {
                get
                {
                    return startBold;
                }
                set
                {
                    startBold = value;
                }
            }

            public bool StopBold
            {
                get
                {
                    return stopBold;
                }
                set
                {
                    stopBold = value;
                }
            }

            public bool StartItalic
            {
                get
                {
                    return startItalic;
                }
                set
                {
                    startItalic = value;
                }
            }

            public bool StopItalic
            {
                get
                {
                    return stopItalic;
                }
                set
                {
                    stopItalic = value;
                }
            }
        }
    }
}