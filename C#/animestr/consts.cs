using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace animestr
{
    public static class Consts
    {
        public const string noString = "BA";

        public const string VERSION = "0.0.1";

        public const string AD_BASE = "http://animedao.me";
        public const string AD_POPULAR = "http://animedao.me/popular-anime";
        public const string AD_SEARCH = "http://animedao.me/search";

        public static readonly string[] ASCII_TEXT3 = new string[] {
@"               _)                         |         ",
@"   _` |  __ \   |  __ `__ \    _ \   __|  __|   __| ",
@"  (   |  |   |  |  |   |   |   __/ \__ \  |    |    ",
@" \__,_| _|  _| _| _|  _|  _| \___| ____/ \__| _|    ",
@"                                                    "};
        public static readonly string[] ASCII_TEXT1 = new string[] {
@"               ..                         .         ",
@"   .. .  .. .   .  .. ... .    . .   ...  ...   ... ",
@"  .   .  .   .  .  .   .   .   ... ... .  .    .    ",
@" ...... ..  .. .. ..  ..  .. ..... ..... .... ..    ",
@"                                                    "};

        public static readonly string[] ASCII_TEXT2 = new string[] {
@"               ,)                         ;         ",
@"   ,` ;  ,, ;   ;  ,, `,, ;    , ;   ,,;  ,,;   ,,; ",
@"  (   ;  ;   ;  ;  ;   ;   ;   ,,; ;,, ;  ;    ;    ",
@" ;,,,,; ,;  ,; ,; ,;  ,;  ,; ;,,,; ,,,,; ;,,; ,;    ",
@"                                                    "};

        public static readonly string[][] ASCII_TEXT_FRAMES = new string[][] {
            ASCII_TEXT3,ASCII_TEXT3,ASCII_TEXT2,ASCII_TEXT1
            };
    }
}
