import base64
import io
import math
import os
import cv2
from PIL import Image
from flask import Flask
import numpy as np
from scipy.ndimage import maximum_filter, minimum_filter
from skimage.segmentation import morphological_chan_vese, checkerboard_level_set
from sklearn.cluster import estimate_bandwidth, MeanShift
from sklearn.linear_model import LogisticRegression
from sklearn.model_selection import train_test_split
from sklearn.svm import SVC
import pickle
class Point(object):
 def __init__(self,x,y):
  self.x = x
  self.y = y

 def getX(self):
  return self.x
 def getY(self):
  return self.y
def getGrayDiff(img,currentPoint,tmpPoint):
 return abs(int(img[currentPoint.x,currentPoint.y]) - int(img[tmpPoint.x,tmpPoint.y]))
def selectConnects(p):
 if p != 0:
  connects = [Point(-1, -1), Point(0, -1), Point(1, -1), Point(1, 0), Point(1, 1), \
     Point(0, 1), Point(-1, 1), Point(-1, 0)]
 else:
  connects = [ Point(0, -1), Point(1, 0),Point(0, 1), Point(-1, 0)]
 return connects
def regionGrow(img,seeds,thresh,p = 1):
 height, weight = img.shape
 seedMark = np.zeros(img.shape)
 seedList = []
 for seed in seeds:
  seedList.append(seed)
 label = 1
 connects = selectConnects(p)
 while(len(seedList)>0):
  currentPoint = seedList.pop(0)

  seedMark[currentPoint.x,currentPoint.y] = label
  for i in range(8):
   tmpX = currentPoint.x + connects[i].x
   tmpY = currentPoint.y + connects[i].y
   if tmpX < 0 or tmpY < 0 or tmpX >= height or tmpY >= weight:
    continue
   grayDiff = getGrayDiff(img,currentPoint,Point(tmpX,tmpY))
   if grayDiff < thresh and seedMark[tmpX,tmpY] == 0:
    seedMark[tmpX,tmpY] = label
    seedList.append(Point(tmpX,tmpY))
 return seedMark
app=Flask(__name__)
imgs=[]
listoffunc=[]
def store_evolution_in(lst):
    def _store(x):
        lst.append(np.copy(x))

    return _store
def im2double(im):
    max_val = np.max(im)
    min_val = np.min(im)
    return np.round((im.astype('float') - min_val) / (max_val - min_val) * 255)
def checkgray(im):
   if len(np.shape(im))==3:
       return cv2.cvtColor(im,cv2.COLOR_BGR2GRAY)
   return im
@app.route('/file/<file>',methods=['POST','GET'])
def Loadimage(file):
            imgs.clear()
            file=file.replace('_', '/').replace('-', '\\')
            pil_img = cv2.imread(file,0)
            imgs.append(pil_img)
            image = Image.fromarray(pil_img)
            byte_arr = io.BytesIO()
            image.save(byte_arr, format='PNG')  #convert the PIL image to byte array
            encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
            return encoded_img
def brainOrChestClassfication(image):
    if os.path.exists("brainOrChestClassfication.pickle"):
        trees = pickle.load(open("brainOrChestClassfication.pickle", "rb"))
        print("saved algo")
    else:
        dir = 'D:\\Data\\train\\NORMAL'
        classes = ['Chest', 'Brain']
        Data = []
        Lables = []
        for img in os.listdir(dir):
            img_path = os.path.join(dir, img)
            Data.append((np.array(cv2.resize(cv2.imread(img_path, 0), (100, 100))).flatten()) / 255)
            Lables.append(classes.index("Chest"))
        dir = 'D:\\datasetBrain\\no'
        for img in os.listdir(dir):
            img_path = os.path.join(dir, img)
            Data.append((np.array(cv2.resize(cv2.imread(img_path, 0), (100, 100))).flatten()) / 255)
            Lables.append(classes.index("Brain"))

        x_train, x_test, y_train, y_test = train_test_split(Data, Lables, test_size=.25, random_state=1)
        trees = SVC()
        trees.fit(x_train, y_train)
        print(trees.score(x_test, y_test))
        with open("brainOrChestClassfication.pickle", "wb") as file:
            pickle.dump(trees, file)
            print("saveing")
    img = cv2.resize(image, (100, 100))
    img = (np.array(img).flatten()) / 255
    return trees.predict([img])[0]
def brainTumor(image):
    dir = 'D:\\datasetBrain'
    classes = ['yes', 'no']
    Data = []
    Lables = []
    # using saved training model ..
    if os.path.exists("train_model.pickle"):
        model = pickle.load(open("train_model.pickle", "rb"))
    # training will happen if the trained model isnot available
    else:
        for category in os.listdir(dir):
            newPath = os.path.join(dir, category)
            for img in os.listdir(newPath):
                img_path = os.path.join(newPath, img)
                if 'Thumbs.db' not in img_path:
                    Data.append((np.array(cv2.resize(cv2.imread(img_path, 0), (100, 100))).flatten()) / 255)
                    Lables.append(classes.index(category))
        X_train, X_test, Y_train, Y_test = train_test_split(Data, Lables, test_size=.25, random_state=0)
        model = LogisticRegression()
        model.fit(X_train, Y_train)
        with open("train_model.pickle", "wb") as file:
            pickle.dump(model, file)
    img = cv2.resize(image, (100, 100))
    img = (np.array(img).flatten()) / 255
    if(classes[model.predict([img])[0]]=="yes"):
        return "has brain tumor"
    return "has not brain tumor"
def covidClassificaion(image):
    classes = ['COVID19', 'NORMAL', 'PNEUMONIA']
    if os.path.exists("covidClassificaion.pickle"):
        model = pickle.load(open("covidClassificaion.pickle", "rb"))
    else:
        dir = 'D:\\DataCovid'
        Data = []
        Lables = []
        for category in os.listdir(dir):
                newPath = os.path.join(dir, category)
                print(category)
                for img in os.listdir(newPath):
                    img_path = os.path.join(newPath, img)
                    Data.append((np.array(cv2.resize(cv2.imread(img_path, 0), (100, 100))).flatten()) / 255)
                    Lables.append(classes.index(category))
        x_train, x_test, y_train, y_test = train_test_split(Data, Lables, test_size=.25, random_state=1)
        model=SVC(C=2)
        model.fit(x_train,y_train)
        with open("covidClassificaion.pickle", "wb") as file:
            pickle.dump(model, file)
            print("saveing")
    img = cv2.resize(image, (100, 100))
    img = (np.array(img).flatten()) / 255
    return classes[model.predict([img])[0]]
@app.route('/Classification',methods=['GET'])
def clf():
    image=checkgray(imgs[-1])
    if brainOrChestClassfication(image=image)==0:
      return covidClassificaion(image)
    return brainTumor(image)

@app.route('/Median',methods=['GET'])
def Median():
        listoffunc.append("median")
        imag=checkgray(imgs[-1])
        """
          x, y = np.shape(imag)
        im = imag.copy()
        for r in range(1, x - 1):
          for c in range(1, y - 1):
            list = []
            for i in range(-1, 2):
                for j in range(-1, 2):
                    list.append(imag[r + i][c + j])
            im[r][c] = np.median(list)
        """
        pil_img = im2double(cv2.medianBlur(imag,3))
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Max',methods=['GET'])
def max():
        listoffunc.append("max")
        imag = checkgray(imgs[-1])
        im = imag.copy()
        im=maximum_filter(im, (3, 3))
        pil_img =im2double(im)
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Mid',methods=['GET'])
def mid():
        listoffunc.append("mid")
        imag = checkgray(imgs[-1])
        maxf = maximum_filter(imag, (3, 3))
        minf = minimum_filter(imag, (3, 3))
        pil_img = (maxf + minf) / 2
        #pil_img=im2double(pil_img)
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Min',methods=['GET'])
def min():
        listoffunc.append("min")
        imag = checkgray(imgs[-1])
        im = imag.copy()
        im=minimum_filter(im, (3, 3))
        pil_img =im2double(im)
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Alpha',methods=['GET'])
def alpha():
        listoffunc.append("alpha")
        imag = checkgray(imgs[-1])
        x, y = np.shape(imag)
        im = imag.copy()
        for r in range(1, x - 1):
          for c in range(1, y - 1):
            value = []
            for i in range(-1, 2):
                for j in range(-1, 2):
                    value.append(imag[r + i][c + j])
            value.remove(np.max(value))
            value.remove(np.min(value))
            sum = np.sum(value)
            im[r][c]= sum / np.size(value)
        pil_img =im
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Undo',methods=['GET'])
def undo():
        if len(listoffunc)>0:
            listoffunc.pop()
        if len(imgs)>1:
          imgs.pop()
        pil_img = imgs[-1]
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Mean',methods=['GET'])
def mean():
        listoffunc.append("mean")
        imag = checkgray(imgs[-1])
        x, y = np.shape(imag)
        im = imag.copy()
        for r in range(1, x - 1):
            for c in range(1, y - 1):
                value = 0
                for i in range(-1, 2):
                    for j in range(-1, 2):
                        value = (value + (imag[r + i][c + j]))
                im[r][c]=value / 9
        pil_img =im
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Geo',methods=['GET'])
def geomean():
        listoffunc.append("geo")
        imag = checkgray(imgs[-1])
        x, y = np.shape(imag)
        im =imag.copy()
        for r in range(1, x-1):
            for c in range(1, y-1):
                a = 1
                for i in range(-1, 2):
                    for j in range(-1, 2):
                        a = a * int(imag[r + i][c + j])
                im[r][c] = a ** (1 / 9)
        pil_img=im2double(im)
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Har',methods=['GET'])
def Harmonic():
        listoffunc.append("Harmonic")
        imag = checkgray(imgs[-1])
        x, y = np.shape(imag)
        im = imag.copy()
        for r in range(1, x - 1):
            for c in range(1, y - 1):
                value = 0.0
                for i in range(-1, 2):
                    for j in range(-1, 2):
                        if imag[r + i][c + j] != 0:
                            value = (value + 1 / (imag[r + i][c + j]))
                if value!=0:
                 im[r][c]= 9 / value
        pil_img=im2double(im)
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/PContrahar',methods=['GET'])
def contraharmonic():
        listoffunc.append("contra")
        im=checkgray(imgs[-1])
        num = np.power(im+0.01, 1.5 + 1)
        denom = np.power(im+0.01,1.5)
        kernel = np.full((3, 3), 1.0)
        im = cv2.filter2D(num, -1, kernel) / cv2.filter2D(denom, -1, kernel)
        im = im / np.max(im)
        pil_img=im2double(im)
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/NContrahar',methods=['GET'])
def negcontraharmonic():
        listoffunc.append("negcontra")
        im=checkgray(imgs[-1])
        num = np.power(im+0.01, -0.5)
        denom = np.power(im+0.01, -1.5)
        kernel = np.full((3, 3), 1.0)
        im = cv2.filter2D(num, -1, kernel) / (cv2.filter2D(denom, -1, kernel))
        im = im / np.max(im)
        pil_img=im2double(im)
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Sobel',methods=['GET'])
def sobel():
    listoffunc.append("sobel")
    image = checkgray(imgs[-1])
    """
    list1 = np.array([[-1, 0, 1], [-2, 0, 2], [-1, 0, 1]])
    im=image.copy()
    x, y = np.shape(image)
    for r in range(1, x - 1):
     for c in range(1, y - 1):
        value = 0
        for i in range(-1, 2):
          for j in range(-1, 2):
             value = (value + (image[r + i][c + j] * list1[i][j]))
        im.itemset((r, c), value / 9)
    """
    sobelx = cv2.Sobel(src=image.copy(), ddepth=cv2.CV_64F, dx=1, dy=0, ksize=3)
    sobely = cv2.Sobel(src=image.copy(), ddepth=cv2.CV_64F, dx=0, dy=1, ksize=3)
    sobelxy = cv2.Sobel(src=image.copy(), ddepth=cv2.CV_64F, dx=1, dy=1, ksize=3)
    pil_img = sobelxy
    pil_img = pil_img.astype(np.uint8)
    imgs.append(pil_img)
    image = Image.fromarray(pil_img)
    byte_arr = io.BytesIO()
    image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
    encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
    return encoded_img
@app.route('/Connected',methods=['GET'])
def connectedcomponents():
    listoffunc.append("Cc")
    img = checkgray(imgs[-1])
    img = cv2.threshold(img, 127, 255, cv2.THRESH_BINARY)[1]  # make img seg
    num_labels, labels_im = cv2.connectedComponents(img)
    label_hue = np.uint8(179 * labels_im / np.max(labels_im))#for make a gray color
    blank_ch = 255 * np.ones_like(label_hue)#make a blank whitw image
    labeled_img = cv2.merge([label_hue, blank_ch, blank_ch])#merge them
    labeled_img = cv2.cvtColor(labeled_img, cv2.COLOR_HSV2BGR)#color them
    labeled_img[label_hue == 0] = 0#make background black
    pil_img = labeled_img
    pil_img = pil_img.astype(np.uint8)
    imgs.append(pil_img)
    image = Image.fromarray(pil_img)
    byte_arr = io.BytesIO()
    image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
    encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
    return encoded_img
@app.route('/MeanShift',methods=['GET'])
def meanShift():
    listoffunc.append("meanShift")
    #resize image to make 3
    img=checkgray(imgs[-1])
    img=cv2.resize(img, (300, 300))
    flat_image = img.reshape((-1, 3))
    flat_image = np.float32(flat_image)
    # mean-shift
    bandwidth = estimate_bandwidth(flat_image, quantile=.06, n_samples=3000)
    ms = MeanShift(bandwidth=bandwidth, max_iter=50, bin_seeding=True)
    ms.fit(flat_image)
    labeled = ms.labels_
    # get number of segments
    segments = np.unique(labeled)
    # get the average color of each segment
    total = np.zeros((segments.shape[0], 3), dtype=float)
    count = np.zeros(total.shape, dtype=float)
    for i, label in enumerate(labeled):
        total[label] = total[label] + flat_image[i]
        count[label] += 1
    avg = total / count
    avg = np.uint8(avg)
    # cast the labeled image into the corresponding average color
    res = avg[labeled]
    pil_img =res.reshape(img.shape)
    pil_img = pil_img.astype(np.uint8)
    imgs.append(pil_img)
    image = Image.fromarray(pil_img)
    byte_arr = io.BytesIO()
    image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
    encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
    return encoded_img
@app.route('/Ideallow',methods=['GET'])
def Ideallow():
    listoffunc.append("il")
    dft = np.fft.fft2(checkgray(imgs[-1]), axes=(0, 1))
    dft_shift = np.fft.fftshift(dft)
    x, y = np.shape(dft)
    midpointx, midpointy = x // 2, y // 2
    maskideal = np.zeros((x, y), np.uint8)
    Do = 50
    for u in range(0, x):
        for v in range(0, y):
            if np.sqrt(((u - midpointx) ** 2) + ((v - midpointy) ** 2)) <= Do:
                maskideal[u][v] = 255

    pil_img = (dft_shift * maskideal) / 255
    pil_img = np.fft.ifftshift(pil_img)
    pil_img = np.fft.ifft2(pil_img, axes=(0, 1))
    pil_img = np.abs(pil_img).clip(0, 255).astype(np.uint8)
    imgs.append(pil_img)
    image = Image.fromarray(pil_img)
    byte_arr = io.BytesIO()
    image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
    encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
    return encoded_img
@app.route('/Butterlow',methods=['GET'])
def butterlow():
    listoffunc.append("Bl")
    dft = np.fft.fft2(checkgray(imgs[-1]), axes=(0, 1))
    dft_shift = np.fft.fftshift(dft)
    x, y = np.shape(dft)
    midpointx, midpointy = x // 2, y // 2
    maskbutter = np.zeros((x, y))
    Do = 50
    for u in range(0, x):
        for v in range(0, y):
            duv = np.sqrt(((u - midpointx) ** 2) + ((v - midpointy) ** 2))
            maskbutter[u][v] = 1 / (1 + (duv / Do) ** 4)

    maskbutter = im2double(maskbutter)
    pil_img = (dft_shift * maskbutter) / 255
    pil_img = np.fft.ifftshift(pil_img)
    pil_img = np.fft.ifft2(pil_img, axes=(0, 1))
    pil_img = np.abs(pil_img).clip(0, 255).astype(np.uint8)
    imgs.append(pil_img)
    image = Image.fromarray(pil_img)
    byte_arr = io.BytesIO()
    image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
    encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
    return encoded_img
@app.route('/Gaussianlow',methods=['GET'])
def gaussianlow():
    listoffunc.append("gl")
    dft = np.fft.fft2(checkgray(imgs[-1]), axes=(0, 1))
    dft_shift = np.fft.fftshift(dft)
    x, y = np.shape(dft)
    midpointx, midpointy = x // 2, y // 2
    maskgussine = np.zeros((x, y))
    Do = 50
    for u in range(0, x):
        for v in range(0, y):
            duv = np.sqrt(((u - midpointx) ** 2) + ((v - midpointy) ** 2))
            maskgussine[u][v] = math.exp((-(duv) ** 2) / (2 * (Do ** 2)))

    maskbutter = im2double(maskgussine)
    pil_img = (dft_shift * maskbutter) / 255
    pil_img = np.fft.ifftshift(pil_img)
    pil_img = np.fft.ifft2(pil_img, axes=(0, 1))
    pil_img = np.abs(pil_img).clip(0, 255).astype(np.uint8)
    imgs.append(pil_img)
    image = Image.fromarray(pil_img)
    byte_arr = io.BytesIO()
    image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
    encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
    return encoded_img
@app.route('/IdealH',methods=['GET'])
def idealH():
    listoffunc.append("iH")
    dft = np.fft.fft2(checkgray(imgs[-1]), axes=(0, 1))
    dft_shift = np.fft.fftshift(dft)
    x, y = np.shape(dft)
    midpointx, midpointy = x // 2, y // 2
    maskideal = np.zeros((x, y), np.uint8)
    Do = 7
    for u in range(0, x):
        for v in range(0, y):
            if np.sqrt(((u - midpointx) ** 2) + ((v - midpointy) ** 2)) > Do:
                maskideal[u][v] = 255

    pil_img = (dft_shift * maskideal) / 255
    pil_img = np.fft.ifftshift(pil_img)
    pil_img = np.fft.ifft2(pil_img, axes=(0, 1))
    pil_img = np.abs(pil_img).clip(0, 255).astype(np.uint8)
    imgs.append(pil_img)
    image = Image.fromarray(pil_img)
    byte_arr = io.BytesIO()
    image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
    encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
    return encoded_img
@app.route('/butterH',methods=['GET'])
def butterH():
    listoffunc.append("BH")
    dft = np.fft.fft2(checkgray(imgs[-1]), axes=(0, 1))
    dft_shift = np.fft.fftshift(dft)
    x, y = np.shape(dft)
    midpointx, midpointy = x // 2, y // 2
    maskbutter = np.zeros((x, y))
    Do = 7
    for u in range(0, x):
        for v in range(0, y):
            duv = np.sqrt(((u - midpointx) ** 2) + ((v - midpointy) ** 2))+0.00000001
            maskbutter[u][v] = 1 / (1 + (Do / duv) ** 4)

    maskbutter = im2double(maskbutter)
    pil_img = (dft_shift * maskbutter) / 255
    pil_img = np.fft.ifftshift(pil_img)
    pil_img = np.fft.ifft2(pil_img, axes=(0, 1))
    pil_img = np.abs(pil_img).clip(0, 255).astype(np.uint8)

    imgs.append(pil_img)
    image = Image.fromarray(pil_img)
    byte_arr = io.BytesIO()
    image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
    encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
    return encoded_img
@app.route('/GaussianH',methods=['GET'])
def gaussianH():
    listoffunc.append("gH")
    dft = np.fft.fft2(checkgray(imgs[-1]), axes=(0, 1))
    dft_shift = np.fft.fftshift(dft)
    x, y = np.shape(dft)
    midpointx, midpointy = x // 2, y // 2
    maskgussine = np.zeros((x, y))
    Do = 7
    for u in range(0, x):
        for v in range(0, y):
            duv = np.sqrt(((u - midpointx) ** 2) + ((v - midpointy) ** 2))
            maskgussine[u][v] = 1-math.exp((-(duv) ** 2) / (2 * (Do ** 2)))

    maskbutter = im2double(maskgussine)
    pil_img = (dft_shift * maskbutter) / 255
    pil_img = np.fft.ifftshift(pil_img)
    pil_img = np.fft.ifft2(pil_img, axes=(0, 1))
    pil_img = np.abs(pil_img).clip(0, 255).astype(np.uint8)
    imgs.append(pil_img)
    image = Image.fromarray(pil_img)
    byte_arr = io.BytesIO()
    image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
    encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
    return encoded_img
@app.route('/Kmeans',methods=['GET'])
def kmeans():
    listoffunc.append("kmeans")
    img = checkgray(imgs[-1])
    img = cv2.resize(img, (300, 300))
    Z = img.reshape((-1, 3))
    Z = np.float32(Z)
    criteria = (cv2.TERM_CRITERIA_EPS + cv2.TERM_CRITERIA_MAX_ITER, 10, 1.0)
    K = 4
    ret, label, center = cv2.kmeans(Z, K, None, criteria, 10, cv2.KMEANS_RANDOM_CENTERS)
    center = np.uint8(center)
    res = center[label.flatten()]
    pil_img = res.reshape((img.shape))
    imgs.append(pil_img)
    image = Image.fromarray(pil_img)
    byte_arr = io.BytesIO()
    image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
    encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
    return encoded_img
@app.route('/Levelset',methods=['GET'])
def LevelSET():
        listoffunc.append("level")
        image = checkgray(imgs[-1])
        image = image - np.mean(image)
        image = cv2.GaussianBlur(image, (3, 3), 0)
        pil_img = image
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Canny',methods=['GET'])
def canny():
        listoffunc.append("canny")
        image = checkgray(imgs[-1])
        pil_img = cv2.Canny(image, 10, 20)
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Global',methods=['GET'])
def globalthreshold():
        listoffunc.append("global")
        image =checkgray(imgs[-1])
        pil_img = cv2.threshold(image, 127, 255, cv2.THRESH_BINARY)[-1]
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Clear',methods=['GET'])
def clear():
        listoffunc.clear()
        pil_img = imgs[0]
        imgs.clear()
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/ostu',methods=['GET'])
def ostu():
        listoffunc.append("ostu")
        image = checkgray(imgs[-1])
        pil_img = cv2.threshold(image, 0,255, cv2.THRESH_BINARY+cv2.THRESH_OTSU)[-1]
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/adpaptive',methods=['GET'])
def  Adaptivethreshold():
        listoffunc.append("adaptive")
        image = checkgray(imgs[-1])
        pil_img =  cv2.adaptiveThreshold(image, 255, cv2.ADAPTIVE_THRESH_MEAN_C,cv2. THRESH_BINARY, 11,2)
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Watershed',methods=['GET'])
def  Watershed():
        listoffunc.append("water")
        image = checkgray(imgs[-1])
        thresh = cv2.threshold(image, 0, 255,
                               cv2.THRESH_BINARY | cv2.THRESH_OTSU)[1]

        contours, hierarchy = cv2.findContours(thresh.copy(),
                                               cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE)
        image = cv2.cvtColor(image, cv2.COLOR_GRAY2BGR)
        cv2.drawContours(image, contours, -1, (255,0,0 ), 3)
        pil_img =  image
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/snakeACWE',methods=['GET'])
def  SnakeAcwe():
        listoffunc.append("Snake")
        image = checkgray(imgs[-1])
        init_ls = checkerboard_level_set(image.shape, 6)
        evolution = []
        callback = store_evolution_in(evolution)
        ls = morphological_chan_vese(image, 35, init_level_set=init_ls,
                                     smoothing=3, iter_callback=callback)
        x, y = np.shape(ls)
        img = image.copy()
        for i in range(0, x):
            for j in range(0, y):
                if ls[i][j] == 1:
                    img[i][j] = 255
                else:
                    img[i][j] = 0
        contours, hierarchy = cv2.findContours(img.copy(),
                                               cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_NONE)
        image = cv2.cvtColor(image, cv2.COLOR_GRAY2BGR)
        cv2.drawContours(image, contours, -1, (255, 0, 0), 3)
        pil_img =  image
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Region',methods=['GET'])
def  Region():
        listoffunc.append("Region")
        img=checkgray(imgs[-1])
        x, y = np.shape(img)
        seeds = [Point(10, 10), Point(82, x // 2), Point(20, y - 1)]
        pil_img = regionGrow(img, seeds, 10)
        pil_img=im2double(pil_img)
        pil_img = pil_img.astype(np.uint8)
        imgs.append(pil_img)
        image = Image.fromarray(pil_img)
        byte_arr = io.BytesIO()
        image.save(byte_arr, format='PNG')  # convert the PIL image to byte array
        encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
        return encoded_img
@app.route('/Chaincode',methods=['GET'])
def chaincode():
    listoffunc.append("Chaincode")
    image =checkgray(imgs[-1])
    rows, cols = image.shape
    result = np.zeros_like(image)
    for x in range(rows):
        for y in range(cols):
            if image[x, y] >= 70:
                result[x, y] = 0
            else:
                result[x, y] = 1
    for i, row in enumerate(result):
        for j, value in enumerate(row):
            if value == 1:
                start_point = (i, j)
                #            print(start_point, value)
                break
        else:
            continue
        break

    directions = [0, 1, 2,
                  7, 3,
                  6, 5, 4]
    dir2idx = dict(zip(directions, range(len(directions))))
    # print(dir2idx)
    change_j = [-1, 0, 1,  # x or columns
                -1, 1,
                -1, 0, 1]

    change_i = [-1, -1, -1,  # y or rows
                0, 0,
                1, 1, 1]

    border = []
    chain = []

    curr_point = start_point
    for direction in directions:
        idx = dir2idx[direction]
        new_point = (start_point[0] + change_i[idx], start_point[1] + change_j[idx])
        if result[new_point] != 0:  # if is ROI
            border.append(new_point)
            chain.append(direction)
            curr_point = new_point
            break
    count = 0

    while curr_point != start_point:
        # figure direction to start search
        b_direction = (direction + 5) % 8
        dirs_1 = range(b_direction, 8)
        dirs_2 = range(0, b_direction)
        dirs = []
        dirs.extend(dirs_1)
        dirs.extend(dirs_2)
        for direction in dirs:
            idx = dir2idx[direction]
            new_point = (curr_point[0] + change_i[idx], curr_point[1] + change_j[idx])
            i,j=new_point
            x,y=np.shape(result)
            if (i>=x or i<0) or (j>=y or j<0):
               continue
            if result[new_point] != 0:  # if is ROI
                border.append(new_point)
                chain.append(direction)
                curr_point = new_point
                break
        if count == 1000:
            break
        count += 1
    img = np.zeros_like(image)
    r, c = img.shape
    for x in range(0, r):
        for y in range(0, c):
            if (result[x][y] == 1):
                img[x][y] = 255
    pil_img=im2double(img)
    pil_img = pil_img.astype(np.uint8)
    imgs.append(pil_img)
    image = Image.fromarray(pil_img)
    byte_arr = io.BytesIO()
    image.save(byte_arr, format='PNG')
    encoded_img = base64.encodebytes(byte_arr.getvalue()).decode('ascii')
    return encoded_img
functiondic={
    "median":Median,"min": min,"max":max,"mean":mean,"Snake":SnakeAcwe,"mid":mid
    ,"alpha":alpha,"water":Watershed,"adaptive":Adaptivethreshold,"ostu":ostu,"global":globalthreshold
    ,"canny":canny,"level":LevelSET,"kmeans":kmeans,"gH":gaussianH,"BH":butterH,"iH":idealH
    ,"gl":gaussianlow,"Bl":butterlow,"il":Ideallow,"meanShift":meanShift,"Cc":connectedcomponents
    ,"sobel":sobel,"negcontra":negcontraharmonic,"contra":contraharmonic,"Harmonic":Harmonic
    ,"geo":geomean,"Region":Region,"Chaincode":chaincode}
@app.route('/files/<files>',methods=['POST'])
def applyallfilter(files):
            image=[]
            listlabels=[]
            files=files.replace('_', '/').replace('-', '\\')
            folder=os.listdir(files)
            for ix in folder:
                path=os.path.join(files,ix)
                listlabels.append(ix)
                im=Image.open(path)
                im=checkgray(np.array(im))
                image.append(np.array(im))
            i=0
            for img in image :
               im=img
               for fun in listoffunc:
                 imgs.append(im)
                 functiondic[fun]()
                 listoffunc.pop()
                 im=imgs[-1]
                 imgs.pop()
                 imgs.pop()
               cv2.imwrite("D:\datasetout\\"+listlabels[i], im)
               i=i+1
            return  "hello"
if __name__=='__main__':
  app.run(port=5000)