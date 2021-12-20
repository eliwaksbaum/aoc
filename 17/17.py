import math

# I don't feel parsing is part of the puzzle, and it was going to be a whole bunch of annoying finds and splits. 
# A computer is just one tool of many.
xr = range(241, 273 + 1)
yr = range(-97, -63 + 1)

##1
                                            # 2r1 <= y0(y0 + 1) - t(t + 1) <= 2r2;
print(-yr.start * (-yr.start - 1)/2)        # the sums of consecutive integers up to n are 2n apart;  t = r1, y0 = r1 - 1

##2
count = 0
for y0 in range(yr.start, -yr.start):
    c1 = math.ceil(y0 + .5 + math.sqrt((y0 + .5)**2 - 2*yr.start))
    c2 = math.floor(y0 + .5 + math.sqrt((y0 + .5)**2 - 2*(yr.stop-1)))
    ytr = range(min(c1, c2), max(c1,c2)+1)
    for x0 in range(math.floor(math.sqrt(2*xr.start)), xr.stop):
        hit = False
        for t in ytr:
            y = t*y0 - t**2 + t*(t+1)/2
            x = t*x0 - t**2 + t*(t+1)/2 if t < x0 else x0*(x0+1)/2
            if y in yr and x in xr:
                hit = True
        count += 1 if hit else 0
print(count)