﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using BearLib;
using GoRogue;
using Microsoft.VisualBasic.CompilerServices;
using SpellSword.Logging;

namespace SpellSword.Render
{
    class StringPrinter
    {

        public static LinkPrint PrintLinkedText(LogMessage message, Writeable writeable, int alpha, int x = 0, int y = 0)
        {
            Dictionary<Coord, ILinkable> linkMask = new Dictionary<Coord, ILinkable>();
            int width = writeable.Width;

            int currentLine = y;
            int currentChar = x;

            foreach (string word in Regex.Split(message.Message, @"({[0-9]+})"))
            {
                bool isLink = IsLink(word);
                int linkIndex = isLink ? LinkIndex(word) : -1;

                string str = isLink ? message.Links[linkIndex].Name : word;
                Color color = Color.FromArgb(alpha, isLink ? message.Links[linkIndex].Color : Color.White);

                if (currentChar + str.Length > writeable.Width)
                {
                    currentLine += 1;
                    currentChar = 0;
                }
                UncheckedWrite(str, currentChar, currentLine, writeable, color, null);

                if(isLink)
                    foreach (Coord pos in Lines.Get(currentChar, currentLine, currentChar + str.Length - 1, currentLine, Lines.Algorithm.BRESENHAM))
                    {
                        linkMask[pos] = message.Links[linkIndex].Linkable;
                    }

                currentChar += str.Length;
            }

            return new LinkPrint(linkMask, new Coord(currentChar, currentLine));
        }

        private static bool IsLink(string str)
        {
            return str.Length >= 3 && str[0] == '{' && str[^1] == '}';
        }

        private static int LinkIndex(string str)
        {
            return int.Parse(str[1..^1]);
        }

        public static Coord PrintColored(string str, Writeable writeable, int x=0, int y=0, params Color[] colors)
        {
            string[] colorZones = str.Split("[color]");

            Color mainColor = colors[0];

            int currentZone = 1;
            bool inZone = false;

            Coord lastEnd = new Coord(x + 1, y);

            foreach (string zone in colorZones)
            {
                lastEnd = Print(zone, writeable, inZone ? colors[currentZone] : mainColor, x: lastEnd.X - 1, y: lastEnd.Y);

                if(inZone)
                    currentZone = (currentZone + 1) % colors.Length;

                inZone = !inZone;
            }

            return lastEnd;
        }

        public static Coord Print(string str, Writeable writeable, int x = 0, int y = 0)
        {
            return Print(str, writeable, Color.White, null, x, y);
        }

        public static Coord Print(string str, Writeable writeable, Color color, Color? backgroundColor = null, int x = 0, int y = 0)
        {
            string[] words = str.Split(' ');
            int width = writeable.Width;

            int currentLine = y;
            int currentChar = x;


            foreach (string word in words)
            {
                if (currentChar + word.Length > writeable.Width)
                {
                    currentLine += 1;
                    currentChar = 0;
                }

                UncheckedWrite(word, currentChar, currentLine, writeable, color, backgroundColor);
                currentChar += word.Length;

                UncheckedWrite(" ", currentChar, currentLine, writeable, color, backgroundColor);
                currentChar += 1;
            }

            return new Coord(currentChar, currentLine);
        }

        private static void UncheckedWrite(string str, int x, int y, Writeable writeable, Color textColor, Color? backgroundColor)
        {
            for (int i = 0; i < str.Length; i++)
            {
                writeable.SetGlyph(y, x + i, new Glyph((Characters) str[i], textColor, backgroundColor));
            }
        }
    }
}
