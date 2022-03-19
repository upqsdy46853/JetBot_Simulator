from jetbotSim import Robot, Camera
import cv2
from dqn_model import DQN
import torch

k = 3
action = 0
states = []
device = 'cpu'
net = DQN((3,45,80),k).to(device)
model_path = "./best_model.dat"
net.load_state_dict(torch.load(model_path, map_location=lambda storage, loc: storage))

def step(action):
    global robot
    if action == 0:
        robot.set_motor(1, 1)
    elif action == 1:
        robot.set_motor(0.2, 0.)
    elif action == 2:
        robot.set_motor(0., 0.2)

def execute(change):
    global states, action

    # Visualize
    img = cv2.resize(change["new"],(80,45))
    img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    #cv2.imshow("camera", img)
    #cv2.waitKey(1)

    states.append(img)

    if len(states) == k:
        state_v = torch.tensor([states]).to(device)
        q_vals_v = net(state_v.float())
        _, act_v = torch.max(q_vals_v, dim=1)
        action = int(act_v)
        states = []

    step(action)
    if change['done']: 
        robot.reset()


robot = Robot()
camera = Camera()
camera.observe(execute)