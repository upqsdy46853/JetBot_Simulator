import numpy as np
import cv2
import copy
import websocket
from websocket import create_connection
import threading
import struct
import time
import json
import random

################################################################
# Connect Server
#ip = "140.109.23.111:9002"
ip = "localhost:9002"
actor = "Actor1"
ws_control = None
ws_camera = None
wst = None
cameraStream = None
on_change = False

def connect_server():
    global ip, actor, ws_control, ws_camera, wst
    ws_control = create_connection("ws://%s/%s/controller/session"%(ip, actor))
    ws_camera = websocket.WebSocketApp("ws://%s/%s/camera/subscribe"%(ip, actor), on_message = on_message_camera)
    wst = threading.Thread(target=ws_camera.run_forever)
    wst.daemon = True
    wst.start()

def on_message_camera(ws, message):
    global cameraStream, on_change
    cameraStream = message
    on_change = True

if __name__ == "__main__":
    connect_server()
    jsonStr = json.dumps({'leftMotor':180.0, 'rightMotor':180.0, 'flag':3})
    ws_control.send(jsonStr)
    count = 0
    while(True):  
        if cameraStream is not None and on_change:
            print("\rReceive:", count, "Frames", end="") 
            nparr = np.fromstring(cameraStream, np.uint8)
            img = cv2.imdecode(nparr, cv2.IMREAD_COLOR)
            img_re = cv2.resize(img, (640,360))
            on_change = False
            cv2.imshow("test", img_re)
            cv2.waitKey(1)
            count += 1
