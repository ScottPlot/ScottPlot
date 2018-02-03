I use OBS Studio to create a screen capture video of a feature I want to demonstrate

Then I use FFMPEG to convert it to an image sequence (going from 30 FPS to 5 FPS)

```
ffmpeg -r 30 -i in.mp4 -r 5 out/%06d.bmp
```

Then I use ImageJ to crop the area I want (and frames I want) and save as animated GIF
