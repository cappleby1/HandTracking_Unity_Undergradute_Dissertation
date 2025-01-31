import time
import cv2
from cvzone.HandTrackingModule import HandDetector
import socket

# Parameters
width, height = 2560, 1664

# Webcam Settings
cap = cv2.VideoCapture(0)
cap.set(3, width)
cap.set(4, height)

# Hand tracking settings
detector = HandDetector(maxHands=1, detectionCon=0.9)

# Socket setup
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 3001)

start_time = time.time()
frame_count = 0

total_frames = 0
detected_frames = 0

counting = False

while True:
    # Get frame
    success, img = cap.read()

    # Find hands & save the coordinates in a dictionary
    hands, img = detector.findHands(img)
    data = []

    if hands:
        if not counting:
            counting = True

        detected_frames += 1

    if counting:
        total_frames += 1

    # Landmark Values x, y, z
    if hands:
        # Get first hand
        hand = hands[0]

        # Get landmark list
        landmarkList = hand['lmList']

        for landmark in landmarkList:
            data.extend([landmark[0], height - landmark[1], landmark[2]])
        sock.sendto(str.encode(str(data)), serverAddressPort)

    cv2.imshow('Image', img)
    cv2.waitKey(1)

    frame_count += 1
    if frame_count % 10 == 0:  # Update FPS every 10 frames
        elapsed_time = time.time() - start_time
        fps = frame_count / elapsed_time
        print("FPS:", fps)

    if total_frames > 0:
        detection_rate = (detected_frames / total_frames) * 100
        print("Detection Rate: {:.2f}%".format(detection_rate))
