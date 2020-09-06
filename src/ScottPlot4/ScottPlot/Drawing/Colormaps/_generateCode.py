line = "public static Colormap CMAP => new Colormap(new Colormaps.CMAP());"
import glob
for fname in sorted(glob.glob("*.cs")):
    print(line.replace("CMAP", fname.replace(".cs","")))
