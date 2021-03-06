﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Render
{
    public enum Characters
    {
        SPACE = 32,
        EXCLAMATION_MARK = 33,
        QUOTATION_MARK = 34,
        HASH = 35,
        DOLLAR_SIGN = 36,
        PERCENT_SIGN = 37,
        AMPERSAND = 38,
        APOSTROPHE = 39,
        LEFT_PARENTHESIS = 40,
        RIGHT_PARENTHESIS = 41,
        ASTERISK = 42,
        PLUS_SIGN = 43,
        COMMA = 44,
        HYPHEN = 45,
        PERIOD = 46,
        SLASH = 47,
        ZERO = 48,
        ONE = 49,
        TWO = 50,
        THREE = 51,
        FOUR = 52,
        FIVE = 53,
        SIX = 54,
        SEVEN = 55,
        EIGHT = 56,
        NINE = 57,
        COLON = 58,
        SEMICOLON = 59,
        LESS_THAN = 60,
        EQUALS = 61,
        GREATER_THAN = 62,
        QUESTION_MARK = 63,
        AT = 64,
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        LEFT_BRACKET = 91,
        BACK_SLASH = 92,
        RIGHT_BRACKET = 93,
        CARET = 94,
        UNDERSCORE = 95,
        BACK_TICK = 96,
        a = 97,
        b = 98,
        c = 99,
        d = 100,
        e = 101,
        f = 102,
        g = 103,
        h = 104,
        i = 105,
        j = 106,
        k = 107,
        l = 108,
        m = 109,
        n = 110,
        o = 111,
        p = 112,
        q = 113,
        r = 114,
        s = 115,
        t = 116,
        u = 117,
        v = 118,
        w = 119,
        x = 120,
        y = 121,
        z = 122,
        LEFT_CURLY_BRACKET = 123,
        VERTICAL_LINE = 124,
        RIGHT_CURLY_BRACKET = 125,
        TILDE = 126,
        LATIN_CAPITAL_LETTER_C_WITH_CEDILLA = 128,
        LATIN_SMALL_LETTER_U_WITH_DIAERESIS = 129,
        LATIN_SMALL_LETTER_E_WITH_ACUTE = 130,
        LATIN_SMALL_LETTER_A_WITH_CIRCUMFLEX = 131,
        LATIN_SMALL_LETTER_A_WITH_DIAERESIS = 132,
        LATIN_SMALL_LETTER_A_WITH_GRAVE = 133,
        LATIN_SMALL_LETTER_A_WITH_RING_ABOVE = 134,
        LATIN_SMALL_LETTER_C_WITH_CEDILLA = 135,
        LATIN_SMALL_LETTER_E_WITH_CIRCUMFLEX = 136,
        LATIN_SMALL_LETTER_E_WITH_DIAERESIS = 137,
        LATIN_SMALL_LETTER_E_WITH_GRAVE = 138,
        LATIN_SMALL_LETTER_I_WITH_DIAERESIS = 139,
        LATIN_SMALL_LETTER_I_WITH_CIRCUMFLEX = 140,
        LATIN_SMALL_LETTER_I_WITH_GRAVE = 141,
        LATIN_CAPITAL_LETTER_A_WITH_DIAERESIS = 142,
        LATIN_CAPITAL_LETTER_A_WITH_RING_ABOVE = 143,
        LATIN_CAPITAL_LETTER_E_WITH_ACUTE = 144,
        LATIN_SMALL_LETTER_AE = 145,
        LATIN_CAPITAL_LETTER_AE = 146,
        LATIN_SMALL_LETTER_O_WITH_CIRCUMFLEX = 147,
        LATIN_SMALL_LETTER_O_WITH_DIAERESIS = 148,
        LATIN_SMALL_LETTER_O_WITH_GRAVE = 149,
        LATIN_SMALL_LETTER_U_WITH_CIRCUMFLEX = 150,
        LATIN_SMALL_LETTER_U_WITH_GRAVE = 151,
        LATIN_SMALL_LETTER_Y_WITH_DIAERESIS = 152,
        LATIN_CAPITAL_LETTER_O_WITH_DIAERESIS = 153,
        LATIN_CAPITAL_LETTER_U_WITH_DIAERESIS = 154,
        CENT_SIGN = 155,
        POUND_SIGN = 156,
        YEN_SIGN = 157,
        PESETA_SIGN = 158,
        LATIN_SMALL_LETTER_F_WITH_HOOK = 159,
        LATIN_SMALL_LETTER_A_WITH_ACUTE = 160,
        LATIN_SMALL_LETTER_I_WITH_ACUTE = 161,
        LATIN_SMALL_LETTER_O_WITH_ACUTE = 162,
        LATIN_SMALL_LETTER_U_WITH_ACUTE = 163,
        LATIN_SMALL_LETTER_N_WITH_TILDE = 164,
        LATIN_CAPITAL_LETTER_N_WITH_TILDE = 165,
        FEMININE_ORDINAL_INDICATOR = 166,
        MASCULINE_ORDINAL_INDICATOR = 167,
        INVERTED_QUESTION_MARK = 168,
        REVERSED_NOT_SIGN = 169,
        NOT_SIGN = 170,
        VULGAR_FRACTION_ONE_HALF = 171,
        VULGAR_FRACTION_ONE_QUARTER = 172,
        INVERTED_EXCLAMATION_MARK = 173,
        LEFT_DOUBLE_ANGLE = 174,
        RIGHT_DOUBLE_ANGLE = 175,
        LIGHT_SHADE = 176,
        MEDIUM_SHADE = 177,
        DARK_SHADE = 178,
        BOX_DRAWINGS_LIGHT_VERTICAL = 179,
        BOX_DRAWINGS_LIGHT_VERTICAL_AND_LEFT = 180,
        BOX_DRAWINGS_VERTICAL_SINGLE_AND_LEFT_DOUBLE = 181,
        BOX_DRAWINGS_VERTICAL_DOUBLE_AND_LEFT_SINGLE = 182,
        BOX_DRAWINGS_DOWN_DOUBLE_AND_LEFT_SINGLE = 183,
        BOX_DRAWINGS_DOWN_SINGLE_AND_LEFT_DOUBLE = 184,
        BOX_DRAWINGS_DOUBLE_VERTICAL_AND_LEFT = 185,
        BOX_DRAWINGS_DOUBLE_VERTICAL = 186,
        BOX_DRAWINGS_DOUBLE_DOWN_AND_LEFT = 187,
        BOX_DRAWINGS_DOUBLE_UP_AND_LEFT = 188,
        BOX_DRAWINGS_UP_DOUBLE_AND_LEFT_SINGLE = 189,
        BOX_DRAWINGS_UP_SINGLE_AND_LEFT_DOUBLE = 190,
        BOX_DRAWINGS_LIGHT_DOWN_AND_LEFT = 191,
        BOX_DRAWINGS_LIGHT_UP_AND_RIGHT = 192,
        BOX_DRAWINGS_LIGHT_UP_AND_HORIZONTAL = 193,
        BOX_DRAWINGS_LIGHT_DOWN_AND_HORIZONTAL = 194,
        BOX_DRAWINGS_LIGHT_VERTICAL_AND_RIGHT = 195,
        BOX_DRAWINGS_LIGHT_HORIZONTAL = 196,
        BOX_DRAWINGS_LIGHT_VERTICAL_AND_HORIZONTAL = 197,
        BOX_DRAWINGS_VERTICAL_SINGLE_AND_RIGHT_DOUBLE = 198,
        BOX_DRAWINGS_VERTICAL_DOUBLE_AND_RIGHT_SINGLE = 199,
        BOX_DRAWINGS_DOUBLE_UP_AND_RIGHT = 200,
        BOX_DRAWINGS_DOUBLE_DOWN_AND_RIGHT = 201,
        BOX_DRAWINGS_DOUBLE_UP_AND_HORIZONTAL = 202,
        BOX_DRAWINGS_DOUBLE_DOWN_AND_HORIZONTAL = 203,
        BOX_DRAWINGS_DOUBLE_VERTICAL_AND_RIGHT = 204,
        BOX_DRAWINGS_DOUBLE_HORIZONTAL = 205,
        BOX_DRAWINGS_DOUBLE_VERTICAL_AND_HORIZONTAL = 206,
        BOX_DRAWINGS_UP_SINGLE_AND_HORIZONTAL_DOUBLE = 207,
        BOX_DRAWINGS_UP_DOUBLE_AND_HORIZONTAL_SINGLE = 208,
        BOX_DRAWINGS_DOWN_SINGLE_AND_HORIZONTAL_DOUBLE = 209,
        BOX_DRAWINGS_DOWN_DOUBLE_AND_HORIZONTAL_SINGLE = 210,
        BOX_DRAWINGS_UP_DOUBLE_AND_RIGHT_SINGLE = 211,
        BOX_DRAWINGS_UP_SINGLE_AND_RIGHT_DOUBLE = 212,
        BOX_DRAWINGS_DOWN_SINGLE_AND_RIGHT_DOUBLE = 213,
        BOX_DRAWINGS_DOWN_DOUBLE_AND_RIGHT_SINGLE = 214,
        BOX_DRAWINGS_VERTICAL_DOUBLE_AND_HORIZONTAL_SINGLE = 215,
        BOX_DRAWINGS_VERTICAL_SINGLE_AND_HORIZONTAL_DOUBLE = 216,
        BOX_DRAWINGS_LIGHT_UP_AND_LEFT = 217,
        BOX_DRAWINGS_LIGHT_DOWN_AND_RIGHT = 218,
        FULL_BLOCK = 219,
        LOWER_HALF_BLOCK = 220,
        LEFT_HALF_BLOCK = 221,
        RIGHT_HALF_BLOCK = 222,
        UPPER_HALF_BLOCK = 223,
        GREEK_SMALL_LETTER_ALPHA = 224,
        LATIN_SMALL_LETTER_SHARP_S = 225,
        GREEK_CAPITAL_LETTER_GAMMA = 226,
        GREEK_SMALL_LETTER_PI = 227,
        GREEK_CAPITAL_LETTER_SIGMA = 228,
        GREEK_SMALL_LETTER_SIGMA = 229,
        MICRO_SIGN = 230,
        GREEK_SMALL_LETTER_TAU = 231,
        GREEK_CAPITAL_LETTER_PHI = 232,
        GREEK_CAPITAL_LETTER_THETA = 233,
        GREEK_CAPITAL_LETTER_OMEGA = 234,
        GREEK_SMALL_LETTER_DELTA = 235,
        INFINITY = 236,
        GREEK_SMALL_LETTER_PHI = 237,
        GREEK_SMALL_LETTER_EPSILON = 238,
        INTERSECTION = 239,
        IDENTICAL_TO = 240,
        PLUS_OR_MINUS = 241,
        GREATER_THAN_OR_EQUAL_TO = 242,
        LESS_THAN_OR_EQUAL_TO = 243,
        TOP_HALF_INTEGRAL = 244,
        BOTTOM_HALF_INTEGRAL = 245,
        DIVISION_SIGN = 246,
        ALMOST_EQUAL_TO = 247,
        DEGREE_SIGN = 248,
        BULLET_OPERATOR = 249,
        MIDDLE_DOT = 250,
        SQUARE_ROOT = 251,
        SUPERSCRIPT_LATIN_SMALL_LETTER_N = 252,
        SUPERSCRIPT_TWO = 253,
        SQUARE = 254,
        EMPTY_SQUARE = 255,
    }
}