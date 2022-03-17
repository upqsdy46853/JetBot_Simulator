from jetbotSim import Robot, Camera
import numpy as np
import cv2

frames = 0
def execute(change):
    global robot, frames
    print("\rFrames", frames, end="")
    frames += 1

    # Visualize
    img = cv2.resize(change["new"],(640,360))
    cv2.imshow("camera", img)
    cv2.waitKey(1)
    i = input()
    if i == 'r':
        robot.reset()
    elif i == 'a':
        robot.set_motor(0., 0.2)
    elif i == 'd':
        robot.set_motor(0.2, 0.)
    else:
        robot.set_motor(0.5, 0.5)


    # Control Example
    #if frames == 1:
    #    robot.forward(0.2)
    #if frames == 80:
    #    robot.left(0.05)
    #if frames == 88:
    #    robot.stop()
    #if frames == 90:
    #    robot.set_motor(0.2,0.2)
    #if frames == 200:
    #    robot.set_left_motor(0)
    #if frames == 201:
    #    robot.set_right_motor(0)
    #if frames == 202:
    #    robot.right(0.05)
    #if frames == 210:
    #    robot.backward(0.2)
    #if frames == 320:
    #    robot.add_motor(0,0.02)
    #if frames == 400:
    #    robot.reset()

robot = Robot()
camera = Camera()
camera.observe(execute)