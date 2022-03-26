from jetbotSim import Robot, Camera
import numpy as np
import cv2
import collections

from dqn_model import DQN
import torch
import torch.nn as nn
import torch.optim as optim


GAMMA = 0.99
BATCH_SIZE = 32
REPLAY_SIZE = 10000
LEARNING_RATE = 1e-4
SYNC_TARGET_FRAMES = 1000
REPLAY_START_SIZE = 32
EPSILON_DECAY_LAST_FRAME = 10**4
EPSILON_START = 1.0
EPSILON_FINAL = 0.02
k = 3

action = 0
frames = 0
episode = 1
total_steps = 0
states = []
next_states = []
rewards = []
total_rewards = []
acc_rewards = 0
best_mean = 0
device = 'cpu'
net = DQN((3,45,80),k).to(device)
tgt_net = DQN((3,45,80),k).to(device)
epsilon = EPSILON_START
optimizer = optim.Adam(net.parameters(), lr=LEARNING_RATE)

Experience = collections.namedtuple('Experience', field_names=['state', 'action', 'reward', 'done', 'new_state'])
class ExperienceBuffer:
    def __init__(self, capacity):
        self.buffer = collections.deque(maxlen=capacity)

    def __len__(self):
        return len(self.buffer)

    def append(self, experience):
        self.buffer.append(experience)

    def sample(self, batch_size):
        indices = np.random.choice(len(self.buffer), batch_size, replace=False)
        states, actions, rewards, dones, next_states = zip(*[self.buffer[idx] for idx in indices])
        return np.array(states), np.array(actions, dtype=np.int64), np.array(rewards, dtype=np.float32), \
               np.array(dones, dtype=np.uint8), np.array(next_states)
exp_buffer = ExperienceBuffer(REPLAY_SIZE)


def calc_loss():
    states, actions, rewards, dones, next_states = exp_buffer.sample(BATCH_SIZE)

    states_v = torch.tensor(np.array( states, copy=False)).to(device)
    next_states_v = torch.tensor(np.array( next_states, copy=False)).to(device)
    actions_v = torch.tensor(actions).to(device)
    rewards_v = torch.tensor(rewards).to(device)
    done_mask = torch.BoolTensor(dones).to(device)

    state_action_values = net(states_v.float()).gather( 1, actions_v.unsqueeze(-1)).squeeze(-1)
    with torch.no_grad():
        next_state_values = tgt_net(next_states_v.float()).max(1)[0]
        next_state_values[done_mask] = 0.0
        next_state_values = next_state_values.detach()

    expected_state_action_values = next_state_values * GAMMA + rewards_v
    return nn.MSELoss()(state_action_values, expected_state_action_values)

def reset():
    global robot, action, frames, states, next_states, rewards, acc_rewards
    robot.reset()
    action = 0
    frames = 0
    states = []
    next_states = []
    rewards = []
    acc_rewards = 0

def step(action):
    global robot
    if action == 0:
        robot.set_motor(0.5, 0.5)
    elif action == 1:
        robot.set_motor(0.2, 0.)
    elif action == 2:
        robot.set_motor(0., 0.2)

def execute(change):
    global frames, states, next_states, rewards, action, total_steps, episode, epsilon, acc_rewards, best_mean

    # Visualize
    img = cv2.resize(change["new"],(80,45))
    img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    img[img<200] = 0
    img[img>=200] = 255 
    #cv2.imshow("camera", img)
    #cv2.waitKey(1)
    reward = change['reward']
    acc_rewards += reward

    frames += 1
    print('Episode:'+ str(episode) + ' Frames:' + str(frames) + ' Epsilon:' + str(epsilon))
    next_states.append(img)
    rewards.append(reward)

    if len(exp_buffer) >= REPLAY_START_SIZE:
        if total_steps % SYNC_TARGET_FRAMES == 0:
            tgt_net.load_state_dict(net.state_dict())
        optimizer.zero_grad()
        loss_t = calc_loss()
        loss_t.backward()
        optimizer.step()

    if len(states) == k and len(next_states) == k:
        exp = Experience(states, action, sum(rewards), False, next_states)
        exp_buffer.append(exp)
    
    if frames % k == 0:
        states = next_states.copy()
        next_states = []
        rewards = []

    if frames % k == 0:
        total_steps += 1
        epsilon = max(EPSILON_FINAL, EPSILON_START - total_steps / EPSILON_DECAY_LAST_FRAME)
        if np.random.random() < epsilon:
            act_v = np.random.randint(3)
        else:
            state_v = torch.tensor([states]).to(device)
            q_vals_v = net(state_v.float())
            _, act_v = torch.max(q_vals_v, dim=1)
        action = int(act_v)

    if frames > 450 or change['done']: 
        episode += 1
        total_rewards.append(acc_rewards)
        mean_reward = np.mean(total_rewards[-30:])
        if best_mean < mean_reward:
            print("Best mean reward updated %.3f -> %.3f, model saved" % (best_mean, mean_reward))
            torch.save(net.state_dict(), "best_model.dat")
            best_mean = mean_reward
        reset()
    else:
        step(action)

    #testing
    #i = input()
    #if i == 'r' or change['done']:
    #    robot.reset()
    #elif i == 'a':
    #    robot.set_motor(0., 0.2)
    #elif i == 'd':
    #    robot.set_motor(0.2, 0.)
    #else:
    #    robot.set_motor(0.5, 0.5)

robot = Robot()
camera = Camera()
camera.observe(execute)