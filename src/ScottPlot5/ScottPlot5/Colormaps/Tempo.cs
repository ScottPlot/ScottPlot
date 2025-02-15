//This is a cmocean colormap
//All credit to Kristen Thyng
//This colormap is under the MIT License
//All cmocean colormaps are available at https://github.com/matplotlib/cmocean/tree/master/cmocean/rgb

namespace ScottPlot.Colormaps;

public class Tempo : IColormap
{
    public string Name => "Tempo";
    readonly CustomPalette Colormap;
    public Color GetColor(double position) => Colormap.GetColor(position);

    public Tempo()
    {
        Color[] colors =
        [
            new( 255, 246, 244 ),
            new( 253, 245, 243 ),
            new( 252, 244, 241 ),
            new( 251, 243, 240 ),
            new( 249, 242, 238 ),
            new( 248, 241, 237 ),
            new( 247, 240, 235 ),
            new( 245, 239, 234 ),
            new( 244, 238, 232 ),
            new( 242, 237, 231 ),
            new( 241, 236, 229 ),
            new( 240, 235, 228 ),
            new( 238, 234, 226 ),
            new( 237, 234, 225 ),
            new( 235, 233, 223 ),
            new( 234, 232, 222 ),
            new( 233, 231, 221 ),
            new( 231, 230, 219 ),
            new( 230, 229, 218 ),
            new( 228, 228, 216 ),
            new( 227, 227, 215 ),
            new( 226, 226, 214 ),
            new( 224, 226, 212 ),
            new( 223, 225, 211 ),
            new( 221, 224, 209 ),
            new( 220, 223, 208 ),
            new( 219, 222, 207 ),
            new( 217, 221, 205 ),
            new( 216, 221, 204 ),
            new( 214, 220, 203 ),
            new( 213, 219, 201 ),
            new( 211, 218, 200 ),
            new( 210, 217, 199 ),
            new( 209, 216, 197 ),
            new( 207, 216, 196 ),
            new( 206, 215, 195 ),
            new( 204, 214, 193 ),
            new( 203, 213, 192 ),
            new( 201, 212, 191 ),
            new( 200, 212, 190 ),
            new( 198, 211, 188 ),
            new( 197, 210, 187 ),
            new( 195, 209, 186 ),
            new( 194, 209, 185 ),
            new( 192, 208, 183 ),
            new( 191, 207, 182 ),
            new( 189, 206, 181 ),
            new( 188, 206, 180 ),
            new( 186, 205, 179 ),
            new( 185, 204, 178 ),
            new( 183, 203, 176 ),
            new( 182, 203, 175 ),
            new( 180, 202, 174 ),
            new( 179, 201, 173 ),
            new( 177, 200, 172 ),
            new( 176, 200, 171 ),
            new( 174, 199, 170 ),
            new( 172, 198, 169 ),
            new( 171, 197, 168 ),
            new( 169, 197, 166 ),
            new( 168, 196, 165 ),
            new( 166, 195, 164 ),
            new( 164, 195, 163 ),
            new( 163, 194, 162 ),
            new( 161, 193, 161 ),
            new( 160, 192, 160 ),
            new( 158, 192, 159 ),
            new( 156, 191, 159 ),
            new( 155, 190, 158 ),
            new( 153, 190, 157 ),
            new( 151, 189, 156 ),
            new( 150, 188, 155 ),
            new( 148, 188, 154 ),
            new( 146, 187, 153 ),
            new( 145, 186, 152 ),
            new( 143, 186, 151 ),
            new( 141, 185, 151 ),
            new( 139, 184, 150 ),
            new( 138, 183, 149 ),
            new( 136, 183, 148 ),
            new( 134, 182, 147 ),
            new( 133, 181, 147 ),
            new( 131, 181, 146 ),
            new( 129, 180, 145 ),
            new( 127, 179, 144 ),
            new( 125, 179, 144 ),
            new( 124, 178, 143 ),
            new( 122, 177, 142 ),
            new( 120, 177, 142 ),
            new( 118, 176, 141 ),
            new( 116, 175, 141 ),
            new( 114, 175, 140 ),
            new( 113, 174, 139 ),
            new( 111, 173, 139 ),
            new( 109, 173, 138 ),
            new( 107, 172, 138 ),
            new( 105, 171, 137 ),
            new( 103, 171, 137 ),
            new( 101, 170, 136 ),
            new( 99, 169, 136 ),
            new( 97, 169, 135 ),
            new( 95, 168, 135 ),
            new( 93, 167, 134 ),
            new( 91, 166, 134 ),
            new( 89, 166, 133 ),
            new( 87, 165, 133 ),
            new( 86, 164, 133 ),
            new( 84, 164, 132 ),
            new( 82, 163, 132 ),
            new( 80, 162, 132 ),
            new( 78, 161, 131 ),
            new( 75, 161, 131 ),
            new( 73, 160, 131 ),
            new( 71, 159, 130 ),
            new( 69, 159, 130 ),
            new( 67, 158, 130 ),
            new( 65, 157, 130 ),
            new( 63, 156, 129 ),
            new( 61, 156, 129 ),
            new( 59, 155, 129 ),
            new( 58, 154, 129 ),
            new( 56, 153, 129 ),
            new( 54, 152, 128 ),
            new( 52, 152, 128 ),
            new( 50, 151, 128 ),
            new( 48, 150, 128 ),
            new( 46, 149, 128 ),
            new( 44, 148, 127 ),
            new( 42, 147, 127 ),
            new( 41, 147, 127 ),
            new( 39, 146, 127 ),
            new( 37, 145, 127 ),
            new( 36, 144, 127 ),
            new( 34, 143, 126 ),
            new( 33, 142, 126 ),
            new( 31, 141, 126 ),
            new( 30, 141, 126 ),
            new( 28, 140, 126 ),
            new( 27, 139, 125 ),
            new( 26, 138, 125 ),
            new( 25, 137, 125 ),
            new( 23, 136, 125 ),
            new( 22, 135, 124 ),
            new( 22, 134, 124 ),
            new( 21, 133, 124 ),
            new( 20, 132, 124 ),
            new( 19, 132, 123 ),
            new( 19, 131, 123 ),
            new( 18, 130, 123 ),
            new( 18, 129, 123 ),
            new( 17, 128, 122 ),
            new( 17, 127, 122 ),
            new( 17, 126, 122 ),
            new( 17, 125, 121 ),
            new( 17, 124, 121 ),
            new( 17, 123, 121 ),
            new( 17, 122, 120 ),
            new( 17, 121, 120 ),
            new( 17, 120, 120 ),
            new( 17, 119, 119 ),
            new( 17, 118, 119 ),
            new( 18, 118, 118 ),
            new( 18, 117, 118 ),
            new( 18, 116, 118 ),
            new( 19, 115, 117 ),
            new( 19, 114, 117 ),
            new( 19, 113, 116 ),
            new( 20, 112, 116 ),
            new( 20, 111, 115 ),
            new( 20, 110, 115 ),
            new( 21, 109, 115 ),
            new( 21, 108, 114 ),
            new( 22, 107, 114 ),
            new( 22, 106, 113 ),
            new( 22, 105, 113 ),
            new( 23, 104, 112 ),
            new( 23, 103, 112 ),
            new( 23, 102, 111 ),
            new( 24, 101, 111 ),
            new( 24, 101, 110 ),
            new( 24, 100, 110 ),
            new( 25, 99, 109 ),
            new( 25, 98, 109 ),
            new( 25, 97, 108 ),
            new( 25, 96, 108 ),
            new( 26, 95, 107 ),
            new( 26, 94, 107 ),
            new( 26, 93, 106 ),
            new( 26, 92, 106 ),
            new( 26, 91, 105 ),
            new( 27, 90, 104 ),
            new( 27, 89, 104 ),
            new( 27, 88, 103 ),
            new( 27, 88, 103 ),
            new( 27, 87, 102 ),
            new( 27, 86, 102 ),
            new( 28, 85, 101 ),
            new( 28, 84, 101 ),
            new( 28, 83, 100 ),
            new( 28, 82, 99 ),
            new( 28, 81, 99 ),
            new( 28, 80, 98 ),
            new( 28, 79, 98 ),
            new( 28, 78, 97 ),
            new( 28, 77, 97 ),
            new( 28, 76, 96 ),
            new( 28, 76, 95 ),
            new( 28, 75, 95 ),
            new( 28, 74, 94 ),
            new( 28, 73, 94 ),
            new( 28, 72, 93 ),
            new( 28, 71, 93 ),
            new( 28, 70, 92 ),
            new( 28, 69, 91 ),
            new( 28, 68, 91 ),
            new( 28, 67, 90 ),
            new( 28, 66, 90 ),
            new( 28, 66, 89 ),
            new( 28, 65, 88 ),
            new( 28, 64, 88 ),
            new( 27, 63, 87 ),
            new( 27, 62, 87 ),
            new( 27, 61, 86 ),
            new( 27, 60, 86 ),
            new( 27, 59, 85 ),
            new( 27, 58, 84 ),
            new( 27, 57, 84 ),
            new( 27, 56, 83 ),
            new( 26, 55, 83 ),
            new( 26, 54, 82 ),
            new( 26, 54, 81 ),
            new( 26, 53, 81 ),
            new( 26, 52, 80 ),
            new( 26, 51, 80 ),
            new( 25, 50, 79 ),
            new( 25, 49, 79 ),
            new( 25, 48, 78 ),
            new( 25, 47, 77 ),
            new( 25, 46, 77 ),
            new( 24, 45, 76 ),
            new( 24, 44, 76 ),
            new( 24, 43, 75 ),
            new( 24, 42, 75 ),
            new( 24, 41, 74 ),
            new( 23, 40, 74 ),
            new( 23, 39, 73 ),
            new( 23, 38, 72 ),
            new( 23, 37, 72 ),
            new( 23, 36, 71 ),
            new( 22, 35, 71 ),
            new( 22, 34, 70 ),
            new( 22, 33, 70 ),
            new( 22, 32, 69 ),
            new( 21, 31, 69 ),
            new( 21, 30, 68 ),
            new( 21, 29, 68 ),
        ];

        Colormap = new CustomPalette(colors);
    }
}

