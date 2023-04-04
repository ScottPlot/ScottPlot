﻿// This colormap was created by Scott Harden on 2020-06-16 and is released under a MIT license.
// It was designed to mimic Turbo, but is not a copy of or derived from Turbo source code.
// https://ai.googleblog.com/2019/08/turbo-improved-rainbow-colormap-for.html

using System;

namespace ScottPlot.Drawing.Colormaps;

public class Turbo : IColormap
{
    public string Name => "Turbo";

    public (byte r, byte g, byte b) GetRGB(byte value)
    {
        byte[] bytes = BitConverter.GetBytes(argb[value]);
        return (bytes[2], bytes[1], bytes[0]);
    }

    private readonly int[] argb =
    {
        -13559489, -13493436, -13427382, -13361328, -13295018, -13228964, -13162911, -13096857,
        -13030547, -12964493, -12898440, -12832130, -12766077, -12700023, -12633970, -12567660,
        -12501607, -12435554, -12369245, -12303192, -12237139, -12171086, -12170313, -12104260,
        -12038208, -12037436, -11971383, -11905331, -11904559, -11838507, -11837991, -11771940,
        -11771168, -11770653, -11770138, -11703831, -11703316, -11702801, -11702287, -11701517,
        -11701003, -11700489, -11765255, -11764742, -11764228, -11828995, -11828482, -11893506,
        -11892737, -11957761, -12022785, -12022017, -12087042, -12152067, -12217092, -12347397,
        -12412423, -12477448, -12542218, -12672780, -12737806, -12802577, -12933139, -12998166,
        -13128729, -13193500, -13324063, -13389091, -13519654, -13584682, -13714989, -13780017,
        -13910581, -13975609, -14106173, -14171201, -14301765, -14366793, -14431822, -14562386,
        -14627414, -14692442, -14757471, -14822499, -14887527, -14952556, -14952048, -15017332,
        -15082361, -15081853, -15147137, -15146629, -15146121, -15145869, -15145361, -15145109,
        -15079065, -15078812, -15013024, -15012515, -14946726, -14880938, -14749356, -14683567,
        -14617778, -14486453, -14355127, -14223801, -14092475, -13961405, -13764543, -13633217,
        -13436355, -13239748, -13108422, -12911559, -12714952, -12518089, -12255946, -12059339,
        -11862476, -11600333, -11403725, -11141582, -10879182, -10682575, -10420431, -10158287,
        -09896143, -09633999, -09372111, -09175503, -08913359, -08651215, -08389327, -08127183,
        -07865294, -07603150, -07341262, -07079117, -06817229, -06555341, -06293452, -06031308,
        -05834955, -05573067, -05311178, -05114826, -04852937, -04656585, -04394952, -04198600,
        -04002247, -03740358, -03544262, -03347910, -03217349, -03020997, -02824900, -02628804,
        -02497987, -02301891, -02171331, -02040770, -01910210, -01779394, -01648834, -01518273,
        -01387713, -01257153, -01126849, -01061824, -00931264, -00866240, -00801216, -00670656,
        -00605888, -00540864, -00475840, -00411072, -00346048, -00346560, -00281792, -00282304,
        -00217536, -00218048, -00153280, -00153793, -00154561, -00155073, -00155841, -00156354,
        -00157122, -00157634, -00158403, -00224451, -00225219, -00291524, -00292036, -00358341,
        -00424389, -00490694, -00491206, -00557511, -00623559, -00755400, -00821705, -00887754,
        -00954058, -01085643, -01151948, -01283533, -01349837, -01481422, -01613263, -01679312,
        -01811153, -01942738, -02074579, -02206164, -02337749, -02469590, -02601175, -02733016,
        -02930137, -03061978, -03193563, -03390940, -03522526, -03654111, -03851488, -03983073,
        -04180450, -04377572, -04509157, -04706534, -04838119, -05035497, -05232618, -05429739,
        -05561580, -05758702, -05956079, -06153200, -06350322, -06482163, -06679284, -06876662,
        -07073783, -07270904, -07468282, -07665403, -07862524, -08059902, -08257023, -08388608,
    };
}
