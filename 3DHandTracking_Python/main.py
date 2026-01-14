import cv2
import mediapipe as mp
from mediapipe.tasks import python
from mediapipe.tasks.python import vision
import socket
import json

# ----------------------------
# Model setup
# ----------------------------
MODEL_PATH = "hand_landmarker.task"

BaseOptions = python.BaseOptions
HandLandmarker = vision.HandLandmarker
HandLandmarkerOptions = vision.HandLandmarkerOptions
VisionRunningMode = vision.RunningMode

options = HandLandmarkerOptions(
    base_options=BaseOptions(model_asset_path=MODEL_PATH),
    running_mode=VisionRunningMode.VIDEO,
    num_hands=2
)

landmarker = HandLandmarker.create_from_options(options)

# ----------------------------
# Webcam
# ----------------------------
cap = cv2.VideoCapture(0)

# Try setting a preferred resolution (optional)
cap.set(cv2.CAP_PROP_FRAME_WIDTH, 1280)
cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 720)

# Get the actual webcam resolution
frame_width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
frame_height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
print(f"Webcam actual resolution: {frame_width} x {frame_height}")

# ----------------------------
# UDP socket
# ----------------------------
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 3001)

# ----------------------------
# Helper: convert landmarks to dicts and mirror for Unity
# ----------------------------
def landmarks_to_dict(hand_landmarks, mirror_x=False):
    data = []
    for lm in hand_landmarks:
        x, y, z = lm
        if mirror_x:
            x = 1 - x  # mirror X for Unity camera
        data.append({"x": float(x), "y": float(y), "z": float(z)})
    return data

# ----------------------------
# Main loop
# ----------------------------
timestamp = 0
while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        break

    # Convert to RGB for MediaPipe
    frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
    mp_image = mp.Image(image_format=mp.ImageFormat.SRGB, data=frame_rgb)

    results = landmarker.detect_for_video(mp_image, timestamp)
    timestamp += 1

    left_frame = []
    right_frame = []

    if results.hand_landmarks and results.handedness:
        for hand_landmarks, handedness in zip(results.hand_landmarks, results.handedness):
            # Convert to tuples
            landmark_array = [(lm.x, lm.y, lm.z) for lm in hand_landmarks]

            hand_label = handedness[0].category_name  # "Left" or "Right"

            # Mirror X for Unity perspective
            if hand_label == "Left":
                left_frame = landmarks_to_dict(landmark_array, mirror_x=True)
            else:
                right_frame = landmarks_to_dict(landmark_array, mirror_x=True)

    # Send normalized landmarks to Unity
    payload = {"left": left_frame, "right": right_frame}
    sock.sendto(json.dumps(payload).encode("utf-8"), serverAddressPort)

    # Optional: draw on OpenCV window
    for hand in [left_frame, right_frame]:
        for lm in hand:
            cx = int(lm["x"] * frame_width)
            cy = int(lm["y"] * frame_height)
            cv2.circle(frame, (cx, cy), 5, (0, 255, 0), -1)

    cv2.imshow("Hands", frame)
    if cv2.waitKey(1) & 0xFF == 27:
        break


# ----------------------------
# Cleanup
# ----------------------------
cap.release()
cv2.destroyAllWindows()
sock.close()
