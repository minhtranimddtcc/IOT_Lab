from keras.models import load_model
from PIL import Image, ImageOps
import numpy as np
import cv2
import time

cam = cv2.VideoCapture(0)

# Load the model
model = load_model('keras_model.h5')


def capture_image():
    ret, frame = cam.read()
    cv2.imwrite("img_detect.png", frame)


def ai_dectection():
    # Create the array of the right shape to feed into the keras model
    # The 'length' or number of images you can put into the array is
    # determined by the first position in the shape tuple, in this case 1.
    data = np.ndarray(shape=(1, 224, 224, 3), dtype=np.float32)
    # Replace this with the path to your image

    image = Image.open('img_detect.png')
    # resize the image to a 224x224 with the same strategy as in TM2:
    # resizing the image to be at least 224x224 and then cropping from the center
    size = (224, 224)
    image = ImageOps.fit(image, size, Image.ANTIALIAS)

    # turn the image into a numpy array
    image_array = np.asarray(image)
    # Normalize the image
    normalized_image_array = (image_array.astype(np.float32) / 127.0) - 1
    # Load the image into the array
    data[0] = normalized_image_array

    # run the inference
    prediction = model.predict(data)
    maxIndex = 0
    for i in range(len(prediction[0])):
        if prediction[0][i] > prediction[0][maxIndex]:
            maxIndex = i
    return maxIndex, int(prediction[0][maxIndex]*100)

# while True:
#     capture_image()
#     ai_dectection()
#     time.sleep(5)
