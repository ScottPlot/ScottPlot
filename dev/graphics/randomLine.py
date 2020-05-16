import numpy as np
import matplotlib.pyplot as plt

vals = np.random.random(100) - .5
vals = np.cumsum(vals)

plt.plot(vals, 'k.')
plt.savefig('randomLine.png', transparent=True)