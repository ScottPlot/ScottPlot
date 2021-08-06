"""
This script generates sample datasets that can be used to
compare analyses between different statistics platforms.
"""

import numpy as np
import pathlib
PATH_HERE = pathlib.Path(__file__).parent.resolve()
np.set_printoptions(suppress=True)


def generateNormalSet1():
    np.random.seed(0)
    vals = np.random.standard_normal(1000) * 12 + 34
    csvPath = PATH_HERE.joinpath("norm_1000_12_34.csv")
    np.savetxt(csvPath, vals, "%.05f")


if __name__ == "__main__":
    generateNormalSet1()
