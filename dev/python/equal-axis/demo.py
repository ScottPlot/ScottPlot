import matplotlib.pyplot as plt
xs = [-10, 10, 10, -10, -10]
ys = [-10, -10, 10, 10, -10]
plt.plot(xs, ys, '.-', lw=5, ms=20)
plt.grid(alpha=.5, ls='--')
plt.gca().axis('equal')
plt.show()