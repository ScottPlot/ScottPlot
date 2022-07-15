"""
This script converts XKCD's color list to C# functions
https://xkcd.com/color/rgb/
"""
import pathlib
import pyperclip

if __name__ == "__main__":
    output = ""
    txtFilePath = pathlib.Path(__file__).parent.joinpath("xkcd-rgb.txt")
    with open(txtFilePath) as f:
        lines = f.readlines()
    lines = [x.replace("/", " slash ",) for x in lines]
    lines = [x.replace("'", "",) for x in lines]
    lines = [x.strip() for x in lines]
    lines = [x for x in lines if len(x) and not x.startswith("#")]
    lines = sorted(lines)
    for line in lines:
        color = line.split("#")[0].strip()
        code = line.split("#")[1].strip()
        words = color.lower().split(" ")
        words = [x.capitalize() for x in words]
        name = "".join(words)
        r = int("0x"+code[0:2], 16)
        g = int("0x"+code[2:4], 16)
        b = int("0x"+code[4:6], 16)
        lineOut = f"public static Color {name} => new({r}, {g}, {b});"
        print(lineOut)
        output += lineOut+"\n"

    print("copied to clipboard")
    pyperclip.copy(output)
