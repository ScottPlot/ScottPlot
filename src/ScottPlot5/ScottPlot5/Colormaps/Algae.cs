//This is a cmocean colormap
//All credit to Kristen Thyng
//This colormap is under the MIT License
//All cmocean colormaps are available at https://github.com/matplotlib/cmocean/tree/master/cmocean/rgb

namespace ScottPlot.Colormaps;

public class Algae : IColormap
{
    public string Name => "Algae";
    readonly CustomPalette Colormap;
    public Color GetColor(double position) => Colormap.GetColor(position);

    public Algae()
    {
        Color[] colors = [
            new( 215, 249, 208 ),
            new( 214, 248, 206 ),
            new( 212, 247, 205 ),
            new( 211, 246, 203 ),
            new( 210, 245, 202 ),
            new( 209, 244, 200 ),
            new( 207, 244, 199 ),
            new( 206, 243, 197 ),
            new( 205, 242, 196 ),
            new( 204, 241, 195 ),
            new( 202, 240, 193 ),
            new( 201, 239, 192 ),
            new( 200, 238, 190 ),
            new( 199, 237, 189 ),
            new( 197, 236, 187 ),
            new( 196, 235, 186 ),
            new( 195, 235, 185 ),
            new( 194, 234, 183 ),
            new( 192, 233, 182 ),
            new( 191, 232, 180 ),
            new( 190, 231, 179 ),
            new( 189, 230, 177 ),
            new( 187, 229, 176 ),
            new( 186, 228, 175 ),
            new( 185, 228, 173 ),
            new( 184, 227, 172 ),
            new( 182, 226, 171 ),
            new( 181, 225, 169 ),
            new( 180, 224, 168 ),
            new( 178, 223, 166 ),
            new( 177, 222, 165 ),
            new( 176, 222, 164 ),
            new( 175, 221, 162 ),
            new( 173, 220, 161 ),
            new( 172, 219, 160 ),
            new( 171, 218, 158 ),
            new( 170, 218, 157 ),
            new( 168, 217, 156 ),
            new( 167, 216, 154 ),
            new( 166, 215, 153 ),
            new( 164, 214, 152 ),
            new( 163, 213, 150 ),
            new( 162, 213, 149 ),
            new( 160, 212, 148 ),
            new( 159, 211, 146 ),
            new( 158, 210, 145 ),
            new( 157, 209, 144 ),
            new( 155, 209, 143 ),
            new( 154, 208, 141 ),
            new( 153, 207, 140 ),
            new( 151, 206, 139 ),
            new( 150, 205, 138 ),
            new( 149, 205, 136 ),
            new( 147, 204, 135 ),
            new( 146, 203, 134 ),
            new( 145, 202, 133 ),
            new( 143, 202, 131 ),
            new( 142, 201, 130 ),
            new( 140, 200, 129 ),
            new( 139, 199, 128 ),
            new( 138, 199, 126 ),
            new( 136, 198, 125 ),
            new( 135, 197, 124 ),
            new( 133, 196, 123 ),
            new( 132, 196, 122 ),
            new( 131, 195, 121 ),
            new( 129, 194, 119 ),
            new( 128, 193, 118 ),
            new( 126, 193, 117 ),
            new( 125, 192, 116 ),
            new( 123, 191, 115 ),
            new( 122, 190, 114 ),
            new( 120, 190, 113 ),
            new( 119, 189, 111 ),
            new( 117, 188, 110 ),
            new( 116, 187, 109 ),
            new( 114, 187, 108 ),
            new( 113, 186, 107 ),
            new( 111, 185, 106 ),
            new( 110, 185, 105 ),
            new( 108, 184, 104 ),
            new( 107, 183, 103 ),
            new( 105, 182, 102 ),
            new( 103, 182, 101 ),
            new( 102, 181, 100 ),
            new( 100, 180, 99 ),
            new( 98, 180, 98 ),
            new( 97, 179, 97 ),
            new( 95, 178, 96 ),
            new( 93, 178, 95 ),
            new( 91, 177, 94 ),
            new( 90, 176, 93 ),
            new( 88, 175, 93 ),
            new( 86, 175, 92 ),
            new( 84, 174, 91 ),
            new( 82, 173, 90 ),
            new( 80, 173, 89 ),
            new( 78, 172, 89 ),
            new( 76, 171, 88 ),
            new( 74, 171, 87 ),
            new( 72, 170, 87 ),
            new( 70, 169, 86 ),
            new( 68, 168, 85 ),
            new( 66, 168, 85 ),
            new( 63, 167, 84 ),
            new( 61, 166, 84 ),
            new( 59, 166, 84 ),
            new( 57, 165, 83 ),
            new( 55, 164, 83 ),
            new( 52, 163, 83 ),
            new( 50, 163, 82 ),
            new( 48, 162, 82 ),
            new( 46, 161, 82 ),
            new( 44, 160, 82 ),
            new( 42, 160, 82 ),
            new( 40, 159, 81 ),
            new( 38, 158, 81 ),
            new( 36, 157, 81 ),
            new( 34, 156, 81 ),
            new( 32, 156, 81 ),
            new( 30, 155, 81 ),
            new( 28, 154, 81 ),
            new( 27, 153, 81 ),
            new( 25, 152, 81 ),
            new( 24, 151, 80 ),
            new( 22, 150, 80 ),
            new( 21, 150, 80 ),
            new( 19, 149, 80 ),
            new( 18, 148, 80 ),
            new( 16, 147, 80 ),
            new( 15, 146, 80 ),
            new( 14, 145, 80 ),
            new( 13, 144, 79 ),
            new( 12, 143, 79 ),
            new( 11, 143, 79 ),
            new( 10, 142, 79 ),
            new( 9, 141, 79 ),
            new( 9, 140, 79 ),
            new( 8, 139, 78 ),
            new( 8, 138, 78 ),
            new( 7, 137, 78 ),
            new( 7, 136, 78 ),
            new( 7, 135, 77 ),
            new( 7, 134, 77 ),
            new( 7, 134, 77 ),
            new( 7, 133, 77 ),
            new( 7, 132, 77 ),
            new( 7, 131, 76 ),
            new( 7, 130, 76 ),
            new( 8, 129, 76 ),
            new( 8, 128, 75 ),
            new( 8, 127, 75 ),
            new( 9, 126, 75 ),
            new( 9, 125, 75 ),
            new( 10, 124, 74 ),
            new( 10, 124, 74 ),
            new( 11, 123, 74 ),
            new( 11, 122, 73 ),
            new( 12, 121, 73 ),
            new( 12, 120, 73 ),
            new( 13, 119, 72 ),
            new( 13, 118, 72 ),
            new( 14, 117, 72 ),
            new( 14, 116, 71 ),
            new( 15, 115, 71 ),
            new( 15, 115, 71 ),
            new( 16, 114, 70 ),
            new( 16, 113, 70 ),
            new( 17, 112, 69 ),
            new( 17, 111, 69 ),
            new( 18, 110, 69 ),
            new( 18, 109, 68 ),
            new( 18, 108, 68 ),
            new( 19, 107, 67 ),
            new( 19, 106, 67 ),
            new( 20, 106, 67 ),
            new( 20, 105, 66 ),
            new( 20, 104, 66 ),
            new( 21, 103, 65 ),
            new( 21, 102, 65 ),
            new( 21, 101, 64 ),
            new( 22, 100, 64 ),
            new( 22, 99, 64 ),
            new( 22, 98, 63 ),
            new( 23, 98, 63 ),
            new( 23, 97, 62 ),
            new( 23, 96, 62 ),
            new( 23, 95, 61 ),
            new( 24, 94, 61 ),
            new( 24, 93, 60 ),
            new( 24, 92, 60 ),
            new( 24, 91, 59 ),
            new( 24, 91, 59 ),
            new( 25, 90, 58 ),
            new( 25, 89, 58 ),
            new( 25, 88, 57 ),
            new( 25, 87, 57 ),
            new( 25, 86, 56 ),
            new( 25, 85, 56 ),
            new( 25, 84, 55 ),
            new( 25, 84, 55 ),
            new( 26, 83, 54 ),
            new( 26, 82, 53 ),
            new( 26, 81, 53 ),
            new( 26, 80, 52 ),
            new( 26, 79, 52 ),
            new( 26, 78, 51 ),
            new( 26, 77, 51 ),
            new( 26, 77, 50 ),
            new( 26, 76, 50 ),
            new( 26, 75, 49 ),
            new( 26, 74, 48 ),
            new( 26, 73, 48 ),
            new( 26, 72, 47 ),
            new( 26, 71, 47 ),
            new( 26, 71, 46 ),
            new( 26, 70, 46 ),
            new( 26, 69, 45 ),
            new( 26, 68, 44 ),
            new( 26, 67, 44 ),
            new( 25, 66, 43 ),
            new( 25, 65, 43 ),
            new( 25, 64, 42 ),
            new( 25, 64, 41 ),
            new( 25, 63, 41 ),
            new( 25, 62, 40 ),
            new( 25, 61, 39 ),
            new( 25, 60, 39 ),
            new( 24, 59, 38 ),
            new( 24, 59, 38 ),
            new( 24, 58, 37 ),
            new( 24, 57, 36 ),
            new( 24, 56, 36 ),
            new( 24, 55, 35 ),
            new( 23, 54, 34 ),
            new( 23, 53, 34 ),
            new( 23, 53, 33 ),
            new( 23, 52, 32 ),
            new( 23, 51, 32 ),
            new( 22, 50, 31 ),
            new( 22, 49, 30 ),
            new( 22, 48, 30 ),
            new( 22, 47, 29 ),
            new( 21, 47, 28 ),
            new( 21, 46, 28 ),
            new( 21, 45, 27 ),
            new( 20, 44, 26 ),
            new( 20, 43, 26 ),
            new( 20, 42, 25 ),
            new( 20, 41, 24 ),
            new( 19, 41, 24 ),
            new( 19, 40, 23 ),
            new( 19, 39, 22 ),
            new( 18, 38, 22 ),
            new( 18, 37, 21 ),
            new( 18, 36, 20 ),
        ];

        Colormap = new CustomPalette(colors);
    }
}

