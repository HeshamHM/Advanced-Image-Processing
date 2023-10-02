# Advanced-Image-Processing

## Overview
https://github.com/HeshamHM/Advanced-Image-Processing/assets/65486855/cf53356c-4d12-4a69-91cf-c82bfca6b226



## Flask API
- The Flask API is a Python-based web service for image filtering.
- It offers endpoints for uploading, processing, and retrieving filtered images.
- The API handles tasks like grayscale, sepia, and custom filter applications.
- You can run the Flask API locally or deploy it to a server.
#### Filters
- Smoothing frequency domain
  - Ideal Lowpass
  - Butterworth Lowpass
  - Gaussian Lowpass
- Sharpening frequency domain
  - Ideal Higthpass
  - Butterworth Higthpass
  - Gaussian Higthpass
- Mean filters
  - Arithmetic mean
  - Geometric mean
  - Harmonic mean
  - Contraharmonic mean positive
  - Contraharmonic mean negative
- Order-Statistic filters
  - Max
  - Min
  - Median
  - Midpoint
  - Alpha-trimmed
- segmentation
  - Snake
  - Connected components
  - Mean shift
  - Level set
  - Global threshold
  - Otsu method
  - Adaptive threshold
  - Watershed
  - K-Means
  - Region growing
  - Chain role
- Edge detection
  - Canny
  - Soble
### C# Windows Application
- The C# Windows app is a graphical interface that connects to the Flask API.
- It enables users to:
  - Upload images for processing.
  - Select and apply various image filters.
  - each filter has a tooltip to know what can you do with these filter
  - view the filtered images.
  - Perform other relevant actions as per your application's features.
  - there is a docs for explaining some filters 
