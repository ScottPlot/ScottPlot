from math import sin, cos
import numpy as np
import matplotlib.pyplot as plt

intensities = np.zeros((100, 100))
for x in range(100):
    for y in range(100):
        intensities[x, y] = (sin(x * .2) + cos(y * .2)) * 100

plt.imshow(intensities, vmin=0, vmax=200, interpolation='bicubic')
plt.colorbar()
plt.show()
