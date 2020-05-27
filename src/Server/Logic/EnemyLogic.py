# _*_ coding:utf-8 _*_

import Queue
import random
import Road
import time
from threading import Thread


class EnemyLogic:
    def __init__(self, user_info={}):
        self.user_info = user_info
        self.enemyInfo = dict()
        self.enemy_num = 0
        self.zombie_num = 0
        self.ack_que = Queue.Queue(32)
        self.req_que = Queue.Queue(32)
        self.road = Road.Road()

    def addTask(self, task):
        self.req_que.put(task, block=True, timeout=None)

    def getTask(self):
        return self.ack_que

    def taskDone(self):
        self.ack_que.task_done()

    def update_userInfo(self, userInfo):
        self.user_info = userInfo

    def send_enemy_info(self):
        for enemy_id in self.enemyInfo:
            self.ack_que.put(self.enemyInfo[enemy_id], block=True, timeout=None)

    def mainLogic(self):
        while True:
            try:
                while self.req_que.qsize() > 0:
                    task = self.req_que.get(block=True, timeout=0.1)#接收消息
                    if task['calling'] == 'enemy_update':
                        self.enemy_update(task)
                    elif task['calling'] == 'enemy_dead':
                        self.enemy_dead(task)
                    self.req_que.task_done()#完成一个任务

                self.enemy_generate()
                self.enemy_behavior()
                time.sleep(0.1)
            except Exception as e:
                pass

    def enemy_update(self, task):
        enemy_name = task['name']
        if enemy_name in self.enemyInfo:
            self.enemyInfo[enemy_name]['hp'] = int(task['hp'])
            self.enemyInfo[enemy_name]['pos_x'] = float(task['pos_x'])
            self.enemyInfo[enemy_name]['pos_y'] = float(task['pos_y'])
            self.enemyInfo[enemy_name]['pos_z'] = float(task['pos_z'])

    def enemy_dead(self, task):
        enemy_name = task['name']
        print(enemy_name, "dead")
        if enemy_name in self.enemyInfo:
            if self.enemyInfo[enemy_name]['type'] == 'Zombie':
                self.zombie_num -= 1
            self.enemyInfo.pop(enemy_name)
            enemy_task = dict()
            enemy_task['calling'] = 'enemy_dead'
            enemy_task['name'] = enemy_name
            self.ack_que.put(enemy_task, block=True, timeout=None)

    """刷怪"""
    def enemy_generate(self):
        try:
            while len(self.enemyInfo) < 5:
                pos_x, pos_z = random.randint(0, 119), random.randint(0, 119)
                while self.road.path[pos_x][pos_z] == 0:
                    pos_x, pos_z = random.randint(0, 119), random.randint(0, 119)
                if self.zombie_num > 0:
                    enemy = {'type': 'Soidier', 'hp': 200, 'state': 'idle', 'target': 'null',
                             'pos_x': pos_x, 'pos_y': 0, 'pos_z': pos_z, }
                else:
                    enemy = {'type': 'Zombie', 'hp': 500, 'state': 'idle', 'target': 'null',
                             'pos_x': pos_x, 'pos_y': 0, 'pos_z': pos_z, }
                    self.zombie_num = 1
                """e = random.random()
                e = 0
                if e > 0.5:
                    enemy = {'type': 'Soidier', 'hp': 200, 'state': 'idle', 'target': 'null',
                             'pos_x': pos_x, 'pos_y': 0, 'pos_z': pos_z, }
                else:
                    enemy = {'type': 'Zombie', 'hp': 500, 'state': 'idle', 'target': 'null',
                             'pos_x': pos_x, 'pos_y': 0, 'pos_z': pos_z, }"""

                key = enemy['type'] + str(self.enemy_num)
                self.enemy_num += 1
                self.enemyInfo[key] = enemy.copy()

                enemy_task = enemy
                enemy_task['calling'] = 'enemy_generate'
                enemy_task['name'] = key
                self.ack_que.put(enemy_task, block=True, timeout=None)
                print("generate enemy", key)
        except Exception as e:
            pass

    """怪物寻路"""
    def enemy_behavior(self):
        try:
            for enemy_id in self.enemyInfo:
                enemy_info = self.enemyInfo[enemy_id]
                """if enemy_info['target'] != 'null':
                    continue"""
                distance = 2147483647
                target = 'null'
                if len(self.user_info) < 1:
                    return
                for name in self.user_info:
                    info = self.user_info[name]
                    distance_x = pow(enemy_info['pos_x']-float(info['pos_x']), 2)
                    distance_y = pow(enemy_info['pos_z']-float(info['pos_z']), 2)
                    if pow(distance_x + distance_y, 0.5) < distance:
                        target = name
                        distance = pow(distance_x + distance_y, 0.5)
                #print(enemy_id, "distance=", distance)
                enemy_info['target'] = target
                enemy_task = enemy_info.copy()
                enemy_task['name'] = enemy_id
                if(enemy_task['type'] == 'Soidier' and distance < 15)or(enemy_task['type'] == 'Zombie'and distance < 1):
                    enemy_task['calling'] = 'enemy_attack'
                    enemy_task['state'] = 'attack'
                else:
                    enemy_task['calling'] = 'enemy_move'
                    enemy_task['state'] = 'run'
                self.ack_que.put(enemy_task, block=True, timeout=None)
        except Exception as e:
            pass
