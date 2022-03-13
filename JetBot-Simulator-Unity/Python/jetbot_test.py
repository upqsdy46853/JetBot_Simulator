from jetbotSim import Robot, Camera
import numpy as np
import cv2

frames = 0
stop_count = 0
def execute(change):
    global robot, frames, stop_count
    img = cv2.resize(change["new"], (224,224)) # Resize
    print("\rFrames", frames, end="")
    frames += 1
    
    # Get red line (hsv=[0~10/156~180, 43~255, 46~255])
    hsv = cv2.cvtColor(img, cv2.COLOR_BGR2HSV)
    lower_red1 = np.array([156, 43, 46])
    upper_red1 = np.array([180, 255, 255])
    mask1 = cv2.inRange(hsv, lower_red1, upper_red1)
    lower_red2 = np.array([0, 43, 46])
    upper_red2 = np.array([10, 255, 255])
    mask2 = cv2.inRange(hsv, lower_red2, upper_red2)
    mask = cv2.bitwise_or(mask1, mask2)
    
    # Filter small region
    kernel = np.uint8(np.ones((3,3)))
    mask = cv2.erode(mask, kernel)
    mask = cv2.dilate(mask, kernel)
    
    # Detect centor of road
    detect_horizon = 200
    coord = np.array([i for i in range(224)])
    detect = mask[detect_horizon,:] 
    road_center = (coord * detect).sum() / detect.sum() if detect.sum() != 0 else -1
    
    # Control motor
    if road_center > 0:
        # P-control
        p_gain = 0.01
        feedback = 112 - road_center
        robot.set_motor(0.2-feedback*p_gain, 0.2+feedback*p_gain)
        stop_count = 0
    else:
        # Stop after several frames of detection fail
        stop_count += 1
        if stop_count > 10:
            robot.stop()

    # Visualize
    res = cv2.bitwise_and(img, img, mask=mask)
    res[detect_horizon,:,0] = 255
    cv2.circle(res, (int(road_center),int(detect_horizon)), 4, (0,255,0), 2)
    #cv2.imshow("road", res)
    #cv2.imshow("camera", img)
    
    img_demo = np.vstack((img, res))
    cv2.imshow("demo", img_demo)

cv2.namedWindow("demo")
cv2.waitKey(0)
robot = Robot()
camera = Camera()
camera.observe(execute)