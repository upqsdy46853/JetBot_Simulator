from jetbotSim import Robot, Camera
import cv2
from dqn_model import DQN
import torch

net = DQN((3,45,80),3).to('cpu')
model_path = "./best_model.dat"
net.load_state_dict(torch.load(model_path, map_location=lambda storage, loc: storage))