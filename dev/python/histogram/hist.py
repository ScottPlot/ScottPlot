"""
This script analyzes standard datasets using using numpy's histogram tools
and displays statistics that can be used to test against when implementing
similar analysis tools in other languages.
"""

import pathlib
import matplotlib.pyplot as plt
import numpy as np
np.set_printoptions(suppress=True)
PATH_HERE = pathlib.Path(__file__).parent.resolve()
CSV_PATH = PATH_HERE.joinpath("../../data/norm_1000_12_34.csv").resolve()
CSV_VALUES = np.loadtxt(CSV_PATH)
print(f"Loaded {len(CSV_VALUES)} values from {CSV_PATH}")
print(f"Mean = {np.mean(CSV_VALUES)}")
print(f"Standard Deviation = {np.std(CSV_VALUES)}")


def makeFigure(values: np.ndarray):
    """
    Save a histogram plot to allow quick inspection
    of the distribution of CSV values.
    """

    hist, edges = np.histogram(values, bins=25, range=(-25, 100))
    print(f"Hist = {hist}")
    print(f"Edges = {edges}")

    plt.stem(edges[:-1], hist, basefmt=" ")
    plt.title(CSV_PATH.name)
    plt.ylabel("Count")
    plt.xlabel("Bin (Left Edge)")
    plt.grid(alpha=.2)

    plt.savefig(str(CSV_PATH)+"_hist.png")
    plt.show()


def cSharpArray(values: np.ndarray):
    return "{ " + ", ".join([str(x) for x in values]) + " }"


def testCaseRange(values: np.ndarray, rangeMin: float, rangeMax: float, rangeBins: int, plotToo: bool = False):
    """
    Create a C# test case for histogram calculations.
    Consistent with np.hist "Values outside the range are ignored"
    """
    print()
    print(f"float rangeMin = {rangeMin};")
    print(f"float rangeMax = {rangeMax};")
    print(f"int rangeBins = {rangeBins};")
    counts, edges = np.histogram(values,
                                 bins=rangeBins,
                                 range=(rangeMin, rangeMax),
                                 density=False)
    print(f"int[] count = {cSharpArray(counts)};")
    print(f"float[] edges = {cSharpArray(edges)};")

    if plotToo:
        plt.stem(edges[:-1], counts, basefmt=" ")
        plt.show()

    densities, edges = np.histogram(values,
                                    bins=rangeBins,
                                    range=(rangeMin, rangeMax),
                                    density=True)
    print(f"float[] densities = {cSharpArray(densities)};")


if __name__ == "__main__":
    # makeFigure(CSV_VALUES)
    testCaseRange(CSV_VALUES, -25, 100, 25)
    testCaseRange(CSV_VALUES, 10, 45, 80)

    print("DONE")
